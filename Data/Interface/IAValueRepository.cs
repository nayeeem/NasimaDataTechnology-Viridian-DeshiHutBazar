using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Model;

namespace Data
{
    public interface IAValueRepository
    {
        Task<List<AValue>> GetAValuesByVariable(EnumAllowedVariable variable);
        Task<List<AValue>> GetAValuesByVariableAndParent(EnumAllowedVariable variable, long parentId);
        string GetAValueText(long valueId);
        Task<List<AValue>> GatAllAValues();
    }
}
