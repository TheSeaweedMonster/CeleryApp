﻿using EyeStepPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CeleryApp.CeleryAPI
{
    // External yara bypass I wrote last year
    public class Yara : Util
    {
        public static int codeCaveAt = 0;
        public static int codeCaveStart = 0;
        public static int codeCaveEnd = 0;

        public static void setCave(ProcInfo pinfo, int start, int end)
        {
            codeCaveStart = start;
            codeCaveEnd = end;
            codeCaveAt = codeCaveStart;

            byte[] bytes = new byte[end - start];
            Array.Clear(bytes, 0, bytes.Length);
            pinfo.writeBytes(codeCaveStart, bytes, bytes.Length);
        }

        public static int silentAlloc(ProcInfo pinfo, int size, uint protect)
        {
            if (codeCaveStart == 0 || codeCaveAt == 0)
            {
                return Imports.VirtualAllocEx(pinfo.handle, 0, size, Imports.MEM_COMMIT, protect);
            }

            int at = codeCaveAt;
            pinfo.setPageProtect(codeCaveAt, size, protect);
            codeCaveAt += size + (size % 4);
            return at;
        }

        public static List<int> getRbxWow64Clone(ProcInfo pinfo)
        {
            List<int> results = new List<int>();
            int start = 0;
            Imports.MEMORY_BASIC_INFORMATION page;

            /*
            07770000 - FF 25 06007707        - jmp dword ptr [07770006] { ->0777000A }
            07770006 - 0A 00                 - or al,[eax]
            07770008 - 77 07                 - ja 07770011
            0777000A - EA 09708877 3300      - jmp 0033:77887009
            07770011 - 00 00                 - add [eax],al
            07770013 - 00 00                 - add [eax],al
            */

            while (Imports.VirtualQueryEx(pinfo.handle, start, out page, 0x1C) != 0)
            {
                if (page.AllocationProtect == Imports.PAGE_EXECUTE_READWRITE && page.State == Imports.MEM_COMMIT)
                {
                    if (pinfo.readByte(start + 0xA) == 0xEA && pinfo.readByte(start + 0x65) != 0xE9)
                    {
                        results.Add(start);
                        //if (results.Count == 2) break; 
                    }
                }

                start += page.RegionSize;
            }

            return results;
        }

        // "stackRef" must be a small allocated memory with READ/WRITE
        // privileges. It enables us to watch what YARA is reading by
        // writing the information there
        public static byte[] makePayload(int stackRef)
        {
            /*
            hNtQueryVirtualMemory(
	            HANDLE                   ProcessHandle,
	            PVOID                    BaseAddress,
	            PVOID					 MemoryInformationClass,
	            PVOID                    MemoryInformation,
	            SIZE_T                   MemoryInformationLength,
	            PSIZE_T                  ReturnLength
            )
            */
            byte[] b = BitConverter.GetBytes(stackRef);
            return new byte[]
            {
                0xFF, 0xD2, 
                0x56, 
                0x57, 
                0x53,
                0x52, 
                0xBF, b[0], b[1], b[2], b[3], // mov edi,REFERENCE_STACK
                0x8B, 0x3F, 
                0x8B, 0x74, 0x24, 0x14,  // HANDLE ProcessHandle
                0x89, 0x37, 
                0x8B, 0x74, 0x24, 0x18,  // PVOID BaseAddress
                0x89, 0x77, 0x04,
                0x8B, 0x74, 0x24, 0x1C,  // PVOID MemoryInformationClass
                0x89, 0x77, 0x08, 
                0x8B, 0x74, 0x24, 0x20,  // PVOID MemoryInformation
                0x89, 0x77, 0x0C,
                0x8B, 0x74, 0x24, 0x24,  // SIZE_T MemoryInformationLength
                0x89, 0x77, 0x10, 
                0xBF, b[0], b[1], b[2], b[3], // mov edi,REFERENCE_STACK
                0x83, 0x7C, 0x24, 0x18, 0x00, // cmp dword ptr [esp+18],00
                0x77, 0x07,
                0xC7, 0x47, 0x04, 0x00, 0x00, 0x00, 0x00, // mov [edi+04],00000000
                0x8B, 0x74, 0x24, 0x20,                   // esi = (PVOID) MemoryInformation
                0x81, 0x7E, 0x10, 0x00, 0x10, 0x00, 0x00, // cmp [esi+10],00001000  // MEM_COMMIT
                0x74, 0x19,
                0x81, 0x7E, 0x10, 0x00, 0x20, 0x00, 0x00, // cmp [esi+10],00002000  // MEM_RESERVE
                0x74, 0x10, 
                0x81, 0x7E, 0x10, 0x00, 0x30, 0x00, 0x00, // cmp [esi+10],00003000  // (MEM_COMMIT | MEM_RESERVE)
                0x74, 0x07,
                0x5A, 
                0x5B, 
                0x5F, 
                0x5E, 
                0xC2, 0x18, 0x00, 
                0x90, 0x90, 0x90, 0x90,
                0x90, 0x90,
                0x90, 0x90, 0x90, 0x90,
                0x90, 0x90,
                0x83, 0x7E, 0x14, 0x40, // cmp dword ptr [esi+14],40 { 64 } (PAGE_EXECUTE_READWRITE)
                0x74, 0x07, 
                0x5A, 
                0x5B, 
                0x5F,
                0x5E, 
                0xC2, 0x18, 0x00,
                0x8B, 0x74, 0x24, 0x20, 
                // mov dword ptr[esi + 0xC], 0x2000000; // Modify the page size (skip the whole thing)
                0xC7, 0x46, 0x08, 0x01, 0x00, 0x00, 0x00, // mov [esi+08],00000001  // Modify the base AllocationProtect
                0xC7, 0x46, 0x10, 0x00, 0x00, 0x01, 0x00, // mov [esi+10],00010000  // Modify the page state
                0xC7, 0x46, 0x14, 0x01, 0x00, 0x00, 0x00, // mov [esi+14],00000001  // Modify the page protection
                0x8B, 0x74, 0x24, 0x18,
                0xBF, b[0], b[1], b[2], b[3], // mov edi,REFERENCE_STACK
                0x83, 0x7F, 0x04, 0x20,
                0x77, 0x0E, 
                0x83, 0x47, 0x04, 0x04,
                0x8B, 0xDF, 
                0x03, 0x5F, 0x04, 
                0x83, 0xC3, 0x04,
                0x89, 0x33,
                0x5A, 
                0x5B,
                0x5F,
                0x5E, 
                0xC2, 0x18, 0x00
            };
        }

        public static bool Bypass(ProcInfo pinfo)
        {
            var caveStart = Imports.GetProcAddress(Imports.GetModuleHandle("combase.dll"), "ObjectStublessClient3");

            if (pinfo.getPage(caveStart).Protect == Imports.PAGE_EXECUTE_READWRITE)
            {
                return true;
            }

            setCave(pinfo, caveStart, caveStart + 0x800);

            var ntQueryVirtualMemory = Imports.GetProcAddress(Imports.GetModuleHandle("ntdll.dll"), "NtQueryVirtualMemory");
            var hookLocation1 = ntQueryVirtualMemory + 0xA;

            var fnLocation = silentAlloc(pinfo, 0x400, Imports.PAGE_EXECUTE_READWRITE);
            var dataLocation = fnLocation + 0x300;
            var watchLocation = dataLocation + 0;

            pinfo.writeInt32(watchLocation + 0, watchLocation - 0x20); // place to write registers for debugging
            pinfo.writeInt32(watchLocation + 4, 0);
            //util.writeInt(dataLocation + 4, 0);// util.getSection(".vmp1").end);
            //util.writeInt(dataLocation + 8, 0x7FFFFFFF);

            var payload = makePayload(watchLocation);
            pinfo.writeBytes(fnLocation, payload, payload.Length);

            pinfo.placeJmp(hookLocation1, fnLocation);//, 5);

            foreach (var rbxWow64 in getRbxWow64Clone(pinfo))
            {
                //MessageBox.Show("Found rbxWow64 clone: " + rbxWow64.ToString("X8"));
                var rbxQueryVirtualMemoryStart = rbxWow64 + 0x50;

                pinfo.placeJmp(rbxQueryVirtualMemoryStart, ntQueryVirtualMemory);
            }

            // IF YOU WANT TO WATCH WHAT YARA IS READING EVERY X SECONDS
            // UNCOMMENT THE FOLLOWING:
            /*
            watchLocation += 8; // we start reading output here
            //MessageBox.Show("WATCH location: " + watchLocation.ToString("X8"));

            while (true)
            {
                var msg = "";

                List<uint> found = new List<uint>();

                for (int i = 0; i < 0x20; i += 4)
                {
                    var spoofed = pinfo.readUInt(watchLocation + i);
                    if (spoofed == 0) break;

                    if (!found.Contains(spoofed))
                    {
                        found.Add(spoofed);
                        //yaraStarted = true;
                        msg += ("Spoofed QUERY location [" + spoofed.ToString("X8") + "]\n");
                    }
                }

                if (msg.Length > 0)
                {
                    MessageBox.Show(msg);
                }

                System.Threading.Thread.Sleep(1000);
            }
            // */

            return true;
        }
    }
}
