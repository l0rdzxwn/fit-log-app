using System.ComponentModel.DataAnnotations.Schema;

namespace FitLogApp.Models
{
    public class Exercise
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public bool isCompleted { get; set; }
        public int RoutineID { get; set; }

        [ForeignKey("RoutineID")]
        public WorkoutRoutine? WorkoutRoutine { get; set; }
    }
}
