using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Catalog.Services
{
    internal interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryCreateDto>> CreateAsync(CategoryCreateDto categoryCreateDto);
        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
