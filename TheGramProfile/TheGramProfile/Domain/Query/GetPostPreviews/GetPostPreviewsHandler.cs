using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheGramProfile.Domain.Models.DTO;
using TheGramProfile.EventBus;
using TheGramProfile.Properties;

namespace TheGramProfile.Domain.Query.GetPostPreviews
{
    public class GetPostPreviewsHandler : IRequestHandler<GetPostPreviewsQuery,List<PostPreviewResponse>>
    {
        private readonly RabbitRPC<List<PostPreviewResponse>> _rpcEventBus;

        public GetPostPreviewsHandler()
        {
            _rpcEventBus = new RabbitRPC<List<PostPreviewResponse>>(RabbitMqChannels.GetPostPreviews);
            
        }

        public async Task<List<PostPreviewResponse>> Handle(GetPostPreviewsQuery request, CancellationToken cancellationToken)
        {
            var results =_rpcEventBus.Request<List<PostPreviewResponse>>(request.UserId);
            _rpcEventBus.Dispose();
            return results;
        }
    }
}