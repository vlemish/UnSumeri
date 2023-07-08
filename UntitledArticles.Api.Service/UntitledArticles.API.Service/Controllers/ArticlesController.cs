using MediatR;
using Microsoft.AspNetCore.Mvc;
using UntitiledArticles.API.Application.Articles.Commands.Add;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Service.Contracts.Requests;

namespace UntitledArticles.API.Service.Controllers
{
    using System.Runtime.CompilerServices;
    using Extensions;
    using Microsoft.AspNetCore.Authorization;
    using UntitiledArticles.API.Application.Articles.Commands;
    using UntitiledArticles.API.Application.Articles.Commands.Move;
    using UntitiledArticles.API.Application.Articles.Queries.GetOneById;
    using UntitiledArticles.API.Application.Models;
    using UntitiledArticles.API.Application.Models.Mediatr;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArticleById([FromRoute] int id, CancellationToken cancellationToken)
        {
            GetOneArticleById query = new(id, HttpContext.GetUserId());
            ResultDto<ArticleDto> queryResult = await this._mediator.Send(query, cancellationToken);
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
            ResultDto commandResult = await this._mediator.Send(request, cancellationToken);
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
            UntitiledArticles.API.Application.Articles.Commands.Update.UpdateArticle request = new(id, HttpContext.GetUserId(),
                updateArticleRequest.Title, updateArticleRequest.Content);
            ResultDto commandResult = await this._mediator.Send(request, cancellationToken);
            return commandResult.ToHttpObjectResult();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id, CancellationToken cancellationToken)
        {
            UntitiledArticles.API.Application.Articles.Commands.Delete.DeleteArticle request = new(id, HttpContext.GetUserId());
            ResultDto<int> commandResult = await this._mediator.Send(request, cancellationToken);
            return commandResult.ToHttpObjectResult();
        }
    }
}
