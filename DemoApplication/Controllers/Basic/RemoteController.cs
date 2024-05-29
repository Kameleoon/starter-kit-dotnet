using Kameleoon;
using Kameleoon.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

/// <summary>
/// The Remote controller shows the basic logic of using the Kameleoon SDK with remote data. It extracts the
/// visitor code from the HTTP request or generates a new one. Then it retrieves the remote data for the visitor
/// and returns a variable value by provided variableKey of assigned variation for a visitor.
/// </summary>

[ApiController]
[Route("[controller]")]
public class RemoteController : ControllerBase
{
    private readonly ILogger<RemoteController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public RemoteController(ILogger<RemoteController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetRemote")]
    public async Task<RemoteResponse> Get(
        [FromQuery(Name = "featureKey")] string featureKey, [FromQuery(Name = "variableKey")] string variableKey)
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            var remoteData = await _kameleoonClient.GetRemoteVisitorData(visitorCode);
            var variable = _kameleoonClient.GetFeatureVariable(visitorCode, featureKey, variableKey);
            return new RemoteResponse()
            {
                VisitorCode = visitorCode,
                Variable = variable,
                CustomData = remoteData
                    .Cast<CustomData>()
                    .Select(cd =>
                    {
                        var values = string.Join(", ", cd.Values);
                        return $"CustomData(index: {cd.Id}, values: {values})";
                    })
            };
        }
        catch (KameleoonException exception)
        {
            if (exception is KameleoonException.VisitorCodeInvalid ||
                exception is KameleoonException.FeatureNotFound ||
                exception is KameleoonException.FeatureEnvironmentDisabled ||
                exception is KameleoonException.FeatureVariationNotFound)
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
