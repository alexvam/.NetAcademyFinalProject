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
    public class TransactionsController : Controller
    {
        private readonly Crowdfunding4Context _context;

        public TransactionsController(Crowdfunding4Context context)
        {
            _context = context;
        }

        // GET: Transactions
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var crowdfunding4Context = _context.Transaction.Include(t => t.Member).Include(t => t.Packages).Include(t => t.Project).Where(m => m.MemberId== this.GetMemberId());
            return View(await crowdfunding4Context.ToListAsync());
        }

        // GET: Transactions/Details/5
        [Authorize]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Member)
                .Include(t => t.Packages)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        [Authorize]
        public IActionResult Create(int id)
        {
            var package = _context.Package.FromSql("SELECT * from dbo.Package").Where(m => m.ProjectId == id);

            ViewData["MemberId"] = this.GetMemberId();
            ViewData["PackagesId"] = new SelectList(package, "PackagesId", "Title");
            ViewData["ProjectId"] = id;
            ViewData["Date"] = DateTime.Now;
            ViewData["Contribution"] = 1;

            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,MemberId,ProjectId,Contribution,Date,PackagesId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "MemberId", transaction.MemberId);
            ViewData["PackagesId"] = new SelectList(_context.Package, "PackagesId", "PackagesId", transaction.PackagesId);
            //ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", transaction.ProjectId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "ConfirmPassword", transaction.MemberId);
            ViewData["PackagesId"] = new SelectList(_context.Package, "PackagesId", "PackagesId", transaction.PackagesId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", transaction.ProjectId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("TransactionId,MemberId,ProjectId,Contribution,Date,PackagesId")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
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
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "ConfirmPassword", transaction.MemberId);
            ViewData["PackagesId"] = new SelectList(_context.Package, "PackagesId", "PackagesId", transaction.PackagesId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", transaction.ProjectId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Member)
                .Include(t => t.Packages)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private long GetMemberId()
        {
            var memberEmail = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            return _context.Member.FromSql("SELECT * from dbo.Member").Where(m => m.EmailAddress == memberEmail).FirstOrDefault().MemberId;

        }

        private bool TransactionExists(long id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}
