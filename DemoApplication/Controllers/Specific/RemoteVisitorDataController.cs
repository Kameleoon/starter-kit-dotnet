using Kameleoon;
using Kameleoon.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class RemoteVisitorDataController : ControllerBase
{
    private readonly ILogger<RemoteVisitorDataController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public RemoteVisitorDataController(ILogger<RemoteVisitorDataController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "GetRemoteVisitorData")]
    public async Task<IEnumerable<string>> Get()
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            var remoteData = await _kameleoonClient.GetRemoteVisitorData(visitorCode);
            return remoteData
                    .Cast<CustomData>()
                    .Select(cd =>
                    {
                        var values = string.Join(", ", cd.Values);
                        return $"CustomData(index: {cd.Id}, values: {values})";
                    });
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
