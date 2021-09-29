using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class SystemConfiguration
    {
        public Guid ServerToken { get; set; }
        public bool? AutoSync { get; set; }
        public bool? IsFieldServer { get; set; }
        public int? SurveyId { get; set; }
        public int? ProjectId { get; set; }
    }
}
