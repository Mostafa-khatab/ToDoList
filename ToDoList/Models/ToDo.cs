using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter a due date")]
        public DateTime DueDate { get; set; }
        [Required(ErrorMessage = "Please Enter a category")]
        public string CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required(ErrorMessage = "Please Enter a Status")]
        public string StatusId { get; set; }
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        public string UserName { get; set; }
        [ForeignKey("UserName")]
        public Users User { get; set; }
        public bool Overdue => StatusId == "open" && DueDate < DateTime.Today;

    }
}
