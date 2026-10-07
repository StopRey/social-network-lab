using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models;

public class Membership
{
    public int UserId { get; set; }

    public int GroupId { get; set; }

    [Display(Name = "Joined")]
    public DateTime JoinedAt { get; set; }

    [Required]
    [StringLength(30)]
    [Display(Name = "Role")]
    public string Role { get; set; } = "Member";

    public User User { get; set; } = null!;

    public Group Group { get; set; } = null!;
}
