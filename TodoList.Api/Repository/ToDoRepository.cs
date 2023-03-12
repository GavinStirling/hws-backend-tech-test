using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TodoList.Api.Data;
using TodoList.Api.Models;

namespace TodoList.Api.Repository
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly TodoContext _todoContext;

        public ToDoRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }
        public async Task<IEnumerable<TodoItem>> GetToDoItemsAsync()
        {
            return  await _todoContext.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }
        public async Task<TodoItem> GetToDoItemAsync(Guid id)
        {
            return await _todoContext.TodoItems.FindAsync(id);
        }
        public async Task<bool> UpdateToDoItemAsync(TodoItem item)
        {
            _todoContext.TodoItems.Update(item);
            return await _todoContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateToDoItemAsync(TodoItem item)
        {
            _todoContext.TodoItems.Add(item);
            return await _todoContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteToDoItemAsync(Guid id)
        {
            var item = await GetToDoItemAsync(id);
            _todoContext.TodoItems.Remove(item);
            return await _todoContext.SaveChangesAsync() > 0;
        }
    }
}
