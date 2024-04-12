using EntregaTudo.Shared.Dto.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntregaTudo.Api.Controllers.Base;

[ApiController]
public abstract class RestApiControllerBase<TEntity, TDto, TContext> : Controller
    where TEntity : class
    where TDto : DtoBase
    where TContext : DbContext
{
    private readonly TContext _context;
    protected RestApiControllerBase(TContext context)
    {
        _context = context;
    }

    [NonAction]
    protected abstract TEntity ToDomain(TDto dto, TEntity? entity = null);

    [NonAction]
    protected abstract TDto ToDto(TEntity domain);

    [NonAction]
    protected TEntity? GetDomain(TDto dto) => _context.Set<TEntity>().Find(dto.Id);

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
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Get(Guid? id)
    {
        var context = await _context.Set<TEntity>().FindAsync(id);

        if (context != null)
            return Ok(context);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Post([FromBody] TDto value)
    {
        try
        {
            if (value == null)
                return BadRequest($"O objeto[{nameof(TDto)}] não deve ser nulo.");

            TEntity domain;

            try
            {
                domain = ToDomain(value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            await _context.Set<TEntity>().AddAsync(domain);

            await _context.SaveChangesAsync();

            return Ok(ToDto(domain));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Put(Guid? id, [FromBody] TDto dto)
    {
        try
        {
            if (!id.HasValue)
                return NotFound();

            dto.Id = id;

            var entity = GetDomain(dto);

            if (entity == null) return Ok();

           var domain = ToDomain(dto, entity);

           _context.ChangeTracker.Clear();

            _context.Update(domain);
            await _context.SaveChangesAsync();

            return Ok(ToDto(entity));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
            return BadRequest("Id não pode ser nulo!");

        var entity = await _context.Set<TEntity>().FindAsync(id);

        if (entity == null) return Ok();

        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }

}