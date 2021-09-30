using System;

namespace common.sismo.models
{
    public class IndemnityReportModel
    {
        public int SurveyId { get; set; }
        public int LandId { get; set; }
        public int AuthorizationId { get; set; }
        public DateTime? IndemnityValueApprovalDate { get; set; }
        public DateTime? ChequeIssueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string SignedIndemnityReportFilePath { get; set; }
    }
}
