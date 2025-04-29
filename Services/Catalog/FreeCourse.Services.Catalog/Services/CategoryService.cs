using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(x => true).ToListAsync();
            if (categories.Count == 0)
            {
                return Response<List<CategoryDto>>.Fail("No category found", 404);
            }
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            return Response<List<CategoryDto>>.Success(categoriesDto, 200);
        }

        public async Task<Response<CategoryCreateDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
            var category = _mapper.Map<Category>(categoryCreateDto);
            await _categoryCollection.InsertOneAsync(category);
            return Response<CategoryCreateDto>.Success(_mapper.Map<CategoryCreateDto>(category), 200);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return Response<CategoryDto>.Fail("Category not found", 404);
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Response<CategoryDto>.Success(categoryDto, 200);
        }


    }
}
