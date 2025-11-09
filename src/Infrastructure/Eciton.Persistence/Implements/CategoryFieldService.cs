using AutoMapper;
using Eciton.Application.Abstractions;
using Eciton.Application.Abstractions.Repositories;
using Eciton.Application.DTOs.CategoryField;
using Eciton.Application.Events;
using Eciton.Application.ReadModels;
using Eciton.Application.ResponceObject;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Domain.Entities;
using Eciton.Domain.Entities.Entity;
using Eciton.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eciton.Persistence.Implements
{
    public class CategoryFieldService : ICategoryFieldService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;
        private readonly ICategoryFieldReadRepository _categoryFieldReadRepository;

        public CategoryFieldService(
            AppDbContext appDbContext,
            IMapper mapper,
            IEventBus eventBus,
            ICategoryFieldReadRepository categoryFieldReadRepository)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _eventBus = eventBus;
            _categoryFieldReadRepository = categoryFieldReadRepository;
        }

        public async Task<Response> CreateAsync(CreateCategoryFieldDTO categoryFieldDto)
        {
            var categoryExists = await _appDbContext.Categories
                .AnyAsync(c => c.Id == categoryFieldDto.CategoryId);

            if (!categoryExists)
                return new Response(ResponseStatusCode.NotFound, "Category not found.");

            var exists = await _appDbContext.CategoryFields
                .AnyAsync(cf => cf.CategoryId == categoryFieldDto.CategoryId && cf.FieldName == categoryFieldDto.FieldName);

            if (exists)
                return new Response(ResponseStatusCode.Error, "Category field already exists.");

            var newField = _mapper.Map<CategoryField>(categoryFieldDto);

            await _appDbContext.CategoryFields.AddAsync(newField);
            await _appDbContext.SaveChangesAsync();

            await _eventBus.PublishAsync(new CategoryFieldCreatedEvent(
                newField.Id,
                newField.CategoryId,
                newField.FieldName,
                newField.DataType));

            return new Response(ResponseStatusCode.Success, "Category field created successfully.");
        }

        public async Task<Response<List<CategoryFieldReadModel>>> GetAllAsync()
        {
            var response = await _categoryFieldReadRepository.GetAllAsync();

            if (response.Data == null || response.Data.Count == 0)
            {
                return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.NotFound, "No category fields found.");
            }

            return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.Success, response.Data);
        }

        public async Task<Response<CategoryFieldReadModel>> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new Response<CategoryFieldReadModel>(ResponseStatusCode.Error, "Id is required.");
            }

            return await _categoryFieldReadRepository.GetByIdAsync(id);
        }

        public async Task<Response<List<CategoryFieldReadModel>>> GetByCategoryIdAsync(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.Error, "Category ID is required.");

            var response = await _categoryFieldReadRepository.GetByCategoryIdAsync(categoryId);

            if (response.Data == null || response.Data.Count == 0)
                return new Response<List<CategoryFieldReadModel>>(ResponseStatusCode.NotFound, "No category fields found for this category.");

            return response;
        }
    }
}
