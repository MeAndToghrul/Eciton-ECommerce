using AutoMapper;
using Eciton.Application.Abstractions;
using Eciton.Application.Abstractions.Repositories;
using Eciton.Application.DTOs.Product;
using Eciton.Application.Events;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Domain.Entities.Entity;
using Eciton.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
namespace Eciton.Persistence.Implements;
public class ProductService : IProductService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly IStorageService _storageService;
    private readonly IEventBus _eventBus;
    private readonly IProductReadRepository _productReadRepository;

    public ProductService(AppDbContext appDbContext, IMapper mapper, IStorageService storageService, IEventBus eventBus,IProductReadRepository productReadRepository)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
        _storageService = storageService;
        _eventBus = eventBus;
        _productReadRepository = productReadRepository;
    }

    public async Task<Response> CreateAsync(CreateProductDTO productDto)
    {
        var categoryExists = await _appDbContext.Categories.AnyAsync(c => c.Id == productDto.CategoryId);
        if (!categoryExists)
            return new Response(ResponseStatusCode.NotFound, "Category not found.");

        var product = _mapper.Map<Product>(productDto);

        if (productDto.ProductImage != null && productDto.ProductImage.Length > 0)
        {
            using var stream = productDto.ProductImage.OpenReadStream();
            var imageUrl = await _storageService.UploadFileAsync(stream, productDto.ProductImage.FileName, "products");
            product.ProductImage = imageUrl;
        }

        var categoryName = await _appDbContext.Categories
            .Where(c => c.Id == product.CategoryId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync();

        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync();

        var productCreatedEvent = new ProductCreatedEvent(
            product.Id,
            product.Brand,
            product.Price,
            categoryName ?? string.Empty, 
            product.StockQuantity,
            product.Model,
            product.ProductImage
        );



        await _eventBus.PublishAsync(productCreatedEvent);

        return new Response(ResponseStatusCode.Success, "Product created successfully.");
    }

    public async Task<Response<List<ProductReadModel>>> GetAllAsync()
    {
        var response = await _productReadRepository.GetAllAsync();

        if (response.Data == null || response.Data.Count == 0)
        {
            return new Response<List<ProductReadModel>>(ResponseStatusCode.NotFound, "No products found.");
        }

        return new Response<List<ProductReadModel>>(ResponseStatusCode.Success, response.Data);
    }

    public async Task<Response<ProductReadModel>> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new Response<ProductReadModel>(ResponseStatusCode.Error, "Id is required.");
        }

        return await _productReadRepository.GetByIdAsync(id);
    }


    public async Task<Response<List<ProductReadModel>>> GetByNameAsync(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand))
            return new Response<List<ProductReadModel>>(ResponseStatusCode.Error, "Product name is required.");

        var response = await _productReadRepository.GetByNameAsync(brand);

        if (response.Data == null || response.Data.Count == 0)
            return new Response<List<ProductReadModel>>(ResponseStatusCode.NotFound, "No products found with this name.");

        return new Response<List<ProductReadModel>>(ResponseStatusCode.Success, response.Data);
    }

}
