using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.repository
{
    public class ProjectExplosiveMaterialTypeRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public ProjectExplosiveMaterialTypeRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
