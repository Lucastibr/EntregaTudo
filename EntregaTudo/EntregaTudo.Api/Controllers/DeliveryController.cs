using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Shared.Dto;
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

    [HttpGet]
    public async Task<IActionResult> GetDeliveryPrice(DeliveryDto deliveryDto)
    {
        var delivery = new Delivery();

        var getCost = delivery.CalculateDeliveryCost(0, 0);

        return Ok(getCost);
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {


        return Ok();
    }
}
