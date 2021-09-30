using System;
using System.Collections.Generic;

namespace common.sismo.models
{
    public class PreplotVersionModel
    {
        public int PreplotVersionNumber { get; set; }
        public int PreplotVersionId { get; set; }
        public DateTime CreationDate { get; set; }
        public String FormatedDate { get; set; }
        public int SurveyId { get; set; }
        public SurveyModel Survey { get; set; }
        public string CreatorUserLogin { get; set; }
        public bool IsActive { get; set; }
        public IList<PreplotPointModel> PreplotPoints { get; set; }
        public string Comment { get; set; }
    }
}
