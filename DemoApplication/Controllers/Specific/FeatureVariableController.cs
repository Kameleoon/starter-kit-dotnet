using Kameleoon;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureVariableController : ControllerBase
{
    private readonly ILogger<FeatureVariableController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public FeatureVariableController(ILogger<FeatureVariableController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetFeatureVariable")]
    public FeatureVariableResponse Get(
        [FromQuery(Name = "featureKey")] string featureKey, [FromQuery(Name = "variableKey")] string variableKey)
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            var variable = _kameleoonClient.GetFeatureVariable(visitorCode, featureKey, variableKey);
            return new FeatureVariableResponse() { VisitorCode = visitorCode, Variable = variable };
        }
        catch (KameleoonException exception)
        {
            if (exception is KameleoonException.VisitorCodeInvalid ||
                exception is KameleoonException.FeatureNotFound ||
                exception is KameleoonException.FeatureEnvironmentDisabled ||
                exception is KameleoonException.FeatureVariableNotFound)
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
