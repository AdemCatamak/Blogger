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
    [Route("articles")]
    public class ArticleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            QueryArticleCommand queryArticleCommand = new()
                                                      {
                                                          Id = id
                                                      };

            PaginatedResponse<Article> paginatedResponse = await _mediator.Send(queryArticleCommand);
            Article article = paginatedResponse.Data.First();

            return StatusCode((int) HttpStatusCode.OK, article);
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1, [FromQuery] Guid? authorId = null)
        {
            QueryArticleCommand queryArticleCommand = new(pageNumber, pageSize)
                                                      {
                                                          AuthorId = authorId
                                                      };

            PaginatedResponse<Article> paginatedResponse = await _mediator.Send(queryArticleCommand);

            return StatusCode((int) HttpStatusCode.OK, paginatedResponse);
        }
    }
}