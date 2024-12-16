using System;
using System.Collections.Generic;

namespace GamifiedPlatform.Models;

public partial class Instructor
{
    public int InstructorId { get; set; }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? LatestQualification { get; set; }

    public string? ExpertiseArea { get; set; }

    public string? Email { get; set; }

    public string? ProfilePicturePath { get; set; }


    public virtual ICollection<EmotionalfeedbackReview> EmotionalfeedbackReviews { get; set; } = new List<EmotionalfeedbackReview>();

    public virtual ICollection<Pathreview> Pathreviews { get; set; } = new List<Pathreview>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
