using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class ProjectBaseService : IProjectBaseService
    {
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IConfiguration _configuration;

        public ProjectBaseService(IProjectBaseRepository projectBaseRepository, IConfiguration configuration)
        {
            _projectBaseRepository = projectBaseRepository;
            _configuration = configuration;
        }

        public async Task<ProjectBaseModel> GetBase(int baseId)
        {

            try
            {
                return await _projectBaseRepository.GetBase(baseId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<ProjectBaseModel>> ListBases(int projectId)
        {
            try
            {
                return await _projectBaseRepository.ListBases(projectId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task SaveBase(ProjectBaseModel dto)
        {

            int idP = 0;
            try
            {
                ProjectBaseModel existingDto = await _projectBaseRepository.GetBase(dto.BaseId);
                if (existingDto == null)
                {
                    idP = await _projectBaseRepository.AddBase(dto);

                }
                else
                {
                    idP = dto.BaseId;
                    await _projectBaseRepository.UpdateBase(dto);
                }
            }
            catch (Exception ex) { throw ex; }

        }

        public async Task<bool> UpdateBaseStatus(int baseId)
        {

            try
            {
                var vDto = await _projectBaseRepository.GetBase(baseId);
                if (vDto == null) return false;
                vDto.IsActive = !vDto.IsActive;
                await _projectBaseRepository.UpdateBase(vDto);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> AddFile(Stream fileStream, String fileName, Int32 baseId)
        {
            try
            {
                String fileExtension = fileName.Split('.').Last().ToString();

                String location = "";
                var file = new byte[0];
                if (fileStream != null)
                {
                    file = new byte[fileStream.Length];
                    await fileStream.ReadAsync(file, 0, (int)fileStream.Length);
                    String path = _configuration["mediaDirectory"];
                    String newFileName = Guid.NewGuid().ToString() + "." + fileExtension;
                    location = path + newFileName;
                    //dto.ImagePath = newFileName;
                    await _projectBaseRepository.AddFile(newFileName, baseId);
                }

                if (fileStream != null)
                {
                    DirectoryHelper.SaveFileFromStream(location, file);
                }
                return true;//dal.ListDisplacementRules(dto.SurveyId)
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
