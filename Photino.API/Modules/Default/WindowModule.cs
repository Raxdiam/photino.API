namespace PhotinoAPI.Modules.Default
{
    [PhotinoName("window")]
    internal class WindowModule : PhotinoModuleBase
    {
        public WindowModule(PhotinoApi api) : base(api){}

        public string GetTitle() => Api.Window.Title;
        public bool GetMaximized() => Api.Window.Maximized;
        public bool GetMinimized() => Api.Window.Minimized;
        public bool GetDevToolsEnabled() => Api.Window.DevToolsEnabled;
        public bool GetContextMenuEnabled() => Api.Window.ContextMenuEnabled;
        public bool GetTopMost() => Api.Window.Topmost;
        public bool GetCentered() => Api.Window.Centered;
        public bool GetChromeless() => Api.Window.Chromeless;
        public bool GetFullScreen() => Api.Window.FullScreen;
        public bool GetResizable() => Api.Window.Resizable;
        public int GetWidth() => Api.Window.Width;
        public int GetHeight() => Api.Window.Height;
        public int GetTop() => Api.Window.Top;
        public int GetLeft() => Api.Window.Left;

        public void SetTitle(string title) => Api.Window.SetTitle(title);
        public void SetMaximized(bool maximized) => Api.Window.SetMaximized(maximized);
        public void SetMinimized(bool minimized) => Api.Window.SetMinimized(minimized);
        public void SetDevToolsEnabled(bool enabled) => Api.Window.SetDevToolsEnabled(enabled);
        public void SetContextMenuEnabled(bool enabled) => Api.Window.SetContextMenuEnabled(enabled);
        public void SetTopMost(bool topMost) => Api.Window.SetTopMost(topMost);
        public void SetChromeless(bool chromeless) => Api.Window.SetChromeless(chromeless);
        public void SetFullScreen(bool fullScreen) => Api.Window.SetFullScreen(fullScreen);
        public void SetResizable(bool resizable) => Api.Window.SetResizable(resizable);
        public void SetWidth(int width) => Api.Window.SetWidth(width);
        public void SetHeight(int height) => Api.Window.SetHeight(height);
        public void SetTop(int top) => Api.Window.SetTop(top);
        public void SetLeft(int left) => Api.Window.SetLeft(left);

        public void Close() => Api.Window.Close();
        public void Load(string path) => Api.Window.Load(path);
        public void LoadRawString(string content) => Api.Window.LoadRawString(content);

        public void Center() => Api.Window.Center();
    }
}
