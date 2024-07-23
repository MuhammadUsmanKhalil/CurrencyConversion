using System.ComponentModel.DataAnnotations;

namespace PflegeVerbundhilfe.TaskManagement.DataTransferObjects
{
    public class CreateTaskDto
    {
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public bool isDone { get; set; }
    }
}
