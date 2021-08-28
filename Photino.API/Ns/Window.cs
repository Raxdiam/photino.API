using System.Drawing;

namespace PhotinoAPI.Ns
{
    [PhotonName("window")]
    public class Window : PhotonApiBase
    {
        public Window(PhotonManager manager) : base(manager) { }

        [PhotonName("getTitle")]
        public string GetTitle() => Window.Title;

        [PhotonName("getMaximized")]
        public bool GetMaximized() => Window.Maximized;

        [PhotonName("getMinimized")]
        public bool GetMinimized() => Window.Minimized;

        [PhotonName("getDevToolsEnabled")]
        public bool GetDevToolsEnabled() => Window.DevToolsEnabled;

        [PhotonName("getContextMenuEnabled")]
        public bool GetContextMenuEnabled() => Window.ContextMenuEnabled;

        [PhotonName("getTopMost")]
        public bool GetTopMost() => Window.Topmost;

        [PhotonName("setTitle")]
        public void SetTitle(string title) => Window.Title = title;

        [PhotonName("setSize")]
        public void SetSize(int width, int height) => Window.Size = new Size(width, height);

        [PhotonName("setLocation")]
        public void SetLocation(int x, int y) => Window.Location = new Point(x, y);

        [PhotonName("setMaximized")]
        public void SetMaximized(bool maximized) => Window.Maximized = maximized;

        [PhotonName("setMinimized")]
        public void SetMinimized(bool minimized) => Window.Minimized = minimized;
        
        [PhotonName("setDevToolsEnabled")]
        public void SetDevToolsEnabled(bool enabled) => Window.DevToolsEnabled = enabled;

        [PhotonName("setContextMenuEnabled")]
        public void SetContextMenuEnabled(bool enabled) => Window.ContextMenuEnabled = enabled;

        [PhotonName("setTopMost")]
        public void SetTopMost(bool topMost) => Window.Topmost = topMost;

        /*[PhotonName("show")]
        public void Show() => Window.;

        [PhotonName("hide")]
        public void Hide() => Window.Hide();*/
        
        [PhotonName("close")]
        public void Close() => Window.Close();
    }
}
