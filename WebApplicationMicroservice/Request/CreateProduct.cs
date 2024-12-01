using Mapster;
using Marten;
using MediatR;

namespace WebApplicationMicroservice.Request
{
    public class CreateProductModel
    {
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }
    }

    public class CreateProductRequest : IRequest<CreateProductResponse>
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }

    }

    public class CreateProductResponse
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }
    }

    public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly IDocumentSession _documentSession;
        public CreateProductHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productModel = request.Adapt<ProductModel>();
            _documentSession.Store(productModel);
            await _documentSession.SaveChangesAsync(cancellationToken);
            var response = request.Adapt<CreateProductResponse>();
            return response;
        }
    }

}
