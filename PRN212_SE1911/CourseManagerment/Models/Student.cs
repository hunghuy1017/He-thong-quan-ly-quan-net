using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseManagerment.Models;

public partial class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudentId { get; set; }

    public string? Roll { get; set; }

    public string? FirstName { get; set; }

    public string? MidName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<RollCallBook> RollCallBooks { get; set; } = new List<RollCallBook>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
