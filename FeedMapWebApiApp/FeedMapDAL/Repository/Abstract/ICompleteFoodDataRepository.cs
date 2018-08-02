using System;
using System.Collections.Generic;
using FeedMapDTO;

namespace FeedMapDAL.Repository.Abstract
{
    public interface ICompleteFoodDataRepository
    {
        IEnumerable<CompleteFoodDataDTO> GetCompleteFoodDatas();
        CompleteFoodDataDTO GetCompleteFoodData(int id);
        int Post(CompleteFoodDataDTO completeFoodDatas);
        void Update(CompleteFoodDataDTO completeFoodDatas, int id);
    }
}
