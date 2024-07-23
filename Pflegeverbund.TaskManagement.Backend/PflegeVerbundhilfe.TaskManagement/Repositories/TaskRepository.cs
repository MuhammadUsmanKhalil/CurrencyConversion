using Microsoft.EntityFrameworkCore;
using PflegeVerbundhilfe.TaskManagement.DataTransferObjects;

namespace PflegeVerbundhilfe.TaskManagement.Repositories
{
    public interface ITaskRepository
    {
        Task AddAsync(TodoTask task);
        Task DeleteAsync(int id);
        Task<TodoTask> GetByIdAsync(int id);
        Task MarkAsCompletedAsync(int id);
        Task<List<TodoTask>> GetCompletedTasksAsync();
        Task<List<TodoTask>> GetOverdueTasksAsync();
        Task<List<TodoTask>> GetInProgressTasksAsync();
        Task<List<TodoTask>> GetAllTasks();

        Task<PagedRecord> GetPagedRecordsAsync(int pageNumber, int pageSize);

        Task SaveAsync();
    }

    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);

            if (task != null)
            {
                _context.TodoTasks.Remove(task);
            }
        }

        public async Task AddAsync(TodoTask task)
        {
            await _context.TodoTasks.AddAsync(task);
        }

        public async Task<TodoTask> GetByIdAsync(int id)
        {
            return _context.TodoTasks.Any() ? await _context.TodoTasks.FindAsync(id) : await Task.FromResult<TodoTask>(null);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsCompletedAsync(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);

            if (task != null)
            {
                task.IsDone = true;

                _context.TodoTasks.Update(task);

                _context.Entry(task).State = EntityState.Modified;
            }

            return;
        }

        public async Task<List<TodoTask>> GetCompletedTasksAsync()
        {
            return await _context.TodoTasks.Where(t => t.IsDone).ToListAsync();
        }

        public async Task<List<TodoTask>> GetOverdueTasksAsync()
        {
            return await _context.TodoTasks.Where(t => !t.IsDone && t.Deadline < DateTime.Now).ToListAsync();
        }

        public async Task<List<TodoTask>> GetInProgressTasksAsync()
        {
            return await _context.TodoTasks.Where(t => !t.IsDone && t.Deadline >= DateTime.Now).ToListAsync();
        }

        public async Task<List<TodoTask>> GetAllTasks()
        {
            return await _context.TodoTasks.ToListAsync();
        }

        public async Task<PagedRecord> GetPagedRecordsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.TodoTasks.CountAsync();
            var items = await _context.TodoTasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedRecord(totalItems, pageNumber, pageSize, items);            
        }
    }
}
