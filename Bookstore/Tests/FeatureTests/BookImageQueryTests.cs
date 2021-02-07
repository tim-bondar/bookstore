using System;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using DB.Abstraction;
using DB.Entities;
using FakeItEasy;
using Features.Books.Queries;
using Xunit;

namespace Tests.FeatureTests
{
    public class BookImageQueryTests
    {
        private readonly BookImageQueryHandler _handler;
        private readonly IBooksRepository _repository;

        public BookImageQueryTests()
        {
            _repository = A.Fake<IBooksRepository>();
            _handler = new BookImageQueryHandler(_repository, Helpers.CreateMapper());
        }

        [Fact]
        public async Task BookImageQuery_should_return_existing_book()
        {
            // Arrange
            var book = new Book
            {
                Author = "Dummy1",
                Title = "Dummy2",
                Description = "Dummy3",
                Price = 1,
                CoverImage = new CoverImage
                {
                    Content = new byte[] {1,2,3},
                    ContentType = "image/jpeg"
                }
            };

            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).Returns(book);

            // Act
            var result = await _handler.Handle(new BookImageQuery(Guid.Empty), default);

            // Assert
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, true)).MustHaveHappenedOnceExactly();
            Assert.Equal(book.CoverImage.Content, result.Content);
            Assert.Equal(book.CoverImage.ContentType, result.ContentType);
        }

        [Fact]
        public async Task BookImageQuery_should_throw_not_found_exception_if_no_book_with_ID_in_DB()
        {
            // Arrange
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).Returns<Book>(null);

            // Act
            var result = await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(new BookImageQuery(Guid.Empty), default));

            // Assert
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, true)).MustHaveHappenedOnceExactly();
            Assert.Equal($"Could not find image for book with ID: {Guid.Empty}.", result.Message);
        }

        [Fact]
        public async Task BookImageQuery_should_throw_not_found_exception_if_no_image_related_to_book()
        {
            // Arrange
            var book = new Book
            {
                Author = "Dummy1",
                Title = "Dummy2",
                Description = "Dummy3",
                Price = 1,
                CoverImage = null
            };

            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).Returns(book);

            // Act
            var result = await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(new BookImageQuery(Guid.Empty), default));

            // Assert
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, true)).MustHaveHappenedOnceExactly();
            Assert.Equal($"Could not find image for book with ID: {Guid.Empty}.", result.Message);
        }
    }
}
