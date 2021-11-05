//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace services.sismo.services
//{
//    public class ProjectExplosiveMaterialService : IProjectExplosiveMaterialService
//    {
//        private readonly IFrontGroupLeaderRepository _frontGroupLeaderRepository;
//        private readonly IConfiguration _configuration;

//        public FrontGroupLeaderService(IFrontGroupLeaderRepository frontGroupLeaderRepository, IConfiguration configuration)
//        {
//            _frontGroupLeaderRepository = frontGroupLeaderRepository;
//            _configuration = configuration;
//        }
//        public async Task< ListProjectExplosiveMaterials(int projectId, Int32 entryType)
//        {
            
//            try
//            {
//                var materials = new ProjectExplosiveMaterialDAL().ListProjectExplosiveMaterials(projectId, entryType);

//                result.Data = new
//                {
//                    Materials = materials,
//                    TotalVolumeKg = GetTotalVolumeAvailable(projectId, 0, "kg"),
//                    TotalVolumeM = GetTotalUnityAvailable(projectId, 0, "m")
//                };
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }
//        public async Task< ListSurveyExplosiveMaterials(int projectId, int surveyId, Int32 entryType)
//        {
            
//            try
//            {
//                var materials = new ProjectExplosiveMaterialDAL().ListProjectExplosiveMaterials(projectId, entryType).Where(d => d.SurveyId == surveyId);

//                result.Data = new
//                {
//                    Materials = materials,
//                    TotalProjectVolumeKg = GetTotalVolumeAvailable(projectId, 0, "kg"),
//                    TotalProjectVolumeM = GetTotalUnityAvailable(projectId, 0, "m"),
//                    TotalSurveyVolumeKg = GetTotalVolumeAvailable(projectId, surveyId, "kg"),
//                    TotalSurveyVolumeM = GetTotalUnityAvailable(projectId, surveyId, "m"),
//                };
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }


//        public async Task< GetDashboardData(Int32 projectId, string date)
//        {
            
//            ProjectExplosiveMaterialTypeDAL projectExplosiveMaterialTypeDAL = new ProjectExplosiveMaterialTypeDAL();
//            ProjectExplosiveMaterialDAL projectExplosiveMaterialDAL = new ProjectExplosiveMaterialDAL();
//            var materialTypes = projectExplosiveMaterialTypeDAL.ListProjectExplosiveMaterialTypes(projectId);
//            List<ProjectExplosiveMaterialTypeSummaryDTO> projectExplosiveMaterialTypeSummaryList = new List<ProjectExplosiveMaterialTypeSummaryDTO>();
//            DateTime limitDate = GeneralParsers.GetDBValue(date, "NormalDate");

//            /*var minDate = DateTime.Now;
//            try {
//                minDate = projectExplosiveMaterialDAL.GetAll().Select(s => s.Date).Min();
//            }
//            catch (Exception ex) {
//            }*/

//            ;
//            int days = DateTime.DaysInMonth(limitDate.Date.Year, limitDate.Date.Month);
//            var dates = new List<DateTime>();
//            var datesToReturn = new List<String>();
//            var minDate = GeneralParsers.GetDBValue("01/" + limitDate.Date.Month.ToString() + "/" + limitDate.Date.Year.ToString(), "NormalDate");
//            var maxDate = GeneralParsers.GetDBValue(days.ToString() + "/" + limitDate.Date.Month.ToString() + "/" + limitDate.Date.Year.ToString(), "NormalDate");
//            DecimalChartSerie ExplosiveStock = new DecimalChartSerie();
//            DecimalChartSerie FuseStock = new DecimalChartSerie();

//            DecimalChartSerie ExplosiveUsed = new DecimalChartSerie();
//            DecimalChartSerie FuseUsed = new DecimalChartSerie();

//            // int countData = limitDate.Subtract(minDate).Days+1;
//            ExplosiveStock.data = new decimal[days];
//            FuseStock.data = new decimal[days];
//            ExplosiveUsed.data = new decimal[days];
//            FuseUsed.data = new decimal[days];

//            ExplosiveStock.type = "spline";
//            FuseStock.type = "spline";
//            ExplosiveUsed.type = "column";
//            FuseUsed.type = "column";

//            ExplosiveStock.name = "Estoque de Emulsão";
//            FuseStock.name = "Estoque de Espoletas";
//            ExplosiveUsed.name = "Uso de Emulsão";
//            FuseUsed.name = "Uso de Espoletas";

//            var cont = 0;


//            var materialMetersIds = projectExplosiveMaterialTypeDAL.ListProjectExplosiveMaterialTypes(projectId).Where(p => p.Unity == "m").Select(s => s.ProjectExplosiveMaterialTypeId).ToList();
//            var materialKgIds = projectExplosiveMaterialTypeDAL.ListProjectExplosiveMaterialTypes(projectId).Where(p => p.Unity == "kg").Select(s => s.ProjectExplosiveMaterialTypeId).ToList();

//            var totalEntriesInKm = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In).Where(p => p.ProjectId == projectId && materialKgIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
//            var totalEntriesOutKm = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out).Where(p => p.ProjectId == projectId && materialKgIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
//            var totalEntriesUseKm = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use).Where(p => p.ProjectId == projectId && materialKgIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();

//            var totalEntriesInM = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In).Where(p => p.ProjectId == projectId && materialMetersIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
//            var totalEntriesOutM = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out).Where(p => p.ProjectId == projectId && materialMetersIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
//            var totalEntriesUseM = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use).Where(p => p.ProjectId == projectId && materialMetersIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();



//            for (var dt = minDate; dt <= maxDate; dt = dt.AddDays(1))
//            {

//                dates.Add(dt);
//                datesToReturn.Add(dt.ToString("dd/MM"));


//                var totalTypeMaterialsInKm = totalEntriesInKm.Where(d => d.EntryDate.Date <= dt).Sum(s => s.AmountIn) + totalEntriesInKm.Where(d => d.EntryDate.Date <= dt).Sum(s => s.DifferenceAmount);

//                var totalMaterialsTypeOutFieldKm = totalEntriesOutKm.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
//                var totalMaterialsTypeOutDestroyedKm = totalEntriesOutKm.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
//                var totalMaterialsTypeOutTransferedKm = totalEntriesOutKm.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
//                var totalMaterialsTypeOutStockKm = totalEntriesOutKm.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);

//                var totalMaterialsTypeUseKm = totalEntriesUseKm.Where(d => d.EntryDate.Date == dt).Sum(s => s.AmountUsed);

//                var totalAmountTypeAvailableStock = totalTypeMaterialsInKm - (totalMaterialsTypeOutFieldKm + totalMaterialsTypeOutDestroyedKm + totalMaterialsTypeOutTransferedKm + totalMaterialsTypeOutStockKm);
//                var totalAmountTypeAvailableField = totalMaterialsTypeOutFieldKm - (totalMaterialsTypeUseKm) + totalMaterialsTypeOutStockKm;


//                var totalTypeMaterialsInM = totalEntriesInM.Where(d => d.EntryDate.Date <= dt).Sum(s => s.AmountIn) + totalEntriesInM.Where(d => d.EntryDate.Date <= dt).Sum(s => s.DifferenceAmount);

//                var totalMaterialsTypeOutFieldM = totalEntriesOutM.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
//                var totalMaterialsTypeOutDestroyedM = totalEntriesOutM.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
//                var totalMaterialsTypeOutTransferedM = totalEntriesOutM.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);
//                var totalMaterialsTypeOutStockM = totalEntriesOutM.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock && s.EntryDate.Date <= dt).Sum(s => s.AmountOut);

//                var totalMaterialsTypeUseM = totalEntriesUseM.Where(d => d.EntryDate.Date == dt).Sum(s => s.AmountUsed);

//                var totalAmountTypeAvailableStockM = totalTypeMaterialsInM - (totalMaterialsTypeOutFieldM + totalMaterialsTypeOutDestroyedM + totalMaterialsTypeOutTransferedM + totalMaterialsTypeOutStockM);
//                var totalAmountTypeAvailableFieldM = totalMaterialsTypeOutFieldM - (totalMaterialsTypeUseM) + totalMaterialsTypeOutStockM;




//                ExplosiveStock.data[cont] = Convert.ToDecimal(totalAmountTypeAvailableStock);
//                FuseStock.data[cont] = Convert.ToDecimal(totalAmountTypeAvailableStockM);
//                ExplosiveUsed.data[cont] = Convert.ToDecimal(totalMaterialsTypeUseKm);
//                FuseUsed.data[cont] = Convert.ToDecimal(totalMaterialsTypeUseM);

//                cont++;
//            }

//            foreach (ProjectExplosiveMaterialTypeDTO dto in materialTypes)
//            {
//                ProjectExplosiveMaterialTypeSummaryDTO materialSummary = GetProjectExplosiveMaterialTypeSummary(projectId, 0, dto.ProjectExplosiveMaterialTypeId, limitDate);
//                projectExplosiveMaterialTypeSummaryList.Add(materialSummary);

//            }
//            result.Data = new
//            {
//                MaterialTypes = materialTypes,
//                ProjectExplosiveMaterialTypeSummaryList = projectExplosiveMaterialTypeSummaryList,
//                DatesToReturn = datesToReturn,
//                ExplosiveStock,
//                FuseStock,
//                ExplosiveUsed,
//                FuseUsed
//            };
//            return result;
//        }

//        public static ProjectExplosiveMaterialTypeSummaryDTO GetProjectExplosiveMaterialTypeSummary(int projectId, int surveyId, int projectExplosiveMaterialTypeId, DateTime dateLimit)
//        {

//            ProjectExplosiveMaterialTypeDAL projectExplosiveMaterialTypeDAL = new ProjectExplosiveMaterialTypeDAL();
//            ProjectExplosiveMaterialDAL projectExplosiveMaterialDAL = new ProjectExplosiveMaterialDAL();

//            //Unique
//            var typeEntriesIn = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In).Where(p => p.ProjectId == projectId && p.ProjectExplosiveMaterialTypeId == projectExplosiveMaterialTypeId && p.EntryDate.Date <= dateLimit.Date).ToList();
//            var typeEntriesOut = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out).Where(p => p.ProjectId == projectId && p.ProjectExplosiveMaterialTypeId == projectExplosiveMaterialTypeId && p.EntryDate.Date <= dateLimit.Date && (surveyId == 0 || p.SurveyId == surveyId)).ToList();
//            var typeEntriesUse = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use).Where(p => p.ProjectId == projectId && p.ProjectExplosiveMaterialTypeId == projectExplosiveMaterialTypeId && p.EntryDate.Date <= dateLimit.Date && (surveyId == 0 || p.SurveyId == surveyId)).ToList();




//            var totalTypeMaterialsIn = typeEntriesIn.Sum(s => s.AmountIn) + typeEntriesIn.Sum(s => s.DifferenceAmount);

//            var totalMaterialsTypeOutField = typeEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field).Sum(s => s.AmountOut);
//            var totalMaterialsTypeOutDestroyed = typeEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed).Sum(s => s.AmountOut);
//            var totalMaterialsTypeOutTransfered = typeEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered).Sum(s => s.AmountOut);
//            var totalMaterialsTypeOutStock = typeEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock).Sum(s => s.AmountOut);

//            var totalMaterialsTypeUse = typeEntriesUse.Sum(s => s.AmountUsed);

//            var totalAmountTypeAvailableStock = totalTypeMaterialsIn - (totalMaterialsTypeOutField + totalMaterialsTypeOutDestroyed + totalMaterialsTypeOutTransfered + totalMaterialsTypeOutStock);
//            var totalAmountTypeAvailableField = totalMaterialsTypeOutField - (totalMaterialsTypeUse) + totalMaterialsTypeOutStock;


//            //Total
//            var materialType = projectExplosiveMaterialTypeDAL.GetProjectExplosiveMaterialType(projectExplosiveMaterialTypeId);

//            var materialTypeIds = projectExplosiveMaterialTypeDAL.ListProjectExplosiveMaterialTypes(projectId).Where(p => p.Unity == materialType.Unity).Select(s => s.ProjectExplosiveMaterialTypeId).ToList();

//            var totalEntriesIn = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
//            var totalEntriesOut = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();
//            var totalEntriesUse = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId)).ToList();


//            var totalMaterialsIn = totalEntriesIn.Sum(s => s.Volume) + totalEntriesIn.Sum(s => (s.Volume / (s.AmountIn == 0 ? 1 : s.AmountIn)) * s.DifferenceAmount);

//            var totalMaterialsVolumeOutField = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field).Sum(s => s.Volume);
//            var totalMaterialsVolumeOutDestroyed = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed).Sum(s => s.Volume);
//            var totalMaterialsVolumeOutTransfered = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered).Sum(s => s.Volume);
//            var totalMaterialsVolumeOutStock = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock).Sum(s => s.Volume);



//            var totalMaterialVolumeAvailableStock = totalMaterialsIn - (totalMaterialsVolumeOutField + totalMaterialsVolumeOutDestroyed + totalMaterialsVolumeOutTransfered + totalMaterialsVolumeOutStock);




//            var projectExplosiveMaterialTypeSummaryDTO = new ProjectExplosiveMaterialTypeSummaryDTO()
//            {
//                TotalTypeMaterialsIn = totalTypeMaterialsIn,
//                TotalAmountAvailableStock = totalAmountTypeAvailableStock,
//                TotalAmountAvailableField = totalAmountTypeAvailableField,
//                TotalMaterialVolumeAvailableStock = totalMaterialVolumeAvailableStock,
//                TotalMaterialsTypeUse = totalMaterialsTypeUse,
//                MaterialType = materialType,
//                TotalMaterialsIn = totalTypeMaterialsIn,
//                TotalMaterialsVolumeOutDestroyed = totalMaterialsTypeOutDestroyed,
//                TotalMaterialsVolumeOutTransfered = totalMaterialsTypeOutTransfered

//            };

//            return projectExplosiveMaterialTypeSummaryDTO;
//        }
//        public async Task< GetAmountAvailable(int projectId, int surveyId, int projectExplosiveMaterialTypeId)
//        {
            
//            try
//            {


//                result.Data = GetProjectExplosiveMaterialTypeSummary(projectId, surveyId, projectExplosiveMaterialTypeId, DateTime.Now);
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public static decimal GetTotalVolumeAvailable(int projectId, int surveyId, String Unity)
//        {
//            decimal? totalMaterialVolumeAvailableStock = 0;
//            try
//            {
//                ProjectExplosiveMaterialTypeDAL projectExplosiveMaterialTypeDAL = new ProjectExplosiveMaterialTypeDAL();
//                ProjectExplosiveMaterialDAL projectExplosiveMaterialDAL = new ProjectExplosiveMaterialDAL();


//                //Total


//                var materialTypeIds = projectExplosiveMaterialTypeDAL.ListProjectExplosiveMaterialTypes(projectId).Where(p => p.Unity == Unity).Select(s => s.ProjectExplosiveMaterialTypeId).ToList();

//                var totalEntriesIn = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();
//                var totalEntriesOut = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();
//                var totalEntriesUse = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();


//                var totalMaterialsIn = totalEntriesIn.Sum(s => s.Volume) + totalEntriesIn.Sum(s => (s.Volume / (s.AmountIn == 0 ? 1 : s.AmountIn)) * s.DifferenceAmount);

//                var totalMaterialsVolumeOutField = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field).Sum(s => s.Volume);
//                var totalMaterialsVolumeOutDestroyed = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed).Sum(s => s.Volume);
//                var totalMaterialsVolumeOutTransfered = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered).Sum(s => s.Volume);
//                var totalMaterialsVolumeOutStock = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock).Sum(s => s.Volume);



//                totalMaterialVolumeAvailableStock = totalMaterialsIn - (totalMaterialsVolumeOutField + totalMaterialsVolumeOutDestroyed + totalMaterialsVolumeOutTransfered + totalMaterialsVolumeOutStock);




//            }
//            catch (Exception ex)
//            {

//            }
//            return totalMaterialVolumeAvailableStock.Value;
//        }


//        public static decimal GetTotalUnityAvailable(int projectId, int surveyId, String Unity)
//        {
//            decimal? totalMaterialUnityAvailableStock = 0;
//            try
//            {
//                ProjectExplosiveMaterialTypeDAL projectExplosiveMaterialTypeDAL = new ProjectExplosiveMaterialTypeDAL();
//                ProjectExplosiveMaterialDAL projectExplosiveMaterialDAL = new ProjectExplosiveMaterialDAL();


//                //Total


//                var materialTypeIds = projectExplosiveMaterialTypeDAL.ListProjectExplosiveMaterialTypes(projectId).Where(p => p.Unity == Unity).Select(s => s.ProjectExplosiveMaterialTypeId).ToList();

//                var totalEntriesIn = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.In).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();
//                var totalEntriesOut = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Out).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();
//                var totalEntriesUse = projectExplosiveMaterialDAL.ListProjectExplosiveMaterials(projectId, (int)ExplosiveMaterialEntryType.Use).Where(p => p.ProjectId == projectId && materialTypeIds.Contains(p.ProjectExplosiveMaterialTypeId) && (surveyId == 0 || surveyId == p.SurveyId)).ToList();


//                var totalMaterialsIn = totalEntriesIn.Sum(s => s.AmountIn) + totalEntriesIn.Sum(s => s.DifferenceAmount);

//                var totalMaterialsVolumeOutField = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Field).Sum(s => s.AmountOut);
//                var totalMaterialsVolumeOutDestroyed = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Destroyed).Sum(s => s.AmountOut);
//                var totalMaterialsVolumeOutTransfered = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Transfered).Sum(s => s.AmountOut);
//                var totalMaterialsVolumeOutStock = totalEntriesOut.Where(s => s.Destiny == ExplosiveMaterialDestiny.Stock).Sum(s => s.AmountOut);



//                totalMaterialUnityAvailableStock = totalMaterialsIn - (totalMaterialsVolumeOutField + totalMaterialsVolumeOutDestroyed + totalMaterialsVolumeOutTransfered + totalMaterialsVolumeOutStock);




//            }
//            catch (Exception ex)
//            {

//            }
//            return totalMaterialUnityAvailableStock.Value;
//        }


//        public async Task< SaveProjectExplosiveMaterials(ProjectExplosiveMaterialDTO dto)
//        {
            
//            try
//            {
//                ProjectExplosiveMaterialTypeDAL projectExplosiveMaterialTypeDAL = new ProjectExplosiveMaterialTypeDAL();
//                var materialType = projectExplosiveMaterialTypeDAL.GetProjectExplosiveMaterialType(dto.ProjectExplosiveMaterialTypeId);
//                if (dto.EntryType == ExplosiveMaterialEntryType.In)
//                    dto.Volume = dto.AmountIn * materialType.Volume;
//                if (dto.EntryType == ExplosiveMaterialEntryType.Out)
//                    dto.Volume = dto.AmountOut * materialType.Volume;
//                if (dto.EntryType == ExplosiveMaterialEntryType.Use)
//                    dto.Volume = dto.AmountUsed * materialType.Volume;

//                new ProjectExplosiveMaterialDAL().SaveProjectExplosiveMaterials(dto);

//                result.Data = true;
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< DeleteProjectExplosiveMaterial(ProjectExplosiveMaterialDTO dto)
//        {
            
//            try
//            {
//                new ProjectExplosiveMaterialDAL().DeleteProjectExplosiveMaterial(dto);

//                result.Data = true;
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }

//        public async Task< GetProjectExplosiveMaterial(int entryId)
//        {
            
//            try
//            {
//                var projectExplosiveMaterialTypeDTO = new ProjectExplosiveMaterialDAL().GetProjectExplosiveMaterial(entryId);

//                result.Data = projectExplosiveMaterialTypeDTO;
//            }
//            catch (Exception ex)
//            {
//                result.ErrorMessage = ex.Message;
//                result.ErrorCode = ErrorCode.DalException;
//            }
//            return result;
//        }
//    }
//}
