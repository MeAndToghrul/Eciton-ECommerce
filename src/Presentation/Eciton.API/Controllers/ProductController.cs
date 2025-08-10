using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Product;
using Eciton.Application.DTOs.Product;
using Eciton.Application.ResponceObject.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Eciton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductService _productService;

        public ProductController(IMediator mediator, IProductService productService)
        {
            _mediator = mediator;
            _productService = productService;
        }

        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Creates a new product.",
            Description = "Creates a new product associated with a category."
        )]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Gets product by ID.",
            Description = "Fetches a product by its ID."
        )]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _productService.GetByIdAsync(id);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return NotFound(response);
        }

        [HttpGet("name/{name}")]
        [SwaggerOperation(
            Summary = "Gets products by name.",
            Description = "Fetches products matching the given name."
        )]
        public async Task<IActionResult> GetByName(string brand)
        {
            var response = await _productService.GetByNameAsync(brand);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return NotFound(response);
        }

        [HttpGet("all")]
        [SwaggerOperation(
            Summary = "Gets all products.",
            Description = "Fetches all products."
        )]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAllAsync();

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return NotFound(response);
        }
    }
}
