using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;

namespace Crowdfunding.Controllers
{
    public class RewardsController : Controller
    {
        private readonly Crowdfunding4Context _context;

        public RewardsController(Crowdfunding4Context context)
        {
            _context = context;
        }

        // GET: Rewards
        public async Task<IActionResult> Index()
        {
            var crowdfunding4Context = _context.Reward.Include(r => r.Project);
            return View(await crowdfunding4Context.ToListAsync());
        }

        // GET: Rewards/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await _context.Reward
                .Include(r => r.Project)
                .FirstOrDefaultAsync(m => m.RewardsId == id);
            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // GET: Rewards/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName");
            return View();
        }

        // POST: Rewards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RewardsId,ProjectId,Title,Description,ItemsIncluded,DeliveryDate,Amount")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reward);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", reward.ProjectId);
            return View(reward);
        }

        // GET: Rewards/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await _context.Reward.FindAsync(id);
            if (reward == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", reward.ProjectId);
            return View(reward);
        }

        // POST: Rewards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("RewardsId,ProjectId,Title,Description,ItemsIncluded,DeliveryDate,Amount")] Reward reward)
        {
            if (id != reward.RewardsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reward);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RewardExists(reward.RewardsId))
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
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", reward.ProjectId);
            return View(reward);
        }

        // GET: Rewards/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await _context.Reward
                .Include(r => r.Project)
                .FirstOrDefaultAsync(m => m.RewardsId == id);
            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var reward = await _context.Reward.FindAsync(id);
            _context.Reward.Remove(reward);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RewardExists(long id)
        {
            return _context.Reward.Any(e => e.RewardsId == id);
        }
    }
}
