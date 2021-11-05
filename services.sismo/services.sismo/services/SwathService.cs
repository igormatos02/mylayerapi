using common.sismo.enums;
using common.sismo.interfaces.repositories;
using common.sismo.interfaces.services;
using common.sismo.models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace services.sismo.services
{
    public class SwathService : ISwathService
    {
        private readonly IPreplotPointRepository _preplotPointRepository;
        private readonly ISwathRepository _swathRepository;
        private readonly IConfiguration _configuration;

        public SwathService(
            IPreplotPointRepository preplotPointRepository,
            ISwathRepository swathRepository, 
            IConfiguration configuration)
        {
            _preplotPointRepository = preplotPointRepository;
            _swathRepository = swathRepository;
            _configuration = configuration;
        }

        public async Task<List<SwathModel>> ListSwaths(int surveyId)
        {
            
            try
            {
                return await _swathRepository.ListSwaths(surveyId);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<List<LineStretchModel>> ListShotStretchsFromSwath(int surveyId, int swathNumber)
        {
            
            try
            {
                var swath = await _swathRepository.GetSwath(surveyId, swathNumber);
                return await _preplotPointRepository.ListStretchesFromSwath(surveyId, PreplotPointType.ShotPoint,
                    swath.InitialShotPoint, swath.FinalShotPoint);
            }
           catch (Exception ex) { throw ex; }
        }

        public async Task<bool> SaveSwath(Stream fileStream, Int32 surveyId, Int32 preplotVersionId)
        {
            try
            {
                var file = new byte[0];
                int number = 1;
                if (fileStream != null)
                {
                    using (StreamReader sr = new StreamReader(fileStream))
                    {

                        while (sr.Peek() >= 0)
                        {
                            String line = sr.ReadLine();
                            if (line.Contains(";"))
                            {
                                String[] columnns = line.Split(';');
                                await _swathRepository.SaveSwath(new SwathModel()
                                {
                                    SurveyId = surveyId,
                                    Name = columnns[0],
                                    SwathNumber = number++,
                                    PreplotVersionId = preplotVersionId,
                                    ActiveReceiverLinesCount = Convert.ToInt32(columnns[1]),
                                    InitialReceiverLine = columnns[2],
                                    FinalReceiverLine = columnns[3],
                                    TotalReceiverStationPerSwath = Convert.ToInt32(columnns[4]),
                                    InitialShotPoint = Convert.ToDecimal(columnns[5]),
                                    FinalShotPoint = Convert.ToDecimal(columnns[6]),
                                    TotalShotPoint = Convert.ToInt32(columnns[7]),
                                });
                            }
                        }
                        await _swathRepository.UpdateSwathPolygonSalva(surveyId);
                      
                    }
                }
                return true;
            }
    catch (Exception ex) { throw ex; }
}
    }
}
