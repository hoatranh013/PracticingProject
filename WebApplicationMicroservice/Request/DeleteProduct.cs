using Mapster;
using Marten;
using MediatR;

namespace WebApplicationMicroservice.Request
{
    public class DeleteProductModel
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductRequest : IRequest<DeleteProductResponse>
    {
        public Guid Id { get; set; }

    }

    public class DeleteProductResponse
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float ProductCost { get; set; }
        public string Original { get; set; }
        public int Number { get; set; }
    }

    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
    {
        private readonly IDocumentSession _documentSession;
        public DeleteProductHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {

            var getProduct = await _documentSession.LoadAsync<ProductModel>(request.Id.ToString(), cancellationToken);
            _documentSession.Delete<ProductModel>(getProduct);
            await _documentSession.SaveChangesAsync(cancellationToken);
            var response = getProduct.Adapt<DeleteProductResponse>();
            return response;

        }
    }

}
