using System.Collections.Generic;
using Model;
using System;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using Common;

namespace Data
{
    public class AValueRepository : WebBusinessEntityContext, IAValueRepository
    {
        public async Task<List<AValue>> GetAValuesByVariable(EnumAllowedVariable variable)
        {
            var resultList =  await _Context.AValues.Where(a => a.Variable == variable && a.IsActive).OrderBy(a=>a.Text).ToListAsync();
            return resultList.ToList();
        }

        public async Task<List<AValue>> GetAValuesByVariableAndParent(EnumAllowedVariable variable, long parentId)
        {
            return await _Context.AValues.Where(a => a.Variable == variable && a.ParentValueId==parentId && a.IsActive).OrderBy(g=>g.Text).ToListAsync();
        }

        public string GetAValueText(long valueID)
        {
            var singleOrDefault = _Context.AValues.SingleOrDefault(a => a.ValueID == valueID);
            return singleOrDefault != null ? singleOrDefault.Text : string.Empty;
        }

        public async Task<List<AValue>> GatAllAValues()
        {
            return await _Context.AValues.Where(a=>a.IsActive).ToListAsync();
        } 
    }
}
