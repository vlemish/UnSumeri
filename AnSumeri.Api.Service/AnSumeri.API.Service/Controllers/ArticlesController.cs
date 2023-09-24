using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;

using MediatR;

using AnSumeri.API.Application.Articles.Queries.FindArticlesByPattern;
using AnSumeri.API.Service.Contracts.Requests;

namespace AnSumeri.API.Service.Controllers
{
    using System.Runtime.CompilerServices;
    using Extensions;
    using Microsoft.AspNetCore.Authorization;
    using AnSumeri.API.Application.Articles.Commands;
    using AnSumeri.API.Application.Articles.Commands.Move;
    using AnSumeri.API.Application.Articles.Queries.GetOneById;
    using AnSumeri.API.Application.Models;
    using AnSumeri.API.Application.Models.Mediatr;

    [Authorize]
    [ApiController]
    [Route("api/articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticleById([FromRoute] int id, CancellationToken cancellationToken)
        {
            GetOneArticleById query = new(id, HttpContext.GetUserId());
            ResultDto<ArticleDto> queryResult = await _mediator.Send(query, cancellationToken);
            return queryResult.ToHttpObjectResult();
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImmutableList<FindArticlesByPatternResult>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindArticlesByPatternAcross([FromQuery] string searchPattern,
            CancellationToken cancellationToken)
        {
            FindArticlesByPattern query = new(HttpContext.GetUserId(), searchPattern);
            ResultDto<ImmutableList<FindArticlesByPatternResult>> queryResult = await _mediator.Send(query, cancellationToken);
            return queryResult.ToHttpObjectResult();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MoveArticleToCategory(int id, [FromQuery] int moveToCategoryId,
            CancellationToken cancellationToken)
        {
            MoveArticle request = new(id, HttpContext.GetUserId(), moveToCategoryId);
            ResultDto commandResult = await _mediator.Send(request, cancellationToken);
            return commandResult.ToHttpObjectResult();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArticle([FromRoute] int id,
            [FromBody] UpdateArticleRequest updateArticleRequest, CancellationToken cancellationToken)
        {
            AnSumeri.API.Application.Articles.Commands.Update.UpdateArticle request = new(id, HttpContext.GetUserId(),
                updateArticleRequest.Title, updateArticleRequest.Content);
            ResultDto commandResult = await _mediator.Send(request, cancellationToken);
            return commandResult.ToHttpObjectResult();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id, CancellationToken cancellationToken)
        {
            AnSumeri.API.Application.Articles.Commands.Delete.DeleteArticle request = new(id, HttpContext.GetUserId());
            ResultDto<int> commandResult = await _mediator.Send(request, cancellationToken);
            return commandResult.ToHttpObjectResult();
        }
    }
}
