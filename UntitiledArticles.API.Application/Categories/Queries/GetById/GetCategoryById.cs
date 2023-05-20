using MediatR;

namespace UntitiledArticles.API.Application.Categories.Queries.GetById
{
    using Models.Mediatr;

    public record GetCategoryById : IRequest<ResultDto<GetCategoryByIdResult>>
    {
        public GetCategoryById(int id, int? depth = null)
        {
            Id = id;
            Depth = depth ?? 2;
        }

        public int Id { get; init; }

        public int Depth { get; init; }

    }
}
