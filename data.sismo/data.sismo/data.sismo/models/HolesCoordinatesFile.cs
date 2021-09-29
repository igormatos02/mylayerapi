using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class HolesCoordinatesFile
    {
        public HolesCoordinatesFile()
        {
            HoleCoordinates = new HashSet<HoleCoordinate>();
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public int SurveyId { get; set; }
        public DateTime UploadTime { get; set; }
        public int? SrsId { get; set; }

        public virtual ICollection<HoleCoordinate> HoleCoordinates { get; set; }
    }
}
