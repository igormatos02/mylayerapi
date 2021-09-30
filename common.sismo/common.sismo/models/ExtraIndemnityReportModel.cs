using System;

namespace common.sismo.models
{
    public class ExtraIndemnityReportModel
    {
        public int LandId { get; set; }
        public int SurveyId { get; set; }
        public int IndemnityReportId { get; set; }
        public decimal IndemnityValue { get; set; }
        public string Reason { get; set; }
        public int AuthorizationId { get; set; }
        public DateTime? IndemnityValueApprovalDate { get; set; }
        public DateTime? ChequeIssueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string SignedIndemnityReportFilePath { get; set; }
    }
}
