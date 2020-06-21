using System;

using DiscussionWeb.Data.Models;
using DiscussionWeb.Models;

using Microsoft.EntityFrameworkCore;

namespace DiscussionWeb.Data.DbContexts
{
	public class DiscussionWebDbContext : DbContext
	{
		public DiscussionWebDbContext(DbContextOptions<DiscussionWebDbContext> options) : base(options)
		{
		}

		public DbSet<Author> Authors { get; set; }

		public DbSet<Post> Posts { get; set; }

		public DbSet<Thread> Threads { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Author>().HasData(
				new Author()
				{
					Id = Guid.Parse("a28888e9-2ba9-473a-a40f-e38cb54f9b35"),
					FirstName = "Tony",
					LastName = "Stark",
					NickName = "Ironman",
					RegistrationEmail = "iron@man.cz",
					Permission = AccessLevel.Admin,
					Registered = DateTime.UtcNow,
				},
				new Author()
				{
					Id = Guid.Parse("b28888e9-2ba9-473a-a40f-e38cb54f9b35"),
					FirstName = "Bruce",
					LastName = "Wayne",
					NickName = "Batman",
					RegistrationEmail = "bat@man.cz",
					Permission = AccessLevel.Admin,
					Registered = DateTime.UtcNow,
				}
				);
			modelBuilder.Entity<Author>().HasMany(a => a.Threads).WithOne(numberOfThreads => numberOfThreads.Author);
			modelBuilder.Entity<Author>().HasMany(a => a.Posts).WithOne(posts => posts.Author);

			modelBuilder.Entity<Thread>().HasData(
				new Thread()
				{
					Id = Guid.Parse("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
					AuthorId = Guid.Parse("a28888e9-2ba9-473a-a40f-e38cb54f9b35"),
					Title = "General Discussion",
					Description = "A general discussion about life universe and everything",
					Posted = DateTime.UtcNow,
					LastEdited = DateTime.UtcNow,
					NumberOfEdits = 0,
				},
				new Thread()
				{
					Id = Guid.Parse("d173e20d-159e-4127-9ce9-b0ac2564ad97"),
					AuthorId = Guid.Parse("b28888e9-2ba9-473a-a40f-e38cb54f9b35"),
					Title = "Csharp Discussion",
					Description = "A general discussion about C#.",
					Posted = DateTime.UtcNow,
					LastEdited = DateTime.UtcNow,
					NumberOfEdits = 0,
				}
				);
			modelBuilder.Entity<Thread>().HasMany(dt => dt.Posts).WithOne(posts => posts.Thread);

			modelBuilder.Entity<Post>().HasData(
				new Post()
				{
					Id = Guid.Parse("28c1db41-f104-46e6-8943-d31c0291e0e3"),
					ThreadId = Guid.Parse("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
					Message = "Hello world!",
					Posted = DateTime.UtcNow,
					LastEdited = DateTime.UtcNow,
					NumberOfEdits = 0
				},
				new Post()
				{
					Id = Guid.Parse("d94a64c2-2e8f-4162-9976-0ffe03d30767"),
					ThreadId = Guid.Parse("d173e20d-159e-4127-9ce9-b0ac2564ad97"),
					Message = "Hello csharp!",
					Posted = DateTime.UtcNow,
					LastEdited = DateTime.UtcNow,
					NumberOfEdits = 0
				}
				);

			base.OnModelCreating(modelBuilder);
		}
	}
}
