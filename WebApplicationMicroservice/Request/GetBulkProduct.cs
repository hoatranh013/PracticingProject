using Mapster;
using Marten;
using MediatR;

namespace WebApplicationMicroservice.Request
{
    public class GetBulkProductModel
    {
        public Guid[] Ids { get; set; }
    }

    public class GetBulkProductRequest : IRequest<GetBulkProductResponse>
    {
        public Guid[] Ids { get; set; }

    }

    public class GetBulkProductResponse
    {
        public ProductModel[] ProductModels { get; set; }
    }

    public class GetBulkProductHandler : IRequestHandler<GetBulkProductRequest, GetBulkProductResponse>
    {
        private readonly IDocumentSession _documentSession;
        public GetBulkProductHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public async Task<GetBulkProductResponse> Handle(GetBulkProductRequest request, CancellationToken cancellationToken)
        {

            var productModels = await _documentSession.LoadManyAsync<ProductModel>(request.Ids);
            var response = new GetBulkProductResponse
            {
                ProductModels = productModels.ToArray()
            };
            return response;
        }
    }

}
