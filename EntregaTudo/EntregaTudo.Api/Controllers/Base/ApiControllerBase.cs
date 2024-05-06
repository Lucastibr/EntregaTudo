using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntregaTudo.Api.Controllers.Base;

[ApiController]
public class ApiControllerBase : Controller
{
    public ApiControllerBase(DbContext context, IWebHostEnvironment hostingEnvironment)
    {
        Context = context;
        HostingEnvironment = hostingEnvironment;
    }

    public DbContext Context { get; }

    public IWebHostEnvironment HostingEnvironment { get; }
}
