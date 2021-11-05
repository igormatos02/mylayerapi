using common.sismo.enums;
using common.sismo.helpers;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
   
    public class ProjectExplosiveMaterialService : IProjectExplosiveMaterialService
    {
        private readonly IProjectExplosiveMaterialRepository _projectExplosiveMaterialRepository;
        private readonly IProjectExplosiveMaterialTypeRepository _projectExplosiveMaterialTypeRepository;
        private readonly IConfiguration _configuration;

        public ProjectExplosiveMaterialService(
            IProjectExplosiveMaterialTypeRepository projectExplosiveMaterialTypeRepository,
            IProjectExplosiveMaterialRepository projectExplosiveMaterialRepository,
            IConfiguration configuration
            )
        {
            _projectExplosiveMaterialRepository = projectExplosiveMaterialRepository;
            _projectExplosiveMaterialTypeRepository = projectExplosiveMaterialTypeRepository;
            _configuration = configuration;
        }

        public async Task<ProjectExplosiveMaterialDataModel> ListProjectExplosiveMaterials(int projectId, Int32 entryType)
        {
            
            try
            {
                var materials = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, entryType);

                return new ProjectExplosiveMaterialDataModel
                {
                            Materials = materials,
                            TotalVolumeKg = await GetTotalVolumeAvailable(projectId, 0, "kg"),
                            TotalVolumeM = await GetTotalUnityAvailable(projectId, 0, "m")
                 };
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ProjectExplosiveMaterialDataModel> ListSurveyExplosiveMaterials(int projectId, int surveyId, Int32 entryType)
        {

            try
            {
                var _materials = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, entryType);
                var materials = _materials.Where(d => d.SurveyId == surveyId).ToList();

                return  new ProjectExplosiveMaterialDataModel
                {
                    Materials = materials,
                    TotalProjectVolumeKg = await GetTotalVolumeAvailable(projectId, 0, "kg"),
                    TotalProjectVolumeM = await GetTotalUnityAvailable(projectId, 0, "m"),
                    TotalSurveyVolumeKg = await GetTotalVolumeAvailable(projectId, surveyId, "kg"),
                    TotalSurveyVolumeM = await GetTotalUnityAvailable(projectId, surveyId, "m"),
                };
            }
            catch (Exception ex) { throw ex; }
        }


        public async Task<ProjectExplosiveMaterialTypeSummaryDataModel> GetDashboardData(Int32 projectId, string date)
        {
            var materialTypes = await _projectExplosiveMaterialTypeRepository.ListProjectExplosiveMaterialTypes(projectId);
            List<ProjectExplosiveMaterialTypeSummaryModel> projectExplosiveMaterialTypeSummaryList = new List<ProjectExplosiveMaterialTypeSummaryModel>();
            DateTime limitDate = DateHelper.GetDBValue(date, "NormalDate");

            int days = DateTime.DaysInMonth(limitDate.Date.Year, limitDate.Date.Month);
            var dates = new List<DateTime>();
            var datesToReturn = new List<String>();
            var minDate = DateHelper.GetDBValue("01/" + limitDate.Date.Month.ToString() + "/" + limitDate.Date.Year.ToString(), "NormalDate");
            var maxDate = DateHelper.GetDBValue(days.ToString() + "/" + limitDate.Date.Month.ToString() + "/" + limitDate.Date.Year.ToString(), "NormalDate");
            DecimalChartSerie ExplosiveStock = new DecimalChartSerie();
            DecimalChartSerie FuseStock = new DecimalChartSerie();

            DecimalChartSerie ExplosiveUsed = new DecimalChartSerie();
            DecimalChartSerie FuseUsed = new DecimalChartSerie();

            // int countData = limitDate.Subtract(minDate).Days+1;
            ExplosiveStock.data = new decimal[days];
            FuseStock.data = new decimal[days];
            ExplosiveUsed.data = new decimal[days];
            FuseUsed.data = new decimal[days];

            ExplosiveStock.type = "spline";
            FuseStock.type = "spline";
            ExplosiveUsed.type = "column";
            FuseUsed.type = "column";

            ExplosiveStock.name = "Estoque de Emulsão";
            FuseStock.name = "Estoque de Espoletas";
            ExplosiveUsed.name = "Uso de Emulsão";
            FuseUsed.name = "Uso de Espoletas";

            var cont = 0;


            var _materialMetersIds = await _projectExplosiveMaterialTypeRepository.ListProjectExplosiveMaterialTypes(projectId);
            var materialMetersIds = _materialMetersIds.Where(p => p.Unity == "m").Select(s => s.ProjectExplosiveMaterialTypeId).ToList();
            var _materialKgIds = await _projectExplosiveMaterialTypeRepository.ListProjectExplosiveMaterialTypes(projectId);
            var materialKgIds = _materialKgIds.Where(p => p.Unity == "kg").Select(s => s.ProjectExplosiveMaterialTypeId).ToList();
            var _totalEntriesInKm = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In);
            var totalEntriesInKm = _totalEntriesInKm.Where(p => p.ProjectId == projectId && materialKgIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
            var _totalEntriesOutKm = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out);
            var totalEntriesOutKm = _totalEntriesOutKm.Where(p => p.ProjectId == projectId && materialKgIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
            var _totalEntriesUseKm = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use);
            var totalEntriesUseKm = _totalEntriesUseKm.Where(p => p.ProjectId == projectId && materialKgIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
            var _totalEntriesInM = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In);
            var totalEntriesInM = _totalEntriesInM.Where(p => p.ProjectId == projectId && materialMetersIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
            var _totalEntriesOutM = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out);
            var totalEntriesOutM = _totalEntriesOutM.Where(p => p.ProjectId == projectId && materialMetersIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
            var _totalEntriesUseM = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use);
            var totalEntriesUseM = _totalEntriesUseM.Where(p => p.ProjectId == projectId && materialMetersIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();


            for (var dt = minDate; dt <= maxDate; dt = dt.AddDays(1))
            {

                dates.Add(dt);
                datesToReturn.Add(dt.ToString("dd/MM"));


                var totalTypeMaterialsInKm = totalEntriesInKm.Where(d => d.EntryDate.Date <= dt).Sum(s => s.AmountIn) + totalEntriesInKm.Where(d => d.EntryDate.Date <= dt).Sum(s => s.DifferenceAmount);

                var totalMaterialsTypeOutFieldKm = totalEntriesOutKm.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
                var totalMaterialsTypeOutDestroyedKm = totalEntriesOutKm.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
                var totalMaterialsTypeOutTransferedKm = totalEntriesOutKm.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
                var totalMaterialsTypeOutStockKm = totalEntriesOutKm.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);

                var totalMaterialsTypeUseKm = totalEntriesUseKm.Where(d => d.EntryDate.Date == dt).Sum(s => s.AmountUsed);

                var totalAmountTypeAvailableStock = totalTypeMaterialsInKm - (totalMaterialsTypeOutFieldKm + totalMaterialsTypeOutDestroyedKm + totalMaterialsTypeOutTransferedKm + totalMaterialsTypeOutStockKm);
                var totalAmountTypeAvailableField = totalMaterialsTypeOutFieldKm - (totalMaterialsTypeUseKm) + totalMaterialsTypeOutStockKm;


                var totalTypeMaterialsInM = totalEntriesInM.Where(d => d.EntryDate.Date <= dt).Sum(s => s.AmountIn) + totalEntriesInM.Where(d => d.EntryDate.Date <= dt).Sum(s => s.DifferenceAmount);

                var totalMaterialsTypeOutFieldM = totalEntriesOutM.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
                var totalMaterialsTypeOutDestroyedM = totalEntriesOutM.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
                var totalMaterialsTypeOutTransferedM = totalEntriesOutM.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
                var totalMaterialsTypeOutStockM = totalEntriesOutM.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);

                var totalMaterialsTypeUseM = totalEntriesUseM.Where(d => d.EntryDate.Date == dt).Sum(s => s.AmountUsed);

                var totalAmountTypeAvailableStockM = totalTypeMaterialsInM - (totalMaterialsTypeOutFieldM + totalMaterialsTypeOutDestroyedM + totalMaterialsTypeOutTransferedM + totalMaterialsTypeOutStockM);
                var totalAmountTypeAvailableFieldM = totalMaterialsTypeOutFieldM - (totalMaterialsTypeUseM) + totalMaterialsTypeOutStockM;




                ExplosiveStock.data[cont] = Convert.ToDecimal(totalAmountTypeAvailableStock);
                FuseStock.data[cont] = Convert.ToDecimal(totalAmountTypeAvailableStockM);
                ExplosiveUsed.data[cont] = Convert.ToDecimal(totalMaterialsTypeUseKm);
                FuseUsed.data[cont] = Convert.ToDecimal(totalMaterialsTypeUseM);

                cont++;
            }

            foreach (ProjectExplosiveMaterialTypeModel dto in materialTypes)
            {
                var materialSummary = await GetProjectExplosiveMaterialTypeSummary(projectId, 0, dto.ProjectExplosiveMaterialTypeId, limitDate);
                projectExplosiveMaterialTypeSummaryList.Add(materialSummary);

            }
            
            var result =  new ProjectExplosiveMaterialTypeSummaryDataModel
            {
                MaterialTypes = materialTypes,
                ProjectExplosiveMaterialTypeSummaryList = projectExplosiveMaterialTypeSummaryList,
                DatesToReturn = datesToReturn
            };
            result.FuseStock = FuseStock;
            result.ExplosiveStock = ExplosiveStock;
            result.ExplosiveUsed = ExplosiveUsed;
            result.FuseUsed = FuseUsed;

            return result;
        }
       

        public async Task<ProjectExplosiveMaterialTypeSummaryModel> GetProjectExplosiveMaterialTypeSummary(int projectId, int surveyId, int projectExplosiveMaterialTypeId, DateTime dateLimit)
        {

            //Unique
            var _typeEntriesIn = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In);
            var typeEntriesIn = _typeEntriesIn.Where(p => p.ProjectId == projectId && p.ProjectExplosiveMaterialTypeId == projectExplosiveMaterialTypeId && p.EntryDate.Date <= dateLimit.Date);
            var _typeEntriesOut = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out);
            var typeEntriesOut = _typeEntriesOut.Where(p => p.ProjectId == projectId && p.ProjectExplosiveMaterialTypeId == projectExplosiveMaterialTypeId && p.EntryDate.Date <= dateLimit.Date && (surveyId == 0 || p.SurveyId == surveyId));
            var _typeEntriesUse = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use);
            var typeEntriesUse = _typeEntriesUse.Where(p => p.ProjectId == projectId && p.ProjectExplosiveMaterialTypeId == projectExplosiveMaterialTypeId && p.EntryDate.Date <= dateLimit.Date && (surveyId == 0 || p.SurveyId == surveyId));



            var totalTypeMaterialsIn = typeEntriesIn.Sum(s => s.AmountIn) + typeEntriesIn.Sum(s => s.DifferenceAmount);

            var totalMaterialsTypeOutField = typeEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field).Sum(s => s.AmountOut);
            var totalMaterialsTypeOutDestroyed = typeEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed).Sum(s => s.AmountOut);
            var totalMaterialsTypeOutTransfered = typeEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered).Sum(s => s.AmountOut);
            var totalMaterialsTypeOutStock = typeEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock).Sum(s => s.AmountOut);

            var totalMaterialsTypeUse = typeEntriesUse.Sum(s => s.AmountUsed);

            var totalAmountTypeAvailableStock = totalTypeMaterialsIn - (totalMaterialsTypeOutField + totalMaterialsTypeOutDestroyed + totalMaterialsTypeOutTransfered + totalMaterialsTypeOutStock);
            var totalAmountTypeAvailableField = totalMaterialsTypeOutField - (totalMaterialsTypeUse) + totalMaterialsTypeOutStock;


            //Total
            var materialType = await _projectExplosiveMaterialTypeRepository.GetProjectExplosiveMaterialType(projectExplosiveMaterialTypeId);

            var _materialTypeIds = await _projectExplosiveMaterialTypeRepository.ListProjectExplosiveMaterialTypes(projectId);
            var materialTypeIds = _materialTypeIds.Where(p => p.Unity == materialType.Unity).Select(s => s.ProjectExplosiveMaterialTypeId).ToList();
            var _totalEntriesIn = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In);
            var totalEntriesIn = _totalEntriesIn.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
            var _totalEntriesOut = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out);
            var totalEntriesOut = _totalEntriesOut.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
            var _totalEntriesUse = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use);
            var totalEntriesUse = _totalEntriesUse.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();

            var totalMaterialsIn = totalEntriesIn.Sum(s => s.Volume) + totalEntriesIn.Sum(s => (s.Volume / (s.AmountIn == 0 ? 1 : s.AmountIn)) * s.DifferenceAmount);
         
            var totalMaterialsVolumeOutField = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field).Sum(s => s.Volume);
            var totalMaterialsVolumeOutDestroyed = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed).Sum(s => s.Volume);
            var totalMaterialsVolumeOutTransfered = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered).Sum(s => s.Volume);
            var totalMaterialsVolumeOutStock = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock).Sum(s => s.Volume);



            var totalMaterialVolumeAvailableStock = totalMaterialsIn - (totalMaterialsVolumeOutField + totalMaterialsVolumeOutDestroyed + totalMaterialsVolumeOutTransfered + totalMaterialsVolumeOutStock);




            var projectExplosiveMaterialTypeSummaryDTO = new ProjectExplosiveMaterialTypeSummaryModel()
            {
                TotalTypeMaterialsIn = totalTypeMaterialsIn,
                TotalAmountAvailableStock = totalAmountTypeAvailableStock,
                TotalAmountAvailableField = totalAmountTypeAvailableField,
                TotalMaterialVolumeAvailableStock = totalMaterialVolumeAvailableStock,
                TotalMaterialsTypeUse = totalMaterialsTypeUse,
                MaterialType = materialType,
                TotalMaterialsIn = totalTypeMaterialsIn,
                TotalMaterialsVolumeOutDestroyed = totalMaterialsTypeOutDestroyed,
                TotalMaterialsVolumeOutTransfered = totalMaterialsTypeOutTransfered

            };

            return projectExplosiveMaterialTypeSummaryDTO;
        }

        public async Task<ProjectExplosiveMaterialTypeSummaryModel> GetAmountAvailable(int projectId, int surveyId, int projectExplosiveMaterialTypeId)
        {
            try
            {
                return  await GetProjectExplosiveMaterialTypeSummary(projectId, surveyId, projectExplosiveMaterialTypeId, DateTime.Now);
            }
             catch (Exception ex) { throw ex; }
        }

        public async Task<decimal> GetTotalVolumeAvailable(int projectId, int surveyId, String Unity)
        {
            decimal? totalMaterialVolumeAvailableStock = 0;
            try
            {
       
                var materialTypes = await _projectExplosiveMaterialTypeRepository.ListProjectExplosiveMaterialTypes(projectId);
                var materialTypeIds = materialTypes.Where(p => p.Unity == Unity).Select(s => s.ProjectExplosiveMaterialTypeId).ToList();

                var _totalEntriesIn = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In);
                var totalEntriesIn = _totalEntriesIn.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();
                var _totalEntriesOut = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out);
                var totalEntriesOut = _totalEntriesOut.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();
                var _totalEntriesUse = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use);
                var totalEntriesUse = _totalEntriesUse.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();

                var totalMaterialsIn = totalEntriesIn.Sum(s => s.Volume) + totalEntriesIn.Sum(s => (s.Volume / (s.AmountIn == 0 ? 1 : s.AmountIn)) * s.DifferenceAmount);

                var totalMaterialsVolumeOutField = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field).Sum(s => s.Volume);
                var totalMaterialsVolumeOutDestroyed = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed).Sum(s => s.Volume);
                var totalMaterialsVolumeOutTransfered = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered).Sum(s => s.Volume);
                var totalMaterialsVolumeOutStock = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock).Sum(s => s.Volume);



                totalMaterialVolumeAvailableStock = totalMaterialsIn - (totalMaterialsVolumeOutField + totalMaterialsVolumeOutDestroyed + totalMaterialsVolumeOutTransfered + totalMaterialsVolumeOutStock);




            }
              catch (Exception ex) { throw ex; }
            return totalMaterialVolumeAvailableStock.Value;
        }

        public async Task<decimal> GetTotalUnityAvailable(int projectId, int surveyId, String Unity)
        {
            decimal? totalMaterialUnityAvailableStock = 0;
            try
            {


                var _materialTypeIds = await _projectExplosiveMaterialTypeRepository.ListProjectExplosiveMaterialTypes(projectId);
                var materialTypeIds = _materialTypeIds.Where(p => p.Unity == Unity).Select(s => s.ProjectExplosiveMaterialTypeId).ToList();
                var _totalEntriesIn = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In);
                var totalEntriesIn = _totalEntriesIn.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();
                var _totalEntriesOut = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out);
                var totalEntriesOut = _totalEntriesOut.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();
                var _totalEntriesUse = await _projectExplosiveMaterialRepository.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use);
                var totalEntriesUse = _totalEntriesUse.Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();

                var totalMaterialsIn = totalEntriesIn.Sum(s => s.AmountIn) + totalEntriesIn.Sum(s => s.DifferenceAmount);

                var totalMaterialsVolumeOutField = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field).Sum(s => s.AmountOut);
                var totalMaterialsVolumeOutDestroyed = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed).Sum(s => s.AmountOut);
                var totalMaterialsVolumeOutTransfered = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered).Sum(s => s.AmountOut);
                var totalMaterialsVolumeOutStock = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock).Sum(s => s.AmountOut);



                totalMaterialUnityAvailableStock = totalMaterialsIn - (totalMaterialsVolumeOutField + totalMaterialsVolumeOutDestroyed + totalMaterialsVolumeOutTransfered + totalMaterialsVolumeOutStock);




            }
            catch (Exception ex) { throw ex; }
            return totalMaterialUnityAvailableStock.Value;
        }

        public async Task<bool> SaveProjectExplosiveMaterials(ProjectExplosiveMaterialModel dto)
        {
            try
            {
                var materialType = await _projectExplosiveMaterialTypeRepository.GetProjectExplosiveMaterialType(dto.ProjectExplosiveMaterialTypeId);
                if (dto.EntryType == ExplosiveMaterialEntryType.In)
                    dto.Volume = dto.AmountIn * materialType.Volume;
                if (dto.EntryType == ExplosiveMaterialEntryType.Out)
                    dto.Volume = dto.AmountOut * materialType.Volume;
                if (dto.EntryType == ExplosiveMaterialEntryType.Use)
                    dto.Volume = dto.AmountUsed * materialType.Volume;

                await _projectExplosiveMaterialRepository.SaveProjectExplosiveMaterials(dto);

                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> DeleteProjectExplosiveMaterial(ProjectExplosiveMaterialModel dto)
        {
            try
            {
               await  _projectExplosiveMaterialRepository.DeleteProjectExplosiveMaterial(dto);

                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<ProjectExplosiveMaterialModel> GetProjectExplosiveMaterial(int entryId)
        {
            try
            {
               return await _projectExplosiveMaterialRepository.GetProjectExplosiveMaterial(entryId);
               
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
