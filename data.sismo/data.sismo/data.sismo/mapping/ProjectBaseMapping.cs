using common.sismo.models;
using data.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace data.sismo.mapping
{
    public static class ProjectBaseMapping
    {

        public static ProjectBaseModel ToModel(this ProjectBase entity)
        {

            return new ProjectBaseModel(); ;

        }
    }
}
