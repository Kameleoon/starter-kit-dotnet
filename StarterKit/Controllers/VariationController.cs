using Kameleoon;
using Microsoft.AspNetCore.Mvc;

namespace StarterKit.Controllers;

/// <summary>
/// The Variation controller shows the basic logic of using Kameleoon SDK. It extracts the visitor code from the HTTP
/// request or generates a new one. Then it adds a CustomData and returns the resultative variation key for a visitor.
/// </summary>

[ApiController]
[Route("[controller]")]
public class VariationController : ControllerBase
{
    private readonly ILogger<VariationController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public VariationController(ILogger<VariationController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetVariation")]
    public Response Get([FromQuery(Name = "featureKey")] string featureKey)
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        // Getting visitor code from request's cookies or generates a new one and adding to response's cookies.
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            // Obtaining the variation key for a visitor
            var variationKey = _kameleoonClient.GetFeatureVariationKey(visitorCode, featureKey);
            return new Response()
            {
                VisitorCode = visitorCode,
                VariationKey = variationKey
            };
        } // Handle Kameleoon exceptions
        catch (KameleoonException exception)
        {
            // It's a list of expected exceptions which could be thrown by GetFeatureVariationKey method
            if (exception is KameleoonException.VisitorCodeInvalid || // Visitor code is invalid
                exception is KameleoonException.FeatureNotFound || // Feature key not found
                exception is KameleoonException.FeatureEnvironmentDisabled) // Feature is disabled for environment
            {
                _logger.LogError(exception, "Expected KameleoonException");
            }
            else
                _logger.LogError(exception, "Unexpected KameleoonException");
            throw; // Re-throwing exceptions is generally not recommended; this is for demonstration only.
        }
        // Handle base exception (for unexpected cases)
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected Exception");
            throw; // Re-throwing exceptions is generally not recommended; this is for demonstration only.
        }
    }
}
