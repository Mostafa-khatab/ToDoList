using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoListController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ToDoListController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyToDoLists()
        {
            var userName = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User not authenticated.");
            }
            var all = await _db.ToDos.Where(us => us.UserName == userName).ToListAsync();

            var res = all.Select(item => new dtoToDo
            {
                Id = item.Id,
                Description = item.Description,
                StatusId = item.StatusId,
                CategoryId = item.CategoryId,
                DueDate = item.DueDate
            }).ToList();

            if (res == null || !res.Any())
            {
                return NotFound($"No ToDo items found for user '{userName}'.");
            }

            return Ok(res);
        }

        [HttpGet("GetAllListesHaveSatusId/{StatusId}")]

        public async Task<IActionResult> GetAllSpasificStatus(string StatusId)
        {
            try
            {
                var userName = User?.Identity?.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User not authenticated.");
                }

                var exist = await _db.Status.FindAsync(StatusId);

                if (exist == null)
                    return BadRequest("Enter Valid Status'{open , closed}'");

                var all = await _db.ToDos.Where(x => x.StatusId == StatusId).Where(us => us.UserName == userName).ToListAsync();
                var res = all.Select(item => new dtoToDo
                {
                    Id = item.Id,
                    Description = item.Description,
                    StatusId = item.StatusId,
                    CategoryId = item.CategoryId,
                    DueDate = item.DueDate
                }).ToList(); 
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllListesHaveCategoryId/{CategoryId}")]

        public async Task<IActionResult> GetAllListesHaveCategoryId(string CategoryId)
        {
            try
            {
                var exist = await _db.Categories.FindAsync(CategoryId);
                if (exist == null)
                    return BadRequest("Enter Valid Status'{work , home , ex , shop , call}'");
                var userName = User?.Identity?.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User not authenticated.");
                }
                var all = await _db.ToDos.Where(x => x.CategoryId == CategoryId).Where(us => us.UserName == userName).ToListAsync();
                var res = all.Select(item => new dtoToDo
                {
                    Id = item.Id,
                    Description = item.Description,
                    StatusId = item.StatusId,
                    CategoryId = item.CategoryId,
                    DueDate = item.DueDate
                }).ToList();
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddList(dtoToDo dto)
        {
            try
            {
                var userName = User?.Identity?.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User not authenticated.");
                }

                var entity = new ToDo
                {
                    Description = dto.Description,
                    DueDate = dto.DueDate,
                    CategoryId = dto.CategoryId,
                    StatusId = dto.StatusId,
                    UserName = userName
                };
                if (string.IsNullOrWhiteSpace(entity.Description) ||
                    string.IsNullOrWhiteSpace(entity.CategoryId) ||
                    string.IsNullOrWhiteSpace(entity.StatusId))
                {
                    return BadRequest("All fields are required.");
                }

                if (entity.Overdue == true)
                {
                    return BadRequest("Cannot add a task marked 'open' that is already overdue.");
                }

                var exist1 = await _db.Status.FindAsync(entity.StatusId);
                var exist2 = await _db.Categories.FindAsync(entity.CategoryId);

                if(exist1 == null)
                {
                    return NotFound("There is no status id with this name");
                }

                if (exist2 == null)
                {
                    return NotFound("There is no category id with this name");
                }

                await _db.ToDos.AddAsync(entity);
                await _db.SaveChangesAsync();

                return Ok(new { dto, Message = "Task added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }

        }

        [HttpPut]

        public async Task<IActionResult> UpdateList(dtoToDo dto)
        {
            try
            {
                var exist = await _db.ToDos.FindAsync(dto.Id);
                if (exist == null)
                    return NotFound("This id not exist");

                var entity = new ToDo
                {
                    Description = dto.Description,
                    DueDate = dto.DueDate,
                    CategoryId = dto.CategoryId,
                    StatusId = dto.StatusId
                };
                if (string.IsNullOrWhiteSpace(entity.Description) ||
                        string.IsNullOrWhiteSpace(entity.CategoryId) ||
                        string.IsNullOrWhiteSpace(entity.StatusId))
                {
                    return BadRequest("All fields are required.");
                }

                if (entity.Overdue == true)
                {
                    return BadRequest("Cannot add a task marked 'open' that is already overdue.");
                }

                var exist1 = await _db.Status.FindAsync(entity.StatusId);
                var exist2 = await _db.Categories.FindAsync(entity.CategoryId);

                if (exist1 == null)
                {
                    return NotFound("There is no status id with this name");
                }

                if (exist2 == null)
                {
                    return NotFound("There is no category id with this name");
                }
                exist.Description = entity.Description;
                exist.CategoryId = entity.CategoryId;
                exist.StatusId = entity.StatusId;
                exist.DueDate = entity.DueDate;
                _db.ToDos.Update(exist);
                await _db.SaveChangesAsync();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteList(int id)
        {
            var exist = await _db.ToDos.FindAsync(id);
            if (exist == null)
                return NotFound("This id not exist");

            _db.ToDos.Remove(exist);
            await _db.SaveChangesAsync();
            return Ok(id);
        }
    }
}
