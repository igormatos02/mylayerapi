using common.sismo.enums;
using System;
using System.Collections.Generic;

namespace common.sismo.models
{
    public class PointProductionModel
    {
        public PointProductionModel()
        {
            SavingErrorAlert = "";
            //SeismicRegisters = new List<SeismicRegisterDTO>();
        }
        public int PreplotPointId { get; set; }
        public int PreplotVersionId { get; set; }
        public int SurveyId { get; set; }
        public int WorkNumber { get; set; }
        public PreplotPointType PreplotPointType { get; set; }
        public int OperationalFrontId { get; set; }
        public string Observation { get; set; }
        public int? ReductionRuleId { get; set; }
        public string ReductionRuleName { get; set; }
        public int? Ffid { get; set; }
        public string LastEditorUserLogin { get; set; }
        public int? DisplacementRuleId { get; set; }
        public string DisplacementRuleName { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int FrontGroupId { get; set; }
        public string FrontGroupName { get; set; }
        public int FrontGroupLeaderId { get; set; }
        public string FrontGroupLeaderName { get; set; }
        public string DateString { get; set; }
        public DateTime Date { get; set; }
        public DateTime SavingDate { get; set; }
     
        public bool IsFromDb { get; set; }
        public bool IsFromAnotherDate { get; set; }
     
        public PointProductionModel PreviousFrontProduction { get; set; }
       
        public string PreviousFrontProductionObservation { get; set; }
      
        public string Line { get; set; }
        public string LinePosplot { get; set; }
        public decimal StationNumber { get; set; }
       
        public string PreplotPointTypeName { get; set; }
        public int? LandId { get; set; }
      
        public int FinalHolesQuantity { get; set; }
        public int SurveyDefaultHolesQuantity { get; set; }
        public string ReductionRuleImagePath { get; set; }
        public decimal SurveyDefaultHolesDepth { get; set; }
       
        public string DisplacementRuleImagePath { get; set; }
       
        public IEnumerable<HoleModel> Holes { get; set; }
        public decimal HolesDepth { get; set; }
        public int? ChargesType { get; set; }
        
        public int? NumberOfChargesInShotPoint { get; set; }
        public int? FusesType { get; set; }
       
        public int? NumberOfFusesInShotPoint { get; set; }
      
        public string TopographyDescription { get; set; }
      
        public QualityControlStatus? QualityControlStatus { get; set; }
       
        public string SavingErrorAlert { get; set; }
       
        public int Index { get; set; }
       
        public bool HasOasis { get; set; }

    }
}
