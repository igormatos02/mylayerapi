using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class SyncSummary
    {
        public DateTime? Date { get; set; }
        public string SourceTable { get; set; }
        public int? AmountSync { get; set; }
    }
}
