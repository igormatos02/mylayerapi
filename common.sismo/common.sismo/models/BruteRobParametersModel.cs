using System.Collections.Generic;

namespace common.sismo.models
{
    public class BruteRobParametersModel
    {
        public int SurveyId { get; set; }
        public int OperationalFrontId { get; set; }
        public int FrontGroupId { get; set; }
        public int FrontGroupLeaderId { get; set; }
        public string Date { get; set; }
        public List<LineStretchModel> LineStretches { get; set; }
        public bool IsActive { get; set; }
    }


    public class BruteRobColumnsIndexes
    {
        public BruteRobColumnsIndexes()
        {
            FfidIndex = -1;
            ShotNumberIndex = -1;
            SwathIndex = -1;
            ItbIndex = -1;
            DateIndex = -1;
            PointNumberIndex = -1;
            NbOfLiveSeisIndex = -1;
            NbOfDeadSeisIndex = -1;
            CommentIndex = -1;
            LineNameIndex = -1;
            BlasterIdIndex = -1;
            BlasterShotStatusIndex = -1;
            UpholeTimeIndex = -1;
            IsNoiseTestIndex = -1;
        }

        public int FfidIndex { get; set; }
        public int ShotNumberIndex { get; set; }
        public int SwathIndex { get; set; }
        public int ItbIndex { get; set; }
        public int DateIndex { get; set; }
        public int PointNumberIndex { get; set; }
        public int NbOfLiveSeisIndex { get; set; }
        public int NbOfDeadSeisIndex { get; set; }
        public int CommentIndex { get; set; }
        public int LineNameIndex { get; set; }
        public int BlasterIdIndex { get; set; }
        public int BlasterShotStatusIndex { get; set; }
        public int UpholeTimeIndex { get; set; }
        public int IsNoiseTestIndex { get; set; }
    }
}
