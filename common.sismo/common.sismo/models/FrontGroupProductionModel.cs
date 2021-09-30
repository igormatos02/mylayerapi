using System;
using System.Collections.Generic;

namespace common.sismo.models
{
    public class FrontGroupProductionModel
    {
        public FrontGroupProductionModel()
        {
            Stretches = new List<StretchModel>();
            ProductionLands = new List<LandModel>();
            IsFromDb = false;
            IsOpen = false;
        }

        public Int32 Index { get; set; }
        public Boolean IsOpen { get; set; }
        public Boolean IsFromDb { get; set; }
        public String FrontGroupLeaderName { get; set; }
        public String FrontGroupName { get; set; }
        public Int32 FrontGroupLeaderId { get; set; }
        public Int32 FrontGroupId { get; set; }
        public Int32 LastStrechIndex { get; set; }
        public List<StretchModel> Stretches { get; set; }
        public String Date { get; set; }
        public List<String> Lines { get; set; }
        public List<LandModel> ProductionLands { get; set; }
    }
}
