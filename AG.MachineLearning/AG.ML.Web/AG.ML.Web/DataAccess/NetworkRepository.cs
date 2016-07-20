using AG.ML.Web.Models;
using MongoDB.Driver;

namespace AG.ML.Web.DataAccess
{
    public class NetworkRepository : RepositoryBase<NeuralNetworkMetaInfo>, INetworkRepository
    {
        public NetworkRepository(IMongoDatabase database) : base(database)
        {
        }

        public override string CollectionName
        {
            get
            {
                return "networks";
            }
        }
    }
}