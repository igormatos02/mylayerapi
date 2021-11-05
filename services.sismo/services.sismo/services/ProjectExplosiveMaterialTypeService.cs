using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class ProjectExplosiveMaterialTypeService : IProjectExplosiveMaterialTypeService
    {
        private readonly IProjectExplosiveMaterialTypeRepository _projectExplosiveMaterialTypeRepository;
        private readonly IConfiguration _configuration;

        public ProjectExplosiveMaterialTypeService(IProjectExplosiveMaterialTypeRepository projectExplosiveMaterialTypeRepository, IConfiguration configuration)
        {
            _projectExplosiveMaterialTypeRepository = projectExplosiveMaterialTypeRepository;
            _configuration = configuration;
        }
        public async Task<List<ProjectExplosiveMaterialTypeModel>> ListProjectExplosiveMaterialTypes(int projectId)
        {

            try
            {
                return await _projectExplosiveMaterialTypeRepository.ListProjectExplosiveMaterialTypes(projectId);

            }
            catch (Exception ex) { throw ex; }
        }

        public async Task SaveProjectExplosiveMaterialTypes(ProjectExplosiveMaterialTypeModel dto)
        {

            try
            {
                await _projectExplosiveMaterialTypeRepository.SaveProjectExplosiveMaterialTypes(dto);

            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ProjectExplosiveMaterialTypeModel> GetProjectExplosiveMaterialType(int projectExplosiveMaterialTypeId)
        {

            try
            {
                return await _projectExplosiveMaterialTypeRepository.GetProjectExplosiveMaterialType(projectExplosiveMaterialTypeId);


            }
            catch (Exception ex) { throw ex; }
        }
    }
}
