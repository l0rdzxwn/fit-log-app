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

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [HttpGet]
        public IActionResult Create(int routineId)
        {
            var exercise = new Exercise
            {
                RoutineID = routineId
            };

            return View(exercise);
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
                return RedirectToAction(nameof(Index));
            }

            return View(exercise);
        }
   }
}
