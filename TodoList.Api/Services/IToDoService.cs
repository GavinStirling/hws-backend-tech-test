using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Services
{
    public interface IToDoService
    {
        Task<IEnumerable<TodoItem>> GetToDoItemsAsync();
        Task<TodoItem> GetToDoItemAsync(Guid id);
        Task<bool> UpdateToDoItemAsync(Guid id, TodoItem item);
        Task<bool> CreateToDoItemAsync(TodoItem item);
        Task<bool> DeleteToDoItemAsync(Guid id);
    }
}
