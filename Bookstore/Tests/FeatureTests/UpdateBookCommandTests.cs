using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using DB.Abstraction;
using DB.Entities;
using FakeItEasy;
using Features.Books.Commands;
using Features.Books.Validators;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.FeatureTests
{
    public class UpdateBookCommandTests
    {
        private readonly UpdateBookCommandHandler _handler;
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        public UpdateBookCommandTests()
        {
            _repository = A.Fake<IBooksRepository>();
            _mapper = Helpers.CreateMapper();
            _handler = new UpdateBookCommandHandler(_repository, _mapper, A.Fake<ILogger<UpdateBookCommandHandler>>());
        }

        [Fact]
        public async Task UpdateBookCommand_should_successfully_store_correct_book()
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/jpeg";

            var newBook = new NewBookModel
            {
                Author = "Correct",
                Title = "Correct",
                Description = "Correct",
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            var book = _mapper.Map<Book>(newBook);
            book.CoverImage = new CoverImage
            {
                Content = file,
                ContentType = contentType
            };

            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).Returns(book);
            A.CallTo(() => _repository.Update(A<Guid>.Ignored, A<Book>.Ignored, A<CoverImage>.Ignored)).Returns(book);

            // Act
            var result = await _handler.Handle(new UpdateBookCommand(Guid.Empty, newBook), default);

            // Assert
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _repository.Update(A<Guid>.Ignored, A<Book>.Ignored, A<CoverImage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(newBook.Title, result.Title);
            Assert.Equal(newBook.Author, result.Author);
            Assert.Equal(newBook.Description, result.Description);
            Assert.Equal(newBook.Price, result.Price);
            Assert.Equal($"/books/{result.Id}/image", result.ImageUrl);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(" ", " ")]
        public async Task UpdateBookCommand_should_not_be_valid_if_required_fields_are_empty(string title, string author)
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/jpeg";

            var newBook = new NewBookModel
            {
                Author = author,
                Title = title,
                Description = "Correct",
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            var validator = new UpdateBookCommandValidator();

            // Act
            var result = await validator.ValidateAsync(new UpdateBookCommand(Guid.Empty, newBook));

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Title)));
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Author)));
        }

        [Fact]
        public async Task UpdateBookCommand_should_not_be_valid_if_image_is_empty()
        {
            // Arrange
            var newBook = new NewBookModel
            {
                Author = "Correct",
                Title = "Correct",
                Description = "Correct",
                Price = 1,
                Image = null
            };

            var validator = new UpdateBookCommandValidator();

            // Act
            var result = await validator.ValidateAsync(new UpdateBookCommand(Guid.Empty, newBook));

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Image)));
        }

        [Fact]
        public async Task UpdateBookCommand_should_not_be_valid_if_image_has_wrong_mime_type()
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/pdf";

            var newBook = new NewBookModel
            {
                Author = "Correct",
                Title = "Correct",
                Description = "Correct",
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            var validator = new UpdateBookCommandValidator();

            // Act
            var result = await validator.ValidateAsync(new UpdateBookCommand(Guid.Empty, newBook));

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Image.ContentType)));
        }

        [Fact]
        public async Task UpdateBookCommand_should_not_be_valid_if_fields_are_too_big()
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/pdf";

            var newBook = new NewBookModel
            {
                Author = new string('a', 129),
                Title = new string('a', 129),
                Description = new string('a', 2001),
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            var validator = new UpdateBookCommandValidator();

            // Act
            var result = await validator.ValidateAsync(new UpdateBookCommand(Guid.Empty, newBook));

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Title)));
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Author)));
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Description)));
        }

        [Fact]
        public async Task UpdateBookCommand_should_throw_not_found_exception_if_no_book_with_ID_in_DB()
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/jpeg";

            var newBook = new NewBookModel
            {
                Author = "Correct",
                Title = "Correct",
                Description = "Correct",
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).Returns<Book>(null);

            // Act
            var result = await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(new UpdateBookCommand(Guid.Empty, newBook), default));

            // Assert
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _repository.Update(A<Guid>.Ignored, A<Book>.Ignored, A<CoverImage>.Ignored)).MustNotHaveHappened();
            Assert.Equal($"Book with ID {Guid.Empty} was not found.", result.Message);
        }
    }
}
