using System;

namespace common.sismo.models
{
    public class RDOMetaDataModel
    {
        public DateTime FrontGroupStartDate { get; set; }
        public String FrontGroupStartDateStr { get; set; }
        public DateTime SelectedDate { get; set; }
        public int TotalWorkedDays { get; set; }

        public int EffectiveWorkedDaysProject { get; set; }
        public int EffectiveWorkedDaysMonth { get; set; }
        public int EffectiveWorkedDaysWeek { get; set; }
        public int EffectiveWorkedDaysDay { get; set; }
        public DateTime WeekInitialDate { get; set; }
        public DateTime WeekFinalDate { get; set; }
        public decimal SumKmDay { get; set; }
        public decimal SumKmMonth { get; set; }
        public decimal SumKmWeek { get; set; }
        public decimal SumKmProject { get; set; }

        public decimal SumKmDayPT { get; set; }
        public decimal SumKmDayER { get; set; }
        public decimal SumKmWeekPT { get; set; }
        public decimal SumKmWeekER { get; set; }
        public decimal SumKmMonthPT { get; set; }
        public decimal SumKmMonthER { get; set; }
        public decimal SumKmProjectPT { get; set; }
        public decimal SumKmProjectER { get; set; }
        public decimal TotalKmPT { get; set; }
        public decimal TotalKmER { get; set; }
        public decimal TotalKmTotal { get; set; }
        public decimal RemainingKmPT { get; set; }
        public decimal RemainingKmER { get; set; }
        public int RemainingPT { get; set; }
        public int RemainingER { get; set; }
        public int TotalRealized { get; set; }
        public int TotalNotRealized { get; set; }
        public int TotalPT { get; set; }
        public int TotalER { get; set; }

        public int TotalNOTRealizedProjectPT { get; set; }
        public int TotalNOTRealizedProjectER { get; set; }
        public int TotalRealizedProjectPT { get; set; }
        public int TotalRealizedProjectER { get; set; }

        public int TotalNOTRealizedDayPT { get; set; }
        public int TotalNOTRealizedDayER { get; set; }
        public int TotalRealizedDayPT { get; set; }
        public int TotalRealizedDayER { get; set; }

        public int TotalNOTRealizedWeekPT { get; set; }
        public int TotalNOTRealizedWeekER { get; set; }
        public int TotalRealizedWeekPT { get; set; }
        public int TotalRealizedWeekER { get; set; }

        public int TotalNOTRealizedMonthPT { get; set; }
        public int TotalNOTRealizedMonthER { get; set; }
        public int TotalRealizedMonthPT { get; set; }
        public int TotalRealizedMonthER { get; set; }



        public int TotalFrontGroupDay { get; set; }
        public int TotalFrontGroupWeek { get; set; }
        public int TotalFrontGroupMonth { get; set; }
        public int TotalFrontGroupProject { get; set; }

        public int TotalNOTRealizedProjectPTManual { get; set; }
        public int TotalRealizedProjectPTManual { get; set; }
        public int TotalNOTRealizedProjectPTMecanic { get; set; }
        public int TotalRealizedProjectPTMecanic { get; set; }

        public int TotalNOTRealizedMonthPTManual { get; set; }
        public int TotalRealizedMonthPTManual { get; set; }
        public int TotalNOTRealizedMonthPTMecanic { get; set; }
        public int TotalRealizedMonthPTMecanic { get; set; }

        public int TotalNOTRealizedWeekPTManual { get; set; }
        public int TotalRealizedWeekPTManual { get; set; }
        public int TotalNOTRealizedWeekPTMecanic { get; set; }
        public int TotalRealizedWeekPTMecanic { get; set; }

        public int TotalNOTRealizedDayPTManual { get; set; }
        public int TotalRealizedDayPTManual { get; set; }
        public int TotalNOTRealizedDayPTMecanic { get; set; }
        public int TotalRealizedDayPTMecanic { get; set; }
        public decimal TotalMetersManualProject { get; set; }
        public decimal TotalMetersMecanicProject { get; set; }

        public decimal TotalMetersManualMonth { get; set; }
        public decimal TotalMetersMecanicMonth { get; set; }

        public decimal TotalMetersManualWeek { get; set; }
        public decimal TotalMetersMecanicweek { get; set; }

        public decimal TotalMetersManualDay { get; set; }
        public decimal TotalMetersMecanicDay { get; set; }
        public int HolesPerShotPoint { get; set; }
        public decimal HolesDepth { get; set; }
        public bool IsActive { get; set; }
    }
}
