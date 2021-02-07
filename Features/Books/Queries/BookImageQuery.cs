using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using DB.Abstraction;
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
            var book = await _repository.GetById(request.BookId, true);
            if (book?.CoverImage == null)
            {
                throw new NotFoundException($"Could not find image for book with ID: {request.BookId}.");
            }

            return _mapper.Map<FileContent>(book.CoverImage);
        }
    }
}
