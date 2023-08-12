using System.Collections.Immutable;
using UntitiledArticles.API.Application.Categories.Queries.FindOne;

namespace UntitiledArticles.API.Application.Categories.Queries.FindMany;

public record FindManyByFilterResult
{
    public List<FindOneByFilterResult> Categories { get; set; }

    // public FindManyByFilterResult(IReadOnlyCollection<FindOneByFilterResult> categories)
    // {
    //     Categories = categories ?? Enumerable.Empty<FindOneByFilterResult>().ToImmutableList();
    // }
}
