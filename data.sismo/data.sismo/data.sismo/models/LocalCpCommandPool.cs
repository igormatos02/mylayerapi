using System;
using System.Collections.Generic;

#nullable disable

namespace data.sismo.models
{
    public partial class LocalCpCommandPool
    {
        public string Command { get; set; }
        public DateTime Date { get; set; }
        public string CommandType { get; set; }
        public Guid? ServerToken { get; set; }
        public string SourceTable { get; set; }
        public string Keys { get; set; }
        public Guid CommandId { get; set; }
    }
}
