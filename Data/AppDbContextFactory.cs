using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IT15_LabExam.Data
{
    // Allows EF tools to create migrations without a live DB connection
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Port=3306;Database=techcore_payroll;User=root;Password=;",
                new MySqlServerVersion(new Version(8, 0, 0)));
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
