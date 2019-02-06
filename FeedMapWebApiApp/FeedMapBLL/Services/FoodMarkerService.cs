using System;
using System.Collections.Generic;
using AutoMapper;
using FeedMapBLL.Domain;
using FeedMapBLL.Services.Abstract;
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;

namespace FeedMapBLL.Services
{
    public class FoodMarkerService : IFoodMarkerService
    {
        private IFoodMarkerRepository _repo;
        private ICompleteFoodDataRepository _completeFoodDataRepo;
        private IMapper _mapper;

        public FoodMarkerService(RepositoryPayload repoPayload,
                                   IMapper mapper)
        {
            _repo = repoPayload.GetFoodMarkerRepository();
            _completeFoodDataRepo = repoPayload.GetCompleteFoodDataRepository();
            _mapper = mapper;
        }

        public IEnumerable<FullFoodAndGeoData> GetCompleteFoodData()
        {
            List<FullFoodAndGeoData> retLst = new List<FullFoodAndGeoData>();

            IEnumerable<CompleteFoodDataDTO> completeFoodDatasDto = _completeFoodDataRepo.GetCompleteFoodDatas();
            foreach (CompleteFoodDataDTO completeFoodDataDto in completeFoodDatasDto)
            {
                retLst.Add(_mapper.Map<FullFoodAndGeoData>(completeFoodDataDto));
            }
            return retLst;
        }

        public FullFoodAndGeoData GetCompleteFoodDataById(int id)
        {
            CompleteFoodDataDTO completeFoodDataDto = _completeFoodDataRepo.GetCompleteFoodData(id);
            return _mapper.Map<FullFoodAndGeoData>(completeFoodDataDto);
        }

        public IEnumerable<FullFoodAndGeoData> GetCompleteFoodDataByUserId(int id)
        {
            List<FullFoodAndGeoData> retLst = new List<FullFoodAndGeoData>();
            IEnumerable<CompleteFoodDataDTO> completeFoodDatasDto = _completeFoodDataRepo.GetCompleteFoodDatasByUserId(id);
            if (completeFoodDatasDto == null) return null;
            foreach (CompleteFoodDataDTO completeFoodDataDto in completeFoodDatasDto)
            {
                retLst.Add(_mapper.Map<FullFoodAndGeoData>(completeFoodDataDto));
            }
            return retLst;
        }

        public FoodMarker GetFoodMarker(int id)
        {
            FoodMarkerDTO foodMarkerDto = _repo.GetFoodMarker(id);
            FoodMarker foodMarker = _mapper.Map<FoodMarker>(foodMarkerDto);
            return foodMarker;
        }

        public IEnumerable<FoodMarker> GetFoodMarkers()
        {
            List<FoodMarker> retLst = new List<FoodMarker>();
            IEnumerable<FoodMarkerDTO> foodMarkersDto = _repo.GetFoodMarkers();
            foreach (FoodMarkerDTO foodMarkerDto in foodMarkersDto)
            {
                FoodMarker foodMarker = _mapper.Map<FoodMarker>(foodMarkerDto);
                retLst.Add(foodMarker);
            }
            return retLst;
        }

        public int PostFoodMarker(FoodMarker foodMarker)
        {
            FoodMarkerDTO foodMarkerDTO = _mapper.Map<FoodMarkerDTO>(foodMarker);
            int id = _repo.Post(foodMarkerDTO);
            return id;
        }

        public int PostFullFoodAndGeoData(FullFoodAndGeoData fullFoodAndGeoData, User user)
        {
            CompleteFoodDataDTO completeFoodDataDTO =
                _mapper.Map<CompleteFoodDataDTO>(fullFoodAndGeoData);
            UserDTO userDTO = _mapper.Map<UserDTO>(user);
            int id = _completeFoodDataRepo.Post(completeFoodDataDTO, userDTO);
            return id;
        }


        public void DeleteFoodMarker(int id)
        {
            _repo.Delete(id);
        }
    }
}
