using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Crowdfunding.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly Crowdfunding4Context _context;

        public ProjectsController(Crowdfunding4Context context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var crowdfunding4Context = _context.Project.Include(p => p.Member).Include(p => p.ProjectCategory).Include(p => p.StatusNavigation);
            return View(await crowdfunding4Context.ToListAsync());
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

        // GET: Projects/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["MemberId"] = this.GetMemberId();
            ViewData["ProjectCategoryId"] = new SelectList(_context.ProjectCategory, "CategoryId", "CategoryDescription");
            ViewData["Status"] = new SelectList(_context.ProjectStatus, "StatusId", "StatusCategory");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,Target,MemberId,ProjectDescription,ProjectCategoryId,StartDate,EndDate,ProjectLocation,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectCategoryId"] = new SelectList(_context.ProjectCategory, "CategoryId", "CategoryDescription", project.ProjectCategoryId);
            ViewData["Status"] = new SelectList(_context.ProjectStatus, "StatusId", "StatusCategory", project.Status);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "ConfirmPassword", project.MemberId);
            ViewData["ProjectCategoryId"] = new SelectList(_context.ProjectCategory, "CategoryId", "CategoryDescription", project.ProjectCategoryId);
            ViewData["Status"] = new SelectList(_context.ProjectStatus, "StatusId", "StatusCategory", project.Status);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProjectId,ProjectName,Target,MemberId,ProjectDescription,ProjectCategoryId,StartDate,EndDate,ProjectLocation,Status")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "ConfirmPassword", project.MemberId);
            ViewData["ProjectCategoryId"] = new SelectList(_context.ProjectCategory, "CategoryId", "CategoryDescription", project.ProjectCategoryId);
            ViewData["Status"] = new SelectList(_context.ProjectStatus, "StatusId", "StatusCategory", project.Status);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(long? id)
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

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public long GetMemberId()
        {
            var memberEmail = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            return _context.Member.FromSql("SELECT * from dbo.Member").Where(m => m.EmailAddress == memberEmail).FirstOrDefault().MemberId;

        }

        private bool ProjectExists(long id)
        {
            return _context.Project.Any(e => e.ProjectId == id);
        }

    }
}
