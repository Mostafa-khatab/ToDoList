using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public interface ITokenServices
    {
        public string CreatToken(AppUser user);
    }
}