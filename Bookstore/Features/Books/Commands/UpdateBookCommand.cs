using System;
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
    public class UpdateBookCommand : IRequest<BookModel>
    {
        public UpdateBookCommand(Guid bookId, AddBookModel model)
        {
            BookId = bookId;
            Book = model;
        }

        public Guid BookId { get; }
        public AddBookModel Book { get; }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookModel>
    {
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateBookCommandHandler(IBooksRepository repository, IMapper mapper, ILogger<UpdateBookCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookModel> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request.Book);
            request.Book.Image.CopyToBook(book);

            _logger.LogInformation($"Updating book with ID: {request.BookId}. Payload: {JsonConvert.SerializeObject(book)}");

            return _mapper.Map<BookModel>(await _repository.Update(request.BookId, book));
        }
    }
}
