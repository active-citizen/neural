using AG.ML.Web.Models;
using System.Collections.Generic;

namespace AG.ML.Web.DataAccess
{
    public interface INetworkRepository
    {
        IEnumerable<NeuralNetworkMetaInfo> GetAll();

        NeuralNetworkMetaInfo GetById(string id);

        void Insert(NeuralNetworkMetaInfo entity);

        void InsertMany(IEnumerable<NeuralNetworkMetaInfo> networks);
    }
}