using System;
using System.Threading;
using System.Threading.Tasks;
using DB.Abstraction;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Features.Books.Commands
{
    public class DeleteBookCommand : IRequest
    {
        public DeleteBookCommand(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; }
    }

    // Will not throw an error if book was already removed.
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IBooksRepository _repository;
        private readonly ILogger _logger;

        public DeleteBookCommandHandler(IBooksRepository repository, ILogger<DeleteBookCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Deleting book with ID: {request.BookId}.");
            await _repository.Delete(request.BookId);
            return Unit.Value;
        }
    }
}
