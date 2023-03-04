using CollaborationApp.Data;
using CollaborationApp.Hubs;
using CollaborationApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CollaborationApp.Controllers
{
    public class TextController : Controller
    {
        private readonly TextDbContext dbContext;


        //inject the injecting serive
        public TextController(TextDbContext dbContext)
        {
            this.dbContext = dbContext;
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
                   Id = Guid.NewGuid(),
                    Content = model.Content // Set the content property to the text editor's content
                };

                // Add the new instance to the database context
                dbContext.Text.Add(textContent);

                // Save changes to the database
                await dbContext.SaveChangesAsync();

                // Return the saved text content as a JSON object
                return Json(new { success = true, id = textContent.Id });
            }
            
            // If model state is not valid, return an error message
            return Json(new { success = false, message = "Invalid model state" });
        }


    }
}