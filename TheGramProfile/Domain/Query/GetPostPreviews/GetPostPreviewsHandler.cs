using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheGramProfile.Domain.Models.DTO;
using TheGramProfile.EventBus.Channels;
using TheGramProfile.EventBus.Requests;
using TheGramProfile.Properties;

namespace TheGramProfile.Domain.Query.GetPostPreviews
{
    public class GetPostPreviewsHandler : IRequestHandler<GetPostPreviewsQuery,List<PostPreviewResponse>>
    {
        private readonly RabbitMQRemoteProcedureCall<List<PostPreviewResponse>> _rabbitRPC;

        public GetPostPreviewsHandler()
        {
            _rabbitRPC = new RabbitMQRemoteProcedureCall<List<PostPreviewResponse>>(RabbitMqChannels.GetPostPreviews);
            
        }

        public async Task<List<PostPreviewResponse>> Handle(GetPostPreviewsQuery request, CancellationToken cancellationToken)
        {
            var response = _rabbitRPC.MakeRemoteProcedureCall<List<PostPreviewResponse>>(request.UserId, cancellationToken);
            _rabbitRPC.Dispose();
            return response;
        }
    }
}