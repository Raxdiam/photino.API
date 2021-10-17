namespace PhotinoAPI
{
    internal class PhotinoApiMessage<T> where T : IPhotinoApiData
    {
        public string Id;
        public T Data;
    }
}
