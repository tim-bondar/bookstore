using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using DB.Abstraction;
using DB.Repositories;
using MediatR;

namespace Features.Books.Queries
{
    public class BookImageQuery : IRequest<FileContent>
    {
        public BookImageQuery(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; }
    }

    public class BookImageQueryHandler : IRequestHandler<BookImageQuery, FileContent>
    {
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        public BookImageQueryHandler(IBooksRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FileContent> Handle(BookImageQuery request, CancellationToken cancellationToken)
        {
            var image = await _repository.GetImageByBookId(request.BookId);
            if (image == null)
            {
                throw new BookNotFoundException($"Could not find image for Book with ID: {request.BookId}.");
            }

            return _mapper.Map<FileContent>(image);
        }
    }
}
