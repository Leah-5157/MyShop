using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities;
public partial class User
{
    public int Id { get; set; }
    [Required, EmailAddress]
    public string UserName { get; set; } = null!;
    [StringLength(12, ErrorMessage = "password must be between 8 till 12 tags", MinimumLength = 8), Required]
    public string Password { get; set; } = null!;
    [Required]
    public string FirstName { get; set; } = null!;
    public string Salt { get; set; } = null!;

    public string? LastName { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
