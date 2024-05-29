using Kameleoon;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

/// <summary>
/// The AllFlags controller shows the basic logic of using the Kameleoon SDK. It extracts the visitor code from
/// the HTTP request or generates a new one. After that it checks all available feature flags and returns a bool
/// value indicating whether flag is active for a visitor or not.
/// </summary>

[ApiController]
[Route("[controller]")]
public class AllFlagsController : ControllerBase
{
    private readonly ILogger<AllFlagsController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public AllFlagsController(ILogger<AllFlagsController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetAllFlags")]
    public AllFlagsResponse Get()
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            var allFlags = _kameleoonClient.GetFeatureList();
            var featuresInfo = allFlags.Select(key => new AllFlagsResponse.FeatureInfo()
            {
                FeatureKey = key,
                Active = _kameleoonClient.IsFeatureActive(visitorCode, key)
            });
            return new AllFlagsResponse()
            {
                VisitorCode = visitorCode,
                FeaturesInfo = featuresInfo
            };
        }
        catch (KameleoonException exception)
        {
            if (exception is KameleoonException.VisitorCodeInvalid)
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
