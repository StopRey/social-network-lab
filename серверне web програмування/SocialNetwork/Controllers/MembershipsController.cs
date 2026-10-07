using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers;

public class MembershipsController : Controller
{
    private readonly AppDbContext _context;

    public MembershipsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int userId, int groupId, string role, string returnTo)
    {
        if (!DomainOptions.GroupRoles.Contains(role))
        {
            role = "Member";
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
        if (!userExists || !groupExists)
        {
            return NotFound();
        }

        var alreadyMember = await _context.Memberships
            .AnyAsync(m => m.UserId == userId && m.GroupId == groupId);

        if (alreadyMember)
        {
            TempData["Error"] = "This user is already a member of the group.";
        }
        else
        {
            _context.Memberships.Add(new Membership
            {
                UserId = userId,
                GroupId = groupId,
                Role = role,
                JoinedAt = DateTime.Now
            });
            await _context.SaveChangesAsync();
            TempData["Success"] = "User added to the group.";
        }

        return RedirectBack(returnTo, userId, groupId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int userId, int groupId, string returnTo)
    {
        var membership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.UserId == userId && m.GroupId == groupId);

        if (membership != null)
        {
            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();
            TempData["Success"] = "User removed from the group.";
        }

        return RedirectBack(returnTo, userId, groupId);
    }

    private IActionResult RedirectBack(string returnTo, int userId, int groupId)
    {
        if (returnTo == "user")
        {
            return RedirectToAction("Details", "Users", new { id = userId });
        }

        return RedirectToAction("Details", "Groups", new { id = groupId });
    }
}
