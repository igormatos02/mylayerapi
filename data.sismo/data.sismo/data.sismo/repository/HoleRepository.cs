//using common.sismo.interfaces.repositories;
//using common.sismo.models;
//using data.sismo.mapping;
//using data.sismo.models;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace data.sismo.repository
//{
//    public class HoleRepository
//    {

//        private readonly IDbContextFactory<MyLayerContext> _contextFactory;
//        public DisplacementRuleRepository(IDbContextFactory<MyLayerContext> contextFactory)
//        {
//            _contextFactory = contextFactory;
//        }
       

//        public void BeginTransaction()
//        {
//            DbContextTransaction = DatabaseContext.Database.BeginTransaction();
//        }

//        public void RollbackTransaction()
//        {
//            DbContextTransaction.Rollback();
//        }

//        public void CommitTransaction()
//        {
//            DbContextTransaction.Commit();
//        }

      

//        public HoleDTO GetHole(int surveyId, int preplotPointId, int preplotVersionId,
//             PreplotPointType preplotPointType, int workNumber, int operationalFrontId, int holeNumber)
//        {
//            return HoleParser.StGetInstance()
//                .CreateDto(GetHoleEntity(surveyId, preplotPointId, preplotVersionId,
//                    preplotPointType, workNumber, operationalFrontId, holeNumber)) as HoleDTO;
//        }

//        private Hole GetHoleEntity(int surveyId, int preplotPointId, int preplotVersionId,
//            PreplotPointType preplotPointType, int workNumber, int operationalFrontId, int holeNumber)
//        {
//            return GetSingle(m => m.SurveyId == surveyId && m.PreplotPointId == preplotPointId &&
//                                  m.PreplotVersionId == preplotVersionId &&
//                                  m.PreplotPointType == (int)preplotPointType &&
//                                  m.WorkNumber == workNumber && m.OperationalFrontId == operationalFrontId &&
//                                  m.HoleNumber == holeNumber);
//        }

//        /// <summary>
//        /// List the holes of a preplot point
//        /// </summary>
//        /// <param name="surveyId"></param>
//        /// <param name="preplotPointId"></param>
//        /// <param name="preplotVersionId"></param>
//        /// <param name="preplotPointType"></param>
//        /// <param name="operationalFrontId"></param>
//        /// <returns></returns>
//        public IEnumerable<HoleDTO> ListHoles(int surveyId, int preplotPointId, int preplotVersionId,
//            PreplotPointType preplotPointType, int operationalFrontId)
//        {
//            return HoleParser.StGetInstance()
//                .CreateDtoList(GetAll(m => m.SurveyId == surveyId && m.PreplotPointId == preplotPointId &&
//                                           m.PreplotVersionId == preplotVersionId &&
//                                           m.PreplotPointType == (int)preplotPointType &&
//                                           m.OperationalFrontId == operationalFrontId)).Cast<HoleDTO>();
//        }

//        public void DeleteHoles(PointProductionDTO production)
//        {
//            var holes = ListHoles(production.SurveyId, production.PreplotPointId, production.PreplotVersionId,
//                production.PreplotPointType, production.OperationalFrontId);
//            DeleteHoles(holes);
//        }

//        public void DeleteHoles(IEnumerable<HoleDTO> holes)
//        {
//            foreach (var holeEntity in holes.Select(holeDto => GetHoleEntity(holeDto.SurveyId, holeDto.PreplotPointId, holeDto.PreplotVersionId, holeDto.PreplotPointType,
//                holeDto.WorkNumber, holeDto.OperationalFrontId, holeDto.HoleNumber)).Where(holeEntity => holeEntity != null))
//            {
//                Delete(holeEntity);
//            }
//        }

//        public void AddHoles(IEnumerable<HoleDTO> holes)
//        {
//            foreach (var holeDto in holes)
//            {
//                Add(HoleParser.StGetInstance().CreateEntity(holeDto));
//            }
//        }

//        public void UpdateHoles(IEnumerable<HoleDTO> holes)
//        {
//            foreach (var holeDto in holes)
//            {
//                var holeEntity = GetHoleEntity(holeDto.SurveyId, holeDto.PreplotPointId, holeDto.PreplotVersionId, holeDto.PreplotPointType,
//                    holeDto.WorkNumber, holeDto.OperationalFrontId, holeDto.HoleNumber);
//                if (holeEntity != null)
//                {
//                    HoleParser.StGetInstance().CreateEntity(holeDto, holeEntity);
//                    SaveChanges();
//                }
//            }
//        }
//    }
//}
