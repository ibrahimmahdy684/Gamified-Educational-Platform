using System;
using System.Collections.Generic;

namespace GamifiedPlatform.Models;

public partial class Takesassesment
{
    public int LearnerId { get; set; }

    public int AssesmentId { get; set; }

    public int? ScoredPoints { get; set; }

    public virtual Assessment Assesment { get; set; } = null!;

    public virtual Learner Learner { get; set; } = null!;
}
