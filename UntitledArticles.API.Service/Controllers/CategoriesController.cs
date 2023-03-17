using MediatR;

using Microsoft.AspNetCore.Mvc;

using UntitiledArticles.API.Application.Categories.Commands.Add;
using UntitiledArticles.API.Application.Categories.Commands.AddSubcategory;
using UntitiledArticles.API.Application.Categories.Queries;

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
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request)
        {
            try
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

                _logger.LogError($"An error occured during processing {AddCategory} request: {response.Status.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError($"An error occured during validation: {ex.Message}", ex);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured during processing {AddCategory} request: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> AddSubcategory([FromRoute] int id, [FromBody] AddSubcategoryRequest request)
        {
            try
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

                _logger.LogError($"An error occured during processing {AddCategory} request: {response.Status.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError($"An error occured during validation: {ex.Message}", ex);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured during processing {AddCategory} request: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            try
            {
                var query = new GetCategory(id);
                GetCategoryResponse response = await _mediator.Send(query);
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
                            _logger.LogError($"An error occured during processing {GetCategoryById} request: {response.Status.Message}");
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        }
                }
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError($"An error occured during validation: {ex.Message}", ex);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured during processing {AddCategory} request: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
