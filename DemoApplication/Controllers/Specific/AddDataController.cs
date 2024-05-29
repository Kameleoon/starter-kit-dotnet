using Kameleoon;
using Kameleoon.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class AddDataController : ControllerBase
{
    private readonly ILogger<AddDataController> _logger;
    private readonly IKameleoonClient _kameleoonClient;

    public AddDataController(ILogger<AddDataController> logger, IKameleoonClient client)
    {
        _logger = logger;
        _kameleoonClient = client;
    }

    [HttpGet(Name = "AddDataController")]
    public AddDataResponse Get([FromQuery(Name = "index")] int index, [FromQuery(Name = "value")] string value)
    {
        // Clean `kameleoonVisitorCode` in browser's cookies, if you need to reset the visitorCode
        var visitorCode = _kameleoonClient.GetVisitorCode(HttpContext.Request, HttpContext.Response);
        try
        {
            _kameleoonClient.AddData(visitorCode, new CustomData(index, value));
            _kameleoonClient.Flush(visitorCode);
            return new AddDataResponse()
            {
                VisitorCode = visitorCode,
                Result = $"CustomData(index: {index}, value: {value}) was added and flushed successfully"
            };
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
