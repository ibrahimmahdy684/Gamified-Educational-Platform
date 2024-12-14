using System;
using System.Collections.Generic;

namespace GamifiedPlatform.Models;

public class LearnerScoreViewModel
{
    public int LearnerId { get; set; }
    public string LearnerName { get; set; }
    public string LearnerEmail { get; set; }
    public int AssessmentId { get; set; }
    public int? ScoredPoints { get; set; }
}
