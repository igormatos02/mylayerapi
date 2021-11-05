using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IParameterGroupRepository
    {
        Task<ParameterGroupModel> GetParameterGroup(int ParameterGroupId);
        Task<List<ParameterGroupModel>> ListParameterGroups();
    }
}
