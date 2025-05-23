﻿using FreeCourse.Services.Catalog.Dtos.Category;
using FreeCourse.Services.Catalog.Dtos.Feature;
using FreeCourse.Services.Catalog.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Dtos.Course
{
    public class CourseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public DateTime CreateTime { get; set; }
        public string CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }
}
