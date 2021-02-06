using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using DB.Abstraction;
using MediatR;

namespace Features.Books.Queries
{
    public class AllBooksQuery : IRequest<List<BookModel>>
    {
    }

    public class AllBooksQueryHandler : IRequestHandler<AllBooksQuery, List<BookModel>>
    {
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        public AllBooksQueryHandler(IBooksRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BookModel>> Handle(AllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _repository.GetAll();
            return books.Select(x => _mapper.Map<BookModel>(x)).ToList();
        }
    }
}
