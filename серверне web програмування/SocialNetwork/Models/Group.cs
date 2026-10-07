using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models;

public class Group
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Enter a group name")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a description")]
    [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Choose a category")]
    [StringLength(50)]
    [Display(Name = "Category")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a city")]
    [StringLength(80)]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [Display(Name = "Private group")]
    public bool IsPrivate { get; set; }

    [Display(Name = "Created")]
    public DateTime CreatedAt { get; set; }

    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}
