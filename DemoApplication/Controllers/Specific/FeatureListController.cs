using Kameleoon;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureListController : ControllerBase
{
    private readonly IKameleoonClient _kameleoonClient;

    public FeatureListController(IKameleoonClient client)
    {
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetFeatureList")]
    public IEnumerable<string> Get()
    {
        return _kameleoonClient.GetFeatureList();
    }
}
