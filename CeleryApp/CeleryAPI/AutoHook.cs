using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeStepPackage;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Runtime.InteropServices;
using ManualMapApi;
using Microsoft.Win32.SafeHandles;
using System.Security.Principal;

namespace Injector
{
    public enum InjectionStatus
    {
        FAILED,
        FAILED_ADMINISTRATOR_ACCESS,
        ALREADY_INJECTING,
        ALREADY_INJECTED,
        SUCCESS
    };

    public class WindowsPlayer : Util
    {
        private static string injectFileName = "Celery.bin";

        public static bool isInjected(ProcInfo pinfo)
        {
            if (pinfo.isOpen())
                if (pinfo.readByte(Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon") + 3) == 0x43)
                    return true;

            return false;
        }

        public static void sendScript(ProcInfo pinfo, string source)
        {
            if (!isInjected(pinfo)) return;

            var functionPtr = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon");

            // to-do:
            // if the last script hasn't been executed yet
            // or the dll slacked on freeing memory, we will
            // do it ourselves
            /*if (sourcePtr == pinfo.readInt(functionPtr + 12))
                Imports.VirtualFreeEx(pinfo.handle, sourcePtr, 0, MEM_RELEASE);
            */

            int dontGoToInf = 0;

            while (pinfo.readUInt32(functionPtr + 8) > 0)
            {
                Thread.Sleep(10);
                if (dontGoToInf++ > 100) return;
            }

            // VVV this check is causing the run button to act slow ??? VVV

            // Wait for a script to finish executing and old source deallocated ...
            //if (!(util.readUInt(function_ptr + 8) > 0 && util.readUInt(function_ptr + 8) < 0xA))

            var oldProtect = pinfo.setPageProtect(functionPtr, 0x20, Imports.PAGE_READWRITE);

            int nothing = 0;
            char[] chars = source.ToCharArray(0, source.Length);
            byte[] bytesUtf8 = Encoding.UTF8.GetBytes(chars);
            var sourcePtr = Imports.VirtualAllocEx(pinfo.handle, 0, bytesUtf8.Length, Imports.MEM_RESERVE | Imports.MEM_COMMIT, Imports.PAGE_READWRITE);
            Imports.WriteProcessMemory(pinfo.handle, sourcePtr, bytesUtf8, bytesUtf8.Length, ref nothing);

            pinfo.writeUInt32(functionPtr + 8, 1); // Type `1` = script was sent to be executed
            pinfo.writeInt32(functionPtr + 12, sourcePtr);
            pinfo.writeInt32(functionPtr + 16, bytesUtf8.Length);

            pinfo.setPageProtect(functionPtr, 0x20, oldProtect);
        }

        public bool findProcess(ref ProcInfo outPInfo)
        {
            foreach (ProcInfo pinfo in openProcessesByName("RobloxPlayerBeta.exe"))
            {
                // if (process.Threads.Count > 10)
                var msg_dos_mode = pinfo.readUInt32(pinfo.baseModule + 0x50);
                if (msg_dos_mode != 0)
                {
                    outPInfo = pinfo;
                    return true;
                }
            }

            return false;
        }

        public bool findHiddenProcess(ref ProcInfo outPInfo)
        {
            foreach (ProcInfo pinfo in openProcessesByName("RobloxPlayerBeta.exe"))
            {
                // if (process.Threads.Count > 10)
                var msg_dos_mode = pinfo.readUInt32(pinfo.baseModule + 0x50);
                if (msg_dos_mode == 0)
                {
                    outPInfo = pinfo;
                    return true;
                }
            }

            return false;
        }

        private static List<ProcInfo> postInjectedMainPlayer = new List<ProcInfo>();
        private static bool isInjectingMainPlayer = false;

        public static InjectionStatus injectMainPlayer(ProcInfo pinfo)
        {
            if (isInjectingMainPlayer)
                return InjectionStatus.ALREADY_INJECTING;

            if (isInjected(pinfo))
                return InjectionStatus.ALREADY_INJECTED;

            isInjectingMainPlayer = true;

            // Prepare our location for sending script inputs
            // 
            var functionPtr = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon");
            var oldProtect = pinfo.setPageProtect(functionPtr, 0x20, Imports.PAGE_READWRITE);
            pinfo.writeUInt32(functionPtr + 8, 0);
            pinfo.writeInt32(functionPtr + 12, 0);
            pinfo.writeInt32(functionPtr + 16, 0);
            pinfo.setPageProtect(functionPtr, 0x20, oldProtect);

            var sourcePtr = 0;
            var mapResult = false;

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "update.txt"))
            {
                if (File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "update.txt") == "true")
                {
                    mapResult = MapInject.ManualMap(pinfo.processRef, "C:\\Users\\javan\\Desktop\\Celery2.0\\dll\\celery.bin");
                    
                    return InjectionStatus.FAILED; // this is personal >:(
                }
            }

            Console.WriteLine("Manual mapping...");
            mapResult = MapInject.ManualMap(pinfo.processRef, AppDomain.CurrentDomain.BaseDirectory + "dll/" + injectFileName);
            
            if (mapResult)
            {
                while (pinfo.isOpen() && !isInjected(pinfo))
                {
                    Thread.Sleep(10);
                }

                // Easier access to processes injected, for when we run scripts and etc.
                postInjectedMainPlayer.Add(pinfo);

                isInjectingMainPlayer = false;

                return InjectionStatus.SUCCESS;
            }
            else
            {
                isInjectingMainPlayer = false;

                return InjectionStatus.FAILED;
            }
        }

        public static List<ProcInfo> getInjectedProcesses()
        {
            List<ProcInfo> results = new List<ProcInfo>();

            foreach (ProcInfo x in postInjectedMainPlayer)
            {
                if (isInjected(x))
                {
                    results.Add(x);
                }
            }

            return results;
        }
    }

    public class MsStorePlayer : Util
    {
        private static bool consoleLoaded = false;
        public static bool consoleInUse = false;
        private static string injectProcessName = "Windows10Universal.exe";
        private static string injectFileName = "celeryuwp.bin";


        public static void showConsole()
        {
            if (!consoleLoaded)
            {
                consoleLoaded = true;
                consoleInUse = true;
                Imports.ConsoleHelper.Initialize();
            }
            else
            {
                consoleInUse = true;
                Imports.ShowWindow(Imports.GetConsoleWindow(), Imports.SW_SHOW);
            }
        }

        public static void hideConsole()
        {
            consoleInUse = false;
            Imports.ConsoleHelper.Clear();
            Imports.ShowWindow(Imports.GetConsoleWindow(), Imports.SW_HIDE);
        }

        public bool findProcess(ref ProcInfo outPInfo)
        {
            foreach (ProcInfo pinfo in openProcessesByName(injectProcessName))
            {
                outPInfo = pinfo;
                return true;
            }

            return false;
        }

        public static bool isInjected(ProcInfo pinfo)
        {
            if (pinfo.isOpen())
                if (pinfo.readByte(Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon") + 3) == 0x43)
                    return true;

            return false;
        }


        public static void sendScript(ProcInfo pinfo, string source)
        {
            if (!isInjected(pinfo)) return;

            var functionPtr = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon");

            // to-do:
            // if the last script hasn't been executed yet
            // or the dll slacked on freeing memory, we will
            // do it ourselves
            /*if (sourcePtr == pinfo.readInt(functionPtr + 12))
                Imports.VirtualFreeEx(pinfo.handle, sourcePtr, 0, MEM_RELEASE);
            */

            int dontGoToInf = 0;

            while (pinfo.readUInt32(functionPtr + 8) > 0)
            {
                Thread.Sleep(10);
                if (dontGoToInf++ > 100) return;
            }

            if (!isInjected(pinfo)) return;
            // VVV this check is causing the run button to act slow ??? VVV

            // Wait for a script to finish executing and old source deallocated ...
            //if (!(util.readUInt(function_ptr + 8) > 0 && util.readUInt(function_ptr + 8) < 0xA))

            //var oldProtect = pinfo.setPageProtect(functionPtr, 0x20, Imports.PAGE_READWRITE);

            int nothing = 0;
            char[] chars = source.ToCharArray(0, source.Length);
            byte[] bytesUtf8 = Encoding.UTF8.GetBytes(chars);
            var sourcePtr = Imports.VirtualAllocEx(pinfo.handle, 0, bytesUtf8.Length, Imports.MEM_RESERVE | Imports.MEM_COMMIT, Imports.PAGE_READWRITE);
            Imports.WriteProcessMemory(pinfo.handle, sourcePtr, bytesUtf8, bytesUtf8.Length, ref nothing);

            pinfo.writeUInt32(functionPtr + 8, 1); // Type `1` = script was sent to be executed
            pinfo.writeInt32(functionPtr + 12, sourcePtr);
            pinfo.writeInt32(functionPtr + 16, bytesUtf8.Length);

            //pinfo.setPageProtect(functionPtr, 0x20, oldProtect);
        }

        private static List<ProcInfo> postInjectedPlayer = new List<ProcInfo>();
        private static bool isInjectingPlayer = false;
        private static bool skipAdministrative = false;

        public static InjectionStatus injectPlayer(ProcInfo pinfo)
        {
            if (isInjectingPlayer)
                return InjectionStatus.ALREADY_INJECTING;

            if (isInjected(pinfo))
                return InjectionStatus.ALREADY_INJECTED;

            if (!skipAdministrative)
            {
                AppDomain myDomain = Thread.GetDomain();

                myDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                WindowsPrincipal myPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
                if (!myPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    if (MessageBox.Show("Celery is not running in administrative mode...There may be issues while injecting. Continue?") == MessageBoxResult.Cancel)
                    {
                        return InjectionStatus.FAILED_ADMINISTRATOR_ACCESS;
                    }
                    skipAdministrative = true;
                }
            }

            isInjectingPlayer = true;

            List<byte> bytes1 = new List<byte>();
            List<byte> bytes2 = new List<byte>();

            // Prepare our location for sending script inputs
            // 
            var functionPtr = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon");
            //pinfo.writeUInt32(functionPtr + 8, 0);
            //pinfo.writeInt32(functionPtr + 12, 0);
            //pinfo.writeInt32(functionPtr + 16, 0);
            //pinfo.setPageProtect(functionPtr, 0x20, oldProtect);
            var read = pinfo.readBytes(functionPtr + 8, 512);
            for (int i = 0; i < 512 - 2; i++)
            {
                if (read[i + 1] == 0x8B && read[i + 2] == 0xFF)
                    break;

                bytes1.Add(0);
            }
            var oldProtect = pinfo.setPageProtect(functionPtr, bytes1.Count + 8, Imports.PAGE_EXECUTE_READWRITE);
            pinfo.writeBytes(functionPtr + 8, bytes1.ToArray());

            // ### comment this out if revert
            functionPtr = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIconEx");
            read = pinfo.readBytes(functionPtr, 512);
            for (int i = 0; i < 512 - 2; i++)
            {
                if (read[i + 1] == 0x8B && read[i + 2] == 0xFF)
                    break;

                bytes2.Add(0);
            }
            oldProtect = pinfo.setPageProtect(functionPtr, bytes2.Count, Imports.PAGE_EXECUTE_READWRITE);
            pinfo.writeBytes(functionPtr, bytes2.ToArray());
            //pinfo.writeUInt32(functionPtr + (13 * sizeof(int)), homePathPointer);
            //pinfo.writeInt32(functionPtr + (14 * sizeof(int)), homePathSize);
            //pinfo.setPageProtect(functionPtr, 0x20, oldProtect);
            

            var sourcePtr = 0;
            var mapResult = false;

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "update.txt"))
            {
                if (File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "update.txt") == "true")
                {
                    //mapResult = MapInject.ManualMap(pinfo.processRef, "C:\\Users\\javan\\Desktop\\Celery2.0\\dll\\celery.bin");

                    return InjectionStatus.FAILED; // this is personal >:(
                }
            }

            Console.WriteLine("Manual mapping...");
            mapResult = MapInject.ManualMap(pinfo.processRef, AppDomain.CurrentDomain.BaseDirectory + "dll/" + injectFileName);

            if (mapResult)
            {
                while (pinfo.isOpen() && !isInjected(pinfo))
                {
                    Thread.Sleep(10);
                }

                // Easier access to processes injected, for when we run scripts and etc.
                postInjectedPlayer.Add(pinfo);

                isInjectingPlayer = false;

                showConsole();
                
                /*
                Imports.AllocConsole();
                Imports.SetConsoleTitle("TEST");
                IntPtr stdHandle = Imports.GetStdHandle(Imports.STD_OUTPUT_HANDLE);
                SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                fileStream.Write(new byte[] { 0x66, 0x6C, 0x65, 0x65, 0x6C }, 0, 5);
                Encoding encoding = System.Text.Encoding.GetEncoding(Imports.MY_CODE_PAGE);
                StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
                standardOutput.AutoFlush = true;
                standardOutput.WriteLine("TEST");
                standardOutput.Write("LOL");
                Console.SetOut(standardOutput);
                */

                return InjectionStatus.SUCCESS;
            }
            else
            {
                isInjectingPlayer = false;

                return InjectionStatus.FAILED;
            }
        }

        public static List<ProcInfo> getInjectedProcesses()
        {
            List<ProcInfo> results = new List<ProcInfo>();
            /*
            foreach (ProcInfo pinfo in postInjectedPlayer)
            {
                if (isInjected(pinfo))
                {
                    results.Add(pinfo);
                }
            }
            */
            foreach (ProcInfo pinfo in openProcessesByName(injectProcessName))
            {
                if (isInjected(pinfo))
                {
                    results.Add(pinfo);
                }
            }

            return results;
        }
    }

    /*
    public static List<int> injectedPids = new List<int>();
    public static bool tryingInject = false;

    public static bool TryInject()
    {
        bool result = false;

        if (!injecting)// && !CheckInjected() * /&& CheckRobloxOpened())
        {
            Console.WriteLine("We werent injecting before, but now we are!");
            injecting = true;

            source_ptr = 0;

            if (!Directory.Exists(System.IO.Path.GetTempPath() + "celery"))
            {
                try
                {
                    Directory.CreateDirectory(System.IO.Path.GetTempPath() + "celery");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An issue occurred. Please restart Celery. Error message: " + ex.Message);
                }
            }

            checkCreateFile(System.IO.Path.GetTempPath() + "celery\\celeryhome.txt", AppDomain.CurrentDomain.BaseDirectory + "dll\\");

            // To-do: inject into either one
            // 

            // Roblox universal windows player? (microsoft store app)
            if (OpenRobloxUWP())
            {
                /*var functionPtr = GetProcAddress(GetModuleHandle("USER32.dll"), "DrawIcon");
                var oldProtect = util.setPageProtect(functionPtr, PAGE_READWRITE);
                util.writeUInt(functionPtr + 8, 0);
                util.writeInt(functionPtr + 12, 0);
                util.writeInt(functionPtr + 16, 0);
                util.setPageProtect(functionPtr, oldProtect);

                functionPtr = GetProcAddress(GetModuleHandle("USER32.dll"), "DrawIconEx");
                oldProtect = util.setPageProtect(functionPtr, PAGE_READWRITE);
                for (int i = 1; i < 24 * 4; i += 4)
                    util.writeInt(functionPtr + i, 0);
                util.setPageProtect(functionPtr, oldProtect);

                Console.WriteLine("Manual mapping...");
                result = MapInject.ManualMap(EyeStep.current_proc, AppDomain.CurrentDomain.BaseDirectory + "dll/celeryuwp.bin");
                if (result)
                {
                    Process process = new Process();
                    // Configure the process using the StartInfo properties.
                    process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "dll/CeleryHelper.exe";
                    process.StartInfo.Arguments = "uwp " + Convert.ToString(EyeStep.current_proc.Id);
                    //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                }* /
            }

            if (Process.GetProcessesByName("RobloxPlayerBeta").Length >= 1)
            {
                Console.WriteLine("Roblox window available");

                if (OpenRobloxWPGhost())
                {
                    CeleryApp.CeleryAPI.Yara.Bypass();
                }

                Process proc = null;

                // Roblox windows player?
                if (OpenRobloxWP(ref proc))
                {
                    bool alreadyInjectedTo = false;

                    /*foreach (var id in injectedPids)
                    {
                        if (proc.Id == id)
                        {
                            alreadyInjectedTo = true;
                            break;
                        }
                    }* /

                    if (!alreadyInjectedTo)
                    {
                        injectedPids.Add(proc.Id);
                        
                        var functionPtr = GetProcAddress(GetModuleHandle("USER32.dll"), "DrawIcon");
                        var oldProtect = util.setPageProtect(functionPtr, PAGE_READWRITE);
                        util.writeUInt(functionPtr + 8, 0);
                        util.writeInt(functionPtr + 12, 0);
                        util.writeInt(functionPtr + 16, 0);
                        util.setPageProtect(functionPtr, oldProtect);

                        source_ptr = 0;

                        bool update = false;

                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "update.txt"))
                        {
                            if (File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "update.txt") == "true")
                            {
                                result = MapInject.ManualMap(EyeStep.current_proc, "C:\\Users\\javan\\Desktop\\Celery2.0\\dll\\celery.bin");
                                update = true;
                            }
                        }

                        if (!update)
                        {
                            Console.WriteLine("Manual mapping...");
                            result = MapInject.ManualMap(EyeStep.current_proc, AppDomain.CurrentDomain.BaseDirectory + "dll/Celery.bin");
                        }

                        if (result)
                        {
                            CeleryApp.MainWindow.writeAppSettings();

                            Thread.Sleep(100);

                            result = CheckInjectionStatus();
                            if (result)
                            {
                                injecting = false;
                                injected = true;
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }

            Thread.Sleep(500);

            injecting = false;
        }

        return result;
    }*/

}

