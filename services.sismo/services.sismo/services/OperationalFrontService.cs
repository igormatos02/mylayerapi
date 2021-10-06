using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class OperationalFrontService : IOperationalFrontService
    {
        public readonly IOperationalFrontRepository _operationalFrontRepository;
        public readonly IProjectRepository _projectRepository;

        public OperationalFrontService(IOperationalFrontRepository operationalFrontRepository, IProjectRepository projectRepository)
        {
            _operationalFrontRepository = operationalFrontRepository;
            _projectRepository = projectRepository;
        }

        public async Task<OperationalFrontModel> GetOperationalFront(int operatinalFrontId)
        {
            try { 
                return await _operationalFrontRepository.GetOperationalFront(operatinalFrontId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<List<OperationalFrontProductionModel>>> GetOperationalFrontProduction(int surveyId, string date)
        {  
            try
            {
                if (date == "")
                    date = DateTime.Now.ToString("dd/MM/yyyy");
                var  production = await  _operationalFrontRepository.GetOperationalFrontProduction(surveyId, date);
                List<Int32> OperationalFrontIds = production.Select(s => s.id).Distinct().ToList();
                List<List<OperationalFrontProductionModel>> layers = new List<List<OperationalFrontProductionModel>>();
                foreach (var id in OperationalFrontIds)
                {
                    List<OperationalFrontProductionModel> l = production.Where(m => m.id == id).ToList();
                    layers.Add(l);
                }
                return layers;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<OperationalFrontModel>> ListProjectOperationalFronts(int projectId)
        {
            try
            {
                return await _operationalFrontRepository.ListProjectOperationalFronts(projectId);
            }
            catch (Exception ex){ throw ex; }
        }

        public async Task<List<OperationalFrontModel>> ListSurveyOperationalFronts(int surveyId)
        {
            try
            {
                return await _operationalFrontRepository.ListSurveyOperationalFronts(surveyId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<OperationalFrontModel>> ListSurveyOperationalFronts(int surveyId, int operationalFrontType)
        {
            try
            {
                return await  _operationalFrontRepository.ListSurveyOperationalFronts(surveyId, operationalFrontType);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<OperationalFrontModel>> ListProjectOperationalFronts(int projectId, int opFrontIdToExclude)
        {
            try
            {
                var projects = await _operationalFrontRepository.ListProjectOperationalFronts(projectId);
                return projects.Where(x => x.OperationalFrontId != opFrontIdToExclude).ToList(); 

            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<object> ListOperationalFrontTypes()
        {
            try
            {
               return Task<object>.FromResult(EnumHelper.ListEnumObjectsToScreen(typeof(OperationalFrontType)));
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<OperationalFrontModel> SaveOperationalFront(OperationalFrontModel operationalFront)
        {
            try
            {
                var existingDto = await _operationalFrontRepository.GetOperationalFront(operationalFront.OperationalFrontId);
                if (existingDto == null)
                    return await _operationalFrontRepository.AddOperationalFront(operationalFront);
                else
                {
                    if (existingDto.PreviousOperationalFrontId != operationalFront.PreviousOperationalFrontId &&
                        await _operationalFrontRepository.HasAnyProduction(operationalFront.OperationalFrontId))
                        throw new Exception("A frente operacional selecionada já possui produção lançada e por isso não é possível alterar sua frente predecessora.");
                    await _operationalFrontRepository.UpdateOperationalFront(operationalFront);
                    return operationalFront;
                }
                //result.Data = dal.ListProjectOperationalFronts(operationalFront.ProjectId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<OperationalFrontModel> DeleteOperationalFront(OperationalFrontModel operationalFront)
        {
            try
            {
                var existingDto = await _operationalFrontRepository.GetOperationalFront(operationalFront.OperationalFrontId);
                if (existingDto != null)
                {
                    if (existingDto.PreviousOperationalFrontId.HasValue)
                        throw new Exception("Não é possível exlcluir uma frente operacional que possua uma frente predecessora.");
                    if (await _operationalFrontRepository.HasAnyProduction(operationalFront.OperationalFrontId))
                        throw new Exception("A frente operacional selecionada já possui produção lançada e por isso não pode ser excluída.");
                    await _operationalFrontRepository.DeleteOperationalFront(operationalFront.OperationalFrontId);
                }
                else
                    throw new Exception("A frente operacional não existe.");
                return operationalFront;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
