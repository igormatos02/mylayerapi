using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class SurveyProjectBase
    {
        public int SurveyId { get; set; }
        public int ProjectBaseId { get; set; }

        public virtual ProjectBase ProjectBase { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
