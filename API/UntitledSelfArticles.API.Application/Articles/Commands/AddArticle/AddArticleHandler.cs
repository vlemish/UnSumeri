using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UntitledArticles.API.Domain.Contracts;

namespace UntitledSelfArticles.API.Application.Articles.Commands.AddArticle
{
    public class AddArticleHandler : IRequestHandler<AddArticle, AddArticleResponse>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AddArticleHandler(IArticleRepository articleRepository, ICategoryRepository categoryRepository)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
        }

        public Task<AddArticleResponse> Handle(AddArticle request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
