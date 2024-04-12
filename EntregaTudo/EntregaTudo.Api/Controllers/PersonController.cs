using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Dal.Context;
using Microsoft.AspNetCore.Mvc;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : RestApiControllerBase<Person, EntregaTudoDbContext>
{
    public PersonController(EntregaTudoDbContext context) : base(context)
    {
    }

}
