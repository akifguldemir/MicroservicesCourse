using AutoMapper;
using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    internal  class CourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(x => true).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }else
            {
                courses = new List<Course>();
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200).Data;

        public async Task<CourseDto> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return Response<CourseDto>.Fail("Course not found", 404).Data;
            }
            course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200).Data;
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();
            if (courses.Count == 0)
            {
                courses = new List<Course>();
            }
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            }
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<CourseDto> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var course = _mapper.Map<Course>(courseCreateDto);
            await _courseCollection.InsertOneAsync(course);
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200).Data;
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var course = _mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.ReplaceOneAsync(x => x.Id == course.Id, course);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Course not found", 404);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Course not found", 404);
        }
    }
}
