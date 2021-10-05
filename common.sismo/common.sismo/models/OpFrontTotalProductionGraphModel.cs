using System;
using System.Collections.Generic;
using System.Text;

namespace common.sismo.models
{
    public class OpFrontTotalProductionGraphModel
    {
        public string FrontName { get; set; }
        public string Color { get; set; }
        public string RealizedStatusName { get; set; }
        public string NotRealizedStatusName { get; set; }
        public string MissingStatusName { get; set; }
        public int OperationalFrontId { get; set; }
        public int OperationalFrontType { get; set; }
        public int TotalRealized { get; set; }
        public int TotalNotRealized { get; set; }
        public int TotalMissing { get; set; }
        public int TotalPoints { get; set; }
        public decimal KmRealized { get; set; }
        public decimal KmNotRealized { get; set; }
        public decimal KmMissing { get; set; }
        public decimal KmTotal { get; set; }
        public decimal PercentRealized { get; set; }
        public decimal PercentNotRealized { get; set; }
        public decimal PercentMissing { get; set; }
        public decimal AreaRealized { get; set; }
        public decimal AreaNotRealized { get; set; }
        public decimal AreaMissing { get; set; }
        public decimal AreaTotal { get; set; }
        public decimal SelectedRealized { get; set; }
        public decimal SelectedNotRealized { get; set; }
        public decimal SelectedMissing { get; set; }
        public decimal SelectedTotal { get; set; }

        public int TotalPointsPT { get; set; }
        public int TotalPointsER { get; set; }
        public int TotalRealizedPT { get; set; }
        public int TotalRealizedER { get; set; }
        public int TotalNotRealizedPT { get; set; }
        public int TotalNotRealizedER { get; set; }

    }
}
