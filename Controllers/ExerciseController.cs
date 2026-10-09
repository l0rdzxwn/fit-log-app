using FitLogApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using FitLogApp.Models;

namespace FitLogApp.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var exercises = await _context.Exercises.Include(r => r.WorkoutRoutine).Where(e => e.RoutineID == e.WorkoutRoutine.ID && e.WorkoutRoutine.UserID == currUserID).ToListAsync();
            return View(exercises);
        }

        
        [HttpGet]
        public IActionResult Create(int routineId)
        {
            ViewBag.routineID = routineId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Sets,Reps,Weight,RoutineID")] Exercise exercise)
        {
            var currUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool ownsRoutine = await _context.Routines.AnyAsync(r => r.ID == exercise.RoutineID && currUserID == r.UserID);
            if (!ownsRoutine)
            {
                return Forbid();
            }

            ModelState.Remove("WorkoutRoutine");

            if (ModelState.IsValid)
            {
                await _context.Exercises.AddAsync(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Routine", new { id = exercise.RoutineID });
            }

            return View(exercise);
        }
   }
}
