using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Enter a first name")]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a last name")]
    [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter an email")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    [StringLength(100)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a date of birth")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of birth")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "Enter a city")]
    [StringLength(80, ErrorMessage = "City cannot be longer than 80 characters")]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Bio cannot be longer than 500 characters")]
    [Display(Name = "Bio")]
    public string? Bio { get; set; }

    [Display(Name = "Registered")]
    public DateTime RegisteredAt { get; set; }

    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
