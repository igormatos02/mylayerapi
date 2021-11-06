using common.sismo.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.services.Report
{
    public interface IBruteRobImportHelperService
    {
        Task<List<KeyValuePair<SeismicRegisterModel, PointProductionModel>>> ExtractSeismicRegistersAndProductions(Stream myStream, BruteRobParametersModel robParameters, List<PreplotPointModel> consideringPoints);

        Task<KeyValuePair<SeismicRegisterModel, PointProductionModel>> BuildSeismicRegisterAndProduction(string[] robLineFields, BruteRobColumnsIndexes columnsIndexes, BruteRobParametersModel robParameters, string fullDateFormat, int previousOpFrontId);
    }
}
