using MediatR;

using Microsoft.AspNetCore.Mvc;

using UntitiledArticles.API.Application.Categories.Commands.Add;
using UntitiledArticles.API.Application.Categories.Commands.AddSubcategory;
using UntitiledArticles.API.Application.Categories.Commands.Delete;
using UntitiledArticles.API.Application.Categories.Commands.Move;
using UntitiledArticles.API.Application.Categories.Queries.GetById;

using UntitledArticles.API.Service.Contracts.Requests;

namespace UntitledArticles.API.Service.Controllers
{
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

        [HttpPost("", Name = nameof(AddCategory))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
        {
            var command = new AddCategory(request.Name);
            AddCategoryResponse response = await _mediator.Send(command);
            if (response.Status.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return new ObjectResult(response.Result.Id)
                {
                    StatusCode = StatusCodes.Status201Created,
                };
            }

            _logger.LogError($"An error occured during processing {nameof(AddCategory)} request: {response.Status.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSubcategory([FromRoute] int id, [FromBody] AddSubcategoryRequest request)
        {
            var command = new AddSubcategory(request.Name, id);
            AddSubcategoryResponse response = await _mediator.Send(command);
            if (response.Status.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return new ObjectResult(response.Result.Id)
                {
                    StatusCode = StatusCodes.Status201Created,
                };
            }

            _logger.LogError($"An error occured during processing {nameof(AddSubcategory)} request: {response.Status.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var query = new GetCategoryById(id);
            GetCategoryByIdResponse response = await _mediator.Send(query);
            if (response.Status.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return Ok(response.Result);
            }

            switch (response.Status.Status)
            {
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.NotFound:
                    {
                        _logger.LogError(response.Status.Message);
                        return NotFound();
                    }
                default:
                    {
                        _logger.LogError($"An error occured during processing {nameof(GetById)} request: {response.Status.Message}");
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
            }
        }

        [HttpPut("{id:int}/move/{moveToId:int?}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Move([FromRoute] int id, [FromRoute] int? moveToId)
        {
            var command = new MoveCategory(id, moveToId);
            MoveCategoryResponse response = await _mediator.Send(command);
            if (response.Status.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return Ok();
            }

            switch (response.Status.Status)
            {
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.NotFound:
                    {
                        _logger.LogError(response.Status.Message);
                        return NotFound();
                    }
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.ParentNotExists:
                    {
                        _logger.LogError(response.Status.Message);
                        return NotFound();
                    }
                default:
                    {
                        _logger.LogError($"An error occured during processing {nameof(Move)} request: {response.Status.Message}");
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteCategory command = new(id);
            DeleteCategoryResponse response = await _mediator.Send(command);
            if (response.Status.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return Ok(id);
            }

            switch (response.Status.Status)
            {
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.NotFound:
                    {
                        _logger.LogError(response.Status.Message);
                        return NotFound();
                    }
                default:
                    {
                        _logger.LogError($"An error occured during processing {nameof(Delete)} request: {response.Status.Message}");
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
            }
        }
    }
}
