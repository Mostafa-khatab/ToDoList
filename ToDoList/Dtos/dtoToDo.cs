using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class dtoToDo
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public string CategoryId { get; set; }
        [Required]
        public string StatusId { get; set; }
    }
}
