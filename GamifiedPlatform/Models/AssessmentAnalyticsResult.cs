using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamifiedPlatform.Models;

public class AssessmentAnalyticsResult
{
    public int AssessmentID { get; set; }
    public string AssessmentTitle { get; set; }
    public int? TotalMarks { get; set; } // Nullable property
    public int? PassingMarks { get; set; } // Nullable property
    public int? AverageScore { get; set; } // Nullable to handle cases with no scores
}

