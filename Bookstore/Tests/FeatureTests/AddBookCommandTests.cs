using System.IO;
using System.Threading.Tasks;
using AutoMapper;
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
    public class AddBookCommandTests
    {
        private readonly AddBookCommandHandler _handler;
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        public AddBookCommandTests()
        {
            _repository = A.Fake<IBooksRepository>();
            _mapper = Helpers.CreateMapper();
            _handler = new AddBookCommandHandler(_repository, _mapper, A.Fake<ILogger<AddBookCommandHandler>>());
        }

        [Fact]
        public async Task AddBookCommand_should_successfully_store_correct_book()
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/jpeg";

            var newBook = new AddBookModel
            {
                Author = "Correct",
                Title = "Correct",
                Description = "Correct",
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            var book = _mapper.Map<Book>(newBook);
            book.ImageContent = file;
            A.CallTo(() => _repository.Add(A<Book>.Ignored)).Returns(book);

            // Act
            var result = await _handler.Handle(new AddBookCommand(newBook), default);

            // Assert
            A.CallTo(() => _repository.Add(A<Book>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(newBook.Title, result.Title);
            Assert.Equal(newBook.Author, result.Author);
            Assert.Equal(newBook.Description, result.Description);
            Assert.Equal(newBook.Price, result.Price);
            Assert.Equal(contentType, result.ImageContentType);
            Assert.Equal(file.Length, result.ImageContent.Length);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(" ", " ")]
        public async Task AddBookCommand_should_not_be_valid_if_required_fields_are_empty(string title, string author)
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/jpeg";

            var newBook = new AddBookModel
            {
                Author = author,
                Title = title,
                Description = "Correct",
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            var validator = new AddBookCommandValidator();

            // Act
            var result = await validator.ValidateAsync(new AddBookCommand(newBook));

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Title)));
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Author)));
        }

        [Fact]
        public async Task AddBookCommand_should_not_be_valid_if_image_is_empty()
        {
            // Arrange
            var newBook = new AddBookModel
            {
                Author = "Correct",
                Title = "Correct",
                Description = "Correct",
                Price = 1,
                Image = null
            };

            var validator = new AddBookCommandValidator();

            // Act
            var result = await validator.ValidateAsync(new AddBookCommand(newBook));

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Image)));
        }

        [Fact]
        public async Task AddBookCommand_should_not_be_valid_if_image_has_wrong_mime_type()
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/pdf";

            var newBook = new AddBookModel
            {
                Author = "Correct",
                Title = "Correct",
                Description = "Correct",
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            var validator = new AddBookCommandValidator();

            // Act
            var result = await validator.ValidateAsync(new AddBookCommand(newBook));

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Image.ContentType)));
        }

        [Fact]
        public async Task AddBookCommand_should_not_be_valid_if_fields_are_too_big()
        {
            // Arrange
            var file = new byte[] { 1, 2, 3 };
            await using var ms = new MemoryStream(file);
            var contentType = "image/pdf";

            var newBook = new AddBookModel
            {
                Author = new string('a', 129),
                Title = new string('a', 129),
                Description = new string('a', 2001),
                Price = 1,
                Image = Helpers.CreateTestFormFile(ms, contentType)
            };

            var validator = new AddBookCommandValidator();

            // Act
            var result = await validator.ValidateAsync(new AddBookCommand(newBook));

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Title)));
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Author)));
            Assert.Contains(result.Errors, failure => failure.PropertyName.EndsWith(nameof(newBook.Description)));
        }
    }
}
