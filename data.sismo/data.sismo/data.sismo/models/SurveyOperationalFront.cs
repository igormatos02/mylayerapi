using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class SurveyOperationalFront
    {
        public int OperationalFrontId { get; set; }
        public int SurveyId { get; set; }

        public virtual OperationalFront OperationalFront { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
