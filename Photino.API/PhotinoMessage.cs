using PhotinoAPI.Interfaces;

namespace PhotinoAPI;

internal class PhotinoMessage : IPhotinoMessage<object>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public object Data { get; set; }
}