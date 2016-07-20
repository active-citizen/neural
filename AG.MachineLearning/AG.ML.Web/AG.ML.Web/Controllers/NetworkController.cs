using AG.ML.Web.Models;
using AG.ML.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AG.ML.Web.Controllers
{
    [RoutePrefix("api/network")]
    public class NetworkController : ApiController
    {
        private readonly INetworkService networkService;

        public NetworkController(INetworkService networkService)
        {
            this.networkService = networkService;
        }

        [Route]
        public IEnumerable<NeuralNetworkMetaInfo> GetList()
        {
            return networkService.GetNetworkInfos();
        }

        [Route("{id}")]
        public NeuralNetworkMetaInfo GetItem(string id)
        {
            return networkService.GetNetworkInfo(id);
        }

        [Route]
        public void PostItem(NeuralNetworkMetaInfo info)
        {
            networkService.SaveNetworkInfo(info);
        }
    }
}
