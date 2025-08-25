using System.Collections.Generic;
using System.Linq;
using Model;
using Data;using System.Threading.Tasks;
using System;
using Common;


namespace WebDeshiHutBazar
{
    public class PostVisitService : IPostVisitService
    {
        private readonly IPostRepository _PostRepository;
        private readonly ILogPostVisitRepository _LogPostVisitRepository;

        public PostVisitService(
            ILogPostVisitRepository logPostVisitRepository,
            IPostRepository postRepository
            )
        {
            _LogPostVisitRepository = logPostVisitRepository;
            _PostRepository = postRepository;
        }

        public async Task<bool> SavePostVisit(long postID, string email, string phone, EnumPostVisitAction visitAction)
        {
            var post = await _PostRepository.GetPostByPostID(postID);
            var stateName = post.Address != null && post.Address.StateID != 0 ? DropDownDataList.GetAllStateList().FirstOrDefault(a => a.Value == post.Address.StateID.ToString()).Text : "";
            var user = post.User;
            LogPostVisit objPostVisitEntity = new LogPostVisit(stateName,
                BusinessObjectSeed.GetCateSubCategoryItemText(post.CategoryID),
                BusinessObjectSeed.GetCateSubCategoryItemText(post.SubCategoryID),
                post,
                user,
                email,
                phone,
                EnumCountry.Bangladesh,
                visitAction);

            var result = await _LogPostVisitRepository.SavePostVisit(objPostVisitEntity);
            return true;
        }

        public async Task<List<LogPostVisitViewModel>> GetUserAllPostVisits (long userID, EnumPostVisitAction visitAction)
        {
            var listModels = await _LogPostVisitRepository.GetAdvertiserVisitedProducts(userID, visitAction);
            LogPostVisitViewModel objModel;
            List<LogPostVisitViewModel> objList = new List<LogPostVisitViewModel>();
            foreach(var item in listModels.ToList())
            {
                objModel = new LogPostVisitViewModel();
                objModel.CategoryName = item.CategoryName;
                objModel.SubCategoryName = item.SubCategoryName;
                objModel.VisitorEmail = item.VisitorEmail;
                objModel.VisitorPhoneNumber = item.VisitorPhoneNumber;
                objModel.StateName = item.StateName;
                objModel.PostVisitLogID = item.PostVisitLogID;
                objModel.AdvertiserAccountClientName = item.AdvertiserAccountClientName;
                objModel.AdvertiserAccountEmail = item.AdvertiserAccountEmail;
                objModel.AdvertiserUserID = item.AdvertiserUserID;
                objModel.Area = item.Area;
                objModel.LogDateTime = item.LogDateTime;
                objModel.PosterName = item.PosterName;
                objModel.PosterPhoneNumber = item.PosterPhoneNumber;
                objModel.PostID = item.PostID;
                objModel.PostItemPrice = item.PostItemPrice;
                objModel.PostTitle = item.PostTitle;
                objModel.PostVisitAction = item.PostVisitAction;
                objModel.PostImageFile1 = item.PostImageFile1;
                objModel.PostImageFile2 = item.PostImageFile2;
                objModel.PostImageFile3 = item.PostImageFile3;
                objModel.PostImageFile4 = item.PostImageFile4;
                objList.Add(objModel);
            }
            return objList;
        }


    }
}
