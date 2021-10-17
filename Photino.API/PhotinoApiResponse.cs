namespace PhotinoAPI
{
    /// <summary>
    /// Response data sent to the front-end
    /// </summary>
    internal class PhotinoApiResponse : IPhotinoApiData
    {
        public object Result;
        public string Error;
    }
}