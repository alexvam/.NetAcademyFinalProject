using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;
using System.Security.Claims;

namespace Crowdfunding.Controllers
{
    public class PackageController : Controller
    {
        private readonly Crowdfunding4Context _context;

        public PackageController(Crowdfunding4Context context)
        {
            _context = context;
        }

        // GET: Packages
        public async Task<IActionResult> Index()
        {
            var crowdfunding4Context = _context.Package.Include(p => p.Project).Include(p => p.Rewards);
            return View(await crowdfunding4Context.ToListAsync());
        }

        // GET: Packages/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package
                .Include(p => p.Project)
                .Include(p => p.Rewards)
                .FirstOrDefaultAsync(m => m.PackagesId == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // GET: Packages/Create
        public IActionResult Create()
        {
            var memberId = this.GetMemberId();

            var memberProjects = _context.Project.FromSql("SELECT * from dbo.Project").Where(u => u.MemberId == memberId);

            var reward = from r in _context.Reward                       
                       from p in _context.Project
                       where p.MemberId == memberId && p.ProjectId == r.ProjectId
                       select new Reward
                       {
                           RewardsId = r.RewardsId,
                           Title = r.Title
                       };

            var packageReward = reward.ToList();

            ViewData["ProjectId"] = new SelectList(memberProjects, "ProjectId", "ProjectName");
            //ViewData["RewardsId"] = new SelectList(_context.Reward, "RewardsId", "RewardsId");
            ViewData["RewardsId"] = new SelectList(packageReward, "RewardsId", "RewardsId");
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PackagesId,ProjectId,RewardsId,Price,Title")] Package package)
        {
            if (ModelState.IsValid)
            {
                _context.Add(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", package.ProjectId);
            ViewData["RewardsId"] = new SelectList(_context.Reward, "RewardsId", "RewardsId", package.RewardsId);
            return View(package);
        }

        // GET: Packages/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", package.ProjectId);
            ViewData["RewardsId"] = new SelectList(_context.Reward, "RewardsId", "RewardsId", package.RewardsId);
            return View(package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PackagesId,ProjectId,RewardsId,Price,Title")] Package package)
        {
            if (id != package.PackagesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(package);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageExists(package.PackagesId))
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
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", package.ProjectId);
            ViewData["RewardsId"] = new SelectList(_context.Reward, "RewardsId", "RewardsId", package.RewardsId);
            return View(package);
        }

        // GET: Packages/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package
                .Include(p => p.Project)
                .Include(p => p.Rewards)
                .FirstOrDefaultAsync(m => m.PackagesId == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var package = await _context.Package.FindAsync(id);
            _context.Package.Remove(package);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private long GetMemberId()
        {
            var memberEmail = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            return _context.Member.FromSql("SELECT * from dbo.Member").Where(m => m.EmailAddress == memberEmail).FirstOrDefault().MemberId;

        }


        private bool PackageExists(long id)
        {
            return _context.Package.Any(e => e.PackagesId == id);
        }
    }
}
