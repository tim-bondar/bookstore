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
    public class SingleBookQueryTests
    {
        private readonly SingleBookQueryHandler _handler;
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        public SingleBookQueryTests()
        {
            _repository = A.Fake<IBooksRepository>();
            _mapper = Helpers.CreateMapper();
            _handler = new SingleBookQueryHandler(_repository, _mapper);
        }

        [Fact]
        public async Task SingleBookQuery_should_return_existing_book()
        {
            // Arrange
            var book = new Book
            {
                Author = "Dummy1",
                Title = "Dummy2",
                Description = "Dummy3",
                Price = 1
            };

            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).Returns(book);

            // Act
            var result = await _handler.Handle(new SingleBookQuery(Guid.Empty), default);

            // Assert
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, false)).MustHaveHappenedOnceExactly();
            Assert.Equal(book.Title, result.Title);
            Assert.Equal(book.Author, result.Author);
            Assert.Equal(book.Description, result.Description);
            Assert.Equal(book.Price, result.Price);
            Assert.Equal($"/books/{result.Id}/image", result.ImageUrl);
        }

        [Fact]
        public async Task SingleBookQuery_should_throw_not_found_exception_if_no_book_with_ID_in_DB()
        {
            // Arrange
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).Returns<Book>(null);

            // Act
            var result = await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(new SingleBookQuery(Guid.Empty), default));

            // Assert
            A.CallTo(() => _repository.GetById(A<Guid>.Ignored, A<bool>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal($"Book with ID {Guid.Empty} was not found.", result.Message);
        }
    }
}
