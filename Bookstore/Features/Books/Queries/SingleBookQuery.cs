using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using DB.Abstraction;
using MediatR;

namespace Features.Books.Queries
{
    public class SingleBookQuery : IRequest<BookModel>
    {
        public Guid BookId { get; }

        public SingleBookQuery(Guid bookId)
        {
            BookId = bookId;
        }
    }

    public class SingleBookQueryHandler : IRequestHandler<SingleBookQuery, BookModel>
    {
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        public SingleBookQueryHandler(IBooksRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BookModel> Handle(SingleBookQuery request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetById(request.BookId);
            return _mapper.Map<BookModel>(book);
        }
    }
}
