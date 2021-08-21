using System.Drawing;
using System.Text;

namespace PhotinoAPI.Ns
{
    [PhotonName("window")]
    public class Window : PhotonApiBase
    {
        public Window(PhotonManager manager) : base(manager) { }

        [PhotonName("setTitle")]
        public void SetTitle(string title) => Window.Title = title;

        [PhotonName("setSize")]
        public void SetSize(int width, int height) => Window.SetSize(width, height);

        [PhotonName("setLocation")]
        public void SetLocation(int x, int y) => Window.SetLocation(new Point(x, y));

        [PhotonName("maximize")]
        public void Maximize() => Window.Maximized = true;

        [PhotonName("minimize")]
        public void Minimize() => Window.Minimized = true;

        [PhotonName("restore")]
        public void Restore()
        {
            if (Window.Maximized) Window.Maximized = false;
            if (Window.Minimized) Window.Minimized = false;
        }

        [PhotonName("setDevToolsEnabled")]
        public void SetDevToolsEnabled(bool enabled) => Window.SetDevToolsEnabled(enabled);

        [PhotonName("setContextMenuEnabled")]
        public void SetContextMenuEnabled(bool enabled) => Window.SetContextMenuEnabled(enabled);

        [PhotonName("setTopMost")]
        public void SetTopMost(bool topMost) => Window.SetTopMost(topMost);

        /*[PhotonName("show")]
        public void Show() => Window.;

        [PhotonName("hide")]
        public void Hide() => Window.Hide();*/
        
        [PhotonName("close")]
        public void Close() => Window.Close();
    }
}
