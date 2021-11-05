using System;
using System.Collections.Generic;
using System.Text;

namespace common.sismo.models
{
    public class SurveyLinesModel
    {
        public List<LineModel> Lines { get;  set; }
        public int LinesCount { get;  set; }
        public int LinesTotalPoints { get;  set; }
        public decimal LinesTotalKm { get;  set; }
    }
}
