using Eciton.Application.Abstractions;
using Eciton.Application.Commands.CategoryField;
using Eciton.Application.DTOs.CategoryField;
using Eciton.Application.ResponceObject.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Eciton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryFieldController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICategoryFieldService _categoryFieldService;

        public CategoryFieldController(IMediator mediator, ICategoryFieldService categoryFieldService)
        {
            _mediator = mediator;
            _categoryFieldService = categoryFieldService;
        }

        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Creates a new category field.",
            Description = "Creates a new field associated with a category."
        )]
        public async Task<IActionResult> Create([FromBody] CreateCategoryFieldCommand command)
        {            
            var response = await _mediator.Send(command);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Gets category field by ID.",
            Description = "Fetches a category field by its ID."
        )]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _categoryFieldService.GetByIdAsync(id);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return NotFound(response);
        }

        [HttpGet("category/{categoryId}")]
        [SwaggerOperation(
            Summary = "Gets category fields by Category ID.",
            Description = "Fetches all category fields for a specific category."
        )]
        public async Task<IActionResult> GetByCategoryId(string categoryId)
        {
            var response = await _categoryFieldService.GetByCategoryIdAsync(categoryId);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return NotFound(response);
        }

        [HttpGet("all")]
        [SwaggerOperation(
            Summary = "Gets all category fields.",
            Description = "Fetches all category fields."
        )]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryFieldService.GetAllAsync();

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return NotFound(response);
        }
    }
}
