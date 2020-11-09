using Microsoft.EntityFrameworkCore;
using SimpleCore.Api.Domains;
using SimpleCore.Api.Domians;

namespace SimpleCore.Domains
{
    /// <summary>
    /// 
    /// </summary>
    public class MyDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("simple_account");
            modelBuilder.Entity<Func>().ToTable("simple_func");
            base.OnModelCreating(modelBuilder);

        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Account> Account { get; set; }

        public DbSet<Func> Func { get; set; }

    }
}
