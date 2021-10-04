using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.repository
{
    public class TerrainChargeControlRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public TerrainChargeControlRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
