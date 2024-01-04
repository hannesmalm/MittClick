using Microsoft.AspNetCore.Mvc;
using MittClick.Models;

namespace MittClick.Controllers
{
    public class ImageController : Controller
    {
        private readonly MittClickDbContext dbContext;

        public ImageController(MittClickDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

		[HttpPost]
		public IActionResult Upload(ImageUploadViewModel model)
		{
			if (ModelState.IsValid)
			{
				var image = new Image
				{
					Data = ConvertToByteArray(model.ImageFile)
				};

				dbContext.Images.Add(image);
				dbContext.SaveChanges();

				return RedirectToAction("Index", "Home");
			}

			return View(model);
		}


		private byte[] ConvertToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public IActionResult DisplayImages()
        {
            var images = dbContext.Images.ToList();
            return View("DisplayImages", images);
        }
    }
}
