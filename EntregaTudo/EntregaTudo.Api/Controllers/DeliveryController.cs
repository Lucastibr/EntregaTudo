using EntregaTudo.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveryController : ApiControllerBase
{
    public DeliveryController(DbContext context, IWebHostEnvironment hostingEnvironment) : base(context, hostingEnvironment)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {


        return Ok();
    }
}
