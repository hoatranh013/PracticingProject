using Mapster;
using Marten;
using MediatR;

namespace WebApplicationMicroservice.Request
{
    public class GetProductModel
    {
        public Guid Id { get; set; }
    }

    public class GetProductRequest : IRequest<GetProductResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetProductResponse
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }
    }

    public class GetProductHandler : IRequestHandler<GetProductRequest, GetProductResponse>
    {
        private readonly IDocumentSession _documentSession;
        public GetProductHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public async Task<GetProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {

            var getProduct = await _documentSession.LoadAsync<ProductModel>(request.Id);
            var response = getProduct.Adapt<GetProductResponse>();
            return response;
        }
    }

}
