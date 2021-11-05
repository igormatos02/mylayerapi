using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IParameterRepository
    {
        Task<ParameterModel> GetParameter(int ParameterId);
        Task<List<ParameterModel>> ListParameters();
    }
}
