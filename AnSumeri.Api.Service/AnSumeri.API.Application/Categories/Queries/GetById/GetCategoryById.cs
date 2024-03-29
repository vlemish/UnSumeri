﻿using MediatR;

namespace AnSumeri.API.Application.Categories.Queries.GetById
{
    using Models.Mediatr;

    public record GetCategoryById : IRequest<ResultDto<GetCategoryByIdResult>>
    {
        public int Id { get; init; }

        public int Depth { get; init; }

        public string UserId { get; init; }

        public GetCategoryById(int id, string userId, int? depth = null)
        {
            Id = id;
            Depth = depth ?? 2;
            UserId = userId;
        }

    }
}
