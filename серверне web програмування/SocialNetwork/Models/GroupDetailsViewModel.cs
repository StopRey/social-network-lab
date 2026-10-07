namespace SocialNetwork.Models;

public class GroupDetailsViewModel
{
    public Group Group { get; set; } = null!;

    public List<User> AvailableUsers { get; set; } = new();
}
