using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Model;


namespace WebDeshiHutBazar
{
    public interface IManageAccountSettingService
    {
        Task<bool> SetAccountViewModel(long userId, AccountViewModel objAccountViewModel,
            DateTime bdDate, EnumCountry? enumCountry, EnumCurrency currency);
    }
}
