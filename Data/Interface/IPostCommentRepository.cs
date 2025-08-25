using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Model;

namespace Data
{
    public interface IPostCommentRepository
    {        
        Task<bool> SaveComment(string comment, long postId, EnumCountry country);
    }
}
