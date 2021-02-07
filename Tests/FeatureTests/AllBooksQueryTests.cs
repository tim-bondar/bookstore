using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DB.Abstraction;
using DB.Entities;
using FakeItEasy;
using Features.Books.Queries;
using Xunit;

namespace Tests.FeatureTests
{
    public class AllBooksQueryTests
    {
        private readonly AllBooksQueryHandler _handler;
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        public AllBooksQueryTests()
        {
            _repository = A.Fake<IBooksRepository>();
            _mapper = Helpers.CreateMapper();
            _handler = new AllBooksQueryHandler(_repository, _mapper);
        }

        [Fact]
        public async Task AllBooksQueryHandler_should_return_collection_of_books()
        {
            // Arrange
            var books = new List<Book>()
            {
                new Book
                {
                    Author = "Dummy1",
                    Title = "Dummy1",
                    Description = "Dummy1",
                    Price = 1
                },
                new Book
                {
                    Author = "Dummy2",
                    Title = "Dummy2",
                    Description = "Dummy2",
                    Price = 1
                }
            };

            A.CallTo(() => _repository.GetAll()).Returns(books);

            // Act
            var result = await _handler.Handle(new AllBooksQuery(), default);

            // Assert
            A.CallTo(() => _repository.GetAll()).MustHaveHappenedOnceExactly();
            Assert.Equal(books.Count, result.Count);
            Assert.Equal(books[0].Title, result[0].Title);
            Assert.Equal($"/books/{books[0].Id}/image", result[0].ImageUrl);
        }
    }
}
