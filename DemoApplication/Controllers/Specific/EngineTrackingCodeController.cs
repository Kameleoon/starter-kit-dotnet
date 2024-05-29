using Kameleoon;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class EngineTrackingCodeController : ControllerBase
{
    private readonly ILogger<EngineTrackingCodeController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public EngineTrackingCodeController(ILogger<EngineTrackingCodeController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetEngineTrackingCode")]
    public EngineTrackingCodeResponse Get([FromQuery(Name = "featureKey")] string featureKey)
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            var variationKey = _kameleoonClient.GetFeatureVariationKey(visitorCode, featureKey);
            var engineTrackingCode = _kameleoonClient.GetEngineTrackingCode(visitorCode);
            return new EngineTrackingCodeResponse()
            {
                VisitorCode = visitorCode,
                VariationKey = variationKey,
                TrackingCode = engineTrackingCode
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
