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

        /*[PhotonName("show")]
        public void Show() => Window.;

        [PhotonName("hide")]
        public void Hide() => Window.Hide();*/
        
        [PhotonName("close")]
        public void Close() => Window.Close();
    }
}
