using Codout.Framework.DAL;
using Codout.Framework.DAL.Entity;
using Codout.Framework.DAL.Repository;
using EntregaTudo.Shared.Dto.Base;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EntregaTudo.Api.Controllers.Base;

[ApiController]
public abstract class RestApiControllerBase<TRepository, TEntity, TDto> : ApiControllerBase
    where TEntity : class, IEntity<ObjectId>, new()
    where TDto : DtoBase
    where TRepository : IRepository<TEntity>
{
    private readonly IUnitOfWork _unitOfWork;
    protected RestApiControllerBase(IWebHostEnvironment webHostEnvironment,
        ILogger logger,
        IUnitOfWork unitOfWork,
        IServiceProvider serviceProvider,
        TRepository repository)
        : base(webHostEnvironment, logger, unitOfWork, serviceProvider)
    {
        Repository = repository;
    }

    public TRepository Repository { get; }

    [NonAction]
    public abstract Task<TDto> ToDtoAsync(TEntity value);

    [NonAction]
    public abstract Task<TEntity> ToDomainAsync(TDto dto);

    [NonAction]
    protected TEntity? GetDomain(TDto dto) => Repository.Get(dto.Id);


    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        try
        {
            var result = Repository.All().ToList();

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
        var context = await Repository.GetAsync(id);

        if (context != null)
            return Ok(context);

        return Ok();
    }

    [HttpPost("Create")]
    [DisableRequestSizeLimit]
    public virtual async Task<IActionResult> Create(TDto dto)
    {

        UnitOfWork.BeginTransaction();
        var domain = await ToDomainAsync(dto);
        await Repository.SaveAsync(domain);
        UnitOfWork.Commit();

        return Ok(ToDtoAsync(domain));
    }

    [HttpPut("Edit")]
    [DisableRequestSizeLimit]
    public virtual async Task<IActionResult> Edit(ObjectId? id, [FromBody] TDto dto)
    {
        if (!id.HasValue)
            return NotFound();

        dto.Id = id;

        var domain = await ToDomainAsync(dto);

        UnitOfWork.Commit();

        return Ok(ToDtoAsync(domain));
    }

    [HttpDelete]
    public virtual async Task<IActionResult> Delete(Guid? id)
    {
        try
        {
            UnitOfWork.BeginTransaction();
            var domain = await Repository.GetAsync(id);
            await Repository.DeleteAsync(domain);
            UnitOfWork.Commit();
            return Json(new { result = true });
        }
        catch (Exception e)
        {
            return Json(new { result = true, message = e.Message });
        }
    }

}