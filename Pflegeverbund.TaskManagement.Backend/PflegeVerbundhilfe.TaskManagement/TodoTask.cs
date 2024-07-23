using System.ComponentModel.DataAnnotations;

namespace PflegeVerbundhilfe.TaskManagement
{     
    public class TodoTask
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public bool IsOverdue { get; set; }

        [Required]
        public bool IsDone { get; set; }        
    }
}