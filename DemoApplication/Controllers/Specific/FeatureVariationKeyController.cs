using Kameleoon;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureVariationKeyController : ControllerBase
{
    private readonly ILogger<FeatureVariationKeyController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public FeatureVariationKeyController(ILogger<FeatureVariationKeyController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetFeatureVariationKey")]
    public FeatureVariationKeyResponse Get([FromQuery(Name = "featureKey")] string featureKey)
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            var variationKey = _kameleoonClient.GetFeatureVariationKey(visitorCode, featureKey);
            return new FeatureVariationKeyResponse() { VisitorCode = visitorCode, VariationKey = variationKey };
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
