using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Crowdfunding.Models;
using Microsoft.EntityFrameworkCore;

namespace Crowdfunding.Controllers
{
    public class HomeController : Controller
    {
        private readonly Crowdfunding4Context _context;

        public HomeController(Crowdfunding4Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchInput)
        {
            List<Project> projects;

            if (string.IsNullOrWhiteSpace(searchInput))
            {
                projects = await _context.Project
                .Include(p => p.Member)
                .Include(p => p.ProjectCategory)
                .Include(p => p.StatusNavigation)
                .ToListAsync();
            } else
            {
                projects = await _context.Project
                .Include(p => p.Member)
                .Include(p => p.ProjectCategory).
                Include(p => p.StatusNavigation).
                Where(p => p.ProjectName.Contains(searchInput)|| p.ProjectDescription.Contains(searchInput)).ToListAsync();               
            }
            
            return View(projects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.Member)
                .Include(p => p.ProjectCategory)
                .Include(p => p.StatusNavigation)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
