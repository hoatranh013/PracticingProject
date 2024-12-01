using Mapster;
using Marten;
using MediatR;

namespace WebApplicationMicroservice.Request
{
    public class UpdateProductModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }
    }

    public class UpdateProductRequest : IRequest<UpdateProductResponse>
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }

    }

    public class UpdateProductResponse
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }
    }

    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
    {
        private readonly IDocumentSession _documentSession;
        public UpdateProductHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public async Task<UpdateProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var getProduct = request.Adapt<ProductModel>();
            _documentSession.Update(getProduct);
            await _documentSession.SaveChangesAsync(cancellationToken);
            var response = request.Adapt<UpdateProductResponse>();
            return response;
        }
    }

}
