using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers;

public class UsersController : Controller
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Users.Include(u => u.Memberships).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(u =>
                EF.Functions.Like(u.FirstName, pattern) ||
                EF.Functions.Like(u.LastName, pattern) ||
                EF.Functions.Like(u.Email, pattern) ||
                EF.Functions.Like(u.City, pattern));
        }

        ViewBag.Search = search;
        var users = await query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .Include(u => u.Memberships)
            .ThenInclude(m => m.Group)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        var joinedIds = user.Memberships.Select(m => m.GroupId).ToList();
        var availableGroups = await _context.Groups
            .Where(g => !joinedIds.Contains(g.Id))
            .OrderBy(g => g.Name)
            .ToListAsync();

        return View(new UserDetailsViewModel
        {
            User = user,
            AvailableGroups = availableGroups
        });
    }

    public IActionResult Create()
    {
        return View(new User { BirthDate = new DateTime(2000, 1, 1) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        ValidateBirthDate(user);

        if (await EmailTaken(user.Email))
        {
            ModelState.AddModelError(nameof(user.Email), "A user with this email already exists.");
        }

        if (!ModelState.IsValid)
        {
            return View(user);
        }

        user.RegisteredAt = DateTime.Now;
        user.BirthDate = user.BirthDate.Date;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        TempData["Success"] = "User created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existing == null)
        {
            return NotFound();
        }

        ValidateBirthDate(user);

        if (await EmailTaken(user.Email, id))
        {
            ModelState.AddModelError(nameof(user.Email), "A user with this email already exists.");
        }

        if (!ModelState.IsValid)
        {
            user.RegisteredAt = existing.RegisteredAt;
            return View(user);
        }

        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;
        existing.Email = user.Email;
        existing.BirthDate = user.BirthDate.Date;
        existing.City = user.City;
        existing.Bio = user.Bio;
        await _context.SaveChangesAsync();
        TempData["Success"] = "User updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .Include(u => u.Memberships)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = "User deleted.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void ValidateBirthDate(User user)
    {
        if (user.BirthDate.Date > DateTime.Today)
        {
            ModelState.AddModelError(nameof(user.BirthDate), "Date of birth cannot be in the future.");
        }
    }

    private Task<bool> EmailTaken(string email, int? exceptId = null)
    {
        return _context.Users.AnyAsync(u => u.Email == email && u.Id != exceptId);
    }
}
