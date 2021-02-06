using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using DB.Abstraction;
using DB.Entities;
using Features.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Features.Books.Commands
{
    public class AddBookCommand : IRequest<BookModel>
    {
        public AddBookCommand(AddBookModel model)
        {
            Book = model;
        }

        public AddBookModel Book { get; }
    }

    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookModel>
    {
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AddBookCommandHandler(IBooksRepository repository, IMapper mapper, ILogger<AddBookCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookModel> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request.Book);
            request.Book.Image.CopyToBook(book);

            _logger.LogInformation($"Adding new book. Payload: {JsonConvert.SerializeObject(book)}");

            return _mapper.Map<BookModel>(await _repository.Add(book));
        }
    }
}
