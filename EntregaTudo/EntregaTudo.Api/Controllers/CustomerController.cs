using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EntregaTudo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        [HttpGet("GetById", Name = "GetCustomerById")]
        public async Task<IActionResult> Get(Guid? id)
        {
            return Ok(await _customerRepository.GetById(id.Value));
        }

        [HttpGet("GetAllCustomers", Name = "GetAllCustomers")]
        public async Task<IActionResult> Get()
        {
            var customers = await _customerRepository.GetAll();
            return Ok(customers);
        }

        [HttpPost(Name = "SaveCustomers")]
        public async Task<IActionResult> Save([FromBody] CustomerDto? customer)
        {
            if (customer == null)
                return BadRequest("Propriedade Customer não pode ser nula!");

            var domain = new Customer
            {
              DocumentNumber  = customer.DocumentNumber,
              Email = customer.Email,
              FirstName = customer.FirstName,
              LastName = customer.LastName,
              PersonType = (PersonType)customer.PersonType,
              PhoneNumber = customer.PhoneNumber
            };

            await _customerRepository.Save(domain);

            return Created();
        }

        [HttpPut(Name = "UpdateCustomers")]
        public async Task<IActionResult> Update([FromBody] CustomerDto? customer)
        {
            if (customer == null)
                return BadRequest("Propriedade Customer não pode ser nula!");

            if (customer.Id == null) return BadRequest("Id necessário para ser atualizado");

            var domain = await _customerRepository.GetById(customer.Id.Value);

            domain.DocumentNumber = customer.DocumentNumber ?? domain.DocumentNumber;
            domain.Email = customer.Email ?? domain.Email;
            domain.FirstName = customer.FirstName ?? domain.FirstName;
            domain.LastName = customer.LastName ?? domain.LastName;
            domain.PersonType = (PersonType)customer.PersonType!;
            domain.PhoneNumber = customer.PhoneNumber ?? domain.PhoneNumber;
            await _customerRepository.Update(domain);

            return Ok(domain);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid? customerId)
        {
            if (customerId == null)
                return BadRequest("Propriedade Customer não pode ser nula!");

            await _customerRepository.Delete(customerId.Value);

            return Ok();
        }
    }
}
