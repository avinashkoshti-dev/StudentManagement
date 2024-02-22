using System;
using System.Collections.Generic;

namespace StudentAPI.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int Standard { get; set; }

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public int Zip { get; set; }

    public string? ProfileImage { get; set; }

    public DateTime Dob { get; set; }
}
