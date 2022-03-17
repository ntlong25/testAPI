using WebBlog.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBlog.Data
{
    public class DataContext : DbContext
    {
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=NTL;Database=BlogManagement;User Id=sa;Password=long2502;");
                //optionsBuilder.UseSqlServer(@"Server=CVPLONGNT62-5\SQLEXPRESS;Database=BlogAPI;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Fluent API with User table
            modelBuilder.Entity<User>(
                user =>
                {
                    user.HasKey(u => u.ID);
                    user.Property(u => u.ID).HasMaxLength(20).UseIdentityColumn();
                    user.Property(u => u.firstName).HasMaxLength(50);
                    user.Property(u => u.middleName).HasMaxLength(50);
                    user.Property(u => u.lastName).HasMaxLength(50);
                    user.Property(u => u.phoneNumber).HasMaxLength(15);
                    user.HasIndex(u => u.phoneNumber).IsUnique();
                    user.Property(u => u.email).HasMaxLength(50);
                    user.HasIndex(u => u.email).IsUnique();
                    user.Property(u => u.passwordHash).HasMaxLength(32);
                    user.Property(u => u.profile).HasColumnType("TEXT");
                });
            // Fluent API with Category table
            modelBuilder.Entity<Category>(
                cate =>
                {
                    cate.HasKey(c => c.ID);
                    cate.Property(c => c.ID).HasMaxLength(20).UseIdentityColumn();
                    cate.HasOne(c => c.Parent).WithMany().HasForeignKey(c => c.parentId);
                    cate.Property(c => c.parentId).HasMaxLength(20);
                    cate.Property(c => c.title).HasMaxLength(75);
                    cate.Property(c => c.metaTitle).HasMaxLength(100);
                    cate.Property(c => c.slug).HasMaxLength(100);
                    cate.Ignore(c => c.PostCategories);
                });
            // Fluent API with Post table
            modelBuilder.Entity<Post>(
                post =>
                {
                    post.HasKey(p => p.ID);
                    post.Property(p => p.ID).HasMaxLength(20).UseIdentityColumn();
                    post.HasOne(u => u.User).WithMany(p => p.Posts)
                                            .HasForeignKey(u => u.authorId)
                                            .OnDelete(DeleteBehavior.NoAction);
                    //post.Property(p => p.authorId).HasMaxLength(20);
                    post.HasOne(p => p.Parent).WithMany().HasForeignKey(p => p.parentId);
                    post.Property(p => p.parentId).HasMaxLength(20);
                    post.Property(p => p.title).HasMaxLength(75);
                    post.Property(p => p.metaTitle).HasMaxLength(100);
                    post.Property(p => p.slug).HasMaxLength(100);
                    post.HasIndex(u => u.slug).IsUnique();
                    post.Property(p => p.published).HasMaxLength(1).HasColumnType("TINYINT");
                    post.Property(p => p.content).HasColumnType("TEXT");
                    post.Ignore(p => p.PostTags); // NotMapped
                    post.Ignore(p => p.PostCategories);
                });
            // Fluent API with PostComment table
            modelBuilder.Entity<PostComment>(
                post =>
                {
                    post.HasKey(pcm => pcm.ID);
                    post.Property(p => p.ID).HasMaxLength(20).UseIdentityColumn();
                    post.HasOne(u => u.Post).WithMany(p => p.PostComments).HasForeignKey(u => u.postId);
                    post.HasOne(u => u.User).WithMany(p => p.PostComments).HasForeignKey(u => u.userId);
                    post.Property(p => p.postId).HasMaxLength(20);
                    post.HasOne(p => p.Parent).WithMany().HasForeignKey(p => p.parentId);
                    post.Property(p => p.parentId).HasMaxLength(20);
                    post.Property(p => p.title).HasMaxLength(100);
                    post.Property(p => p.published).HasMaxLength(1).HasColumnType("TINYINT");
                    post.Property(p => p.content).HasColumnType("TEXT");
                });
            // Fluent API with PostCategory table
            modelBuilder.Entity<PostCategory>(
                post =>
                {
                    post.HasKey(nameof(PostCategory.postId), nameof(PostCategory.categoryId));
                    post.HasOne(pct => pct.Post).WithMany(p => p.PostCategories).HasForeignKey(pct => pct.postId);
                    post.Property(p => p.postId).HasMaxLength(20);
                    post.HasOne(pct => pct.Category).WithMany(p => p.PostCategories).HasForeignKey(pct => pct.categoryId);
                    post.Property(p => p.categoryId).HasMaxLength(20);
                });
            // Fluent API with PostMeta table
            modelBuilder.Entity<PostMeta>(
                post =>
                {
                    post.HasKey(p => p.ID);
                    post.Property(p => p.ID).HasMaxLength(20).UseIdentityColumn();
                    post.HasOne(pm => pm.Post).WithMany(p => p.PostMetas).HasForeignKey(pm => pm.postId);
                    post.Property(p => p.postId).HasMaxLength(20);
                    post.HasIndex(p => p.postId).IsUnique();
                    post.Property(p => p.key).HasMaxLength(50);
                    post.Property(p => p.content).HasColumnType("TEXT");
                });
            // Fluent API with PostTag table
            modelBuilder.Entity<PostTag>(
                post =>
                {
                    post.HasKey(nameof(PostTag.postId), nameof(PostTag.tagId));
                    post.HasOne(pct => pct.Post).WithMany(p => p.PostTags).HasForeignKey(pct => pct.postId);
                    post.Property(p => p.postId).HasMaxLength(20);
                    post.HasOne(pct => pct.Tag).WithMany(p => p.PostTags).HasForeignKey(pct => pct.tagId);
                    post.Property(p => p.tagId).HasMaxLength(20);
                });
            // Fluent API with Tag table
            modelBuilder.Entity<Tag>(
                tag =>
                {
                    tag.HasKey(p => p.ID);
                    tag.Property(p => p.ID).HasMaxLength(20).UseIdentityColumn();
                    tag.Property(p => p.title).HasMaxLength(75);
                    tag.Property(p => p.metaTitle).HasMaxLength(100);
                    tag.Property(p => p.slug).HasMaxLength(100);
                    tag.Property(p => p.content).HasColumnType("TEXT");
                    tag.Ignore(t => t.PostTags);
                });
            
//            modelBuilder.Entity<User>().HasData(
//                new User
//                {
//                    ID = 1,
//                    firstName = "Long",
//                    middleName = "Thanh",
//                    lastName = "Nguyen",
//                    mobile = "0334052502",
//                    email = "long20002000ht@gmail.com",
//                    passwordHash = "long2000",
//                    registeredAt = Convert.ToDateTime("2022/02/23"),
//                    lastLogin = Convert.ToDateTime("2022/02/23"),
//                    intro = "abcdefgh",
//                    profile = "Author"
//                },
//                new User 
//                {
//                    ID=2,
//                    firstName = "Huy",
//                    middleName = "",
//                    lastName = "Nguyen",
//                    mobile = "0123456789",
//                    email = "huy@gmail.com",
//                    passwordHash = "huy123",
//                    registeredAt = Convert.ToDateTime("2022/02/23"),
//                    lastLogin = Convert.ToDateTime("2022/02/23"),
//                    intro = "abcdefgh",
//                    profile = "Author"
//                },
//                new User
//                {
//                    ID = 3,
//                    firstName = "Quan",
//                    middleName = "Manh",
//                    lastName = "Phan",
//                    mobile = "0987654321",
//                    email = "quan@gmail.com",
//                    passwordHash = "quan123",
//                    registeredAt = Convert.ToDateTime("2022/02/23"),
//                    lastLogin = Convert.ToDateTime("2022/02/23"),
//                    intro = "abcdefgh",
//                    profile = "Person"
//                });
//            modelBuilder.Entity<Post>().HasData(
//                    new Post
//                    {
//                        ID = 1,
//                        authorId = 1,
//                        title = "Ha Noi dep",
//                        metaTitle = "Ha Noi dep qua haha",
//                        slug = "ABC",
//                        summary = "AAA",
//                        published = 2,
//                        createAt = Convert.ToDateTime("2022/01/31"),
//                        updateAt = Convert.ToDateTime("2022/02/10"),
//                        publishedAt = Convert.ToDateTime("2022/02/02"),
//                        content = @"It provides a complete guide to designing a database schema in MySQL 
//to manage the users and blog posts of a blogging platform. 
//The database design can be used to further develop the blog website or mobile application."
//                    }
//                );
//            modelBuilder.Entity<PostComment>().HasData(
//                    new PostComment
//                    {
//                        ID = 1,
//                        postId = 1,
//                        title = "Qua hay",
//                        published = 1,
//                        createAt = Convert.ToDateTime("2022/02/05"),
//                        publishedAt = Convert.ToDateTime("2022/02/05"),
//                        content = "Bai viet qua hay"
//                    }
//                );
//            modelBuilder.Entity<Tag>().HasData(
//                    new Tag
//                    {
//                        ID = 1,
//                        title = "Qua hay",
//                        metaTitle = "Bai viet qua hay",
//                        slug = "Slug la gi???",
//                        content = "Bai viet qua hay. Good chop"
//                    }
//                );
//            modelBuilder.Entity<PostMeta>().HasData(
//                    new PostMeta
//                    {
//                        ID = 1,
//                        postId = 1,
//                        key = "Day la post meta",
//                        content = "Content cua PostMeta"
//                    }
//                );
//            modelBuilder.Entity<Category>().HasData(
//                new Category
//                {
//                    ID = 1,
//                    title = "News",
//                    metaTitle = "Danh muc News",
//                    slug = "Slug cua Category",
//                    content = "content cua Category :D"
//                });
//            modelBuilder.Entity<PostTag>().HasData(
//                    new PostTag
//                    {
//                        postId = 1,
//                        tagId = 1
//                    }
//                );
//            modelBuilder.Entity<PostCategory>().HasData(
//                    new PostCategory
//                    {
//                        postId = 1,
//                        categoryId = 1
//                    }
//                );
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostComment> PostComments { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<PostTag> PostTags { get; set; }
        public virtual DbSet<PostMeta> PostMetas { get; set; }
        public virtual DbSet<PostCategory> PostCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
