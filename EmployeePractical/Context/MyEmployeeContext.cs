using EmployeePractical.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeePractical.Context
{
    public class MyEmployeeContext : DbContext
    {
        public MyEmployeeContext(DbContextOptions<MyEmployeeContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Employee> GetEmployees { get; set; }

    }
}
