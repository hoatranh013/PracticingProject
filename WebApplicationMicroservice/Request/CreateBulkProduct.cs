using Mapster;
using Marten;
using MediatR;

namespace WebApplicationMicroservice.Request
{
    public class ProductModel
    {
        public Guid? Id { get; set; }
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }
    }
    public class CreateBulkProductModel
    {
        public ProductModel[] ProductModels { get; set; }
    }

    public class CreateBulkProductRequest : IRequest<CreateBulkProductResponse>
    {
        public ProductModel[] CreateProductModels { get; set; }

    }

    public class CreateBulkProductResponse
    {
        public ProductModel[] CreateProductModels { get; set; }
    }

    public class CreateBulkProductHandler : IRequestHandler<CreateBulkProductRequest, CreateBulkProductResponse>
    {
        private readonly IDocumentSession _documentSession;
        public CreateBulkProductHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<CreateBulkProductResponse> Handle(CreateBulkProductRequest request, CancellationToken cancellationToken)
        {
            _documentSession.StoreObjects(request.CreateProductModels);
            await _documentSession.SaveChangesAsync(cancellationToken);
            var response = request.Adapt<CreateBulkProductResponse>();
            return response;
        }
    }
}
