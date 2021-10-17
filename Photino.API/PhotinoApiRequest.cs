namespace PhotinoAPI
{
    /// <summary>
    /// Request data sent by the font-end
    /// </summary>
    internal class PhotinoApiRequest : IPhotinoApiData
    {
        public string Module;
        public string Method;
        public object[] Parameters;
    }
}