using Microsoft.EntityFrameworkCore;

namespace Task.Model
{
    public class productContext:DbContext
    {
        public productContext(DbContextOptions<productContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
