using CollaborationApp.Data;
using CollaborationApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TextEditor.Controllers
{
    public class TextController : Controller
    {
        private readonly TextDbContext _context;

        public TextController(TextDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var latestDocument = await _context.Text.OrderByDescending(d => d.LastEdited).FirstOrDefaultAsync();
            ViewBag.DocumentContent = latestDocument?.Content;
            return View();
        }

        [HttpGet]
        public async Task<string> GetTextContent()
        {
            var latestDocument = await _context.Text.OrderByDescending(d => d.LastEdited).FirstOrDefaultAsync();
            return latestDocument?.Content;
        }

        [HttpPost]
        public async Task<IActionResult> SaveTextContent([FromBody] Text document)
        {
            if (document == null)
            {
                return BadRequest();
            }

            var latestDocument = await _context.Text.OrderByDescending(d => d.LastEdited).FirstOrDefaultAsync();

            if (latestDocument == null || document.Content != latestDocument.Content)
            {
                document.LastEdited = DateTime.Now;
                _context.Text.Add(document);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
