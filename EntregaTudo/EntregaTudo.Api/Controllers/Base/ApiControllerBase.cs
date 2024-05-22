using Codout.Framework.DAL;
using Microsoft.AspNetCore.Mvc;

namespace EntregaTudo.Api.Controllers.Base;

[ApiController]
public class ApiControllerBase : Controller
{
    public ApiControllerBase(IWebHostEnvironment webHostEnvironment,
        ILogger logger,
        IServiceProvider serviceProvider)
    {
        WebHostEnvironment = webHostEnvironment;
        Logger = logger;
        ServiceProvider = serviceProvider;
    }

    public IWebHostEnvironment WebHostEnvironment { get; }

    public ILogger Logger { get; }

    public IServiceProvider ServiceProvider { get; }
}
