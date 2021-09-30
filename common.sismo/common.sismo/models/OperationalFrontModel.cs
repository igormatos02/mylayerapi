using common.sismo.enums;
using System;

namespace common.sismo.models
{
    public class OperationalFrontModel
    {
        public int OperationalFrontId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public OperationalFrontType OperationalFrontType { get; set; }
        public OperationalFrontType? PreviousOperationalFrontType { get; set; }
        public string OperationalFrontTypeName { get; set; }
        public bool IsChecked { get; set; }
        public bool IsActive { get; set; }
        public int? PreviousOperationalFrontId { get; set; }
        public string PreviousOperationalFrontName { get; set; }
        public string OperationalFrontColor { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
