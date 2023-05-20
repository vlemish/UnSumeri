using MediatR;
using Microsoft.AspNetCore.Mvc;
using UntitiledArticles.API.Application.Articles.Commands.Add;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Service.Contracts.Requests;

namespace UntitledArticles.API.Service.Controllers
{
    using UntitiledArticles.API.Application.Articles.Commands;
    using UntitiledArticles.API.Application.Articles.Queries.GetOneById;
    using UntitiledArticles.API.Application.Models;
    using UntitiledArticles.API.Application.Models.Mediatr;

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
        public async Task<IActionResult> GetArticleById(int id, CancellationToken cancellationToken)
        {
            GetOneArticleById query = new(id);
            ResultDto<ArticleDto> queryResult = await this._mediator.Send(query, cancellationToken);
            switch (queryResult.OperationStatus.Status)
            {
                case OperationStatusValue.OK:
                {
                    return this.Ok(queryResult.Payload);
                }
                case OperationStatusValue.NotFound:
                {
                    return this.NotFound();
                }
                default:
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
