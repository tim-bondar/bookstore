using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Controllers;
using Core.Models;
using FakeItEasy;
using Features.Books.Commands;
using Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Tests.ControllerTests
{
    public class BooksControllerTests
    {
        private readonly BooksController _controller;
        private readonly IMediator _mediator;

        public BooksControllerTests()
        {
            _mediator = A.Fake<IMediator>();
            _controller = new BooksController(_mediator);
        }

        [Fact]
        public async Task Get_should_return_OK_result()
        {
            // Arrange
            var book = new BookModel
            {
                Id = Guid.NewGuid()
            };

            A.CallTo(() => _mediator.Send(A<SingleBookQuery>.Ignored, A<CancellationToken>.Ignored)).Returns(book);

            // Act
            var result = await _controller.Get(book.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => _mediator.Send(A<SingleBookQuery>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAll_should_return_OK_result()
        {
            // Arrange
            A.CallTo(() => _mediator.Send(A<AllBooksQuery>.Ignored, A<CancellationToken>.Ignored)).Returns(new List<BookModel>());

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => _mediator.Send(A<AllBooksQuery>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Create_should_return_Created_result()
        {
            // Arrange
            A.CallTo(() => _mediator.Send(A<AddBookCommand>.Ignored, A<CancellationToken>.Ignored)).Returns(new BookModel());

            // Act
            var result = await _controller.Create(new NewBookModel());

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            A.CallTo(() => _mediator.Send(A<AddBookCommand>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Update_should_return_OK_result()
        {
            // Arrange
            A.CallTo(() => _mediator.Send(A<UpdateBookCommand>.Ignored, A<CancellationToken>.Ignored)).Returns(new BookModel());

            // Act
            var result = await _controller.Update(Guid.Empty, new NewBookModel());

            // Assert
            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => _mediator.Send(A<UpdateBookCommand>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Delete_should_return_NoContent_result()
        {
            // Arrange

            // Act
            var result = await _controller.Delete(Guid.Empty);

            // Assert
            Assert.IsType<NoContentResult>(result);
            A.CallTo(() => _mediator.Send(A<DeleteBookCommand>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
