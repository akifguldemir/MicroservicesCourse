using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBased;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile file, CancellationToken cancellationToken)
        {
            if(file.Length > 0 && file != null)
            { 
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                var returnedPath = Path.Combine("photos", file.FileName);

                PhotoDto photoDto = new()
                {
                    Url = returnedPath
                };

                return CreatedActionResultInstance(Response<PhotoDto>.Success(photoDto,200));

            }

            return CreatedActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));

        }

        public IActionResult Delete(string fileName, CancellationToken cancellationToken)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return CreatedActionResultInstance(Response<NoContent>.Success(204));
            }
            return CreatedActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
        }
    }
}
