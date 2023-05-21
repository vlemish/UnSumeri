﻿using MediatR;

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
    using Domain.Contracts;
    using UntitiledArticles.API.Application.Articles.Commands;
    using UntitiledArticles.API.Application.Articles.Commands.Add;
    using UntitiledArticles.API.Application.Models.Mediatr;

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
            var query = new GetCategoryById(id);
            ResultDto<GetCategoryByIdResult> response = await _mediator.Send(query);
            if (response.OperationStatus.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return Ok(response.Payload);
            }

            switch (response.OperationStatus.Status)
            {
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.NotFound:
                {
                    _logger.LogError(response.OperationStatus.Message);
                    return NotFound();
                }
                default:
                {
                    _logger.LogError($"An error occured during processing {nameof(GetById)} request: {response.OperationStatus.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesRequest request, CancellationToken cancellationToken)
        {
            GetAllCategories query = new GetAllCategories(new LoadOptions(request.Offset, request.Skip), request.OrderByOption, request.Depth);
            ResultDto<IPaginatedResult<GetAllCategoriesResult>> response = await _mediator.Send(query, cancellationToken);
            if (response.OperationStatus.Status == OperationStatusValue.OK)
            {
                _logger.LogInformation(response.OperationStatus.Message);
                return Ok(response.Payload);
            }

            _logger.LogError(response.OperationStatus.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        [HttpPost("", Name = nameof(AddCategory))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
        {
            var command = new AddCategory(request.Name);
            ResultDto<AddCategoryResult> response = await _mediator.Send(command);
            if (response.OperationStatus.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return new ObjectResult(response.Payload.Id)
                {
                    StatusCode = StatusCodes.Status201Created,
                };
            }

            _logger.LogError($"An error occured during processing {nameof(AddCategory)} request: {response.OperationStatus.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("{id:int}/article")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddArticle([FromRoute] int id, [FromBody] AddArticleRequest request, CancellationToken cancellationToken)
        {
            AddArticle addArticle = new(id, request.Title, request.Content);
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

        [HttpPost("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddSubcategory([FromRoute] int id, [FromBody] AddSubcategoryRequest request)
        {
            var command = new AddSubcategory(request.Name, id);
            ResultDto<AddSubcategoryResult> response = await _mediator.Send(command);
            if (response.OperationStatus.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return new ObjectResult(response.Payload.Id)
                {
                    StatusCode = StatusCodes.Status201Created,
                };
            }

            _logger.LogError($"An error occured during processing {nameof(AddSubcategory)} request: {response.OperationStatus.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut("{id:int}/move/{moveToId:int?}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Move([FromRoute] int id, [FromRoute] int? moveToId)
        {
            var command = new MoveCategory(id, moveToId);
            ResultDto response = await _mediator.Send(command);
            if (response.OperationStatus.Status == UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.OK)
            {
                return Ok();
            }

            switch (response.OperationStatus.Status)
            {
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.NotFound:
                {
                    _logger.LogError(response.OperationStatus.Message);
                    return NotFound();
                }
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.ParentNotExists:
                {
                    _logger.LogError(response.OperationStatus.Message);
                    return NotFound();
                }
                default:
                {
                    _logger.LogError($"An error occured during processing {nameof(Move)} request: {response.OperationStatus.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteCategory command = new(id);
            ResultDto response = await _mediator.Send(command);

            switch (response.OperationStatus.Status)
            {
                case OperationStatusValue.OK:
                {
                    _logger.LogInformation(response.OperationStatus.Message);
                    return Ok(id);
                }
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.NotFound:
                {
                    _logger.LogError(response.OperationStatus.Message);
                    return NotFound();
                }
                default:
                {
                    _logger.LogError($"An error occured during processing {nameof(Delete)} request: {response.OperationStatus.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            UpdateCategory command = new(id, request.Name);
            ResultDto response = await _mediator.Send(command, cancellationToken);

            switch (response.OperationStatus.Status)
            {
                case OperationStatusValue.OK:
                {
                    _logger.LogInformation(response.OperationStatus.Message);
                    return NoContent();
                }
                case UntitiledArticles.API.Application.OperationStatuses.OperationStatusValue.NotFound:
                {
                    _logger.LogError(response.OperationStatus.Message);
                    return NotFound();
                }
                default:
                {
                    _logger.LogError($"An error occured during processing {nameof(Delete)} request: {response.OperationStatus.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

        }
    }
}
