using System.Security.Claims;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.DataSeeding;

public static class AppDataInit
{
    public static void MigrateDatabase(AppDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DeleteDatabase(AppDbContext context)
    {
        context.Database.EnsureDeleted();
    }

    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        foreach (var (roleName, id) in InitialData.Roles)
        {
            var role = roleManager.FindByNameAsync(roleName).Result;

            if (role != null) continue;

            role = new AppRole()
            {
                Id = id ?? Guid.NewGuid(),
                Name = roleName,
            };

            var result = roleManager.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException("Role creation failed!");
            }
        }


        foreach (var userInfo in InitialData.Users)
        {
            var user = userManager.FindByEmailAsync(userInfo.name).Result;
            if (user == null)
            {
                user = new AppUser()
                {
                    Id = userInfo.id ?? Guid.NewGuid(),
                    Email = userInfo.name,
                    UserName = userInfo.name,
                    EmailConfirmed = true,
                    FirstName = userInfo.firstName,
                    LastName = userInfo.lastName,
                };
                var result = userManager.CreateAsync(user, userInfo.password).Result;
                if (!result.Succeeded)
                {
                    throw new ApplicationException("User creation failed!");
                }

                result = userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, user.FirstName)).Result;
                if (!result.Succeeded)
                {
                    throw new ApplicationException("Claim adding failed!");
                }

                result = userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.LastName)).Result;
                if (!result.Succeeded)
                {
                    throw new ApplicationException("Claim adding failed!");
                }
            }

            foreach (var role in userInfo.roles)
            {
                if (userManager.IsInRoleAsync(user, role).Result)
                {
                    Console.WriteLine($@"User {user.UserName} already in role {role}");
                    continue;
                }

                var roleResult = userManager.AddToRoleAsync(user, role).Result;
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
                else
                {
                    Console.WriteLine($@"User {user.UserName} added to role {role}");
                }
            }
        }
    }

    /*public static async Task SeedData(AppDbContext context)
    {
        if (!context.Groups.Any())
        {
            var groups = InitialData.Groups.Select(g => new Group
            {
                Id = g.id ?? Guid.NewGuid(),
                GroupName = g.GroupName,
                FandomName = g.FandomName
            }).ToList();

            context.Groups.AddRange(groups);
        }

        if (!context.Artists.Any())
        {
            var artists = InitialData.Artists.Select(a => new Artist
            {
                Id = a.id ?? Guid.NewGuid(),
                StageName = a.StageName,
                FirstName = a.FirstName,
                LastName = a.Lastname,
                IsSolo = a.isSolo,
                BirthDate = a.BirthDate.ToUniversalTime()
            }).ToList();

            context.Artists.AddRange(artists);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($@"Error: {ex.Message}");
            Console.WriteLine($@"Inner Exception: {ex.InnerException?.Message}");
            Console.WriteLine($@"Stack Trace: {ex.StackTrace}");
            throw;
        }


        if (!context.ArtistsInGroups.Any())
        {
            var artistDict = context.Artists.ToDictionary(a => a.StageName, a => a);
            var groupDict = context.Groups.ToDictionary(g => g.GroupName, g => g);

            foreach (var artist in InitialData.Artists)
            {
                foreach (var groupName in artist.groups)
                {
                    if (!artistDict.TryGetValue(artist.StageName, out var artistEntity) ||
                        !groupDict.TryGetValue(groupName, out var groupEntity))
                    {
                        Console.WriteLine($@"Missing artist or group: {artist.StageName} - {groupName}");
                        continue;
                    }

                    var entity = new ArtistInGroup
                    {
                        Id = Guid.NewGuid(),
                        ArtistId = artistEntity.Id,
                        GroupId = groupEntity.Id,
                        CreatedBy = "System",
                        CreatedAt = DateTime.UtcNow
                    };
                    context.ArtistsInGroups.Add(entity);
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error: {ex.Message}");
                Console.WriteLine($@"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($@"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        if (!context.Albums.Any())
        {
            var artistDict = context.Artists.ToDictionary(a => a.StageName, a => a);
            var groupDict = context.Groups.ToDictionary(g => g.GroupName, g => g);

            var albumsToAdd = new List<Album>();

            foreach (var albumData in InitialData.Albums)
            {
                var album = new Album
                {
                    Id = albumData.id ?? Guid.NewGuid(),
                    Title = albumData.Title,
                    ReleaseDate = albumData.ReleaseDate.ToUniversalTime()
                };

                if (albumData.Artist != null)
                {
                    if (artistDict.TryGetValue(albumData.Artist, out var artistEntity))
                    {
                        album.ArtistId = artistEntity.Id;
                        albumsToAdd.Add(album);
                    }
                    else
                    {
                        Console.WriteLine($@"Missing artist: {albumData.Artist} for album: {albumData.Title}");
                    }
                }
                else if (albumData.Group != null)
                {
                    if (groupDict.TryGetValue(albumData.Group, out var groupEntity))
                    {
                        album.GroupId = groupEntity.Id;
                        albumsToAdd.Add(album);
                    }
                    else
                    {
                        Console.WriteLine($@"Missing group: {albumData.Group} for album: {albumData.Title}");
                    }
                }
                else
                {
                    Console.WriteLine($@"Album {albumData.Title} has neither artist nor group specified");
                }
            }

            context.Albums.AddRange(albumsToAdd);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error: {ex.Message}");
                Console.WriteLine($@"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($@"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        if (!context.AlbumVersions.Any())
        {
            var albums = context.Albums.ToDictionary(a => a.Title, a => a);
            
            var versionsToAdd = new List<AlbumVersion>();
            
            foreach (var versionData in InitialData.AlbumVersions)
            {
                var version = new AlbumVersion
                {
                    Id = versionData.id ?? Guid.NewGuid(),
                    VersionName = versionData.Name
                };
                
                if (albums.TryGetValue(versionData.albumName, out var albumEntity))
                {
                    version.AlbumId = albumEntity.Id;
                    versionsToAdd.Add(version);
                }
            }
            
            context.AlbumVersions.AddRange(versionsToAdd);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error: {ex.Message}");
                Console.WriteLine($@"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($@"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        if (!context.CardTypes.Any())
        {
            var cardTypes = InitialData.CardTypes.Select(g => new CardType
            {
                Id = g.id ?? Guid.NewGuid(),
                CardTypeName = g.name
            }).ToList();
            
            context.CardTypes.AddRange(cardTypes);
            
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error: {ex.Message}");
                Console.WriteLine($@"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($@"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
        
    }*/
}