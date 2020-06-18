using System.Collections.Generic;
using MediatR;
using TheGramProfile.Domain.Models.DTO;

namespace TheGramProfile.Domain.Query.GetPostPreviews
{
    public class GetPostPreviewsQuery : IRequest<List<PostPreviewResponse>>
    {
        public GetPostPreviewsQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}