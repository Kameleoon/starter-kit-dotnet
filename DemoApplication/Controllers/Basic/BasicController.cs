using Kameleoon;
using Kameleoon.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

/// <summary>
/// The Basic controller shows the basic logic of using Kameleoon SDK. It extracts the visitor code from the HTTP
/// request or generates a new one. Then it adds a CustomData and returns the resultative variation key for a visitor.
/// </summary>

[ApiController]
[Route("[controller]")]
public class BasicController : ControllerBase
{
    private readonly ILogger<BasicController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public BasicController(ILogger<BasicController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetBasic")]
    public BasicResponse Get([FromQuery(Name = "index")] int index,
            [FromQuery(Name = "value")] string value, [FromQuery(Name = "featureKey")] string featureKey)
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            _kameleoonClient.AddData(visitorCode, new CustomData(index, value));
            var variationKey = _kameleoonClient.GetFeatureVariationKey(visitorCode, featureKey);
            return new BasicResponse()
            {
                VisitorCode = visitorCode,
                VariationKey = variationKey
            };
        }
        catch (KameleoonException exception)
        {
            if (exception is KameleoonException.VisitorCodeInvalid ||
                exception is KameleoonException.FeatureNotFound ||
                exception is KameleoonException.FeatureEnvironmentDisabled)
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
