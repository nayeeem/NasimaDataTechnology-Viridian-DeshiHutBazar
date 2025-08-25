using System.Collections.Generic;
using System;
using System.Linq;

namespace WebDeshiHutBazar
{
    public class PageingController : BaseController
    {
        public PageingController()
        { }
        
        private static bool IsAtTheStart(int prev, int start)
        {
            return prev < start;
        }

        private static bool IsAtTheEnd(int next, int end)
        {
            return next > end;
        }

        public List<PostViewModel> GetPostListForPage(List<PostViewModel> listPostViewModels, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            return listPostViewModels.Skip(skip).Take(pageSize).ToList();
        }

        protected PagingModel SetPageingUrl(PagingModel objModel, string action, string controllerName, string interestsString)
        {
            int prev = objModel.PrevPageNumber; //when equiv to 0 then disable link
            int next = objModel.NextPageNumber; //when equiv to total then disable next link
            int total = objModel.TotalPageCount;
            if (IsAtTheEnd(next, total))
            {
                objModel.NextUrl = string.Empty;
                objModel.IsNextDisable = true;
            }
            else
            {
                objModel.NextUrl = Url.Action(action, controllerName,
                    new { pageNumber = objModel.NextPageNumber, interests = interestsString });
                objModel.IsNextDisable = false;
            }

            if (IsAtTheStart(prev, 1))
            {
                objModel.PrevUrl = string.Empty;
                objModel.IsPrevDisable = true;
            }
            else
            {
                objModel.PrevUrl = Url.Action(action, controllerName,
                    new
                    {
                        pageNumber = objModel.PrevPageNumber,
                        interests = interestsString
                    });
                objModel.IsPrevDisable = false;
            }
            return objModel;
        }        

        protected PagingModel SetPageingUrl(PagingModel objModel, string action, string controllerName, long? subcategoryid)
        {
            int prev = objModel.PrevPageNumber; //when equiv to 0 then disable link
            int next = objModel.NextPageNumber; //when equiv to total then disable next link
            int total = objModel.TotalPageCount;

            if (IsAtTheEnd(next, total))
            {
                objModel.NextUrl = string.Empty;
                objModel.IsNextDisable = true;
            }
            else
            {
                objModel.NextUrl = Url.Action(action, controllerName,
                    new
                    {
                        subcategoryid,
                        pageNumber = objModel.NextPageNumber
                    });
                objModel.IsNextDisable = false;
            }

            if (IsAtTheStart(prev, 1))
            {
                objModel.PrevUrl = string.Empty;
                objModel.IsPrevDisable = true;
            }
            else
            {
                objModel.PrevUrl = Url.Action(action, controllerName,
                    new
                    {
                        subcategoryid,
                        pageNumber = objModel.PrevPageNumber
                    });
                objModel.IsPrevDisable = false;
            }
            return objModel;
        }

        public PagingModel SetPageingModel(PagingModel objPagingModel, int dataSourceRowCount, int pageNumber, int pageSize
            , string action, string controllerName, string interestsString, long? subcategoryid)
        {
            objPagingModel.CurrPageNumber = pageNumber;
            objPagingModel.PrevPageNumber = pageNumber - 1;
            objPagingModel.NextPageNumber = pageNumber + 1;
            objPagingModel.TotalPageCount = GetTotalPageCount(dataSourceRowCount, pageSize);
            if (subcategoryid.HasValue)
            {
                objPagingModel = SetPageingUrl(objPagingModel, action, controllerName, subcategoryid);
            }
            else
            {
                objPagingModel = SetPageingUrl(objPagingModel, action, controllerName, interestsString);
            }
            
            return objPagingModel;
        }

        private bool HasFractinalPageCount(int dataSourceRowCount, int pageSize)
        {
            return dataSourceRowCount % pageSize != 0;
        }

        public short GetFractionPageCount(int dataSourceRowCount, int absCount, int pageSize = 20)
        {
            if (dataSourceRowCount > pageSize * absCount)
                return 1;
            else return 0;
        }

        public int GetAbsPageCount(int dataSourceRowCount, int pageSize)
        {

            return Math.Abs(dataSourceRowCount / pageSize);
        }

        public int GetTotalPageCount(int dataSourceRowCount, int pageSize)
        {
            int solid = Convert.ToInt32(dataSourceRowCount / pageSize);
            if (dataSourceRowCount % pageSize > 0)
            {
                return solid + 1;
            }
            return solid;
        }
    }
}
