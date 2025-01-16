using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
    
    public DbSet<AppUser> Users { get; set; }


   

    
}
