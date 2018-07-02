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

        /*public ActionResult<List<String>> GetTopProjects()
        {


            return (from d in _context.Project
                              join f in _context.Transaction
                              on d.ProjectId equals f.ProjectId
                              join g in _context.Package
                              on f.PackagesId equals g.PackagesId
                              
                              orderby g.Price  select d.ProjectName).Take(5).ToList();
            //select g.Price).Sum();
           // return pric;
        }*/

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