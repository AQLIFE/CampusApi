
using Microsoft.EntityFrameworkCore;
using ApiDB.Model;
using System.Diagnostics.CodeAnalysis;

namespace ApiDB.Comon
{
    public class WebContext : DbContext
    {

        public DbSet<Parts>? Part { get; set; }

        public WebContext(DbContextOptions<WebContext> options)
            : base(options)
        {
        }

        /*
         *  protected override void OnModelCreating(ModelBuilder modelBuilder)
         *  {
         *     modelBuilder.Entity<SystemPart>().ToTable("SystemPart");//指定表名称
         *  }
         *
         *  设置模型对应的实体表
         */

    }
}
