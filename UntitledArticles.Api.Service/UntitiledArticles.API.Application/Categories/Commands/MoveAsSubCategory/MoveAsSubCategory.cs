﻿using MediatR;

namespace UntitiledArticles.API.Application.Categories.Commands.MoveAsSubCategory;

using Models.Mediatr;

public record MoveAsSubCategory(int Id, string UserId, int MoveToId) : CategoryBaseRequest<ResultDto>(UserId);
