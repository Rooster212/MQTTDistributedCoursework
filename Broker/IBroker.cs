using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace BrokerApp
{
    [ServiceContract]
    public interface IBroker
    {
        [OperationContract]
        string[] GetServers();

        [OperationContract]
        List<MQTTClient> GetClients();

        [OperationContract]
        bool TestCommunication();

    }
}
