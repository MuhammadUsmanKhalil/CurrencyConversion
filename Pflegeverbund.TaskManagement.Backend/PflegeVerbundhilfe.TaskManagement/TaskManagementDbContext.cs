using Microsoft.EntityFrameworkCore;

namespace PflegeVerbundhilfe.TaskManagement
{
    public class TaskManagementDbContext : DbContext
    {
        public DbSet<TodoTask> TodoTasks { get; set; }

        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {
        }                
    }
}
