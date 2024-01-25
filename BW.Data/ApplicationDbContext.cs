using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BW.Data.Entities;

namespace BW.Data;

public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<BookEntity> Books { get; set; }
    public DbSet<BookSubjectEntity> BooksToSubjests { get; set; }

    public DbSet<LibraryEntity> Libraries { get; set; }

    public DbSet<RatingEntity> Ratings { get; set; }

    public DbSet<SubjectEntity> Subjects { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);


    //     modelBuilder.Entity<UserEntity>().ToTable("Users");
    //     modelBuilder.Entity<PostEntity>().HasOne(n => n.Author).WithMany(u => u.Posts).HasForeignKey(n => n.AuthorId);
    //     modelBuilder.Entity<CommentEntity>().HasOne(n => n.Author).WithMany().HasForeignKey(n => n.AuthorId);
    //     modelBuilder.Entity<CommentEntity>().HasOne(n => n.Post).WithMany(u => u.Comments).HasForeignKey(n => n.PostId);
    //     modelBuilder.Entity<ReplyEntity>().HasOne(n => n.Author).WithMany().HasForeignKey(n => n.AuthorId);
    //     modelBuilder.Entity<ReplyEntity>().HasOne(n => n.Parent).WithMany().HasForeignKey(n => n.ParentId);
    //     modelBuilder.Entity<PostEntity>().HasOne(n => n.Author).HasForeignKey(n => n.AuthorId);
    // }
}