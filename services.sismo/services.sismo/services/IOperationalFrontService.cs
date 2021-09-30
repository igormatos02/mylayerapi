using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using System;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class OperationalFrontService : IOperationalFrontService
    {
        public readonly IOperationalFrontRepository _operationalFrontRepository;

        public OperationalFrontService(IOperationalFrontRepository operationalFrontRepository)
        {
            _operationalFrontRepository = operationalFrontRepository;
        }

        public async Task<OperationalFrontModel> GetOperationalFront(int operatinalFrontId)
        {
            try { 
                return await _operationalFrontRepository.GetOperationalFront(operatinalFrontId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
