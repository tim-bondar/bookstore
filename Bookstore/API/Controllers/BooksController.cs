using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Core.Models;
using Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<BookModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new AllBooksQuery()));
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new SingleBookQuery(id)));
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookModel book)
        {
            // TODO
            return Ok();
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            // TODO
            return Ok();
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] BookModel book)
        {
            // TODO
            return Ok();
        }
    }
}
