using System;
using System.Collections.Generic;
using FeedMapDTO;

namespace FeedMapDAL.Repository.Abstract
{
    public interface ICompleteFoodDataRepository
    {
        IEnumerable<CompleteFoodDataDTO> GetCompleteFoodDatas();
        IEnumerable<CompleteFoodDataDTO> GetCompleteFoodDatasByUserId(int id);
        CompleteFoodDataDTO GetCompleteFoodData(int id);
        int Post(CompleteFoodDataDTO completeFoodDatas, UserDTO user);
        void Update(CompleteFoodDataDTO completeFoodDatas, int id);
    }
}
