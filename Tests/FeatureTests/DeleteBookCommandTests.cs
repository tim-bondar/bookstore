using System;
using System.Threading.Tasks;
using DB.Abstraction;
using FakeItEasy;
using Features.Books.Commands;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.FeatureTests
{
    public class DeleteBookQueryTests
    {
        private readonly DeleteBookCommandHandler _handler;
        private readonly IBooksRepository _repository;

        public DeleteBookQueryTests()
        {
            _repository = A.Fake<IBooksRepository>();
            _handler = new DeleteBookCommandHandler(_repository, A.Fake<ILogger<DeleteBookCommandHandler>>());
        }

        [Fact]
        public async Task DeleteBookCommand_should_return_call_Delete_repository_method()
        {
            // Arrange

            // Act
            await _handler.Handle(new DeleteBookCommand(Guid.Empty), default);

            // Assert
            A.CallTo(() => _repository.Delete(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
