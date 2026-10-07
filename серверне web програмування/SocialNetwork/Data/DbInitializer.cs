using SocialNetwork.Models;

namespace SocialNetwork.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any())
        {
            return;
        }

        var users = new List<User>
        {
            new()
            {
                FirstName = "Vadym",
                LastName = "Herasymenko",
                Email = "vadym.herasymenko@email.com",
                BirthDate = new DateTime(2002, 5, 12),
                City = "Lviv",
                Bio = "Student interested in ASP.NET Core.",
                RegisteredAt = new DateTime(2025, 9, 2, 10, 15, 0)
            },
            new()
            {
                FirstName = "Maria",
                LastName = "Koval",
                Email = "maria.koval@email.com",
                BirthDate = new DateTime(2001, 11, 3),
                City = "Kyiv",
                Bio = "Reads technical books and runs a book club.",
                RegisteredAt = new DateTime(2025, 9, 5, 14, 40, 0)
            },
            new()
            {
                FirstName = "Ihor",
                LastName = "Melnyk",
                Email = "ihor.melnyk@email.com",
                BirthDate = new DateTime(2000, 2, 20),
                City = "Lviv",
                Bio = "Plays football after classes.",
                RegisteredAt = new DateTime(2025, 9, 8, 18, 5, 0)
            },
            new()
            {
                FirstName = "Olena",
                LastName = "Shevchenko",
                Email = "olena.shevchenko@email.com",
                BirthDate = new DateTime(2003, 7, 28),
                City = "Dnipro",
                Bio = "Likes hiking in the Carpathians.",
                RegisteredAt = new DateTime(2025, 10, 1, 9, 0, 0)
            },
            new()
            {
                FirstName = "Taras",
                LastName = "Bondar",
                Email = "taras.bondar@email.com",
                BirthDate = new DateTime(1999, 12, 1),
                City = "Ivano-Frankivsk",
                Bio = "Organizes weekend trips.",
                RegisteredAt = new DateTime(2025, 10, 12, 16, 20, 0)
            }
        };

        var groups = new List<Group>
        {
            new()
            {
                Name = ".NET Developers",
                Description = "We talk about C#, ASP.NET Core and study projects.",
                Category = "IT",
                City = "Lviv",
                IsPrivate = false,
                CreatedAt = new DateTime(2025, 9, 2, 11, 0, 0)
            },
            new()
            {
                Name = "Book Club",
                Description = "We discuss one book every two weeks.",
                Category = "Education",
                City = "Kyiv",
                IsPrivate = false,
                CreatedAt = new DateTime(2025, 9, 6, 12, 30, 0)
            },
            new()
            {
                Name = "Evening Football",
                Description = "A private group for Wednesday games.",
                Category = "Sport",
                City = "Lviv",
                IsPrivate = true,
                CreatedAt = new DateTime(2025, 9, 10, 19, 0, 0)
            },
            new()
            {
                Name = "Carpathian Trips",
                Description = "Routes, stays and group trips.",
                Category = "Travel",
                City = "Ivano-Frankivsk",
                IsPrivate = false,
                CreatedAt = new DateTime(2025, 10, 3, 8, 45, 0)
            }
        };

        context.Users.AddRange(users);
        context.Groups.AddRange(groups);
        context.SaveChanges();

        context.Memberships.AddRange(
            new Membership { UserId = users[0].Id, GroupId = groups[0].Id, Role = "Administrator", JoinedAt = new DateTime(2025, 9, 2, 11, 5, 0) },
            new Membership { UserId = users[1].Id, GroupId = groups[0].Id, Role = "Member", JoinedAt = new DateTime(2025, 9, 7, 15, 0, 0) },
            new Membership { UserId = users[2].Id, GroupId = groups[0].Id, Role = "Member", JoinedAt = new DateTime(2025, 9, 9, 13, 20, 0) },
            new Membership { UserId = users[1].Id, GroupId = groups[1].Id, Role = "Administrator", JoinedAt = new DateTime(2025, 9, 6, 12, 35, 0) },
            new Membership { UserId = users[0].Id, GroupId = groups[1].Id, Role = "Member", JoinedAt = new DateTime(2025, 9, 15, 17, 10, 0) },
            new Membership { UserId = users[2].Id, GroupId = groups[2].Id, Role = "Administrator", JoinedAt = new DateTime(2025, 9, 10, 19, 10, 0) },
            new Membership { UserId = users[0].Id, GroupId = groups[2].Id, Role = "Member", JoinedAt = new DateTime(2025, 9, 18, 20, 0, 0) },
            new Membership { UserId = users[3].Id, GroupId = groups[3].Id, Role = "Moderator", JoinedAt = new DateTime(2025, 10, 4, 10, 0, 0) },
            new Membership { UserId = users[4].Id, GroupId = groups[3].Id, Role = "Administrator", JoinedAt = new DateTime(2025, 10, 3, 9, 0, 0) },
            new Membership { UserId = users[1].Id, GroupId = groups[3].Id, Role = "Member", JoinedAt = new DateTime(2025, 10, 20, 11, 30, 0) }
        );

        context.SaveChanges();
    }
}
