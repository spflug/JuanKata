using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    [HttpGet]
    public IEnumerable<int> Get()
    {
        return Enumerable.Range(1, 5)
            .ToArray();
    }
}