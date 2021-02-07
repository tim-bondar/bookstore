using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
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
        public UpdateBookCommand(Guid bookId, NewBookModel model)
        {
            BookId = bookId;
            Book = model;
        }

        public Guid BookId { get; }
        public NewBookModel Book { get; }
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
            var book = await _repository.GetById(request.BookId);

            if (book == null)
            {
                throw new NotFoundException($"Book with ID {request.BookId} was not found.");
            }

            book = _mapper.Map<Book>(request.Book);
            var img = request.Book.Image.ToCoverImage();

            _logger.LogInformation($"Updating book with ID: {request.BookId}. Payload: {JsonConvert.SerializeObject(book)}. With image size: {img.Content.Length} bytes.");

            return _mapper.Map<BookModel>(await _repository.Update(request.BookId, book, img));
        }
    }
}
