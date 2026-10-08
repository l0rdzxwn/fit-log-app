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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,TargetMuscle,DaysOfWeek")] WorkoutRoutine routine)
        {
            var currUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            routine.UserID = currUserID;

            ModelState.Remove("UserID");
            if (ModelState.IsValid)
            {
                await _context.Routines.AddAsync(routine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(routine);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var currUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var routine = await _context.Routines.Include(e => e.Exercises).FirstOrDefaultAsync(r => r.ID == id && r.UserID == currUserID);
            return View(routine);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
