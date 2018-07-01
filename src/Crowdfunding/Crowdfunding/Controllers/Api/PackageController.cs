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
    public class PackageController : ControllerBase
    {
        private readonly Crowdfunding4Context _context;

        public PackageController(Crowdfunding4Context context)
        {
            _context = context;
        }

        // GET /api/package
        public ActionResult<IEnumerable<Package>> GetPackages()
        {
            return _context.Package.ToList();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var package = _context.Package.Find(id);
            _context.Package.Remove(package);
            _context.SaveChanges();
            return NoContent();
        }
    }
}