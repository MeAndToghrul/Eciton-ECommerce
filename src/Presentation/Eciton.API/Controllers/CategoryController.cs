using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Category;
using Eciton.Application.ResponceObject.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Eciton.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICategoryService _categoryService;

    public CategoryController(IMediator mediator, ICategoryService categoryService)
    {
        _mediator = mediator;
        _categoryService = categoryService;
    }

    [HttpPost("create")]
    [SwaggerOperation(
        Summary = "Creates a new category.",
        Description = "Creates a new category with name and image. Image file is uploaded to Cloudinary."
    )]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateCategoryCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.ResponseStatusCode == ResponseStatusCode.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Gets category by ID.",
        Description = "Fetches a category from MongoDB ReadModel by its ID."
    )]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _categoryService.GetByIdAsync(id);

        if (response.ResponseStatusCode == ResponseStatusCode.Success)
            return Ok(response);

        return NotFound(response);
    }
}
