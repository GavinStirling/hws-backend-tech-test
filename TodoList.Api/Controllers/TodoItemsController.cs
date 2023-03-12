using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Data;
using TodoList.Api.Models;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public TodoItemsController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _toDoService.GetToDoItemsAsync();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var result = await _toDoService.GetToDoItemAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            try
            {
                var updated = await _toDoService.UpdateToDoItemAsync(id, todoItem);
                if (!updated)
                {
                    return BadRequest();
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemIdExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        } 

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            if (string.IsNullOrEmpty(todoItem?.Description))
            {
                return BadRequest("Description is required");
            }
            else if (TodoItemDescriptionExists(todoItem.Description))
            {
                return BadRequest("Description already exists");
            }

            var created = await _toDoService.CreateToDoItemAsync(todoItem);
            if (!created)
            {
                return BadRequest();
            }
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // POST: api/TodoItems 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            if (await _toDoService.GetToDoItemAsync(id) == null)
            {
                return NotFound("To do item not found");
            }
            await _toDoService.DeleteToDoItemAsync(id);

            return Ok($"Deleted to do item with ID: {id}");
        }

        private bool TodoItemIdExists(Guid id)
        {
            return _toDoService.GetToDoItemAsync(id) != null;
        }

        private bool TodoItemDescriptionExists(string description)
        {
            return _toDoService.GetToDoItemsAsync().Result.Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
