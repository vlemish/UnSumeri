using MediatR;
using Microsoft.AspNetCore.Mvc;
using UntitiledArticles.API.Application.Categories.Commands.Add;
using UntitiledArticles.API.Application.Categories.Commands.AddSubcategory;
using UntitiledArticles.API.Application.Categories.Commands.Delete;
using UntitiledArticles.API.Application.Categories.Commands.Move;
using UntitiledArticles.API.Application.Categories.Commands.Update;
using UntitiledArticles.API.Application.Categories.Queries.GetAll;
using UntitiledArticles.API.Application.Categories.Queries.GetById;
using UntitiledArticles.API.Application.OperationStatuses;
using UntitledArticles.API.Domain.Pagination;
using UntitledArticles.API.Service.Contracts.Requests;

namespace UntitledArticles.API.Service.Controllers
{
    using System.Security.Claims;
    using System.Text;
    using Domain.Contracts;
    using Extensions;
    using Microsoft.AspNetCore.Authorization;
    using UntitiledArticles.API.Application.Articles.Commands;
    using UntitiledArticles.API.Application.Articles.Commands.Add;
    using UntitiledArticles.API.Application.Models.Mediatr;

    [Authorize]
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMediator _mediator;

        public CategoriesController(ILogger<CategoriesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int id, int? depth)
        {
            var query = new GetCategoryById(id, HttpContext.GetUserId(), depth);
            ResultDto<GetCategoryByIdResult> response = await _mediator.Send(query);
            return response.ToHttpObjectResult();
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesRequest request,
            CancellationToken cancellationToken)
        {
            GetAllCategories query = new GetAllCategories(new LoadOptions(request.Offset, request.Skip),
                HttpContext.GetUserId(), request.OrderByOption, request.Depth);
            ResultDto<IPaginatedResult<GetAllCategoriesResult>> response =
                await _mediator.Send(query, cancellationToken);
            return response.ToHttpObjectResult();
        }

        [HttpPost("", Name = nameof(AddCategory))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
        {
            var command = new AddCategory(request.Name, HttpContext.GetUserId());
            ResultDto<AddCategoryResult> response = await _mediator.Send(command);
            return response.ToHttpObjectResult();
        }

        [HttpPost("{id:int}/article")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddArticle([FromRoute] int id, [FromBody] AddArticleRequest request,
            CancellationToken cancellationToken)
        {
            AddArticle addArticle = new(id, HttpContext.GetUserId(), request.Title, request.Content);
            ResultDto<AddArticleResult> response = await _mediator.Send(addArticle, cancellationToken);
            return response.ToHttpObjectResult();
        }

        [HttpPost("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSubcategory([FromRoute] int id, [FromBody] AddSubcategoryRequest request)
        {
            var command = new AddSubcategory(request.Name, HttpContext.GetUserId(), id);
            ResultDto<AddSubcategoryResult> response = await _mediator.Send(command);
            return response.ToHttpObjectResult();
        }

        [HttpPut("{id:int}/move/{moveToId:int?}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Move([FromRoute] int id, [FromRoute] int? moveToId)
        {
            var command = new MoveCategory(id, HttpContext.GetUserId(), moveToId);
            ResultDto response = await _mediator.Send(command);
            return response.ToHttpObjectResult();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteCategory command = new(id, HttpContext.GetUserId());
            ResultDto<DeleteCategoryResult> response = await _mediator.Send(command);
            return response.ToHttpObjectResult();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequest request,
            CancellationToken cancellationToken)
        {
            UpdateCategory command = new(id, HttpContext.GetUserId(), request.Name);
            ResultDto response = await _mediator.Send(command, cancellationToken);
            return response.ToHttpObjectResult();
        }
    }
}
