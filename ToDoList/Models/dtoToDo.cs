using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class dtoToDo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string CategoryId { get; set; }
        public string StatusId { get; set; }
    }
}
