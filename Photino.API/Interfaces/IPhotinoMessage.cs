namespace PhotinoAPI.Interfaces
{
    internal interface IPhotinoMessage<T>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public T Data { get; set; }
    }
}
