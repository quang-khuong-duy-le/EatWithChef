using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.DataAccess.Abstract
{
    public interface IChefRepository:IDisposable
    {
        /// <summary>
        /// Get chef profile by user id input.
        /// </summary>
        /// <param name="ChefId">User id parameter</param>
        /// <returns>Chef which has parameter input</returns>
        ChefDTO GetChefProfileById(int ChefId);

        /// <summary>
        /// Get all available chef to chef page.
        /// </summary>
        /// <returns>List available chef.</returns>
        List<ChefDTO> GetAllChef();

        FAQDTO GetFAQById(int FQAId);

        List<FAQDTO> GetAllFAQOfChef(int chefId);

        bool CreateFAQ(FAQ faq);
    }
}
