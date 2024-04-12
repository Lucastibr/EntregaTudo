using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntregaTudo.Api.Controllers.Base;

//Verificar essa classe;
[ApiController]
public abstract class RestApiControllerBase<TEntity, TContext> : Controller
    where TEntity : class
    where TContext : DbContext
{
    private readonly TContext _context;
    protected RestApiControllerBase(TContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        try
        {
            var result = await _context.Set<TEntity>()
                .ToListAsync();
                
            return Ok(result);
        }
        catch (Exception e)
        {
          //  Logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}