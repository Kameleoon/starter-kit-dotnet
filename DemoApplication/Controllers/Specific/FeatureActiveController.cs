using Kameleoon;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureActiveController : ControllerBase
{
    private readonly ILogger<FeatureActiveController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public FeatureActiveController(ILogger<FeatureActiveController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetFeatureActive")]
    public FeatureActiveResponse Get([FromQuery(Name = "featureKey")] string featureKey)
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            var active = _kameleoonClient.IsFeatureActive(visitorCode, featureKey);
            return new FeatureActiveResponse() { VisitorCode = visitorCode, Active = active };
        }
        catch (KameleoonException exception)
        {
            // list of expected exceptions
            if (exception is KameleoonException.VisitorCodeInvalid ||
                exception is KameleoonException.FeatureNotFound)
            {
                _logger.LogError(exception, "Expected KameleoonException");
            }
            else
                _logger.LogError(exception, "Unexpected KameleoonException");
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected Exception");
            throw;
        }
    }
}
