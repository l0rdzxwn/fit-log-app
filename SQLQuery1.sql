SELECT r.ID, r.Name, r.TargetMuscle, r.DaysOfWeek, r.userID, e.Name, e.Sets, e.Reps, e.Weight, e.isCompleted FROM Routines AS r
INNER JOIN Exercises AS e
ON r.ID = e.RoutineID