using common.sismo.models;
using System;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services
{
    public interface IOperationalFrontService
    {
        Task<OperationalFrontModel> GetOperationalFront(Int32 operationalFrontId);
    }
}
