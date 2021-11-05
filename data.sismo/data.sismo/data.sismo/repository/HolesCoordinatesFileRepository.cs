using common.sismo.interfaces.repositories;
using common.sismo.models;
using data.sismo.mapping;
using data.sismo.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.sismo.repository
{
    public class HolesCoordinatesFileRepository : IHolesCoordinatesFileRepository
    {
        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
        public HolesCoordinatesFileRepository(IDbContextFactory<MyLayerContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> InsertFile(HolesCoordinatesFileModel file)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity =file.ToEntity();
            context.HolesCoordinatesFiles.Add(entity);
            await context.SaveChangesAsync();
            return entity.FileId;
        }

        public async Task InsertFiles(IEnumerable<HolesCoordinatesFileModel> files)
        {
            foreach (var f in files)
                await InsertFile(f);
        }

        public async Task DeleteFile(int surveyId, int fileId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.HolesCoordinatesFiles.FirstOrDefaultAsync(m => m.SurveyId == surveyId && m.FileId == fileId);
            context.HolesCoordinatesFiles.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteFileWithoutId(HolesCoordinatesFileModel file)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.HolesCoordinatesFiles.FirstOrDefaultAsync(m => m.FileName == file.FileName && m.SurveyId == file.SurveyId && m.UploadTime == file.UploadTime);
            context.HolesCoordinatesFiles.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteFiles(int surveyId, IEnumerable<int> filesIds)
        {
            foreach (var id in filesIds)
                await DeleteFile(surveyId, id);
        }

        public async Task<List<HolesCoordinatesFileModel>> ListFiles(int surveyId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entities = await context.HolesCoordinatesFiles.Where(m => m.SurveyId == surveyId).ToListAsync();
            return entities.Select(x => x.ToModel()).ToList();
        }

        public async Task<HolesCoordinatesFileModel> GetFileDetails(int fileId)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity =  await context.HolesCoordinatesFiles.FirstOrDefaultAsync(m => m.FileId == fileId);
            return entity.ToModel();
        }
    }
}
