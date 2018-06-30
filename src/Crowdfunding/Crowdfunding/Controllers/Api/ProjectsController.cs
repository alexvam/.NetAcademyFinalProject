using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crowdfunding.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfunding.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly Crowdfunding4Context _context;

        public ProjectsController(Crowdfunding4Context context)
        {
            _context = context;
        }

        // GET /api/customers
        public ActionResult<IEnumerable<Project>> GetProjects()
        {
            return _context.Project.ToList();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id)
        {
            var project = _context.Project.Find(id);
            _context.Project.Remove(project);
            _context.SaveChanges();
            return NoContent();
        }
    }
}