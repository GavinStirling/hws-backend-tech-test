using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TodoList.Api.Logging;
using TodoList.Api.Models;
using TodoList.Api.Repository;

namespace TodoList.Api.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _todoRepository;
        private readonly ILoggerAdapter<ToDoService> _logger;

        public ToDoService(IToDoRepository toDoRepository, ILoggerAdapter<ToDoService> logger)
        {
            _todoRepository = toDoRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TodoItem>> GetToDoItemsAsync()
        {
            _logger.LogInformation("Retrieving all to do items.");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _todoRepository.GetToDoItemsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving all to do items");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("All to do items retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<TodoItem> GetToDoItemAsync(Guid id)
        {
            _logger.LogInformation("Retrieving to do item with id {0}", id);
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _todoRepository.GetToDoItemAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving to do item with id {0}", id);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("To do item with id {0} retrieved in {1}ms", id, stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<bool> UpdateToDoItemAsync(Guid id, TodoItem item)
        {
            _logger.LogInformation("Updating to do item with id {0}", id);
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _todoRepository.UpdateToDoItemAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while updating to do item with id {0}", id);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("To do item with id {0} updated in {1}ms",id, stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<bool> CreateToDoItemAsync(TodoItem item)
        {
            _logger.LogInformation("Creating to do item with description {0}", item.Description);
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _todoRepository.CreateToDoItemAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while creating the new to do item with id {0}", item.Id);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("To do item with id {0} created in {1}ms", item.Id, stopWatch.ElapsedMilliseconds);
            }
        }

        public async Task<bool> DeleteToDoItemAsync(Guid id)
        {
            _logger.LogInformation("Deleting to do item with id {0}", id);
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _todoRepository.DeleteToDoItemAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while deleting the to do item with id {0}", id);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("To do item with id {0} deleted in {1}ms", id, stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
