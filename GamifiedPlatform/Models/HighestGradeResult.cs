using System;
using System.Collections.Generic;

namespace GamifiedPlatform.Models;

public class HighestGradeResult
{

    public int AssessmentId { get; set; }
    public string AssessmentTitle { get; set; }
    public decimal HighestGrade { get; set; }
}
