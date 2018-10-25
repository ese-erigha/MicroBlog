using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MicroBlog.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity;

namespace MicroBlog.Core
{
    public class DatabaseContext : IdentityDbContext<Entities.ApplicationUser, IdentityRole<long>,long>
    {
        readonly DateTime myDate = DateTime.ParseExact("1000-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        const string _isDeletedProperty = "IsDeleted";
        static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(bool));

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate(); //Creates the database on first initialization
        }

        public DatabaseContext() : base()
        {
            Database.Migrate(); //Creates the database on first initialization
        }

        static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod, parm, Expression.Constant(_isDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Entities.Comment>()
                   .HasOne(c => c.Post)
                   .WithMany(i => i.Comments)
                   .HasForeignKey(c => c.PostId);

            builder.Entity<Entities.Comment>()
                   .HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId);

            builder.Entity<Entities.Post>()
                   .HasOne(p => p.User)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(p => p.UserId);
                    

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType) == true)
                {
                    entity.GetOrAddProperty(_isDeletedProperty, typeof(bool));

                    builder.Entity(entity.ClrType)
                                .HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }

            base.OnModelCreating(builder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {

            OnBeforeSave();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSave();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        void OnBeforeSave()
        {
            SoftDelete();
            AddTimestamps();
        }

        void AddTimestamps()
        {
            var entries = ChangeTracker.Entries();
            var now = DateTime.UtcNow;
            foreach (var entry in entries)
            {

                if (entry.Entity is Entities.BaseEntity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entity.UpdatedAt = now;
                            break;

                        case EntityState.Added:
                            entity.CreatedAt = now;
                            entity.UpdatedAt = now;
                            break;
                    }
                }
            }
        }

        void SoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
            {
                if (entry.Entity is ISoftDeletable)
                {
                    entry.Property(_isDeletedProperty).CurrentValue = true;
                    entry.State = EntityState.Modified;
                }
            }
        }
    }
}
