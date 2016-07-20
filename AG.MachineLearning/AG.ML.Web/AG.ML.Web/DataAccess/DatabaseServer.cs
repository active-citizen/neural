using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace AG.ML.Web.DataAccess
{
    public class DatabaseServer
    {
        private readonly IMongoDatabase dataBase;

        public DatabaseServer(MongoUrl mongoUrl)
        {
            // going camelCase for mongodb driver
            var convension = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCaseConvention", convension, type => true);

            var client = new MongoClient(mongoUrl);
            dataBase = client.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase Database
        {
            get
            {
                return dataBase;
            }
        }
    }
}