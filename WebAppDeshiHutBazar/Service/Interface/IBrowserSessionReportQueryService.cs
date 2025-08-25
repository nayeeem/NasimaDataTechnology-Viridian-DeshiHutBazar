using System.Collections.Generic;


namespace WebDeshiHutBazar
{
    public interface IBrowserSessionReportQueryService
    {
        List<BrowserUserSessionQueryModel> GetAllData();
    }
}
