using Microsoft.AspNetCore.Mvc;
using PflegeVerbundhilfe.TaskManagement.DataTransferObjects;
using PflegeVerbundhilfe.TaskManagement.Repositories;

namespace PflegeVerbundhilfe.TaskManagement
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskController> _logger;
        public TaskController(ILogger<TaskController> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        // POST: api/Tasks/
        [HttpPost(Name = nameof(CreateTask))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<TodoTask>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Bad Request : Task payload data not provided correctly.!");
                return BadRequest(ModelState);
            }

            if (createTaskDto.Deadline < DateTime.UtcNow)
            {
                _logger.LogError("Bad Request : The due date cannot be in the past.!");
                return BadRequest("The due date cannot be in the past.");
            }

            var task = new TodoTask
            {
                Description = createTaskDto.Description,
                Deadline = createTaskDto.Deadline,
                IsDone = false,
                IsOverdue = createTaskDto.Deadline < DateTime.UtcNow && !createTaskDto.isDone
            };

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveAsync();

            _logger.LogError($"Task created at path : {nameof(GetTaskById)} with id : {task.Id}");
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }


        // POST: api/Tasks
        [HttpDelete(Name = nameof(DeleteTasks))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteTasks([FromBody] int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                _logger.LogError($"Request body is empty. Please provide task id's.");
                return BadRequest("No task IDs provided.");
            }

            foreach (var id in ids)
            {
                var task = await _taskRepository.GetByIdAsync(id);
                
                if(task != null)
                    await _taskRepository.DeleteAsync(id);
                else
                    _logger.LogError($"DeleteTask : Task with id : {id}. not found!");
            }

            await _taskRepository.SaveAsync();
            return NoContent();
        }                

        // PUT: api/Tasks/{id}/complete
        [HttpPut("complete", Name = nameof(MarkTasksAsCompleted))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MarkTasksAsCompleted([FromBody] int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                _logger.LogError($"Request body is empty. Please provide task id's.");
                return BadRequest("No task IDs provided.");
            }

            foreach (var id in ids)
            {
                await _taskRepository.MarkAsCompletedAsync(id);               
            }

            await _taskRepository.SaveAsync();
            return NoContent();
        }


        [HttpGet("{id}", Name = nameof(GetTaskById))]
        [ProducesResponseType(302)]
        [ProducesResponseType(404)]
        //GET : api/Tasks/id
        public async Task<ActionResult<TodoTask>> GetTaskById(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                _logger.LogError($"Not Found : No task found with id {id}.");
                return NotFound();
            }

            return task;
        }

        // GET: api/Tasks/completed
        [HttpGet("completed", Name = nameof(GetCompletedTasks))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Task>>> GetCompletedTasks()
        {
            return Ok(await _taskRepository.GetCompletedTasksAsync());
        }

        // GET: api/Tasks/overdue
        [HttpGet("overdue", Name = nameof(GetOverdueTasks))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<TodoTask>>> GetOverdueTasks()
        {
            return Ok(await _taskRepository.GetOverdueTasksAsync());
        }

        // GET: api/Tasks/in-progress
        [HttpGet("in-progress", Name = nameof(GetInProgressTasks))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Task>>> GetInProgressTasks()
        {
            return Ok(await _taskRepository.GetInProgressTasksAsync());
        }

        // GET: api/Tasks
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [HttpGet(Name = nameof(GetAllTasks))]

        public async Task<ActionResult<IEnumerable<TodoTask>>> GetAllTasks()
        {
            var allTasks = await _taskRepository.GetAllTasks();
            
            return Ok(allTasks);
        }

        [HttpGet("page", Name = nameof(GetPage))]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PagedRecord>> GetPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _taskRepository.GetPagedRecordsAsync(pageNumber, pageSize));            
        }
    }
}
