using FitLogApp.Data;
using FitLogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace FitLogApp.Controllers
{
    [Authorize]
    public class RoutineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoutineController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var routines = await _context.Routines.Include(e => e.Exercises).Where(r => r.UserID == currUser).ToListAsync();
            return View(routines);
        }

        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
