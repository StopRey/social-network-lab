namespace SocialNetwork.Models;

public class UserDetailsViewModel
{
    public User User { get; set; } = null!;

    public List<Group> AvailableGroups { get; set; } = new();
}
