using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Api.Helpers;
using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using VehicleStatus = EntregaTudo.Shared.Enums.VehicleStatus;
using VehicleType = EntregaTudo.Shared.Enums.VehicleType;

namespace EntregaTudo.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class DeliveryPersonController(
    IWebHostEnvironment webHostEnvironment,
    ILogger<DeliveryPersonController> logger,
    IServiceProvider serviceProvider,
    IDeliveryPersonRepository repository,
    ICustomerRepository customerRepository,
    IOrderRepository orderRepository,
    TwillioSettings twillioSettings)
    : RestApiControllerBase<IDeliveryPersonRepository, DeliveryPerson, DeliveryPersonDto>(webHostEnvironment, logger,
        serviceProvider, repository)
{
    public override async Task<DeliveryPersonDto> ToDtoAsync(DeliveryPerson domain)
    {
        return new DeliveryPersonDto
        {
            Id = domain.Id,
            FirstName = domain.FirstName,
            DocumentNumber = domain.DocumentNumber,
            Email = domain.Email,
            LastName = domain.LastName,
            PersonType = (Shared.Enums.PersonType)domain.PersonType,
            PhoneNumber = domain.PhoneNumber,
            Password = domain.PasswordHash,
            Vehicle = new VehicleDto
            {
                Id = domain.Id,
                Brand = domain.Vehicle?.Brand,
                LicensePlate = domain.Vehicle?.LicensePlate,
                LoadCapacity = domain.Vehicle?.LoadCapacity,
                ManufactureYear = domain.Vehicle?.ManufactureYear,
                Model = domain.Vehicle?.Model,
                VehicleStatus = (VehicleStatus?)domain.Vehicle?.VehicleStatus,
                VehicleType = (VehicleType?)domain.Vehicle?.VehicleType,
            }
        };
    }

    public override async Task<DeliveryPerson> ToDomainAsync(DeliveryPersonDto dto)
    {

        var vehicleId = dto.Vehicle?.Id ?? ObjectId.GenerateNewId();

        return new DeliveryPerson
        {
            Id = dto.Id ?? ObjectId.Empty,
            FirstName = dto.FirstName,
            DocumentNumber = dto.DocumentNumber,
            Email = dto.Email,
            LastName = dto.LastName,
            PersonType = PersonType.DeliveryPerson,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = Repository.HashPassword(dto.Password),
            Vehicle = new Vehicle
            {
                Id = vehicleId,
                Brand = dto.Vehicle?.Brand,
                LicensePlate = dto.Vehicle?.LicensePlate,
                LoadCapacity = dto.Vehicle?.LoadCapacity,
                ManufactureYear = dto.Vehicle?.ManufactureYear,
                Model = dto.Vehicle?.Model,
                VehicleStatus = Core.Domain.Enum.VehicleStatus.Available,
                VehicleType = (Core.Domain.Enum.VehicleType?)dto.Vehicle?.VehicleType,
            }
        };
    }

    [Authorize]
    [HttpGet("available-orders")]
    public async Task<IActionResult> AvailableOrders()
    {
        if (User.Identity is { IsAuthenticated: false })
            return Unauthorized();

        var firstName = User.FindFirst("FirstName")?.Value;
        var lastName = User.FindFirst("LastName")?.Value;
        var licensePlate = User.FindFirst("LicensePlate")?.Value;

        var orders = orderRepository.Find(x => x.DeliveryStatus == DeliveryStatus.Pending)
            .ToList();

        // Obter os IDs de clientes de forma única para evitar buscas repetidas
        var customerIds = orders.Select(o => o.CustomerId).Distinct().ToList();

        // Buscar todos os clientes necessários de uma vez
        var customers = customerRepository
            .Find(x => customerIds.Contains(x.Id.ToString()))
            .ToDictionary(x => x.Id.ToString(), x => x);
        
        var ordersAvailable = orders.Select(s => 
        {
            var customer = customers.GetValueOrDefault(s.CustomerId);

            return new AvailableOrdersDto
            {
                Id = s.Id.ToString(),
                DeliveryCode = s.DeliveryCode,
                Address = new AddressDto
                {
                    AddressComplement = s.DestinationDelivery.AddressComplement,
                    City = s.DestinationDelivery.City,
                    StreetAddress = s.DestinationDelivery.StreetAddress,
                    Neighborhood = s.DestinationDelivery.Neighborhood,
                    PostalCode = s.DestinationDelivery.PostalCode,
                    Latitude = s.DestinationDelivery.Latitude,
                    Longitude = s.DestinationDelivery.Longitude,
                },
                OrderPrice = s.DeliveryCost,
                DeliveryPersonName = $"{firstName} {lastName}",
                PhoneNumber = customer?.PhoneNumber,
                LicensePlate = licensePlate,
                CustomerName = $"{customer?.FirstName}"
            };
        }).ToList();

        return Json(new {data = ordersAvailable});
    }

    [HttpPost("send")]
    public async Task SendMessageToCustomer(string phoneNumber, string message)
    {
        //TODO: Atualizar o status do pedido por aqui, buscar o nome do usuario e setar o pedido pro status em andamento.
        phoneNumber = phoneNumber.Trim();

        TwilioClient.Init(twillioSettings.AccountSid, twillioSettings.AuthToken);

        var number = $"+{phoneNumber}";

        var messageOptions = new CreateMessageOptions(
            new PhoneNumber(number))
        {
            From = new PhoneNumber(twillioSettings.FromNumber),
            Body = message
        };
        var result = await MessageResource.CreateAsync(messageOptions);

        var data = result.Body;
    }
}
