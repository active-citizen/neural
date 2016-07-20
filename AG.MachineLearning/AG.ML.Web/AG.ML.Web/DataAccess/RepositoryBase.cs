using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AG.ML.Web.DataAccess
{
    public abstract class RepositoryBase<T>
    {
        private readonly IMongoDatabase database;

        private readonly MongoCollectionSettings collectionSettings;

        private IMongoCollection<T> collection;

        protected RepositoryBase(IMongoDatabase database)
            : this(database, new MongoCollectionSettings())
        {
        }

        protected RepositoryBase(IMongoDatabase database, MongoCollectionSettings collectionSettings)
        {
            this.database = database;
            this.collectionSettings = collectionSettings;
        }

        public abstract string CollectionName { get; }

        public IMongoCollection<T> Collection
        {
            get { return collection ?? (collection = InitializeCollection(collectionSettings)); }
        }

        protected IMongoDatabase Database
        {
            get { return database; }
        }

        public virtual T GetById(string id)
        {
            var filter = new FilterDefinitionBuilder<T>()
                .Eq("_id", new BsonString(id));
            return Collection.Find(filter).FirstOrDefault();
        }

        public virtual void Drop()
        {
            Database.DropCollection(CollectionName);
        }

        public virtual void Insert(T entity)
        {
            Collection.InsertOne(entity);
        }

        public virtual void InsertMany(IEnumerable<T> entities)
        {
            Collection.InsertMany(entities);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Collection.Find(new BsonDocument()).ToList();
        }

        public virtual void RemoveById(string id)
        {
            var filter = new FilterDefinitionBuilder<T>()
                .Eq("_id", new BsonObjectId(new ObjectId(id)));

            Collection.FindOneAndDelete(filter);
        }

        protected static MongoCollectionSettings GetReadOnlyRepositorySettings()
        {
            return new MongoCollectionSettings { ReadPreference = ReadPreference.SecondaryPreferred };
        }

        /*
        protected virtual MongoCursor<T> FindMany(IMongoQuery query)
        {
            log.DebugFormat("{0} find: {1}", CollectionName, query);
            return Collection.FindAs<T>(query).SetBatchSize(1000);
        }

        protected virtual T FindOne(IMongoQuery query)
        {
            log.DebugFormat("{0} find: {1}", CollectionName, query);
            return Collection.FindOneAs<T>(query);
        }

        protected virtual long? Update(IMongoQuery query, IMongoUpdate update, UpdateFlags updateFlags = UpdateFlags.None)
        {
            log.DebugFormat("'{0}': find '{1}' and update '{2}'.", CollectionName, query, update);

            var writeConcern = Collection.Update(query, update, updateFlags);
            return writeConcern != null ? writeConcern.DocumentsAffected : (long?)null;
        }
        
        protected virtual TCommandResult RunCommandAs<TCommandResult>(IMongoCommand command) where TCommandResult : CommandResult
        {
            return database.RunCommandAs<TCommandResult>(command);
        }

        protected virtual IEnumerable<IMongoIndexKeys> GetIndexes()
        {
            return Enumerable.Empty<IMongoIndexKeys>();
        }

        protected virtual IEnumerable<IMongoIndexKeys> GetUniqueIndexes()
        {
            return Enumerable.Empty<IMongoIndexKeys>();
        }

        protected virtual IEnumerable<KeyValuePair<IMongoIndexKeys, TimeSpan>> GetTtlIndexes()
        {
            return Enumerable.Empty<KeyValuePair<IMongoIndexKeys, TimeSpan>>();
        }
        */
        protected virtual IMongoCollection<T> InitializeCollection(MongoCollectionSettings settings)
        {
            var mongoCollection = database.GetCollection<T>(CollectionName, settings);
            /*
            var indexes = GetIndexes();
            foreach (var index in indexes)
            {
                log.DebugFormat("Adding indexes to the collection '{0}' - {1}", CollectionName, index);
                mongoCollection.EnsureIndex(index);
            }

            var uniqueIndexes = GetUniqueIndexes();
            foreach (var index in uniqueIndexes)
            {
                log.DebugFormat("Adding unique indexes to the collection '{0}' - {1}", CollectionName, index);
                mongoCollection.EnsureIndex(index, IndexOptions.SetUnique(true).SetSparse(true));
            }

            var ttlIndexes = GetTtlIndexes();
            foreach (var pair in ttlIndexes)
            {
                log.DebugFormat("Adding ttl indexes to the collection '{0}' - {1} (expireAfterSeconds: {2})", CollectionName, pair.Key, pair.Value);
                mongoCollection.EnsureIndex(pair.Key, IndexOptions.SetTimeToLive(pair.Value));

                // we cannot use ensureIndex to change ttl time of existing index, so we have to do the trick below.
                var index = new BsonDocument
                                {
                                    { "keyPattern", pair.Key.ToBsonDocument() },
                                    { "expireAfterSeconds", (int)pair.Value.TotalSeconds }
                                };
                var command = new CommandDocument { { "collMod", CollectionName }, { "index", index } };
                database.RunCommand(command);
            }
            */
            return mongoCollection;
        }

        protected virtual bool IsSystemField(BsonElement bsonElement)
        {
            return bsonElement.Name.StartsWith("_", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}