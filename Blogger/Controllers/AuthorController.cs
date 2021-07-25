using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blogger.Application.Commands;
using Blogger.Domain;
using Blogger.Domain.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Controllers
{
    [Route("authors")]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var queryAuthorCommand = new QueryAuthorCommand();
            queryAuthorCommand.AuthorIdList.Add(id);

            PaginatedResponse<Author> authorPaginatedResponse = await _mediator.Send(queryAuthorCommand);
            Author author = authorPaginatedResponse.Data.First();
            
            return StatusCode((int) HttpStatusCode.OK, author);
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1)
        {
            var queryAuthorCommand = new QueryAuthorCommand(pageNumber, pageSize);

            PaginatedResponse<Author> authorPaginatedResponse = await _mediator.Send(queryAuthorCommand);

            return StatusCode((int) HttpStatusCode.OK, authorPaginatedResponse);
        }
    }
}