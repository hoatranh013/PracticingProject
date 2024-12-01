using Carter;
using Mapster;
using MediatR;
using WebApplicationMicroservice.Request;

namespace WebApplicationMicroservice.Carter
{
    public class CarterModule : ICarterModule
    {
        private IMediator _mediator;
        public CarterModule(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/create-product", async (CreateProductModel request) =>
            {
                var createProductRequest = request.Adapt<CreateProductRequest>();
                createProductRequest.Id = Guid.NewGuid();
                var createProductResponse = await _mediator.Send(createProductRequest);
            });
            app.MapPost("/create-bulk-product", async (CreateBulkProductModel request) =>
            {
                var createBulkProductRequest = request.Adapt<CreateBulkProductRequest>();
                foreach (var productModel in createBulkProductRequest.CreateProductModels)
                {
                    productModel.Id = Guid.NewGuid();
                }
                var createBulkProductResponse = await _mediator.Send(createBulkProductRequest);
            });
            app.MapDelete("/delete-product", async (DeleteProductModel request) =>
            {
                var deleteProductRequest = request.Adapt<DeleteProductRequest>();
                var deleteProductResponse = await _mediator.Send(deleteProductRequest);
            });
            app.MapPost("/get-bulk-product", async  (GetBulkProductModel request) =>
            {
                var getBulkProductRequest = request.Adapt<GetBulkProductRequest>();
                var getBulkProductResponse = await _mediator.Send(getBulkProductRequest);
            });
            app.MapPost("/get-product", async (GetProductModel request) =>
            {
                var getProductRequest = request.Adapt<GetProductRequest>();
                var getProductResponse = await _mediator.Send(getProductRequest);
            });
            app.MapPut("/update-product", async  (UpdateProductModel request) =>
            {
                var updateProductRequest = request.Adapt<UpdateProductRequest>();
                var updateProductResponse = await _mediator.Send(updateProductRequest);

            });
        }
    }
}
