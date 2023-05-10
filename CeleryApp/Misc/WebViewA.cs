using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using System.Drawing;

namespace CeleryApp
{
    public class WebViewA : WebView2
    {
        public bool IsDoMLoaded { get; set; } = false;
        private string ToSetText;
        private string LatestRecievedText;
        public string Theme { get; set; } = "Dark";

        public event EventHandler EditorReady;

        public async void WebViewInitialize(string BrowserExecutableFolder, string UserDataFolder, CoreWebView2EnvironmentOptions Options)
        {
            var Enviroment = await CoreWebView2Environment.CreateAsync(BrowserExecutableFolder, UserDataFolder, Options);
            await this.EnsureCoreWebView2Async(Enviroment);
        }

        public WebViewA(string Text = "")
        {
            WebViewInitialize(null, Path.GetTempPath(), null);
            this.DefaultBackgroundColor = Color.FromArgb(0, 25, 25, 25);
            this.Source = new Uri($"{Directory.GetCurrentDirectory()}\\bin\\Monaco\\index.html");
            this.CoreWebView2InitializationCompleted += WebViewAPI_CoreWebView2InitializationCompleted;
            this.ToSetText = Text;
        }

        protected virtual void OnEditorReady()
        {
            EventHandler Handler = EditorReady;
            if (Handler != null)
                Handler(this, new EventArgs());
        }

        private void WebViewAPI_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            this.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            this.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            this.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            this.CoreWebView2.Settings.AreDevToolsEnabled = false;
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e) => LatestRecievedText = e.TryGetWebMessageAsString();

        private async void CoreWebView2_DOMContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            await Task.Delay(1000);
            IsDoMLoaded = true;
            SetText(ToSetText);
            OnEditorReady();
            SetTheme(Theme);
        }

        public async Task<string> GetText()
        {
            if (IsDoMLoaded)
            {
                await this.ExecuteScriptAsync("window.chrome.webview.postMessage(editor.getValue())");
                await Task.Delay(50);//Task.Delay(200);

                return LatestRecievedText;
            }
            return string.Empty;
        }

        public async void SetText(string Text)
        {
            if (IsDoMLoaded)
                await CoreWebView2.ExecuteScriptAsync($"SetText(\"{HttpUtility.JavaScriptStringEncode(Text)}\")");
        }

        public void AddIntellisense(string Label, Types Type, string Description, string Insert)
        {
            if (IsDoMLoaded)
            {
                string SelectedType = Type.ToString();
                if (Type == Types.None)
                    SelectedType = "";
                this.ExecuteScriptAsync($"AddIntellisense({Label}, {SelectedType}, {Description}, {Insert});");
            }
        }

        public void Refresh()
        {
            if (IsDoMLoaded)
                this.ExecuteScriptAsync($"Refresh();");
        }

        private async void SetTheme(string ThemeName)
        {
            if (IsDoMLoaded)
                await CoreWebView2.ExecuteScriptAsync($"SetTheme(\"{ThemeName}\");");
        }
    }

    public enum Types
    {
        Class,
        Color,
        Constructor,
        Enum,
        Field,
        File,
        Folder,
        Function,
        Interface,
        Keyword,
        Method,
        Module,
        Property,
        Reference,
        Snippet,
        Text,
        Unit,
        Value,
        Variable,
        None
    }
}