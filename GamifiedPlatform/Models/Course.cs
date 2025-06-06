﻿using System;
using System.Collections.Generic;

namespace GamifiedPlatform.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? Title { get; set; }

    public string? LearningObjective { get; set; }

    public int? CreditPoints { get; set; }

    public string? DifficultyLevel { get; set; }

    public string? PreRequisites { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; } // New property

    // New properties to hold the highest grade details
    public int? HighestAssessmentId { get; set; }
    public string? HighestAssessmentTitle { get; set; }
    public int? HighestAssessmentGrade { get; set; }


    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    public virtual ICollection<Ranking> Rankings { get; set; } = new List<Ranking>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

    public virtual ICollection<Course> Prereqs { get; set; } = new List<Course>();
}
