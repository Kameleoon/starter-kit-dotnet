using Kameleoon;
using Kameleoon.Types;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class ActiveFeaturesController : ControllerBase
{
    private readonly ILogger<ActiveFeaturesController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public ActiveFeaturesController(ILogger<ActiveFeaturesController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetActiveFeatures")]
    public IReadOnlyDictionary<string, Variation> Get()
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            return _kameleoonClient.GetActiveFeatures(visitorCode);
        }
        catch (KameleoonException exception)
        {
            if (exception is KameleoonException.VisitorCodeInvalid)
                _logger.LogError(exception, "Expected KameleoonException");
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
