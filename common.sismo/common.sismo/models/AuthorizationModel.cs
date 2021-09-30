using common.sismo.enums;
using System;

namespace common.sismo.models
{
    public class AuthorizationModel
    {
        public int AuthorizationId { get; set; }
        public int SurveyId { get; set; }
        public ProductionStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string LandOwnerCpfCnpj { get; set; }
        public string Observation { get; set; }
    }
}
