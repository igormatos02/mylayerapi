using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.repository
{
    public class ReductionRuleRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ReductionRuleRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
       
    }
}
