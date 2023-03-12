using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TodoList.Api.Models;

namespace TodoList.Api.Repository
{
    public interface IToDoRepository
    {
        Task<IEnumerable<TodoItem>> GetToDoItemsAsync();
        Task<TodoItem> GetToDoItemAsync(Guid id);
        Task<bool> UpdateToDoItemAsync(TodoItem item);
        Task<bool> CreateToDoItemAsync(TodoItem item);
        Task<bool> DeleteToDoItemAsync(Guid id);
    }
}
