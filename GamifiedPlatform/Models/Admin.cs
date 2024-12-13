using System;
using System.Collections.Generic;

namespace GamifiedPlatform.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Country { get; set; }

    public string? Email { get; set; }

    public virtual User User { get; set; } = null!;
}
