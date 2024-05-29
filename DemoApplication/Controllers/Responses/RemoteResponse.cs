namespace DemoApplication.Controllers;

public class RemoteResponse
{
    public string VisitorCode { get; set; } = "";
    public object? Variable { get; set; }
    public IEnumerable<string> CustomData { get; set; } = Enumerable.Empty<string>();
}
