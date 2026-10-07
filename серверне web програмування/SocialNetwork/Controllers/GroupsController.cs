using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers;

public class GroupsController : Controller
{
    private readonly AppDbContext _context;

    public GroupsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Groups.Include(g => g.Memberships).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(g =>
                EF.Functions.Like(g.Name, pattern) ||
                EF.Functions.Like(g.Category, pattern) ||
                EF.Functions.Like(g.City, pattern));
        }

        ViewBag.Search = search;
        var groups = await query.OrderBy(g => g.Name).ToListAsync();
        return View(groups);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups
            .Include(g => g.Memberships)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null)
        {
            return NotFound();
        }

        var memberIds = group.Memberships.Select(m => m.UserId).ToList();
        var availableUsers = await _context.Users
            .Where(u => !memberIds.Contains(u.Id))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();

        return View(new GroupDetailsViewModel
        {
            Group = group,
            AvailableUsers = availableUsers
        });
    }

    public IActionResult Create()
    {
        FillCategories();
        return View(new Group());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Group group)
    {
        ValidateCategory(group);

        if (!ModelState.IsValid)
        {
            FillCategories();
            return View(group);
        }

        group.CreatedAt = DateTime.Now;
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Group created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        FillCategories(group.Category);
        return View(group);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Group group)
    {
        if (id != group.Id)
        {
            return NotFound();
        }

        var existing = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
        if (existing == null)
        {
            return NotFound();
        }

        ValidateCategory(group);

        if (!ModelState.IsValid)
        {
            group.CreatedAt = existing.CreatedAt;
            FillCategories(group.Category);
            return View(group);
        }

        existing.Name = group.Name;
        existing.Description = group.Description;
        existing.Category = group.Category;
        existing.City = group.City;
        existing.IsPrivate = group.IsPrivate;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Group updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups
            .Include(g => g.Memberships)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group != null)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Group deleted.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void ValidateCategory(Group group)
    {
        if (!DomainOptions.Categories.Contains(group.Category))
        {
            ModelState.AddModelError(nameof(Group.Category), "Choose a category from the list.");
        }
    }

    private void FillCategories(string? selected = null)
    {
        ViewBag.Categories = new SelectList(DomainOptions.Categories, selected);
    }
}
