using System;
using System.Collections.Generic;
using System.Text;

namespace common.sismo.models
{
    public class DailyProductionModel
    {
        public string Date { get; set; }

        public int DailyGoalProductions { get; set; }
        public decimal DailyGoalKm { get; set; }

        public int ProductionRealizedCount { get; set; }
        public decimal ProductionRealizedKm { get; set; }

        public int ProductionNotRealizedCount { get; set; }
        public decimal ProductionNotRealizedKm { get; set; }

        public int FrontGroupsCount { get; set; }

        public decimal TotalKm { get; set; }

        public decimal TotaRealizedKm { get; set; }
        public decimal TotaNotRealizedKm { get; set; }
        public int Meta { get; set; }

    }
}
