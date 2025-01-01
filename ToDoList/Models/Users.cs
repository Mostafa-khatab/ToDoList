using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Users
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        
    }
}
