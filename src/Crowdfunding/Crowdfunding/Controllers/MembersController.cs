using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crowdfunding.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Crowdfunding.Controllers
{
    public class MembersController : Controller
    {
        private readonly Crowdfunding4Context _context;

        public MembersController(Crowdfunding4Context context)
        {
            _context = context;
        }

        // GET: Members
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.Where(u => u.MemberId == this.GetMemberId()).ToListAsync());
        }

        // GET: Projects Created
        public async Task<ActionResult> Projects()
        {
            var memberId = this.GetMemberId();
            
            var memberProjects = await _context.Project.FromSql("SELECT * from dbo.Project").Where(u => u.MemberId == memberId).ToListAsync();            

            return View(memberProjects);

        }

        // GET: Projects Created
        public async Task<ActionResult> Funded([Bind("MemberId")] Member member)
        {
            var memberId = this.GetMemberId();

            var back = from p in _context.Project
                            from t in _context.Transaction
                            where t.MemberId == memberId && t.ProjectId == p.ProjectId
                            select new Project
                            {
                                ProjectId = p.ProjectId,
                                ProjectName = p.ProjectName
                            };

            var projectsBacked = back.ToList();

            return View(projectsBacked);

        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,PhoneNumber,Address,Country,City,PostCode,FirstName,LastName,EmailAddress,Password,ConfirmPassword,Birthday")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Login
        public IActionResult Login(string returnUrl = null)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Members/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("EmailAddress,Password")] Member member, string returnUrl = null)
        {
            var usr = _context.Member.Where(u => u.EmailAddress == member.EmailAddress && u.Password == member.Password).FirstOrDefault();


            if (ModelState.IsValid && usr != null)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, usr.EmailAddress));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                if (returnUrl == null)
                {
                    returnUrl = TempData["returnUrl"]?.ToString();
                }
                // an erxete apo to Comments Creat na pigenei sto project page
                if (returnUrl== "/Comments/Create")
                {
                    return  RedirectToAction("Index", "Projects");

                }

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction(nameof(Projects));
            }

            else
            {
                ViewBag.Message = "Invalid username or password.";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("MemberId,PhoneNumber,Address,Country,City,PostCode,FirstName,LastName,EmailAddress,Password,ConfirmPassword,Birthday")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var member = await _context.Member.FindAsync(id);
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private long GetMemberId()
        {
            var memberEmail = User.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();

            return _context.Member.FromSql("SELECT * from dbo.Member").Where(m => m.EmailAddress == memberEmail).FirstOrDefault().MemberId;

        }

        private bool MemberExists(long id)
        {
            return _context.Member.Any(e => e.MemberId == id);
        }


        public ActionResult Dashboard()
        {
            return View();

        }


    }


    
}
