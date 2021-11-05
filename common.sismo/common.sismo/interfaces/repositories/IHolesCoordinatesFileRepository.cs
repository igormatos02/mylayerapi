using common.sismo.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace common.sismo.interfaces.repositories
{
    public interface IHolesCoordinatesFileRepository
    {
       Task<int> InsertFile(HolesCoordinatesFileModel file);

       Task InsertFiles(IEnumerable<HolesCoordinatesFileModel> files);

       Task DeleteFile(int surveyId, int fileId);

       Task DeleteFileWithoutId(HolesCoordinatesFileModel file);

       Task DeleteFiles(int surveyId, IEnumerable<int> filesIds);

       Task<List<HolesCoordinatesFileModel>> ListFiles(int surveyId);

       Task<HolesCoordinatesFileModel> GetFileDetails(int fileId);
    }
}
