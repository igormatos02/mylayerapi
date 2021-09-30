using System;

namespace common.sismo.models
{
    public class HolesCoordinatesFileModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public int SurveyId { get; set; }
        public DateTime UploadTime { get; set; }
        public string UploadTimeString { get; set; }
        public int CoordinatesCount { get; set; }
        public bool IsActive { get; set; }
        public int? SrsId { get; set; }
        public string SrsName { get; set; }
    }
}
