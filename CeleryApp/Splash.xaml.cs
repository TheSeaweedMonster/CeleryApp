#region Include 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.IO.Compression;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.Win32.SafeHandles;
using System.Text;
#endregion
namespace CeleryApp
{
    public partial class Splash : Window
    {
        Animation Animate = new Animation();
        WebClient wc = new WebClient();
        Random Random = new Random();
        DispatcherTimer DispatcherTimer = new DispatcherTimer();
        public Splash() => InitializeComponent();
        #region Quotes
        string[] Quotes =
        {
            "Rexi is ass at C#, trust me",
            "Rexi is cool :sunglasses:",
            "Following the white rabbit....",
            "Not panicking...totally not panicking",
            "Making stuff up. Please wait...",
            "The world is your litterbox",
            "Sacrificing a resistor to the machine gods....",
            "The architects are still drafting",
            "Alt+F4 won't give you admin in every game *wink*",
            "I got cut off I have to resize it a hair smaller -Celery",
            "Rexi sucks at error handling",
            "Testing your patience..",
            "Fun Fact : Celery actually hates Celery",
            "Stay awhile and listen..",
            "You edhall not pass! yet..",
            "Well, this is embarrassing.",
            "What the what?",
            "Downloading more RAM..",
            "Deleting System32 folder",
            "Debugging Debugger...",
            "Still faster than Windows update",
            "We're working very Hard .... Really",
            "Our premium plan is faster",
            "When nothing is going right, go left",
            "↑↑↓↓←→←→BA Start",
            "ly full homo -Rexi",
            "Making more meme strings...",
            "Go ahead -- hold your breath",
            "Stan approved Celery",
            "while true do while true do end end",
            "Also Try Misako!",
            "am pro",
            "POV : IllIlllIllIlllIlllIlllIIIl",
            "C++ gives me cancer",
            "const std::atomic_int > const auto > const std::uintptr_t",
            "H++ rocks",
            "synap x crack workign 2021 "
                };
        #endregion


        /*public System.Windows.Media.Imaging.BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                System.Windows.Media.Imaging.BitmapImage bitmapimage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }*/

        internal static class IconUtilities
        {
            [DllImport("gdi32.dll", SetLastError = true)]
            private static extern bool DeleteObject(IntPtr hObject);

            public static ImageSource ToImageSource(Icon icon)
            {
                Bitmap bitmap = icon.ToBitmap();
                IntPtr hBitmap = bitmap.GetHbitmap();
                MessageBox.Show("Icon width: " + bitmap.Width + ", height: " + bitmap.Height + ".");


                ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                if (!DeleteObject(hBitmap))
                {
                    throw new Win32Exception();
                }

                return wpfBitmap;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // this Title was "SplashApp", and MainWindow title was "CeleryApp"

            //ImageSource imageSource = IconUtilities.ToImageSource(new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "CeleryLogo.ico"));
            //this.Icon = imageSource;

            //this.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "CeleryLogo.ico")); // UriKind.Relative

            //this.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("CeleryLogo.png", UriKind.Relative));

            //Animate.OpacityAnimation(LoadingGrid, 0, 100, 10000);
            Animate.MoveAnimation(LoadingGrid, new Thickness(0, 25, 0, -25), new Thickness(0, -2, 0, 0));
            await Task.Delay(600); // 1200
            Animate.MoveAnimation(CeleryLogo, new Thickness(-300, 56, 657, 88), new Thickness(15, 56, 298, 94));
            await Task.Delay(200); // 300
            Animate.MoveAnimation(WelcomeLabel, new Thickness(620, 51, -250, 0), new Thickness(284, 51, 0, 0));
            await Task.Delay(150);
            Animate.MoveAnimation(WelcomeLabel1, new Thickness(760, 86, -320, 0), new Thickness(379, 86, 0, 0));
            Animate.MoveAnimation(WelcomeLabel2, new Thickness(708, 119, -297, 0), new Thickness(401, 115, 0, 0));
            await Task.Delay(300); // 500
            Animate.MoveAnimation(LoadBar, new Thickness(10, 248, 0, -21), new Thickness(10, 248, 0, 0));
            await Task.Delay(500); // 1000
            RandomizedText.Opacity = 100;
            int Randomized = Random.Next(Quotes.Length);
            RandomizedText.Content = Quotes[Randomized];
            LoadBar.Value = 0;
        }
        private void ProgressValueIncrease(object sender, EventArgs e) => LoadBar.Value += 1;//0.5;

        private async void Init(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow Window = new MainWindow();

            if (LoadBar.Value == 0)
            {
                DispatcherTimer.Tick += ProgressValueIncrease;
                DispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
                DispatcherTimer.Start();
            }
            else if (LoadBar.Value == 10)
            {
                if (!Directory.Exists("bin") || !Directory.Exists("scripts"))
                {
                    {
                        DispatcherTimer.Stop();
                        CeleryLogo.Visibility = Visibility.Hidden;
                        WelcomeLabel.Visibility = Visibility.Hidden;
                        WelcomeLabel1.Visibility = Visibility.Hidden;
                        WelcomeLabel2.Visibility = Visibility.Hidden;
                        RandomizedText.Visibility = Visibility.Hidden;
                        LoadBar.Visibility = Visibility.Hidden;
                        FirstTimeLabel.Content = "Hi, " + Environment.UserName + ".";
                        FirstTimeSetup.Margin = new Thickness(0, 0, 0, 0);
                        await Task.Delay(500);
                        Animate.OpacityAnimation(FirstTimeLabel, 0, 100, 5000);
                        await Task.Delay(4000);
                        Animate.OpacityAnimation(FirstTimeLabel, 100, 0, 5000);
                        FirstTimeLabel.FontSize = 15;
                        FirstTimeLabel.Content = "This seems to be your first time starting Celery.";
                        Animate.OpacityAnimation(FirstTimeLabel, 0, 100, 5000);
                        await Task.Delay(4000);
                        Animate.OpacityAnimation(FirstTimeLabel, 100, 0, 5000);
                        FirstTimeLabel.FontSize = 18;
                        FirstTimeLabel.Content = "Give us a moment.";
                        Animate.OpacityAnimation(FirstTimeLabel, 0, 100, 5000);
                        List<string> Dirs = new List<string> { "bin", "scripts" };
                        for (int i = 0; i < Dirs.Count; i++) { Directory.CreateDirectory(Dirs[i]); }
                        Animate.OpacityAnimation(FirstTimeLabel, 100, 0, 5000);
                        FirstTimeLabel.Content = "Restarting Celery..";
                        await Task.Delay(2000);
                        Process.Start("http://celerystick.xyz/");
                        new Splash().Show();
                        this.Hide();
                        
                    }
                }
  
            }
            else if (LoadBar.Value == 100)
            {

                Animate.MoveAnimation(CeleryLogo, new Thickness(15, 56, 298, 94), new Thickness(-300, 56, 657, 88));
                await Task.Delay(300);
                Animate.MoveAnimation(WelcomeLabel, new Thickness(284, 51, 0, 0), new Thickness(620, 51, -250, 0));
                await Task.Delay(150);
                Animate.MoveAnimation(WelcomeLabel1, new Thickness(379, 86, 0, 0), new Thickness(760, 86, -320, 0));
                Animate.MoveAnimation(WelcomeLabel2, new Thickness(401, 115, 0, 0), new Thickness(708, 119, -297, 0));
                RandomizedText.Visibility = Visibility.Hidden;
                await Task.Delay(500);
                Animate.MoveAnimation(LoadBar, new Thickness(10, 248, 0, 0), new Thickness(10, 278, 0, -21));
 
                Window.Show();
                Window.Topmost = true;
                this.Close();
                Window.Topmost = false;
            }
        }
    }
}
