using AG.ML.Web.DataAccess;
using AG.ML.Web.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AG.ML.Web.Services
{
    public interface INetworkService
    {
        IEnumerable<NeuralNetworkMetaInfo> GetNetworkInfos();
        void SaveNetworkInfo(NeuralNetworkMetaInfo info);
        NeuralNetworkMetaInfo GetNetworkInfo(string id);
    }

    public class NetworkService : INetworkService
    {
        private readonly INetworkRepository repository;

        public NetworkService(INetworkRepository repository)
        {
            this.repository = repository;
        }

        public void SaveNetworkInfo(NeuralNetworkMetaInfo info)
        {
            if (string.IsNullOrEmpty(info.Id))
            {
                info.Id = Guid.NewGuid().ToString();
            }
            repository.Insert(info);
        }

        public NeuralNetworkMetaInfo GetNetworkInfo(string id)
        {
            return repository.GetById(id);
        }

        public IEnumerable<NeuralNetworkMetaInfo> GetNetworkInfos()
        {
            /*
            var networks = 
            new List<NeuralNetworkMetaInfo>
            {
                new NeuralNetworkMetaInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Deep believe network, take 1"
                },
                new NeuralNetworkMetaInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Deep believe network, take 2"
                }
            };
            repository.InsertMany(networks);
            */
            return repository.GetAll().ToList();
        }
    }
}