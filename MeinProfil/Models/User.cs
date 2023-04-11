using System;
using System.Collections.Generic;

namespace MeinProfil.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Job { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string ProfilePicture { get; set; } = null!;

    public string IdentityNumber { get; set; } = null!;
}
