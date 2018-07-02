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
using System.Net.Mail;
using Microsoft.IdentityModel.Protocols;
using System.Net;

namespace Crowdfunding.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly Crowdfunding4Context await_context;

        public ProjectsController(Crowdfunding4Context context)
        {
            await_context = context;
        }
        
        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var crowdfunding4Context = await_context.Project.Include(p => p.Member).Include(p => p.ProjectCategory).Include(p => p.StatusNavigation);
            return View(await crowdfunding4Context.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await await_context.Project
                .Include(p => p.Member)
                .Include(p => p.ProjectCategory.CategoryDescription)
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
            ViewData["ProjectCategoryId"] = new SelectList(await_context.ProjectCategory, "CategoryId", "CategoryDescription");
            ViewData["Status"] = new SelectList(await_context.ProjectStatus, "StatusId", "StatusCategory");
            ViewData["StartDate"] = DateTime.Now;
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
                await_context.Add(project);
                await await_context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Json(new
                {
                    RedirectUrl = Url.Action("create", "rewards", new { id = project.ProjectId})
                });
            }
            ViewData["ProjectCategoryId"] = new SelectList(await_context.ProjectCategory, "CategoryId", "CategoryDescription", project.ProjectCategoryId);
            ViewData["Status"] = new SelectList(await_context.ProjectStatus, "StatusId", "StatusCategory", project.Status);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await await_context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(await_context.Member, "MemberId", "ConfirmPassword", project.MemberId);
            ViewData["ProjectCategoryId"] = new SelectList(await_context.ProjectCategory, "CategoryId", "CategoryDescription", project.ProjectCategoryId);
            ViewData["Status"] = new SelectList(await_context.ProjectStatus, "StatusId", "StatusCategory", project.Status);
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
                    await_context.Update(project);
                    await await_context.SaveChangesAsync();
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
            ViewData["MemberId"] = new SelectList(await_context.Member, "MemberId", "ConfirmPassword", project.MemberId);
            ViewData["ProjectCategoryId"] = new SelectList(await_context.ProjectCategory, "CategoryId", "CategoryDescription", project.ProjectCategoryId);
            ViewData["Status"] = new SelectList(await_context.ProjectStatus, "StatusId", "StatusCategory", project.Status);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await await_context.Project
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
            var project = await await_context.Project.FindAsync(id);
            await_context.Project.Remove(project);
            await await_context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private long GetMemberId()
        {
            var memberEmail = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            return await_context.Member.FromSql("SELECT * from dbo.Member").Where(m => m.EmailAddress == memberEmail).FirstOrDefault().MemberId;

        }

        private bool ProjectExists(long id)
        {
            return await_context.Project.Any(e => e.ProjectId == id);
        }


        public async Task<IActionResult> ProjectsShow(long ID, ProjectsPackages projects )

        {
            var projectID = await GetProjectAsync(ID);// auto tha prepei na ginete kai sto click apo project se project

        

            if (projects == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            projects.ProjectId = projectID.ProjectId;
           // projects.MemberId = GetMemberId();
            projects.ProjectName =  await_context.Project.FromSql("SELECT * from dbo.Project").Where(p => p.ProjectId == ID).FirstOrDefault().ProjectName;
            projects.ProjectDescription = await_context.Project.FromSql("SELECT * from dbo.Project").Where(p => p.ProjectId == ID).FirstOrDefault().ProjectDescription;
            projects.StartDate = await_context.Project.FromSql("SELECT * from dbo.Project").Where(p => p.ProjectId == ID).FirstOrDefault().StartDate;
            projects.EndDate = await_context.Project.FromSql("SELECT * from dbo.Project").Where(p => p.ProjectId == ID).FirstOrDefault().EndDate;
            projects.Target = await_context.Project.FromSql("SELECT * from dbo.Project").Where(p => p.ProjectId == ID).FirstOrDefault().Target;
            projects.ProjectLocation= await_context.Project.FromSql("SELECT * from dbo.Project").Where(p => p.ProjectId == ID).FirstOrDefault().ProjectLocation;

            projects.FirstName = await_context.Project.Include(p => p.Member).Where(p => p.ProjectId == ID).FirstOrDefault().Member.FirstName;
            projects.EmailAddress = await_context.Project.Include(p => p.Member).Where(p => p.ProjectId == ID).FirstOrDefault().Member.EmailAddress;


            projects.CategoryDescription = (from d in await_context.Project
                                            join f in await_context.ProjectCategory
                                            on d.ProjectCategoryId equals f.CategoryId
                                            where d.ProjectId == ID
                                           select  f.CategoryDescription).FirstOrDefault().ToString();

            projects.TransactionId= (from d in await_context.Project
                                     join f in await_context.Transaction
                                     on d.ProjectId equals f.ProjectId
                                     where d.ProjectId == ID
                                     select f.TransactionId).Count();

            projects.Price = (from d in await_context.Project
                              join f in await_context.Transaction
                              on d.ProjectId equals f.ProjectId
                              join g in await_context.Package
                              on f.PackagesId equals g.PackagesId
                              where d.ProjectId == ID
                              select g.Price).Sum();


            projects.FundedProgress = (projects.Price / projects.Target)*100;
            var today = DateTime.Today;
            projects.DaysLeft = (projects.EndDate - today);


            //projects.ListPackages = await GetItemsAsyncPackages(ID);
            //projects.ListRewards= await GetItemsAsyncReward(ID);
            // projects.ListComments = await GetItemsAsyncComment(ID);
            projects.PackageRewards = await GetItemsAsyncPackagesRewards(ID);
            projects.CommentMember = await GetItemsAsyncCommentMember(ID);
            projects.ListBackers = await GetBackers(ID);
            //projects.MemberId = GetMemberId();
            //projects.ProjectId = await_context.Project.FromSql("SELECT * from dbo.Project").Where(p => p.ProjectId == ID).FirstOrDefault().ProjectId;


            if (projects.FundedProgress >= 100)
            {
                var user = new Transaction();
                List<Transaction> users = projects.ListBackers.ToList();
                foreach (var item in users)
                {
                    users.Count();
                    user.MailUser(item);
                }
            }

            return View(projects);
        }


        //var project = await _context.Project
        //    .Include(p => p.Member)
        //    .Include(x => x.ProjectCategory)
        //    .Include(f => f.Transaction).Where(f => f.ProjectId == ID)
        //    .Include(rp => rp.Package).Where(rp => rp.ProjectId == ID)
        //    .FirstOrDefaultAsync(p => p.ProjectId == ID);


        private Task<Project> GetProjectAsync(long? ID) =>
          await_context.Project
          .FirstOrDefaultAsync(p => p.ProjectId == ID);

        private async Task<List<Package>> GetItemsAsyncPackagesRewards(long? ID)
        {
            return await await_context.Package.Include(x=>x.Rewards).Where(x => x.ProjectId == ID).ToListAsync();
        }

        //private async Task<List<Reward>> GetItemsAsyncReward(long? ID)
        //{
        //    return await await_context.Reward.Where(x => x.ProjectId == ID).ToListAsync();
        //}

        //private async Task<List<Comment>> GetItemsAsyncComment(long? ID)
        //{
        //    return await await_context.Comment.Where(x => x.ProjectId == ID).ToListAsync();
        //}

        private async Task<List<Comment>> GetItemsAsyncCommentMember(long? ID)
        {
            return await await_context.Comment.Include(x=>x.Member).Where(x => x.ProjectId == ID).ToListAsync();
        }
        private async Task<List<Transaction>> GetBackers(long? ID)
        {
            return await await_context.Transaction.Include(x => x.Member).Include(x=>x.Packages).Where(x => x.ProjectId == ID).ToListAsync();
        }


        public ActionResult ActiveProjectsShow()
        {
            return View();

        }
    }
}
