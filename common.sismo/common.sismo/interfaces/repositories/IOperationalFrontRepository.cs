using common.sismo.models;
using System;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IOperationalFrontRepository
    {
        Task<OperationalFrontModel> GetOperationalFront(Int32 operationalFrontId);
    }
}
