using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Core.Models;
using Features.Books.Commands;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(Guid id, [FromForm] NewBookModel book)
        {
            return Ok(await _mediator.Send(new UpdateBookCommand(id, book)));
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] NewBookModel book)
        {
            var result = await _mediator.Send(new AddBookCommand(book));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteBookCommand(id));
            return NoContent();
        }

        [HttpGet("{id}/image")]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var file = await _mediator.Send(new BookImageQuery(id));
            return File(file.Content, file.ContentType);
        }
    }
}
