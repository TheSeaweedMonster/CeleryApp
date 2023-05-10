#region Includes
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CeleryApp.Misc;
using CeleryApp.Properties;
using Dragablz;
using Microsoft.Win32;
using static CeleryApp.Misc.Miscellaneous;
using EyeStepPackage;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.IO.Compression;
using System.Web.UI.WebControls;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using WK.Libraries.BetterFolderBrowserNS.Helpers;
using System.Web.Configuration;

#endregion

namespace CeleryApp
{
    public partial class MainWindow : Window //Hiro Is Cute, rexi sucks at driving, seizure likes powerpoint slideshow animations AND CHRIS IS A FUCKING RETARD AT ANDROID EDITOR -Rexi, YOU DONT EVEN KNOW HOW ITS FUCKIMG CALLED YOU RETARD -chr1s <3
    { // I love making checks via bools, for example; usefulbutueslessbool! -rexi
        DispatcherTimer dispatcher = new DispatcherTimer();
        int TabCount = 0;

        bool NotifState;
        bool NotifAlready;

        bool InvisState;
        bool MiniBarState = false;

        bool AutoAttaching = false;
        bool CanAutoAttach = true;
        int AdWatchDelay = 0;

        int autoInjectDelay = 0;
        string userKey = "";

        Animation Animate = new Animation();
        TabItem TabSettings = new TabItem();
        WebViewA Browser;

        public MainWindow()
        {
            Directory.CreateDirectory("scripts");
            InitializeComponent();
            NewTab("Main Tab");
            watch();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ExecutorCheck_Click(object sender, RoutedEventArgs e)
        {
            if (MiniBarState == false)
            {
                SimpleMoveAnimation(MainButtons_Copy, new Thickness(0, 47, 2.4, 0), new Thickness(0, 47, -257, 0));
                SlideBar.Content = "󱦱"; // CLOSE
                MiniBarState = true;
            }
            else if (MiniBarState == true)
            {
                SimpleMoveAnimation(MainButtons_Copy, new Thickness(0, 47, -257, 0), new Thickness(0, 47, 2.4, 0));
                SlideBar.Content = "󱦰"; // OPEN
                MiniBarState = false;
            }

            //SimpleMoveAnimation(MainButtons_Copy, new Thickness(458, 40, 10, 0), new Thickness(688, 41, -220, 0));
        }

        private async void ExecutorCheck_Checked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)TryFindResource("Storyboard1")).Begin();
            await Task.Delay(370);
            TabControlShit.Visibility = Visibility.Visible;
            Browser.Visibility = Visibility.Visible;
            MainButtons_Copy.Visibility = Visibility.Visible;
            mainTreeView.Visibility = Visibility.Visible;
            G1.Visibility = Visibility.Visible;
            InvisState = false;
            Browser.Visibility = Visibility.Visible;

            SettingsWindow.Visibility = Visibility.Hidden;
            GameHubWindow.Visibility = Visibility.Hidden;

        }

        private void ExecutorCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)TryFindResource("Storyboard3")).Begin();

            CloseOutput();
            Browser.Visibility = Visibility.Hidden;
            TabControlShit.Visibility = Visibility.Hidden; // 
            mainTreeView.Visibility = Visibility.Hidden;   //
            G1.Visibility = Visibility.Hidden;             //
            MainButtons_Copy.Visibility = Visibility.Hidden;
            Browser.Visibility = Visibility.Hidden;

            SettingsWindow.Visibility = Visibility.Visible;
            GameHubWindow.Visibility = Visibility.Visible;

        }

        private void GameHubCheck_Checked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)TryFindResource("Storyboard1")).Begin();
            SimpleMoveAnimation(GameHubWindow, new Thickness(10, Height, -10, -Height), new Thickness(4, 4, 4, -80));

            CloseOutput();
            TabControlShit.Visibility = Visibility.Hidden;
            Browser.Visibility = Visibility.Hidden;
            mainTreeView.Visibility = Visibility.Hidden;
            MainButtons_Copy.Visibility = Visibility.Hidden;
            G1.Visibility = Visibility.Hidden;
            Browser.Visibility = Visibility.Hidden;
            InvisState = true;

            SettingsWindow.Visibility = Visibility.Hidden;
            GameHubWindow.Visibility = Visibility.Visible;
            /* if (InvisState == true)
             {
                 return;
             }
             else
             {
                 //TabControlShit.Visibility = Visibility.Hidden;
                 Browser.Visibility = Visibility.Hidden;
                 InvisState = true;
             }*/
        }

        private async void GameHubCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)TryFindResource("Storyboard3")).Begin();
            SimpleMoveAnimation(GameHubWindow, new Thickness(4, 4, 4, -80), new Thickness(10, (Height + 50), -10, -(Height + 50)));
            /*if (InvisState == false)
            {
                await Task.Delay(770);*/
            GameHubWindow.Visibility = Visibility.Hidden;

            Browser.Visibility = Visibility.Visible;
            /*    Fade(TabControlShit);
                Fade(Browser);
            }
            else
            {
                Browser.Visibility = Visibility.Hidden;
                InvisState = true;
            }*/
        }

        private void SettingsCheck_Checked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)TryFindResource("Storyboard1")).Begin();
            SimpleMoveAnimation(SettingsWindow, new Thickness(-200, 4, 200, 0), new Thickness(4, 4, 4, 4));

            CloseOutput();
            TabControlShit.Visibility = Visibility.Hidden;
            MainButtons_Copy.Visibility = Visibility.Hidden;
            Browser.Visibility = Visibility.Hidden;
            mainTreeView.Visibility = Visibility.Hidden;
            G1.Visibility = Visibility.Hidden;
            Browser.Visibility = Visibility.Hidden;
            InvisState = true;

            SettingsWindow.Visibility = Visibility.Visible;
            GameHubWindow.Visibility = Visibility.Hidden;
            /*
            if (InvisState == true)
            {
                return;
            }
            else
            {
                //TabControlShit.Visibility = Visibility.Hidden;
                Browser.Visibility = Visibility.Hidden;
                InvisState = true;
            }*/
        }

        private async void SettingsCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Storyboard)TryFindResource("Storyboard3")).Begin();
            SimpleMoveAnimation(SettingsWindow, new Thickness(4, 4, 4, 4), new Thickness((Width + 50), 4, -(Width + 50), 0));
            // if (InvisState == false)
            //{
            //    await Task.Delay(770);

            SettingsWindow.Visibility = Visibility.Hidden; // #

            Browser.Visibility = Visibility.Visible;
            /*    Fade(TabControlShit);
                Fade(Browser);
            }
            else
            {
                Browser.Visibility = Visibility.Hidden;
                InvisState = true;
            }*/
        }

        // Redo celery workspace
        // Write the CeleryApp directory to a file in TEMP
        // which can be retrieved in the DLL.

        public void askCeleryKey(bool x = true)
        {
            /*if (x)
            {
                var result = MessageBox.Show("Do you already have a key?", "Celery Key", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.No)
                {
                    Process.Start("http://celerystick.xyz/Celery/genKey.php");
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            else
            {
                var result = MessageBox.Show("Do you already have a key?", "New key required", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.No)
                {
                    Process.Start("http://celerystick.xyz/Celery/genKey.php");
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            userKey = Microsoft.VisualBasic.Interaction.InputBox("Please enter your celery key:", "Celery Key").Replace(" ", "");

            try
            {
                File.WriteAllText(System.IO.Path.GetTempPath() + "celery\\celerykey.txt", userKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();

            DispatcherTimer update2Timer = new DispatcherTimer();
            update2Timer.Tick += new EventHandler(update2Timer_Tick);
            update2Timer.Interval = new TimeSpan(0, 1, 0); // 1 minute timer
            update2Timer.Start();

            DispatcherTimer updateTimer = new DispatcherTimer();
            updateTimer.Tick += new EventHandler(updateTimer_Tick);
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            updateTimer.Start();

            OutputWindow.Visibility = Visibility.Hidden;
            TabControlShit.Visibility = Visibility.Hidden;
            Browser.Visibility = Visibility.Hidden;
            G1.Visibility = Visibility.Hidden;
            MainButtons_Copy.Visibility = Visibility.Hidden;

            WelcomeLabel_Copy.Content = Environment.UserName;
            SimpleMoveAnimation(HomeWindow, new Thickness(-701, 40, 0, -4), new Thickness(0, 41, 0, 0));


            new Thread(o =>
            {
                //Thread.Sleep(5000);

                if (!Directory.Exists(System.IO.Path.GetTempPath() + "celery"))
                {
                    try { Directory.CreateDirectory(System.IO.Path.GetTempPath() + "celery"); }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An exception occurred. Please restart Celery. Error message: " + ex.Message);
                    }
                }

                if (File.Exists(System.IO.Path.GetTempPath() + "celery\\celeryhome.txt"))
                {
                    try {
                        File.WriteAllText(System.IO.Path.GetTempPath() + "celery\\celeryhome.txt", AppDomain.CurrentDomain.BaseDirectory + "dll\\");
                    }  catch (Exception ex)
                    {  }
                }
                else
                {
                    FileHelp.checkCreateFile(System.IO.Path.GetTempPath() + "celery\\celeryhome.txt", AppDomain.CurrentDomain.BaseDirectory + "dll\\");
                }

                // Check if the user has a previous download of celery before
                // updating celerydir.txt file
                if (Directory.Exists(System.IO.Path.GetTempPath() + "celery"))
                {
                    // Celery stores its current directory to this file on startup
                    // (we haven't written to this yet, in case this is an updated celery exe)
                    if (File.Exists(System.IO.Path.GetTempPath() + "celery\\celerydir.txt"))
                    {
                        try {
                            // Read the file contents for the current (or outdated) Celery directory
                            var dirPath = File.ReadAllText(System.IO.Path.GetTempPath() + "celery\\celerydir.txt");

                            // Check whether this is a DIFFERENT Celery directory than the one we are in right now
                            if (dirPath.Length > 0 && dirPath != AppDomain.CurrentDomain.BaseDirectory)
                            {
                                // Remove the trailing "\" character for Directory.Exists check
                                dirPath = dirPath.Substring(0, dirPath.Length - 1);
                                if (Directory.Exists(dirPath))
                                {
                                    // DOUBLE-CHECK the appversion.txt, ensure that it is an outdated celery
                                    /*if (File.Exists(dirPath + "appversion.txt"))
                                    {
                                        var content = File.ReadAllText(dirPath + "appversion.txt");
                                        if (content == File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "appversion.txt"))
                                        {*/
                                            // Give the user a request to delete the old Celery folder(s)
                                            if (MessageBox.Show("Delete old directory ('" + dirPath + "')?", "Update Finished") == MessageBoxResult.OK)
                                            {
                                                Directory.Delete(dirPath);
                                                CreateNotification("Removed previous celery directory");
                                            }
                                    //    }
                                    //}
                                }
                            }
                        } catch (Exception ex) {
                            MessageBox.Show("An exception occurred. Unable to finish updating. Message: " + ex.Message);
                        }
                    }
                }

                // Ok. Outdated Celery check is finished. Update celerydir.txt
                FileHelp.checkCreateFile(System.IO.Path.GetTempPath() + "celery\\celerydir.txt", AppDomain.CurrentDomain.BaseDirectory);
                if (File.Exists(System.IO.Path.GetTempPath() + "celery\\celerydir.txt"))
                {
                    try
                    {
                        // remember, AppDomain.CurrentDomain.BaseDirectory has a trailing "\" character at the end.
                        // we remove this when we call Directory.Exists
                        File.WriteAllText(System.IO.Path.GetTempPath() + "celery\\celerydir.txt", AppDomain.CurrentDomain.BaseDirectory);
                    }
                    catch (Exception ex)
                    {  }
                }

                FileHelp.checkCreateFile(AppDomain.CurrentDomain.BaseDirectory + "dll/uwpversion.txt", "0.100");
                FileHelp.checkCreateFile(AppDomain.CurrentDomain.BaseDirectory + "appversion.txt", "0.100");
                checkUpdates();
            }).Start();
        }

        public string HttpReadPlainText(string uri)
        {
            HttpWebRequest request;
            try {
                request = (HttpWebRequest)WebRequest.Create(uri);
            }
            catch (System.NotSupportedException ex) {
                System.Console.Out.WriteLine("NotSupportedException occurred when trying to initiate WebRequest for " + uri);
                return "";
            }
            catch (System.Security.SecurityException ex) {
                System.Console.Out.WriteLine("SecurityException occurred when trying to initiate WebRequest for " + uri);
                return "";
            }
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            /*request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Sec-Fetch-Dest", "document");
            request.Headers.Add("Sec-Fetch-Mode", "navigate");
            request.Headers.Add("Sec-Fetch-Site", "none");
            request.Headers.Add("Sec-Fetch-User", "?1");
            request.UserAgent = "Mozilla/5.0";*/
            HttpWebResponse response;
            try {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (System.Net.WebException ex) {
                System.Console.Out.WriteLine("WebException occurred when trying to get a response from server " + uri);
                //CreateNotification("WebException occured...");
                //MessageBox.Show(ex.Message);
                return "";
            }
            catch (System.Net.ProtocolViolationException ex) {
                System.Console.Out.WriteLine("ProtocolViolationException occurred when trying to get a response from server " + uri);
                return "";
            }
            catch (System.InvalidOperationException ex) {
                System.Console.Out.WriteLine("InvalidOperationException occurred when trying to get a response from server " + uri);
                return "";
            }
            catch (System.NotSupportedException ex) {
                System.Console.Out.WriteLine("NotSupportedException occurred when trying to get a response from server " + uri);
                return "";
            }
            Stream stream = null;
            try {
                stream = response.GetResponseStream();
            }
            catch (System.Net.ProtocolViolationException ex) {
                System.Console.Out.WriteLine("ProtocolViolationException occurred when trying to open response stream, server " + uri);
                return "";
            }
            catch (System.ObjectDisposedException ex) {
                System.Console.Out.WriteLine("ObjectDisposedException occurred when trying to open response stream, server " + uri);
                return "";
            }
            if (stream == null)
                return "";
            StreamReader reader = new StreamReader(stream);
            string data = "";
            try {
                data = reader.ReadToEnd();
            }
            catch (System.OutOfMemoryException ex)  {
                System.Console.Out.WriteLine("OutOfMemoryException occurred when trying to read response stream, server " + uri);
                return "";
            }
            catch (System.IO.IOException ex) {
                System.Console.Out.WriteLine("IOException occurred when trying to read response stream, server " + uri);
                return "";
            }
            return data;
        }

        public byte[] HttpReadBytes(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                List<byte> bytes = new List<byte>();

                while (true)
                {
                    var next = stream.ReadByte();
                    if (next == -1) break;

                    bytes.Add((byte)next);
                }

                return bytes.ToArray();
            }
        }

        private void update2Timer_Tick(object sender, EventArgs e)
        {
            checkUpdates();
        }

        public class MouseOperations
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool SetForegroundWindow(uint hWnd);
            
            [DllImport("user32.dll")]
            private static extern uint GetForegroundWindow();

            [Flags]
            public enum MouseEventFlags
            {
                LeftDown = 0x00000002,
                LeftUp = 0x00000004,
                MiddleDown = 0x00000020,
                MiddleUp = 0x00000040,
                Move = 0x00000001,
                Absolute = 0x00008000,
                RightDown = 0x00000008,
                RightUp = 0x00000010
            }

            [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool SetCursorPos(int x, int y);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool GetCursorPos(out MousePoint lpMousePoint);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
            //Mouse actions
            private const int MOUSEEVENTF_LEFTDOWN = 0x02;
            private const int MOUSEEVENTF_LEFTUP = 0x04;
            private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
            private const int MOUSEEVENTF_RIGHTUP = 0x10;

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool GetWindowRect(uint hWnd, out RECT lpRect);

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;        // x position of upper-left corner
                public int Top;         // y position of upper-left corner
                public int Right;       // x position of lower-right corner
                public int Bottom;      // y position of lower-right corner
            }

            public static void SetCursorPosition(int x, int y)
            {
                SetCursorPos(x, y);
            }

            public static void SetCursorPosition(MousePoint point)
            {
                SetCursorPos(point.X, point.Y);
            }

            public static MousePoint GetCursorPosition()
            {
                MousePoint currentMousePoint;
                var gotPoint = GetCursorPos(out currentMousePoint);
                if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
                return currentMousePoint;
            }

            public static void MouseEvent(MouseEventFlags value)
            {
                MousePoint position = GetCursorPosition();

                mouse_event
                    ((uint)value,
                     (uint)position.X,
                     (uint)position.Y,
                     0,
                     0);
            }

            public static void doMouse1Click()
            {
                //Call the imported function with the cursor's current position
                MousePoint position = GetCursorPosition();
                uint X = (uint)position.X;
                uint Y = (uint)position.Y;
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            }

            public static void doMouse2Click()
            {
                //Call the imported function with the cursor's current position
                MousePoint position = GetCursorPosition();
                uint X = (uint)position.X;
                uint Y = (uint)position.Y;
                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
            }

            public static void doMouse1Down()
            {
                //Call the imported function with the cursor's current position
                MousePoint position = GetCursorPosition();
                uint X = (uint)position.X;
                uint Y = (uint)position.Y;
                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            }

            public static void doMouse1Up()
            {
                //Call the imported function with the cursor's current position
                MousePoint position = GetCursorPosition();
                uint X = (uint)position.X;
                uint Y = (uint)position.Y;
                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            }

            public static void doMouse2Down()
            {
                //Call the imported function with the cursor's current position
                MousePoint position = GetCursorPosition();
                uint X = (uint)position.X;
                uint Y = (uint)position.Y;
                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, X, Y, 0, 0);
            }

            public static void doMouse2Up()
            {
                //Call the imported function with the cursor's current position
                MousePoint position = GetCursorPosition();
                uint X = (uint)position.X;
                uint Y = (uint)position.Y;
                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    mouse_event(MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
            }

            public static void mouseMoveRel(int x, int y)
            {
                RECT rc;
                GetWindowRect((uint)Imports.FindWindow(null, "Roblox"), out rc);
                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    SetCursorPos(rc.Left + x, rc.Top + y);
            }

            public static void mouseMoveAbs(int x, int y)
            {
                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    SetCursorPos(x, y);
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MousePoint
            {
                public int X;
                public int Y;

                public MousePoint(int x, int y)
                {
                    X = x;
                    Y = y;
                }
            }
        }

        public static class KeyOperations
        {
            /// <summary>
            /// Synthesizes keystrokes corresponding to the specified Unicode string,
            /// sending them to the currently active window.
            /// </summary>
            /// <param name="s">The string to send.</param>
            public static void SendString(string s)
            {
                // Construct list of inputs in order to send them through a single SendInput call at the end.
                List<INPUT> inputs = new List<INPUT>();

                // Loop through each Unicode character in the string.
                foreach (char c in s)
                {
                    // First send a key down, then a key up.
                    foreach (bool keyUp in new bool[] { false, true })
                    {
                        // INPUT is a multi-purpose structure which can be used 
                        // for synthesizing keystrokes, mouse motions, and button clicks.
                        INPUT input = new INPUT
                        {
                            // Need a keyboard event.
                            type = INPUT_KEYBOARD,
                            u = new InputUnion
                            {
                                // KEYBDINPUT will contain all the information for a single keyboard event
                                // (more precisely, for a single key-down or key-up).
                                ki = new KEYBDINPUT
                                {
                                    // Virtual-key code must be 0 since we are sending Unicode characters.
                                    wVk = 0,

                                    // The Unicode character to be sent.
                                    wScan = c,

                                    // Indicate that we are sending a Unicode character.
                                    // Also indicate key-up on the second iteration.
                                    dwFlags = KEYEVENTF_UNICODE | (keyUp ? KEYEVENTF_KEYUP : 0),

                                    dwExtraInfo = GetMessageExtraInfo(),
                                }
                            }
                        };

                        // Add to the list (to be sent later).
                        inputs.Add(input);
                    }
                }

                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
            }
            
            public static void pressKey(uint c)
            {
                // Construct list of inputs in order to send them through a single SendInput call at the end.
                List<INPUT> inputs = new List<INPUT>();

                // INPUT is a multi-purpose structure which can be used 
                // for synthesizing keystrokes, mouse motions, and button clicks.
                INPUT input = new INPUT
                {
                    // Need a keyboard event.
                    type = INPUT_KEYBOARD,
                    u = new InputUnion
                    {
                        // KEYBDINPUT will contain all the information for a single keyboard event
                        // (more precisely, for a single key-down or key-up).
                        ki = new KEYBDINPUT
                        {
                            // Virtual-key code must be 0 since we are sending Unicode characters.
                            wVk = 0,
                            wScan = (ushort)c, // The Unicode character to be sent.
                            dwFlags = KEYEVENTF_UNICODE,
                            dwExtraInfo = GetMessageExtraInfo(),
                        }
                    }
                };

                inputs.Add(input);

                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
            }
            
            public static void releaseKey(uint c)
            {
                // Construct list of inputs in order to send them through a single SendInput call at the end.
                List<INPUT> inputs = new List<INPUT>();

                // INPUT is a multi-purpose structure which can be used 
                // for synthesizing keystrokes, mouse motions, and button clicks.
                INPUT input = new INPUT
                {
                    // Need a keyboard event.
                    type = INPUT_KEYBOARD,
                    u = new InputUnion
                    {
                        // KEYBDINPUT will contain all the information for a single keyboard event
                        // (more precisely, for a single key-down or key-up).
                        ki = new KEYBDINPUT
                        {
                            // Virtual-key code must be 0 since we are sending Unicode characters.
                            wVk = 0,
                            wScan = (ushort)c, // The Unicode character to be sent.
                            dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP,
                            dwExtraInfo = GetMessageExtraInfo(),
                        }
                    }
                };

                inputs.Add(input);
                
                if (GetForegroundWindow() == (uint)Imports.FindWindow(null, "Roblox"))
                    SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
            }

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool SetForegroundWindow(uint hWnd);


            [DllImport("user32.dll")]
            private static extern uint GetForegroundWindow();

            const int INPUT_MOUSE = 0;
            const int INPUT_KEYBOARD = 1;
            const int INPUT_HARDWARE = 2;
            const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
            const uint KEYEVENTF_KEYUP = 0x0002;
            const uint KEYEVENTF_UNICODE = 0x0004;
            const uint KEYEVENTF_SCANCODE = 0x0008;
            const uint XBUTTON1 = 0x0001;
            const uint XBUTTON2 = 0x0002;
            const uint MOUSEEVENTF_MOVE = 0x0001;
            const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
            const uint MOUSEEVENTF_LEFTUP = 0x0004;
            const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
            const uint MOUSEEVENTF_RIGHTUP = 0x0010;
            const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
            const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
            const uint MOUSEEVENTF_XDOWN = 0x0080;
            const uint MOUSEEVENTF_XUP = 0x0100;
            const uint MOUSEEVENTF_WHEEL = 0x0800;
            const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
            const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

            struct INPUT
            {
                public int type;
                public InputUnion u;
            }

            [StructLayout(LayoutKind.Explicit)]
            struct InputUnion
            {
                [FieldOffset(0)]
                public MOUSEINPUT mi;
                [FieldOffset(0)]
                public KEYBDINPUT ki;
                [FieldOffset(0)]
                public HARDWAREINPUT hi;
            }

            [StructLayout(LayoutKind.Sequential)]
            struct MOUSEINPUT
            {
                public int dx;
                public int dy;
                public uint mouseData;
                public uint dwFlags;
                public uint time;
                public IntPtr dwExtraInfo;
            }

            [StructLayout(LayoutKind.Sequential)]
            struct KEYBDINPUT
            {
                /*Virtual Key code.  Must be from 1-254.  If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0.*/
                public ushort wVk;
                /*A hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application.*/
                public ushort wScan;
                /*Specifies various aspects of a keystroke.  See the KEYEVENTF_ constants for more information.*/
                public uint dwFlags;
                /*The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.*/
                public uint time;
                /*An additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.*/
                public IntPtr dwExtraInfo;
            }

            [StructLayout(LayoutKind.Sequential)]
            struct HARDWAREINPUT
            {
                public uint uMsg;
                public ushort wParamL;
                public ushort wParamH;
            }

            [DllImport("user32.dll")]
            static extern IntPtr GetMessageExtraInfo();

            [DllImport("user32.dll", SetLastError = true)]
            static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            /*
             Handle synchronous events remotely, by listening to signals from the DLL.
            Events include: mouse api functions, file IO, clipboard IO, keyboard api functions, etc.

            This bypasses process-restriction security by using this UI
            to handle necessary windows calls

            Information is written to and read from User32.DrawIcon and User32.DrawIconEx.
            This is because, as I've found to be the case, these functions
            are never used even from the onset of the roblox app.
            It's just extra empty storage that can be easily located
            from both the UI (here) and the DLL.

             */
            foreach (var pinfo in Injector.MsStorePlayer.getInjectedProcesses())
            {
                var functionPtr = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIconEx");


                // This will enable output on the UI (if enabled in the DLL).
                // My DLL relays signals to a location each time an output
                // message is received via LogService.
                // This is not synchronous, but asynchronous.
                // It may miss a few outputs, so it's not meant for
                // high-speed printing or accurate debugging.
                // It's just for viewing prints/warns/errors here and there
                var type = pinfo.readInt32(functionPtr + 0x20);
                var length = pinfo.readInt32(functionPtr + 0x28);
                if (type == 1 || type == 2 || type == 3 || type == 4) // output type that was sent
                {
                    var ptr = pinfo.readInt32(functionPtr + 0x24);
                    var str = pinfo.readString(ptr, length);

                    AddOutput(str, (OutputType)(type - 1));

                    //var oldProtect2 = pinfo.setPageProtect(functionPtr, 0x40, Imports.PAGE_READWRITE);
                    pinfo.writeInt32(functionPtr + 0x20, 0);
                    //ppinfo.setPageProtect(functionPtr, 0x40, oldProtect2);
                }


                /*
                 Yeah, I know, this is bad, handling something that requires
                 high speed in a super slow remote-based system.
                 Well it works, until I bypass the security checks for
                 windows10universal.
                 */
                var mouseDataStart = functionPtr + (10 * sizeof(int));
                var mouseTransmitType = pinfo.readInt32(mouseDataStart);
                if (mouseTransmitType == 1)
                {
                    MouseOperations.doMouse1Down();
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 2)
                {
                    MouseOperations.doMouse1Up();
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 3)
                {
                    MouseOperations.doMouse1Click();
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 4)
                {
                    MouseOperations.doMouse2Down();
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 5)
                {
                    MouseOperations.doMouse2Up();
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 6)
                {
                    MouseOperations.doMouse2Click();
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 7)
                {
                    MouseOperations.mouseMoveRel(pinfo.readInt32(mouseDataStart + 4), pinfo.readInt32(mouseDataStart + 8));
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 8)
                {
                    MouseOperations.mouseMoveAbs(pinfo.readInt32(mouseDataStart + 4), pinfo.readInt32(mouseDataStart + 8));
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 9) // pressKey
                {
                    KeyOperations.pressKey(pinfo.readUInt32(mouseDataStart + 4));
                    pinfo.writeUInt32(mouseDataStart, 0);
                }
                else if (mouseTransmitType == 10) // releaseKey
                {
                    KeyOperations.releaseKey(pinfo.readUInt32(mouseDataStart + 4));
                    pinfo.writeUInt32(mouseDataStart, 0);
                }


                var dataStart = functionPtr + (15 * sizeof(int));
                uint oldProtect = 0;

                var transmitType = pinfo.readInt32(dataStart);
                if (transmitType == 1) // PRINT_CONSOLE (string data)
                {
                    var printSize = pinfo.readInt32(dataStart + 4);
                    var printPointer = pinfo.readInt32(dataStart + 8);
                    if (printSize > 0)
                    {
                        var str = pinfo.readString(printPointer, printSize);
                        Console.Out.WriteLine(str);
                    }
                }
                else if (transmitType == 2) // PRINT_CONSOLEW (wstring data)
                {
                    var printSize = pinfo.readInt32(dataStart + 4);
                    var printPointer = pinfo.readInt32(dataStart + 8);
                    if (printSize > 0)
                    {
                        var str = pinfo.readWString(printPointer, printSize);
                        Console.Out.WriteLine(str);
                    }
                }
                else if (transmitType == 3) // string READ_FILE (wstring filePath)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    if (filePathSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Reading file path: " + filePath);

                        if (File.Exists(filePath))
                        {
                            var contents = File.ReadAllText(filePath);
                            var contentsPointer = Imports.VirtualAllocEx(pinfo.handle, 0, contents.Length + 4, Imports.MEM_COMMIT | Imports.MEM_RESERVE, Imports.PAGE_READWRITE);
                            
                            pinfo.writeString(contentsPointer, contents);
                            pinfo.writeInt32(dataStart + 12, contents.Length);
                            pinfo.writeInt32(dataStart + 16, contentsPointer);
                        }
                        else
                        {
                            pinfo.writeInt32(dataStart + 12, 0);
                            pinfo.writeInt32(dataStart + 16, 0);
                        }
                    }
                }
                else if (transmitType == 4) // wstring READ_FILE (wstring filePath)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    if (filePathSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        Console.Out.WriteLine("Reading file path: " + filePath);

                        if (File.Exists(filePath))
                        {
                            Console.Out.WriteLine("File eixsts");
                            var contents = File.ReadAllText(filePath);
                            var contentsPointer = Imports.VirtualAllocEx(pinfo.handle, 0, (contents.Length * 2) + 4, Imports.MEM_COMMIT | Imports.MEM_RESERVE, Imports.PAGE_READWRITE);

                            pinfo.writeWString(contentsPointer, contents);
                            pinfo.writeInt32(dataStart + 12, contents.Length);
                            pinfo.writeInt32(dataStart + 16, contentsPointer);
                        }
                        else
                        {
                            pinfo.writeInt32(dataStart + 12, 0);
                            pinfo.writeInt32(dataStart + 16, 0);
                        }
                    }
                }
                else if (transmitType == 5) // WRITE_FILE (wstring filePath, string data)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    var dataSize = pinfo.readInt32(dataStart + 12);
                    var dataPointer = pinfo.readInt32(dataStart + 16);
                    if (filePathSize > 0 && dataSize >= 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Writing file path: " + filePath);

                        var data = pinfo.readBytes(dataPointer, dataSize);
                        //var data = pinfo.readString(dataPointer, dataSize);
                        //Console.Out.WriteLine(data);

                        FileHelp.checkCreateFile(filePath);
                        try { File.WriteAllBytes(filePath, data); } catch (Exception ex) { }
                    }
                }
                else if (transmitType == 6) // WRITE_FILEW (wstring filePath, wstring data)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    var dataSize = pinfo.readInt32(dataStart + 12);
                    var dataPointer = pinfo.readInt32(dataStart + 16);
                    if (filePathSize > 0 && dataSize >= 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Reading file path: " + filePath);

                        var data = pinfo.readBytes(dataPointer, dataSize * 2);
                        //var data = pinfo.readWString(dataPointer, dataSize);
                        //Console.Out.WriteLine(data);

                        FileHelp.checkCreateFile(filePath);
                        try { File.WriteAllBytes(filePath, data); } catch (Exception ex) { }
                    }
                }
                else if (transmitType == 7) // sendMessageBox (string)
                {
                    var printSize = pinfo.readInt32(dataStart + 4);
                    var printPointer = pinfo.readInt32(dataStart + 8);
                    if (printSize > 0)
                    {
                        var str = pinfo.readString(printPointer, printSize);

                        Imports.MessageBoxA(Imports.FindWindow(null, "Roblox"), str, "[Celery]", 0);
                    }
                }
                else if (transmitType == 8) // sendMessageBoxW (wstring)
                {
                    var printSize = pinfo.readInt32(dataStart + 4);
                    var printPointer = pinfo.readInt32(dataStart + 8);
                    if (printSize > 0)
                    {
                        var str = pinfo.readWString(printPointer, printSize);

                        Imports.MessageBoxW(Imports.FindWindow(null, "Roblox"), str, "", 0);
                    }
                }
                else if (transmitType == 9) // APPEND_FILE (wstring filePath, string data)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    var dataSize = pinfo.readInt32(dataStart + 12);
                    var dataPointer = pinfo.readInt32(dataStart + 16);
                    if (filePathSize > 0 && dataSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Reading file path: " + filePath);

                        var data = pinfo.readString(dataPointer, dataSize);
                        //var data = pinfo.readString(dataPointer, dataSize);
                        //Console.Out.WriteLine(data);

                        FileHelp.checkCreateFile(filePath);
                        try { File.AppendAllText(filePath, data); } catch (Exception ex) { }
                    }
                }
                else if (transmitType == 10) // APPEND_FILEW (wstring filePath, wstring data)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    var dataSize = pinfo.readInt32(dataStart + 12);
                    var dataPointer = pinfo.readInt32(dataStart + 16);
                    if (filePathSize > 0 && dataSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Reading file path: " + filePath);

                        var bytes = pinfo.readBytes(dataPointer, dataSize * 2);
                        //var data = pinfo.readWString(dataPointer, dataSize);
                        //Console.Out.WriteLine(data);
                        string data = "";

                        foreach (byte b in bytes)
                            data += (char)b;

                        FileHelp.checkCreateFile(filePath);
                        try { File.AppendAllText(filePath, data); } catch (Exception ex) { }
                    }
                }
                else if (transmitType == 11) // CREATE_DIRECTORY (wstring filePath, wstring data)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    if (filePathSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Reading file path: " + filePath);

                        try { Directory.CreateDirectory(filePath); } catch (Exception ex) { }
                    }
                }
                else if (transmitType == 12) // DELFILE (wstring filePath)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    if (filePathSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Reading file path: " + filePath);

                        if (File.Exists(filePath))
                            try { File.Delete(filePath); } catch (Exception ex) { }
                    }
                }
                else if (transmitType == 13) // DELFOLDER (wstring filePath)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    if (filePathSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Reading file path: " + filePath);

                        if (Directory.Exists(filePath))
                            try { Directory.Delete(filePath); } catch (Exception ex) { }
                    }
                }
                else if (transmitType == 14) // LISTFILES (wstring filePath)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    if (filePathSize > 0)
                    {
                        var dirPath = pinfo.readWString(filePathPointer, filePathSize);
                        //Console.Out.WriteLine("Reading path: " + dirPath);

                        if (Directory.Exists(dirPath))
                        {
                            string contents = "";

                            foreach (string filePath in System.IO.Directory.GetFiles(dirPath))
                            {
                                var wsStart = filePath.LastIndexOf("dll\\workspace") + 14;
                                if (wsStart < filePath.Length)
                                {
                                    contents += filePath.Substring(wsStart, filePath.Length - wsStart);
                                    contents += "|";
                                }
                            }

                            contents.TrimEnd('|');

                            var contentsPointer = Imports.VirtualAllocEx(pinfo.handle, 0, (contents.Length * 2) + 4, Imports.MEM_COMMIT | Imports.MEM_RESERVE, Imports.PAGE_READWRITE);

                            pinfo.writeWString(contentsPointer, contents);
                            pinfo.writeInt32(dataStart + 12, contents.Length);
                            pinfo.writeInt32(dataStart + 16, contentsPointer);
                        }
                        else
                        {
                            pinfo.writeInt32(dataStart + 12, 0);
                            pinfo.writeInt32(dataStart + 16, 0);
                        }
                    }
                }
                else if (transmitType == 15) // bool isFile (wstring filePath)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    if (filePathSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        pinfo.writeInt32(dataStart + 12, File.Exists(filePath) ? 1 : 0);
                    }
                }
                else if (transmitType == 16) // bool isFolder (wstring filePath)
                {
                    var filePathSize = pinfo.readInt32(dataStart + 4);
                    var filePathPointer = pinfo.readInt32(dataStart + 8);
                    if (filePathSize > 0)
                    {
                        var filePath = pinfo.readWString(filePathPointer, filePathSize);
                        pinfo.writeInt32(dataStart + 12, Directory.Exists(filePath) ? 1 : 0);
                    }
                }
                else if (transmitType == 17) // setclipboard (wstring data)
                {
                    var printSize = pinfo.readInt32(dataStart + 4);
                    var printPointer = pinfo.readInt32(dataStart + 8);
                    if (printSize > 0)
                    {
                        var str = pinfo.readWString(printPointer, printSize);
                        Clipboard.SetText(str);
                    }
                }
                else if (transmitType == 18) // wstring getclipboard
                {
                    var contents = Clipboard.GetText();
                    var contentsPointer = Imports.VirtualAllocEx(pinfo.handle, 0, (contents.Length * 2) + 4, Imports.MEM_COMMIT | Imports.MEM_RESERVE, Imports.PAGE_READWRITE);

                    pinfo.writeWString(contentsPointer, contents);
                    pinfo.writeInt32(dataStart + 4, contents.Length);
                    pinfo.writeInt32(dataStart + 8, contentsPointer);
                }

                if (pinfo.isOpen())
                    pinfo.writeInt32(dataStart, 0);
            }
        }

        public void askUpdateApp()
        {
            if (MessageBoxResult.OK == MessageBox.Show("Celery app has been updated (full restart required...). Press Ok to visit the download page. Download the latest zip and Extract All. NOTE: To keep your auto-run script and other files that you don't want to lose, you will have to transfer them into the new directories.", "Full App Update", MessageBoxButton.OKCancel))
            {
                Process.Start("http://celerystick.xyz/Celery/download.php");
            }
        }

        public static bool incognitoEnabled = false;
        public static bool viewPacketsEnabled = false;
        public static bool experimentalEnabled = false;

        public static void writeAppSettings()
        {
            // This was for Windows Player.
            /*foreach (var pinfo in Injector.WindowsPlayer.getInjectedProcesses())
            {
                var functionPtr = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIconEx");
                var oldProtect = pinfo.setPageProtect(functionPtr, 0x20, Imports.PAGE_READWRITE);
                pinfo.writeInt32(functionPtr + 4, (incognitoEnabled) ? 1 : 0);
                pinfo.writeInt32(functionPtr + 8, (viewPacketsEnabled) ? 1 : 0);
                pinfo.writeInt32(functionPtr + 12, (experimentalEnabled) ? 1 : 0);
                pinfo.setPageProtect(functionPtr, 0x20, oldProtect);
            }*/
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.K))
            {
                askCeleryKey();
            }
        }

        public string getUpdateUrl(string settings, string tag)
        {
            string updateUrl = "";
            var updateUrlStart = settings.IndexOf(tag) + tag.Length + 2; // skip '='
            if (updateUrlStart > 0)
            {
                while (updateUrlStart < settings.Length)
                {
                    if (settings[updateUrlStart] == '\"') break;
                    updateUrl += settings[updateUrlStart++];
                }
            }
            return updateUrl;
        }

        public bool postFileUpdate(string settings, string settingsUpdateUrlTag, string updatedFileName)
        {
            string updateUrl = getUpdateUrl(settings, settingsUpdateUrlTag);
            if (updateUrl.Length != 0)
            {
                if (MessageBox.Show("Update required for Celery file(s). Press Ok to download.", "Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    while (Injector.WindowsPlayer.getInjectedProcesses().Count > 0 || Injector.MsStorePlayer.getInjectedProcesses().Count > 0)
                    {
                        MessageBox.Show("Please close active roblox games before updating");
                        Thread.Sleep(500);
                    }

                    var newBinary = HttpReadBytes(updateUrl);
                    if (newBinary.Length > 0)
                    {
                        FileHelp.checkCreateFile(AppDomain.CurrentDomain.BaseDirectory + "dll/" + updatedFileName);

                        // Try to write contents. If it fails  (e.g. due to permissions), DO NOT overwrite uwp version file yet
                        try { File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "dll\\" + updatedFileName, newBinary); }
                        catch (Exception ex) { return false; }

                        CreateNotification("Update complete!");
                        return true;
                    }
                }
            }

            return false;
        }

        public void checkUpdates()
        {
            var settings = HttpReadPlainText("https://raw.githubusercontent.com/TheSeaweedMonster/Celery/main/settings.txt");
            if (settings.Length == 0)
                return;


            float latestAppVersion, currentAppVersion, latestUwpVersion, currentUwpVersion;
            float.TryParse(settings.Substring(settings.IndexOf("appversion=") + 11, 5), out latestAppVersion);
            float.TryParse(settings.Substring(settings.IndexOf("uwpversion=") + 11, 5), out latestUwpVersion);
            float.TryParse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "appversion.txt"), out currentAppVersion);
            float.TryParse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "dll\\uwpversion.txt"), out currentUwpVersion);

            if (latestAppVersion > currentAppVersion)
            {
                var updateUrl = getUpdateUrl(settings, "appurl");
                if (updateUrl.Length > 0)
                {
                    if (MessageBox.Show("Update required for Celery App. Press Ok to download.", "Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        BetterFolderBrowserDialog openFolder = new BetterFolderBrowserDialog();
                        if (openFolder.ShowDialog())
                        {
                            var extractPath = openFolder.FileName;
                            if (extractPath.Length != 0)
                            {
                                string zipPath = AppDomain.CurrentDomain.BaseDirectory + "download.zip";

                                // Check if the user may have already a pending download but there was an issue
                                if (!File.Exists(zipPath))
                                {
                                    WebClient webClient = new WebClient();
                                    webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                                    webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");

                                    try { webClient.DownloadFileAsync(new Uri(updateUrl), zipPath); } catch (Exception ex) { return; }
                                }

                                /*
                                string startPath = AppDomain.CurrentDomain.BaseDirectory;
                                startPath = startPath.Substring(0, startPath.LastIndexOf("\\"));
                                startPath = startPath.Substring(0, startPath.LastIndexOf("\\") + 1);
                                string extractPath = startPath + "CeleryApp";
                                */
                                try
                                {
                                    // Couldn't create a new directory to extract contents to? Void everything.
                                    try { Directory.CreateDirectory(extractPath); } catch (Exception ex) { return; }
                                    //ZipFile.CreateFromDirectory(startPath, extractPath);
                                    try { ZipFile.ExtractToDirectory(zipPath, extractPath); } catch (Exception ex) { return; }
                                }
                                catch (Exception ex) { return; }

                                CreateNotification("Downloaded CeleryApp to " + extractPath);
                            }
                        }
                    }
                }
                else
                {
                    // Uh oh, my bad
                }
            }

            if (latestUwpVersion > currentUwpVersion)
            {
                if (MessageBox.Show("Update required for celery file(s). Press Ok to download.", "Update", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    if (postFileUpdate(settings, "uwpdllurl", "celeryuwp.bin") && postFileUpdate(settings, "uwpoffurl", "uwpoff.bin") && postFileUpdate(settings, "uwpiniturl", "uwpinit.lua"))
                    {
                        try { File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "dll\\uwpversion.txt", Convert.ToString(latestUwpVersion)); }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        MessageBox.Show("Update failed...Wait 1 minute or restart CeleryApp");
                    }
                }
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Injector.MsStorePlayer.getInjectedProcesses().Count == 0)
            {
                if (Injector.MsStorePlayer.consoleInUse)
                    Injector.MsStorePlayer.hideConsole();
            }

            // Wait a second before making attempts
            if (autoInjectDelay > 10)
                autoInjectDelay = 0;
            else
            {
                if (autoInjectDelay > 0)
                    autoInjectDelay++;
            }

            if (autoInjectDelay == 0)
            {
                if (Settings.Default.Autolaunch)
                {
                    bool injected = false;

                    /*foreach (var pinfo in Util.openProcessesByName("RobloxPlayerBeta.exe"))
                    {
                        if (!Injector.WindowsPlayer.isInjected(pinfo))
                        {
                            var status = Injector.WindowsPlayer.injectMainPlayer(pinfo);
                            if (status == Injector.InjectionStatus.SUCCESS)
                            {
                                //injected = true;
                                CreateNotification("Celery injected");
                                writeAppSettings();
                            }
                            else if (status == Injector.InjectionStatus.ALREADY_INJECTING)
                            {
                                // to-do: idk
                                Thread.Sleep(250);
                            }
                            else if (status == Injector.InjectionStatus.FAILED)
                            {
                                CreateNotification("Injection failed! Unknown error.");
                            }

                            //if (!incognitoEnabled && injected) break;
                        }
                    }*/

                    //if (!incognitoEnabled && injected)
                    //{
                    //    //return;
                    //}
                    //else
                    //{
                        foreach (var pinfo in Util.openProcessesByName("Windows10Universal.exe"))
                        {
                            if (!Injector.MsStorePlayer.isInjected(pinfo))
                            {
                                var status = Injector.MsStorePlayer.injectPlayer(pinfo);
                                if (status == Injector.InjectionStatus.SUCCESS)
                                {
                                    //Injector.MsStorePlayer.sendScript(pinfo, File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "dll/autoexec/autoexec.lua"));
                                    
                                    injected = true;
                                    CreateNotification("Celery injected");
                                }
                                else if (status == Injector.InjectionStatus.ALREADY_INJECTING)
                                {
                                    // to-do: idk
                                    Thread.Sleep(250);
                                }
                                else if (status == Injector.InjectionStatus.FAILED)
                                {
                                    CreateNotification("Injection failed! Unknown error.");
                                }
                                else if (status == Injector.InjectionStatus.FAILED_ADMINISTRATOR_ACCESS)
                                {
                                    CreateNotification("Please run CeleryApp.exe as an administrator");
                                }

                                if (!incognitoEnabled && injected) break;
                            }
                            else
                            {

                            }
                        }
                    //}
                }
            }
        }

        private void button_MouseEnter_1(object sender, MouseEventArgs e)
        {
            ((Storyboard)TryFindResource("Attach")).Begin();
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)TryFindResource("Attach2")).Begin();

        }

        //----------------------------- HOME -----------------------------//
        private async void StartUsingCelery(object sender, RoutedEventArgs e)
        {
            SimpleMoveAnimation(HomeWindow, new Thickness(0, 41, 0, 0), new Thickness(-701, 40, 0, -4));
            G1.Visibility = Visibility.Visible;
            TabControlShit.Visibility = Visibility.Visible;
            await Task.Delay(770);
            Browser.Visibility = Visibility.Visible;
            MainButtons_Copy.Visibility = Visibility.Visible;
            SimpleMoveAnimation(MainButtons_Copy, new Thickness(Width, 47, -Width, 0), new Thickness(0, 47, 10, 0));
            Fade(TabControlShit);
            Fade(Browser);

            //askCeleryKey();
        }

        private void ReportBugs(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.celerystick.xyz/");
        }

        public enum ResizePoints
        {
            TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left
        }

        public Point resizeStart, resizeAmount;
        public bool resizeActive = false;
        public ResizePoints resizePoint;
        //public UserActivityHook actHook;

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        //----------------------------- WINDOW -----------------------------//
        private void Window_MoveFinish(object sender, MouseButtonEventArgs e)
        {
        }

        private void Window_Move(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                base.ResizeMode = ResizeMode.NoResize;
                base.DragMove();
                base.ResizeMode = ResizeMode.CanResizeWithGrip;
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeBtrn(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        //----------------------------- TREEVIEW -----------------------------//
        FileSystemWatcher fs;
        string watchingFolder;
        private void watch()
        {

            mainTreeView.Items.Clear();
            var rootDirectoryInfo = new DirectoryInfo("./scripts");
            mainTreeView.Items.Add(CreateDirectoryNode(rootDirectoryInfo));
            watchingFolder = "./scripts";
            fs = new FileSystemWatcher(watchingFolder, "*.*");

            fs.EnableRaisingEvents = true;
            fs.IncludeSubdirectories = true;

            fs.Created += new FileSystemEventHandler(changed);
            fs.Changed += new FileSystemEventHandler(changed);
            fs.Renamed += new RenamedEventHandler(renamed);
            fs.Deleted += new FileSystemEventHandler(changed);
        }

        private void changed(object source, FileSystemEventArgs e)
        {
            mainTreeView.Dispatcher.Invoke(delegate
            {
                mainTreeView.Items.Clear();
                var rootDirectoryInfo = new DirectoryInfo("./scripts");
                mainTreeView.Items.Add(CreateDirectoryNode(rootDirectoryInfo));
            });
        }

        private void renamed(object source, RenamedEventArgs e)
        {
            mainTreeView.Dispatcher.Invoke(delegate
            {
                mainTreeView.Items.Clear();
                var rootDirectoryInfo = new DirectoryInfo("./scripts");
                mainTreeView.Items.Add(CreateDirectoryNode(rootDirectoryInfo));
            });
        }

        private TreeViewItem GetTreeView(string tag, string text, string imagePath)
        {
            var item = new TreeViewItem();
            item.Foreground = Brushes.Gray;
            item.Tag = tag;
            item.IsExpanded = false;
            //item.Header = text;

            var stack = new StackPanel();
            stack.Orientation = System.Windows.Controls.Orientation.Horizontal;

            var image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(new Uri("pack://application:,,/ScriptList/" + imagePath));
            image.Width = 16;
            image.Height = 16;
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);

            var lbl = new System.Windows.Controls.Label();
            lbl.Content = text;
            lbl.Foreground = Brushes.Gray;

            stack.Children.Add(image);
            stack.Children.Add(lbl);

            item.Header = stack;
            item.ToolTip = imagePath;
            ToolTipService.SetIsEnabled(item, false);
            return item;
        }

        public void ListDirectory(System.Windows.Controls.TreeView treeView, string path)
        {
            treeView.Items.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Items.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private TreeViewItem CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = GetTreeView(directoryInfo.FullName, directoryInfo.Name, "2folder.ico");
            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Items.Add(CreateDirectoryNode(directory));
            }


            foreach (var file in directoryInfo.GetFiles())
            {
                if (file.Extension == ".lua")
                {
                    directoryNode.Items.Add(GetTreeView(file.FullName, file.Name, "lua.png"));
                }
                else if (file.Extension == ".txt")
                {
                    directoryNode.Items.Add(GetTreeView(file.FullName, file.Name, "txt.ico"));
                }
                else
                {
                    directoryNode.Items.Add(GetTreeView(file.FullName, file.Name, "file.ico"));
                }
            }

            return directoryNode;

        }

        private void mainTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (mainTreeView.SelectedItem != null)
                {
                    TreeViewItem item = mainTreeView.SelectedItem as TreeViewItem;
                    string itemName = item.Tag.ToString();

                    if (itemName.EndsWith(".txt") || itemName.EndsWith(".lua") && item.ToolTip.ToString().EndsWith("folder.png") == false)
                    {
                        StreamReader reader = new StreamReader(item.Tag.ToString());
                        Browser.SetText(reader.ReadToEnd());
                    }
                }

            }
            catch (Exception ee)
            {
                NotifState = true;
                CreateNotification(ee.ToString());
                Clipboard.SetText(ee.ToString());
                NotifState = false;
            }
        }

        //----------------------------- EXECUTOR -----------------------------//
        private void Inject_Click(object sender, RoutedEventArgs e)
        {
            // EXE Version of Celery
            /*
            if (Process.GetProcessesByName("Celery").Length == 0)
            {
                Process process = new Process();
                // Configure the process using the StartInfo properties.
                process.StartInfo.FileName = "Celery.exe";
                process.StartInfo.Arguments = "-n";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                process.Start();
            }
            else
            {
                var proc = Process.GetProcessesByName("Celery").First();
                proc.Close();
            }
            */

            bool injected = false;

            // Old code. For multi injecting to Windows Player and
            // Windows10universal according to user preference
            // 
            /*foreach (var pinfo in Util.openProcessesByName("RobloxPlayerBeta.exe"))
            {
                if (!Injector.WindowsPlayer.isInjected(pinfo))
                {
                    var status = Injector.WindowsPlayer.injectMainPlayer(pinfo);
                    if (status == Injector.InjectionStatus.SUCCESS)
                    {
                        injected = true;
                        CreateNotification("Celery injected");
                        writeAppSettings();
                    }
                    else if (status == Injector.InjectionStatus.ALREADY_INJECTING)
                    {
                        // to-do: idk
                        Thread.Sleep(250);
                    }
                    else if (status == Injector.InjectionStatus.FAILED)
                    {
                        CreateNotification("Injection failed! Unknown error.");
                    }

                    if (!incognitoEnabled && injected) break;
                }
            }*/

            if (!incognitoEnabled && injected)
            {
                //return;
            }
            else
            {
                foreach (var pinfo in Util.openProcessesByName("Windows10Universal.exe"))
                {
                    if (!Injector.MsStorePlayer.isInjected(pinfo))
                    {
                        var status = Injector.MsStorePlayer.injectPlayer(pinfo);
                        if (status == Injector.InjectionStatus.SUCCESS)
                        {
                            injected = true;
                            //Injector.MsStorePlayer.sendScript(pinfo, File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "dll/autoexec/autoexec.lua"));

                            CreateNotification("Celery injected");
                        }
                        else if (status == Injector.InjectionStatus.ALREADY_INJECTING)
                        {
                            // to-do: idk
                            Thread.Sleep(250);
                        }
                        else if (status == Injector.InjectionStatus.FAILED)
                        {
                            CreateNotification("Injection failed! Unknown error.");
                        }
                        else if (status == Injector.InjectionStatus.FAILED_ADMINISTRATOR_ACCESS)
                        {
                            CreateNotification("Please run CeleryApp.exe as an administrator");
                        }

                        if (!incognitoEnabled && injected) break;
                    }
                }
            }
            
        }

        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            bool scriptWasSent = false;

            /*foreach (var pinfo in Injector.WindowsPlayer.getInjectedProcesses())
            {
                Injector.WindowsPlayer.sendScript(pinfo, await ((WebViewA)this.TabControlShit.SelectedContent).GetText());
                scriptWasSent = true;
            }*/

            foreach (var pinfo in Injector.MsStorePlayer.getInjectedProcesses())
            {
                Injector.MsStorePlayer.sendScript(pinfo, await ((WebViewA)this.TabControlShit.SelectedContent).GetText());
                scriptWasSent = true;
            }
            
            if (!scriptWasSent)
            {
                CreateNotification("Please attach Celery first");
            }
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|Lua files (*.lua*)|*.lua*";
            if (openFileDialog.ShowDialog() == true)
                Browser.SetText(File.ReadAllText(openFileDialog.FileName));
        }

        private async void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|Lua files (*.lua*)|*.lua*";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, await Browser.GetText());
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Browser.SetText("");
        }

        private void NewTab_Click(object sender, RoutedEventArgs e)
        {
            //TabCount = TabControlShit.Items.Count;
            if (TabCount < 6)
            {
                TabCount++;
                NewTab("New Tab" + TabCount);
            }
            else
            {
                NotifState = true;
                CreateNotification("Exceeded Limit for creating tabs.");
                NotifState = false;
            }
        }

        private void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            TabCount--;
            TabControlShit.Items.Remove(TabControlShit.SelectedItem);
        }

        //----------------------------- TABCONTROL -----------------------------//
        public TabItem NewTab(string title)
        {
            Browser = new WebViewA();
            Browser.UpdateWindowPos();

            var TabHeader = new System.Windows.Controls.TextBox
            {
                // Text
                Text = title,
                IsEnabled = false,
                TextWrapping = TextWrapping.NoWrap,
                IsHitTestVisible = false,

                // Colors
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Foreground = Brushes.White,

                // Font
                FontFamily = new FontFamily("Bahschrift"),
                FontSize = 12
            };
            TabSettings = new TabItem
            {
                Style = (base.TryFindResource("Tab") as System.Windows.Style),
                Content = Browser,
                Header = TabHeader,
                AllowDrop = true
            };
            this.TabControlShit.SelectedIndex = this.TabControlShit.Items.Add(TabSettings);

            return TabSettings;
        }

        //----------------------------- ANIMATIONS -----------------------------//
        Storyboard SliderStoryBoard = new Storyboard();

        public async void SimpleMoveAnimation(DependencyObject Object, Thickness Get, Thickness Set)
        {
            ThicknessAnimation Animation = new ThicknessAnimation
            {
                From = new Thickness?(Get),
                To = new Thickness?(Set),
                Duration = TimeSpan.FromMilliseconds(400.0), // 500.0
                EasingFunction = this.MarginEasing
            };
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.MarginProperty));
            this.SliderStoryBoard.Children.Add(Animation);
            this.SliderStoryBoard.Begin();
            await Task.Delay(500); // 100
            this.SliderStoryBoard.Children.Remove(Animation);
        }

        // gift from MCgamin
        public void Fade(DependencyObject Object)
        {
            DoubleAnimation FadeIn = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(1000.0),
            };
            Storyboard.SetTarget(FadeIn, Object);
            Storyboard.SetTargetProperty(FadeIn, new PropertyPath("Opacity", 1));
            this.SliderStoryBoard.Children.Add(FadeIn);
            this.SliderStoryBoard.Begin();
            this.SliderStoryBoard.Children.Remove(FadeIn);
        }

        public void FadeOut(DependencyObject Object)
        {
            DoubleAnimation Fade = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(1000.0),
            };
            Storyboard.SetTarget(Fade, Object);
            Storyboard.SetTargetProperty(Fade, new PropertyPath("Opacity", 1));
            this.SliderStoryBoard.Children.Add(Fade);
            this.SliderStoryBoard.Begin();
            this.SliderStoryBoard.Children.Remove(Fade);
        }

        private IEasingFunction MarginEasing { get; set; } = new QuadraticEase
        {
            EasingMode = EasingMode.EaseInOut
        };

        //----------------------------- NOTIFICATION -----------------------------//
        private async void CreateNotification(string Notification)
        {
            if (NotifAlready == true)
            {
                return;
            }

            try
            {
                NotifAlready = true;
                do
                {
                    NotificationBorder.Visibility = Visibility.Visible;
                    NotificationIcon.Visibility = Visibility.Visible;
                    ExclamationMark.Visibility = Visibility.Visible;
                    NotificationText.Visibility = Visibility.Visible;


                    NotificationBorder.Width = 26;

                    NotificationIcon.Width = 0; // notificationicon is broken when we summon it, so i put it to 0 for now -rexi
                    NotificationIcon.Height = 0;

                    ExclamationMark.Margin = new Thickness(303, 3, 0, 0);
                    NotificationIcon.Margin = new Thickness(301, 0, 332, 301);
                    NotificationBorder.Margin = new Thickness(297, 5, 0, 0);
                    NotificationText.Content = Notification;
                    NotificationText.Visibility = Visibility.Hidden;
                    Animate.OpacityAnimation(NotificationBorder, 0, 100, 2500);
                    await Task.Delay(100);
                    Animate.OpacityAnimation(NotificationIcon, 0, 100, 2500);
                    await Task.Delay(200); // 500
                    Animate.OpacityAnimation(ExclamationMark, 0, 100, 2500);
                    await Task.Delay(500); // 1000

                    NotificationText.Visibility = Visibility.Visible; // SET THE NOTIFICATION TEXT TO VISIBLE SINCE WE NEED IT

                    Animate.TimedMoveAnimation(NotificationIcon, new Thickness(301, 0, 332, 301), new Thickness(112, 0, 522, 301), 600);
                    Animate.TimedMoveAnimation(ExclamationMark, new Thickness(303, 3, 0, 0), new Thickness(115, 3, 0, 0), 600);
                    Animate.TimedMoveAnimation(NotificationBorder, new Thickness(297, 5, 0, 0), new Thickness(108, 5, 0, 0), 600);
                    await Task.Delay(500);
                    Animate.WidthAnimation(NotificationBorder, 27, 320, 700);
                    await Task.Delay(500);
                    Animate.OpacityAnimation(NotificationText, 0, 100, 2500);
                    await Task.Delay(2800);

                    NotificationText.Visibility = Visibility.Hidden; // SET THE NOTIFICATION TEXT TO INVISIBLE AFTER WERE DONE

                    // this fix actually made lots of shit clearer, as in by fixing the spasm -rexi

                    Animate.WidthAnimation(NotificationBorder, 320, 27, 800);
                    await Task.Delay(200);
                    Animate.TimedMoveAnimation(NotificationBorder, new Thickness(108, 5, 0, 0), new Thickness(297, 5, 0, 0), 600);
                    Animate.TimedMoveAnimation(ExclamationMark, new Thickness(113, 3, 0, 0), new Thickness(303, 3, 0, 0), 600);
                    Animate.TimedMoveAnimation(NotificationIcon, new Thickness(112, 0, 522, 301), new Thickness(301, 0, 332, 301), 600);
                    await Task.Delay(200);
                    Animate.OpacityAnimation(ExclamationMark, 100, 0, 1200);
                    Animate.OpacityAnimation(NotificationBorder, 100, 0, 1200);
                    await Task.Delay(100);
                    Animate.OpacityAnimation(NotificationIcon, 100, 0, 1200);
                }
                while (NotifState == true);
                NotifAlready = false;

                NotificationBorder.Visibility = Visibility.Hidden;
                NotificationIcon.Visibility = Visibility.Hidden;
                ExclamationMark.Visibility = Visibility.Hidden;
                NotificationText.Visibility = Visibility.Hidden;
            }
            catch (System.InvalidOperationException e)
            {
                System.Console.Out.WriteLine("Handled InvalidOperationException due to multiple access calling threads");
                System.Console.Out.WriteLine("Tried to send notification message: `" + Notification + "`");
                System.Console.Out.WriteLine(e.Message);
            }
        }

        //----------------------------- SETTINGS -----------------------------//
        private void TopMostCheck_Checked(object sender, RoutedEventArgs e)
        {
            Topmost = true;
        }

        private void TopMostCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
        }

        private void AutoAttachCheck_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.Autolaunch = true;
        }

        private void AutoAttachCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.Autolaunch = false;
        }

        private void IncognitoCheck_Checked(object sender, RoutedEventArgs e)
        {
            incognitoEnabled = true;
            //writeAppSettings();
        }

        private void IncognitoCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            incognitoEnabled = false;
            //writeAppSettings();
        }

        private void ViewPacketsCheck_Checked(object sender, RoutedEventArgs e)
        {
            viewPacketsEnabled = true;
            writeAppSettings();
        }

        private void ViewPacketsCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            viewPacketsEnabled = false;
            writeAppSettings();
        }

        private void ExperimentalCheck_Checked(object sender, RoutedEventArgs e)
        {
            experimentalEnabled = true;
            writeAppSettings();
        }

        private void ExperimentalCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            experimentalEnabled = false;
            writeAppSettings();
        }

        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Opacity = OpacitySlider.Value;
        }

        public void AutoAttach(object sender, EventArgs e)
        {
            /*if (Settings.Default.Autolaunch == true)
            {
                if (Injector.TryInject())
                {
                    CreateNotification("Success! Celery was Injected");
                }
            }*/
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        enum OutputType
        {
            Output,
            Info,
            Warning,
            Error
        };

        private void AddOutput(string message, OutputType type)
        {
            var newlines = 0;

            foreach (char x in message)
            {
                if (x == '\n')
                {
                    newlines++;
                }
            }

            ListViewItem msg = new ListViewItem
            {
                // Text
                Content = message,

                // Colors
                Padding = new Thickness(4, 4, 0, 0),
                Background = Brushes.Transparent,

                // Transformation
                Height = 22 + (16 * newlines),
                ToolTip = "Copy to clipboard",

                // Font
                FontFamily = new FontFamily("Bahschrift"),//FontFamily("Segoe UI"),
                FontSize = 13,
            };

            switch (type)
            {
                case OutputType.Output:
                    msg.Foreground = Brushes.White;
                    break;
                case OutputType.Warning:
                    msg.Foreground = Brushes.Gold;
                    break;
                case OutputType.Info:
                    msg.Foreground = Brushes.AliceBlue;
                    break;
                case OutputType.Error:
                    msg.Foreground = Brushes.IndianRed;
                    break;
            }

            if (outputList.Items.Count > 25)
            {
                outputList.Items.RemoveAt(0);
            }

            outputList.Items.Add(msg);
        }

        private void CloseOutput()
        {
            if (!(OutputWindow.Visibility == Visibility.Visible)) return;

            try { outputList.Items.Clear(); } catch (NullReferenceException ex) { }

            OutputWindow.Visibility = Visibility.Hidden;
            OutputWindow.Margin = new Thickness(OutputWindow.Margin.Left, OutputWindow.Margin.Top, OutputWindow.Margin.Right, -180);
            TabControlShit.Margin = new Thickness(TabControlShit.Margin.Left, TabControlShit.Margin.Top, TabControlShit.Margin.Right, 14);
            TabControlShit.Height += 110;
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            if (OutputWindow.Visibility == Visibility.Hidden)
            {
                OutputWindow.Visibility = Visibility.Visible;
                OutputWindow.Margin = new Thickness(OutputWindow.Margin.Left, OutputWindow.Margin.Top, OutputWindow.Margin.Right, 14);
                TabControlShit.Margin = new Thickness(TabControlShit.Margin.Left, TabControlShit.Margin.Top, TabControlShit.Margin.Right, 126);
                TabControlShit.Height -= 110;
            }
            else
            {
                CloseOutput();
            }
        }

        //----------------------------- SCRIPTHUB -----------------------------//
        private void ExecuteInfYield(object sender, RoutedEventArgs e)
        {
            foreach (var pinfo in Injector.MsStorePlayer.getInjectedProcesses())
            {
                Injector.MsStorePlayer.sendScript(pinfo, "loadstring(game:HttpGet('https://raw.githubusercontent.com/TheSeaweedMonster/Luau/main/scripts/infyield.lua'))()");
            }
        }

        private void ExecuteDexV2(object sender, RoutedEventArgs e)
        {
            foreach (var pinfo in Injector.MsStorePlayer.getInjectedProcesses())
            {
                Injector.MsStorePlayer.sendScript(pinfo, "loadstring(game:HttpGet('https://raw.githubusercontent.com/TheSeaweedMonster/Luau/main/scripts/dexv2.lua'))()");
            }
        }

        private void ExecuteUnnamedEsp(object sender, RoutedEventArgs e)
        {
            foreach (var pinfo in Injector.MsStorePlayer.getInjectedProcesses())
            {
                Injector.MsStorePlayer.sendScript(pinfo, "loadstring(game:HttpGet('https://raw.githubusercontent.com/TheSeaweedMonster/Luau/main/scripts/unnamedesp.lua'))()");
            }
        }

        private void outputList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (outputList.SelectedItem != null)
            {
                try
                {
                    Clipboard.SetText((string)((ListViewItem) outputList.SelectedItem).Content);
                } catch (Exception ex)
                {
                }
            }
        }

        private void TabControlShit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
