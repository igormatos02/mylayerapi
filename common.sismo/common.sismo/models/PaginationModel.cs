using System;

namespace common.sismo.models
{
    public class PaginationModel
    {
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String SortCollumns { get; set; }

    }
}
