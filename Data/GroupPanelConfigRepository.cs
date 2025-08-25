using System.Linq;
using System.Collections.Generic;
using Model;
using Common;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Data
{
    public class GroupPanelConfigRepository : WebBusinessEntityContext, IGroupPanelConfigRepository
    {
        public async Task<bool> SaveChanges()
        {
            var result = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GroupPanelConfig>> GetAllPublishedGroupPanelConfig(EnumCountry country)
        {
            var listConfigs = await _Context.GroupPanelConfigurations.Where(a =>
                                    a.EnumCountry == country &&
                                    a.IsActive &&
                                    a.EnumGroupPanelStatus == EnumGroupPanelStatus.Published)
                                .OrderBy(a => a.EnumPublicPage)
                                .ThenBy(a => a.Order)
                                .ToListAsync();
            return listConfigs.ToList();
        }

        public async Task<List<GroupPanelConfig>> GetAllPublishedGroupPanelConfig(EnumCountry country, 
            EnumPublicPage page, 
            EnumGroupPanelStatus?  isPublished)
        {
            if (isPublished.HasValue && isPublished.Value == EnumGroupPanelStatus.Published)
            {
                var listConfigs = await _Context.GroupPanelConfigurations.Where(a =>
                                        a.EnumCountry == country &&
                                        a.IsActive &&
                                        a.EnumPublicPage == page &&
                                        a.EnumGroupPanelStatus.HasValue &&
                                        a.EnumGroupPanelStatus == EnumGroupPanelStatus.Published)
                                    .OrderBy(a => a.Order)
                                    .ToListAsync();
                return listConfigs.ToList();
            }
            else
            {
                var listConfigs = await _Context.GroupPanelConfigurations.Where(a =>
                                        a.EnumCountry == country &&
                                        a.IsActive &&
                                        a.EnumPublicPage == page)
                                    .OrderBy(a => a.Order)
                                    .ToListAsync();
                return listConfigs.ToList();
            }
        }

        public async Task<List<GroupPanelConfig>> GetAllGroupPanelConfig(EnumCountry country)
        {
            return await _Context.GroupPanelConfigurations.Where(a =>
                                                            a.EnumCountry == country &&
                                                            a.IsActive).ToListAsync();            
        }

        public async Task<List<GroupPanelConfig>> GetAllUserGroupPanelConfig(EnumCountry country, long userId)
        {
            var listConfigs = await _Context.GroupPanelConfigurations.Where(a =>
                                                            a.EnumCountry == country &&
                                                            a.IsActive &&
                                                            a.PanelConfigUserID == userId).OrderBy(a => a.Order).ToListAsync();
            return listConfigs.ToList();
        }



        public async Task<bool> AddGroupPanelConfig(GroupPanelConfig objGroupPanelConfiguration, 
            long currentUserID,
            EnumCountry country)
        {
            if (objGroupPanelConfiguration == null)
                return false;
            objGroupPanelConfiguration.UpdateModifiedDate(currentUserID, country);
            var count = _Context.GroupPanelConfigurations.Count(a => a.IsActive);
            objGroupPanelConfiguration.Order = count + 1;
            _Context.GroupPanelConfigurations.Add(objGroupPanelConfiguration);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateGroupPanelConfig(GroupPanelConfig singleConfigEntity, 
            long currentUserID,
            EnumCountry country)
        {
            var objConfig = await _Context.GroupPanelConfigurations.FirstOrDefaultAsync(a => a.GroupPanelConfigID == singleConfigEntity.GroupPanelConfigID);
            objConfig.Order = singleConfigEntity.Order;
            objConfig.Device = singleConfigEntity.Device;
            objConfig.UpdateModifiedDate(currentUserID, country);
            await _Context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Ignoring page & device and publish all configs.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="page"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public async Task<bool> PublishGroupPanelConfig(EnumDeviceType device, 
            EnumPublicPage? page, 
            long currentUserID,
            EnumCountry country)
        {
            var objConfigList = await _Context.GroupPanelConfigurations.Where(a => 
                                                                            a.IsActive == true &&
                                                                            a.ShowOrHide == EnumShowOrHide.Yes
                                                                            ).ToListAsync();           
            foreach(var item in objConfigList.ToList())
            {
                item.EnumGroupPanelStatus = EnumGroupPanelStatus.Published;
                item.UpdateModifiedDate(currentUserID, country);
            }
            var result = await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGroupPanelConfig(int id, 
            long currentUserID,
            EnumCountry country)
        {
            var objItem = await _Context.GroupPanelConfigurations.FirstOrDefaultAsync(a => a.GroupPanelConfigID == id);
            objItem.IsActive = false;           
            foreach(var item in objItem.ListPanelPost.ToList())
            {
                item.IsActive = false; 
                item.UpdateModifiedDate(currentUserID, country);
            }
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<GroupPanelConfig> GetSingleGroupPanelConfig(EnumCountry country, int groupPanelConfigID)
        {
            var objConfig = await _Context.GroupPanelConfigurations.FirstOrDefaultAsync(a => 
                                                                a.GroupPanelConfigID == groupPanelConfigID && 
                                                                a.EnumCountry == country &&
                                                                a.IsActive);
            return objConfig;
        }
    }
}
