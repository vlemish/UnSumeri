using MediatR;
using Microsoft.AspNetCore.Mvc;
using UntitiledArticles.API.Application.Articles.Commands.Add;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Service.Contracts.Requests;

namespace UntitledArticles.API.Service.Controllers
{
    using UntitiledArticles.API.Application.Articles.Commands;
    using UntitiledArticles.API.Application.Models.Mediatr;

    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromRoute] int categoryId, [FromBody] AddArticleRequest request, CancellationToken cancellationToken)
        {
            AddArticle addArticle = new(categoryId, request.Title, request.Content);
            ResultDto<AddArticleResult> response = await _mediator.Send(addArticle, cancellationToken);
            switch (response.OperationStatus.Status)
            {
                case OperationStatusValue.OK:
                {
                    return new ObjectResult(response.Payload.Id)
                    {
                        StatusCode = StatusCodes.Status201Created,
                    };
                }
                case OperationStatusValue.NotFound:
                {
                    return NotFound();
                }
                case OperationStatusValue.Duplicate:
                {
                    return Conflict();
                }
                default:
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
