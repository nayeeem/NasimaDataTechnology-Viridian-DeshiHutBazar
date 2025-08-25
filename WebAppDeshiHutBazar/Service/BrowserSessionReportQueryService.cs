using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Model;


namespace WebDeshiHutBazar
{
    public class BrowserSessionReportQueryService : IBrowserSessionReportQueryService
    {
        WebDeshiHutBazarEntityContext _dbContext;
        List<BrowserUserSessionQueryModel> listDataQueryModel;
        
        public BrowserSessionReportQueryService()
        {
            _dbContext = new WebDeshiHutBazarEntityContext();
        }

        public List<BrowserUserSessionQueryModel> GetAllData()
        {
            listDataQueryModel = _dbContext.Database.SqlQuery<BrowserUserSessionQueryModel>("GetBrowserUserSessionLog @BrowserLogID", new SqlParameter("@BrowserLogID", 500) { DbType = DbType.Int32, IsNullable=false }).ToList();            
            return listDataQueryModel;
        }
    }
}
