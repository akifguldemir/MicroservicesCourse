﻿using FreeCourse.Services.Catalog.Dtos.Course;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBased;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : CustomBaseController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync();
            return CreatedActionResultInstance(response);

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var response = await _courseService.GetByIdAsync(Id);

           return CreatedActionResultInstance(response);
        }

        [HttpGet]
        [Route("api/[controller]/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId);

            return CreatedActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            var response = await _courseService.CreateAsync(courseCreateDto);
            return CreatedActionResultInstance(response);

        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var response = await _courseService.UpdateAsync(courseUpdateDto);
            return CreatedActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var response = await _courseService.DeleteAsync(Id);
            return CreatedActionResultInstance(response);
        }
    }
}
