using CollaborationApp.Data;
using CollaborationApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CollaborationApp.Controllers
{
    public class TextController : Controller
    {
        private readonly TextDbContext _dbContext;

        public TextController(TextDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveTextContent([FromBody] Text model)
        {
            if (ModelState.IsValid)
            {
                // Create a new instance of your model class
                var textContent = new Text
                {
                    Content = model.Content // Set the content property to the text editor's content
                };

                // Add the new instance to the database context
                _dbContext.Text.Add(textContent);

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                // Return the saved text content as a JSON object
                return Json(new { success = true, id = textContent.Id });
            }

            // If model state is not valid, return an error message
            return Json(new { success = false, message = "Invalid model state" });
        }
    }
}