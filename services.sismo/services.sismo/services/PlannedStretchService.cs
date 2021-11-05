using common.sismo.enums;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class PlannedStretchService : IPlannedStretchService
    {
        private readonly IOperationalFrontRepository _operationalFrontRepository;
        private readonly IPreplotPointRepository _preplotPointRepository;
        private readonly IPlannedStretchRepository _plannedStretchRepository;
        private readonly IStretchRepository _stretchRepository;
        private readonly IConfiguration _configuration;

        public PlannedStretchService(
            IOperationalFrontRepository operationalFrontRepository,
            IPreplotPointRepository preplotPointRepository,
            IPlannedStretchRepository plannedStretchRepository,
            IStretchRepository stretchRepository,
            IConfiguration configuration)
        {
            _operationalFrontRepository = operationalFrontRepository;
            _preplotPointRepository = preplotPointRepository;
            _plannedStretchRepository = plannedStretchRepository;
            _stretchRepository = stretchRepository;
            _configuration = configuration;
        }

        public async Task<bool> AddPlannedStretch(PlannedStretchModel stretch)
        {
            
            try
            {
                var operationalFrontId = await _operationalFrontRepository.GetOperationalFrontType(stretch.OperationalFrontId);

                   var preplotPointType =  await _preplotPointRepository.GetPreplotPointTypeByOpFrontType(operationalFrontId);

                //Verifica se tem interseção com um trecho já criado
                //if (plannedStretchDal.HasIntersectionedStretches(stretch.SurveyId, stretch.OperationalFrontId, stretch.Line,
                //    stretch.InitialStation, stretch.FinalStation) ||
                //    new StretchDAL().HasAnyIntersectionedStretches(stretch.SurveyId, stretch.OperationalFrontId, stretch.Line,
                //    stretch.InitialStation, stretch.FinalStation))
                //    throw new Exception("Já existe uma programação da "+ 
                //        opFrontDal.GetOperationalFrontName(stretch.OperationalFrontId) + 
                //        " criada ou já realizada para uma ou mais estacas contidas no trecho selecionado.");

                await CalculateKmAndPointsAndLineGeom(stretch, preplotPointType);

                stretch.PlanningDateTime = stretch.PlanningDateTime == DateTime.MinValue ? DateTime.Now : stretch.PlanningDateTime;

                await _plannedStretchRepository.AddStretch(stretch);
                return true;
            }
            catch (Exception ex) { throw ex; }

        }

        public async Task<bool> EditPlannedStretch(PlannedStretchModel oldStretch, PlannedStretchModel newStretch)
        {

            try
            {
                await DeletePlannedStretch(oldStretch);

                var insertResult = await AddPlannedStretch(newStretch);
                if (insertResult != true)
                {  
                    await AddPlannedStretch(oldStretch);
                    throw new Exception("Error on Editing Stretch");
                }

                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<bool> DeletePlannedStretch(PlannedStretchModel stretch)
        {

            try
            {
                return await _plannedStretchRepository.DeletePlannedStretch(stretch);
            }
            catch (Exception ex) { throw ex; }
        }


        private async Task CalculateKmAndPointsAndLineGeom(PlannedStretchModel stretch,PreplotPointType preplotPointType)
        {
            var nextStretch = await _plannedStretchRepository.GetNextStation(stretch);
            var previousStretch = await _plannedStretchRepository.GetPreviousStationt(stretch);
            var initialStation = stretch.InitialStation;
            var finalStation = stretch.FinalStation;
            var isPreviousStretchConnected = await _preplotPointRepository.ArePointsConnected(stretch.SurveyId, stretch.Line,
                previousStretch.FinalStation, stretch.InitialStation, preplotPointType);
            var isNextStretchConnected = await _preplotPointRepository.ArePointsConnected(stretch.SurveyId, stretch.Line,
                stretch.FinalStation, nextStretch.InitialStation, preplotPointType);
            if (previousStretch.ExecutionDateString != null && isPreviousStretchConnected && previousStretch.KmRight == false)
            {
                initialStation = previousStretch.FinalStation;
                stretch.KmLeft = true;
            }
            if (nextStretch.ExecutionDateString != null && isNextStretchConnected && nextStretch.KmLeft == false)
            {
                finalStation = nextStretch.InitialStation;
                stretch.KmRight = true;
            }
            Geometry geom;
            int countPoints=0;
            stretch.Km = await _stretchRepository.CalculateKm(stretch.SurveyId, preplotPointType, stretch.Line, initialStation,
                finalStation,  null,  countPoints);
           // stretch.StretchLineGeometry = geom;
            stretch.TotalStations = countPoints;
        }

        public async Task<List<PlannedStretchModel>> ListPlannedStretches(int surveyId, [Optional] int operationalFrontId,
            [Optional] string date, [Optional] string line, [Optional] decimal initialStation,
            [Optional] decimal finalStation, [Optional] int frontGroupLeaderId,
            [Optional] int frontGroupId)
        {

            try
            {
                return await _plannedStretchRepository.ListPlannedStretches(surveyId, operationalFrontId, date, line,
                    initialStation, finalStation, frontGroupLeaderId, frontGroupId);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
