using System.ComponentModel.DataAnnotations;

namespace FitLogApp.Models
{
    public class WorkoutRoutine
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter the program name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a target muscle")]
        public string TargetMuscle { get; set; }

        [Required(ErrorMessage = "Please enter a day on a week.")]
        public string DaysOfWeek { get; set; }
        public string UserID { get; set; }
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
