using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Crowdfunding.Controllers
{
    public class CommentsController : Controller
    {
        private readonly Crowdfunding4Context _context;

        public CommentsController(Crowdfunding4Context context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var crowdfunding4Context = _context.Comment.Include(c => c.Member).Include(c => c.Project).Where(m => m.MemberId == this.GetMemberId());
            return View(await crowdfunding4Context.ToListAsync());

        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Member)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public  IActionResult Create()
        {
            ViewData["Date"] = DateTime.Now;
            return View();

 
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Comment comment)
        {

            // na kanei valisation kai text kai imerominies. den bgazei omos minimata se periptosi lathous
            ViewData["Date"] = DateTime.Now;
            comment.MemberId = GetMemberId();
            DateTime highdate = new DateTime(9999, 12, 31);
            DateTime lowDate = new DateTime(1753, 01, 01);
            
          

            if (ModelState.IsValid && comment.Date> lowDate&& comment.Date< highdate && comment.Comment1!=null)
            {

                _context.Add(comment);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ProjectsShow", "Projects", new {id = comment.ProjectId });
            }

                ViewBag.Message = "Invalid value.";
               return RedirectToAction("ProjectsShow", "Projects", new { id = comment.ProjectId });
        }

            // GET: Comments/Edit/5
            public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            //ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "ConfirmPassword", comment.MemberId);
            //ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", comment.ProjectId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CommentId,MemberId,ProjectId,Comment1,Date")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
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
            //ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "ConfirmPassword", comment.MemberId);
            //ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectName", comment.ProjectId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Member)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private long GetMemberId()
        {
            var memberEmail = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            return _context.Member.FromSql("SELECT * from dbo.Member").Where(m => m.EmailAddress == memberEmail).FirstOrDefault().MemberId;

        }

        private Task<Project> GetProjectAsync(long? id) =>
        _context.Project
        .FirstOrDefaultAsync(p => p.ProjectId == id);


        private bool CommentExists(long id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}
