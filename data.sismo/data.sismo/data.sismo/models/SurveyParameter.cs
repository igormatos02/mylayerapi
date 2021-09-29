using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class SurveyParameter
    {
        public int SurveyId { get; set; }
        public int ParameterId { get; set; }
        public string Value { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual Parameter Parameter { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
