using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Api.Helpers;
using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Domain.Infrastructure;
using EntregaTudo.Dal.Context;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveryController : ApiControllerBase
{

    public DeliveryController(EntregaTudoDbContext context, IWebHostEnvironment hostingEnvironment) : base(context, hostingEnvironment)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetDeliveryPrice(DeliveryDto? deliveryDto)
    {
        var delivery = new Delivery
        {
            OriginDelivery = new Address
            {
                PostalCode = "75384618"
            },
            DestinationDelivery = new Address
            {
                PostalCode = "74915120"
            },
            Items =
            [
                new ItemDelivery
                {
                    Weight = 2.5f
                }
            ]
        };

        var distanceOrigin = await CepHelper.SearchAddress(delivery.OriginDelivery.PostalCode);
        var distanceDestiny = await CepHelper.SearchAddress(delivery.DestinationDelivery.PostalCode);
        var distanceInKm = CepHelper.GetDistance(Convert.ToDouble(distanceOrigin.Lng, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceOrigin.Lat, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceDestiny.Lng, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceDestiny.Lat, CultureInfo.InvariantCulture));

        var getCost = delivery.CalculateDeliveryCost(distanceInKm, 2);

        return Ok(getCost);
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        return Ok();
    }
}
