using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using ServiceStack.Messaging;
using ServiceStack.Redis;
using Weasel.Core.Migrations;
using WebApplicationMicroservice.Request;

namespace WebApplicationMicroservice.Notification
{
    public class CreateBulkProductNotification : INotification
    {
        public ProductModel[] ProductModels { get; set; }
    }

    public class CreateBulkProductNotificationRedis : INotificationHandler<CreateBulkProductNotification>
    {
        private readonly IRedisClientsManager _redisClientsManager;
        public CreateBulkProductNotificationRedis(IRedisClientsManager redisClientsManager)
        {
            _redisClientsManager = redisClientsManager;
        }
        public async Task Handle(CreateBulkProductNotification notification, CancellationToken cancellationToken)
        {
            using (var redis = _redisClientsManager.GetClient())
            {
                foreach (var product in notification.ProductModels)
                {
                    redis.Set(product.Id.ToString(), JsonConvert.SerializeObject(product));
                }
            }
        }
    }

    public class CreateBulkProductNotificationMongoDB : INotificationHandler<CreateBulkProductNotification>
    {
        private readonly IMongoDatabase _database;
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<ProductModel> _productCollection;
        public CreateBulkProductNotificationMongoDB(IMongoClient mongoClient, IConfiguration configuration)
        {
            _configuration = configuration;
            var databaseName = _configuration.GetValue<string>("MongoDbSettings:DatabaseName");
            _database = mongoClient.GetDatabase(databaseName);
            var productCollectionName = configuration.GetValue<string>("MongoDbSettings:Collections:ProductCollection");
            _productCollection = _database.GetCollection<ProductModel>(productCollectionName);
        }
        public async Task Handle(CreateBulkProductNotification notification, CancellationToken cancellationToken)
        {
            await _productCollection.InsertManyAsync(notification.ProductModels);
        }
    }

    public class CreateBulkProductNotificationRabbitMQ : INotificationHandler<CreateBulkProductNotification>
    {
        private readonly IBus _bus;
        public CreateBulkProductNotificationRabbitMQ(IBus bus)
        {
            _bus = bus;
        }
        public async Task Handle(CreateBulkProductNotification notification, CancellationToken cancellationToken)
        {
            var getMessage = JsonConvert.SerializeObject(notification);
            await _bus.Publish(getMessage);
        }
    }

}
