using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class ToDoItemsControllerTests
    {
        private readonly TodoItemsController _sut;
        private readonly IToDoService _toDoService = Substitute.For<IToDoService>();

        public ToDoItemsControllerTests()
        {
            _sut = new TodoItemsController(_toDoService);
        }

        [Fact]
        public async Task GetById_ReturnOkAndObject_WhenToDoItemExists()
        {
            // Arrange
            var toDoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Buy milk and bread",
                IsCompleted = false,
            };
            _toDoService.GetToDoItemAsync(toDoItem.Id).Returns(toDoItem);
            var responseItem = toDoItem;

            // Act
            var result = (OkObjectResult)await _sut.GetTodoItem(toDoItem.Id);

            // Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(responseItem);
        }

        [Fact]
        public async Task GetById_ReturnNotFound_WhenToDoItemDoesNotExist()
        {
            // Arrange
            _toDoService.GetToDoItemAsync(Arg.Any<Guid>()).ReturnsNull();

            // Act
            var result = (NotFoundResult)await _sut.GetTodoItem(Guid.NewGuid());

            // Assert
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoToDoItemsExist()
        {
            // Arrange
            _toDoService.GetToDoItemsAsync().Returns(Enumerable.Empty<TodoItem>());

            // Act
            var result = (OkObjectResult)await _sut.GetTodoItems();

            // Assert
            result.StatusCode.Should().Be(200);
            result.Value.As<IEnumerable<TodoItem>>().Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturnToDoList_WhenToDoItemsExist()
        {
            // Arrange
            var toDoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Buy milk and bread",
                IsCompleted = false,
            };
            var toDoItems = new[] {toDoItem };
            var itemResponse = toDoItems;
            _toDoService.GetToDoItemsAsync().Returns(toDoItems);

            // Act
            var result = (OkObjectResult)await _sut.GetTodoItems();

            // Assert
            result.StatusCode.Should().Be(200);
            result.Value.As<IEnumerable<TodoItem>>().Should().BeEquivalentTo(itemResponse);
        }

        [Fact]
        public async Task Create_ShouldCreateToDo_WhenCreateToDoRequestIsValid()
        {
            // Arrange
            var toDoRequest = new TodoItem
            {
                Description = "Created item",
                IsCompleted = false,
            };
            var toDoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = toDoRequest.Description,
                IsCompleted = toDoRequest.IsCompleted
            };
            _toDoService.CreateToDoItemAsync(Arg.Do<TodoItem>(x => toDoItem = x)).Returns(true);

            // Act
            var result = (CreatedAtActionResult)await _sut.PostTodoItem(toDoRequest);

            // Assert
            result.StatusCode.Should().Be(201);
            result.Value.As<TodoItem>().Should().BeEquivalentTo(toDoItem);
            result.RouteValues!["id"].Should().BeEquivalentTo(toDoItem.Id);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenCreateToDoRequestIsNotValid()
        {
            // Arrange
            _toDoService.CreateToDoItemAsync(Arg.Any<TodoItem>()).Returns(false);

            // Act
            var result = (BadRequestObjectResult)await _sut.PostTodoItem(new TodoItem());

            // Assert
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenCreateToDoRequestHasNoDescription()
        {
            // Arrange
            _toDoService.CreateToDoItemAsync(Arg.Any<TodoItem>()).Returns(false);

            // Act
            var result = (BadRequestObjectResult)await _sut.PostTodoItem(new TodoItem());

            // Assert
            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("Description is required");
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenCreateToDoRequestHasSameDescription()
        {
            // Arrange
            var toDoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Buy milk and bread",
                IsCompleted = false,
            };
            var toDoItems = new[] { toDoItem };
            var invalidItem = new TodoItem
            {
                Description = toDoItem.Description,
                IsCompleted = false,
            };

            _toDoService.CreateToDoItemAsync(Arg.Any<TodoItem>()).Returns(false);


            // Act
            var result = (BadRequestResult)await _sut.PostTodoItem(invalidItem);

            // Assert
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenToDoItemWasDeleted()
        {
            // Arrange
            var toDoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Buy milk and bread",
                IsCompleted = false,
            };
            _toDoService.GetToDoItemAsync(toDoItem.Id).Returns(toDoItem);
            _toDoService.DeleteToDoItemAsync(Arg.Any<Guid>()).Returns(true);

            // Act
            var result = (OkObjectResult)await _sut.DeleteTodoItem(toDoItem.Id);

            // Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be($"Deleted to do item with ID: {toDoItem.Id}");
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenNoToDoItemWasDeleted()
        {
            // Arrange
            _toDoService.GetToDoItemAsync(Arg.Any<Guid>()).ReturnsNull();
            _toDoService.DeleteToDoItemAsync(Arg.Any<Guid>()).Returns(false);

            // Act
            var result = (NotFoundObjectResult)await _sut.DeleteTodoItem(Guid.NewGuid());

            // Assert
            result.StatusCode.Should().Be(404);
            result.Value.Should().Be("To do item not found");
        }
    }
}
