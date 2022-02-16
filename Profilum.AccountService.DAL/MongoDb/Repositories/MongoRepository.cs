using MongoDB.Bson;
using MongoDB.Driver;

namespace Profilum.AccountService.DAL.MongoDb.Repositories
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _dbCollection;

        public MongoRepository(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            _dbCollection = db.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        

        public async Task<TEntity> Single(object value, string fieldName)
        {
            var query = new BsonDocument(fieldName, BsonValue.Create(value));
            var entity = await _dbCollection.FindSync<TEntity>(query).FirstOrDefaultAsync();

            if (entity == null)
                throw new NullReferenceException($"Document with {fieldName}  {value} not found");

            return entity;
        }

        public async Task<IEnumerable<TEntity>> All()
        {
            var entity = await _dbCollection.FindSync(_ => true).ToListAsync();
            return entity;
        }

        public async Task<bool> Exists(object value, string fieldName)
        {
            var query = new BsonDocument(fieldName, BsonValue.Create(value));
            var entity = await _dbCollection.FindSync<TEntity>(query).FirstOrDefaultAsync();
            return (entity != null);
        }
        

        public async Task Save(TEntity item)
        {
            await _dbCollection.InsertOneAsync(item);
        }
        
        public async Task<bool> Update(object value, string fieldName, TEntity item)
        {
            var query = new BsonDocument(fieldName, BsonValue.Create(value));
            var updateResult = await _dbCollection.ReplaceOneAsync(query, item);

            return updateResult.ModifiedCount > 0;
        }

        public async Task Delete(object value, string fieldName) 
        {
            var query = new BsonDocument(fieldName, BsonValue.Create(value));
            await _dbCollection.DeleteOneAsync(query);
        }
        
        public async Task DeleteAll() 
        {
            var query = new BsonDocument();
            await _dbCollection.DeleteManyAsync(query);
        }
    }
}