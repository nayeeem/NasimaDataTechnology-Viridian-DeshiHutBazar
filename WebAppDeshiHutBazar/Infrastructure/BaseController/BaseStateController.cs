using System.Web.Mvc;
using Model;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDeshiHutBazar
{    
    public partial class BaseController : Controller
    {
        protected void SetSessionRechargeSubCategoryID(int? subCategoryID)
        {
            this.Session["SubCategoryID"] = subCategoryID;
        }

        protected int? GetSessionRechargeSubCategoryID()
        {
            return (int?) this.Session["SubCategoryID"];
        }

        protected void ClearSessionRechargeSubCategoryID()
        {
            this.Session["SubCategoryID"] = null;            
        }

        protected void ClearShoppingCartSession()
        {
            this.Session["UserOrderID"] = null;
            this.Session["PackageShoppingCart"] = null;
        }
       
        protected void SetUserOrderID(long? id)
        {
            this.Session["UserOrderID"] = id;
        }

        protected long? GetUserOrderID()
        {
            return (long?) this.Session["UserOrderID"];
        }

        protected long? GetProductUserOrderID()
        {
            return (long?)this.Session["ProductUserOrderID"];
        }

        protected void SetPackageShoppingCart(List<ShoppingCartViewModel> cartItems)
        {
            this.Session["PackageShoppingCart"] = cartItems;
        }

        protected List<ShoppingCartViewModel> GetPackageShoppingCart()
        {
            try
            {
                if (this.Session["PackageShoppingCart"] != null)
                {
                    return (List<ShoppingCartViewModel>) this.Session["PackageShoppingCart"];
                }
                return null;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return null;
            }
        }

        protected void SetProductShoppingCart(List<CartSingleItemViewModel> cartItems)
        {
            this.Session["ProductShoppingCart"] = cartItems;
        }

        protected List<CartSingleItemViewModel> GetProductShoppingCart()
        {
            try
            {
                if (this.Session["ProductShoppingCart"] != null)
                {
                    return (List<CartSingleItemViewModel>)this.Session["ProductShoppingCart"];
                }
                return null;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return null;
            }
        }

        protected void SetBrowserId(long browserId)
        {
            this.Session["Browser"] = browserId;
        }

        protected long GetBrowserId()
        {
            try
            {
                if (this.Session["Browser"] != null)
                {
                    return (long)this.Session["Browser"];
                }
                return 0;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return 0;
            }
        }
        
        protected void SetSessionCheckout(bool checkout)
        {
            this.Session["Checkout"] = checkout;
        }

        protected bool? GetSessionCheckout()
        {
            var objSession = this.Session["Checkout"];
            if (objSession != null)
                return (bool)objSession;
            return null;
        }

        protected void ClearSessionCheckout()
        {
            this.Session["Checkout"] = null;
        }

        #region user
        protected void SetSessionUser(UserModel userModel)
        {
            this.Session["User"] = userModel;
        }

        protected void SetAdminSessionUser(UserModel userModel)
        {
            this.Session["AdminUser"] = userModel;
        }

        protected UserModel GetSessionUser()
        {
            var objSession = this.Session["User"];
            if(objSession!=null)
                return (UserModel)objSession;
            return null;
        }

        protected UserModel GetAdminSessionUser()
        {
            var objSession = this.Session["AdminUser"];
            if (objSession != null)
                return (UserModel)objSession;
            return null;
        }

        protected void ClearSessionUser()
        {
            this.Session["User"] = null;
        }

        protected void ClearAdminSessionUser()
        {
            this.Session["AdminUser"] = null;
        }

        protected long GetSessionUserId()
        {
            var user = GetSessionUser();
            if (user == null) return -1;
            return user.UserID;
        }

        protected bool IsUserAlreadySignedin()
        {
            return GetSessionUserId() != -1;
        }

        protected bool IsCurrentSessionAnAdminUser()
        {
            var user = GetSessionUser();
            if (user != null)
            {
                return user.IsAdminUser;
            }
            return false;
        }

        protected void SetTempSessionUserId(long userId)
        {
            this.Session["TempUserId"] = userId;
        }

        protected long GetTempSessionUserId()
        {
            var id = this.Session["TempUserId"];
            if (id != null)
                return (long)id;
            return -1;
        }

        protected void ClearTempSessionUserId()
        {
            this.Session["TempUserId"] = null;
        }
        #endregion

        #region search model
        protected void SetSessionSearchModel(SearchModel searchModel)
        {
            this.Session["search"] = searchModel;
        }

        protected SearchModel GetSessionSearchModel()
        {
            var objSessionSearchModel = this.Session["search"];
            if (objSessionSearchModel != null)
                return (SearchModel)objSessionSearchModel;
            return null;
        }

        protected void ClearSessionSearchModel()
        {
            this.Session["search"] = null;
        }
        
        #endregion

        #region new post images
        protected void SetSessionNewPostImage(FileViewModel file)
        {
            if(this.Session["NewPostImageList"] != null)
            {
                var listImageVM = (List<FileViewModel>) this.Session["NewPostImageList"];
                var length = listImageVM.ToList().Count;
                file.FileID = length + 1;
                file.IsNewItem = true;
                listImageVM.Add(file);
                this.Session["NewPostImageList"] = listImageVM.ToList();
            }
            else
            {
                List<FileViewModel> objListFiles = new List<FileViewModel>();
                file.FileID = 1;
                file.IsNewItem = true;
                objListFiles.Add(file);
                this.Session["NewPostImageList"] = objListFiles.ToList();
            }
            
        }

        protected List<FileViewModel> GetSessionNewPostImage()
        {
            if (this.Session["NewPostImageList"] != null)
            {
                var listImageVM = (List<FileViewModel>)this.Session["NewPostImageList"];
                return listImageVM.ToList();
            }
            else
            {
                return new List<FileViewModel>();
            }
        }

        protected bool RemoveSessionNewPostImage(long id)
        {
            if (this.Session["NewPostImageList"] != null)
            {
                var listImageVM = (List<FileViewModel>)this.Session["NewPostImageList"];
                var item = listImageVM.FirstOrDefault(a => a.FileID == id);
                if(item != null)
                {
                    listImageVM.Remove(item);
                    var counter = 1;
                    foreach (var ittem in listImageVM)
                    {
                        ittem.FileID = counter;
                        counter++;
                    }
                    this.Session["NewPostImageList"] = listImageVM.ToList();
                    return true;
                }               
            }
            return false;
        }

        protected void SetSessionNewPostImage1(FileViewModel file)
        {
            this.Session["ImgByte1"] = file;
        }

        protected FileViewModel GetSessionNewPostImage1()
        {
            var objSessionFile = this.Session["ImgByte1"];
            if (objSessionFile != null)
                return (FileViewModel)objSessionFile;
            return null;
        }


        protected void SetSessionNewPostImage2(FileViewModel file)
        {
            this.Session["ImgByte2"] = file;
        }

        protected FileViewModel GetSessionNewPostImage2()
        {
            var objSessionFile = this.Session["ImgByte2"];
            if (objSessionFile != null)
                return (FileViewModel)objSessionFile;
            return null;
        }

        protected void SetSessionNewPostImage3(FileViewModel file)
        {
            this.Session["ImgByte3"] = file;
        }

        protected FileViewModel GetSessionNewPostImage3()
        {
            var objSessionFile = this.Session["ImgByte3"];
            if (objSessionFile != null)
                return (FileViewModel)objSessionFile;
            return null;
        }

        protected void SetSessionNewPostImage4(FileViewModel file)
        {
            this.Session["ImgByte4"] = file;
        }

        protected FileViewModel GetSessionNewPostImage4()
        {
            var objSessionFile = this.Session["ImgByte4"];
            if (objSessionFile != null)
                return (FileViewModel)objSessionFile;
            return null;
        }

        protected void ClearNewPostImageSessions()
        {
            if (this.Session["NewPostImageList"] != null)
            {
                this.Session["NewPostImageList"] = null;
            }
        }
        #endregion

        #region manage post images
        protected void SetManagePostImageSession(long postId, long serialNo, FileViewModel file)
        {
            this.Session[postId + "Img" + serialNo] = file;
        }

        protected FileViewModel GetManagePostImageSession(long postId, long serialNo)
        {
            var objSession = this.Session[postId + "Img" + serialNo];
            if (objSession != null)
                return (FileViewModel)objSession;
            return null;
        }

        protected void ClearManagePostImageSession(long postId)
        {
            this.Session[postId + "Img1"] = null;
            this.Session[postId + "Img2"] = null;
            this.Session[postId + "Img3"] = null;
            this.Session[postId + "Img4"] = null;
        }
        #endregion

        #region Image Session for Fabia Images
        protected void SetProcessImageSession(byte[] file)
        {
            this.Session["ProcessImage"] = file;
        }

        protected void SetServiceImageSession(byte[] file)
        {
            this.Session["ServiceImage"] = file;
        }

        protected byte[] GetProcessImageSession()
        {
            var objSession = this.Session["ProcessImage"];
            if (objSession != null)
                return (byte[])objSession;
            return null;
        }

        protected byte[] GetServiceImageSession()
        {
            var objSession = this.Session["ServiceImage"];
            if (objSession != null)
                return (byte[])objSession;
            return null;
        }

        protected void ClearProcessImageSession()
        {
            this.Session["ProcessImage"] = null;           
        }

        protected void ClearServiceImageSession()
        {
            this.Session["ServiceImage"] = null;
        }
        #endregion

        protected void SetSearchResultListPostVM(List<PostViewModel> listPostVM)
        {
            Session["SearchResultListPostVM"] = listPostVM;
        }

        protected List<PostViewModel> GetSearchResultListPostVM()
        {
            var result = Session["SearchResultListPostVM"];
            if (result != null)
            {
                return (List<PostViewModel>) result;
            }
            return new List<PostViewModel>();
        }

        protected void ClearSearchResultListPostVM()
        {
            Session["SearchResultListPostVM"] = null;
        }

        protected void SetSearchPostViewModel(PostViewModel searchModel)
        {
            this.Session["searchpostvm"] = searchModel;
        }

        protected PostViewModel GetSearchPostViewModel()
        {
            var objSearchPostViewModel = this.Session["searchpostvm"];
            if (objSearchPostViewModel != null)
                return (PostViewModel)objSearchPostViewModel;
            return null;
        }

        protected void ClearSearchPostViewModel()
        {
            this.Session["searchpostvm"] = null;
        }

        protected void ClearProviderProfileImageSession()
        {
            this.Session["providerprofileimage"] = null;
        }

        protected void SetProviderProfileImageSession(byte[] image)
        {
            this.Session["providerprofileimage"] = image;
        }

        protected byte[] GetProviderProfileImageSession()
        {
            var image = this.Session["providerprofileimage"];
            if (image != null)
                return (byte[])image;
            return null;
        }

        protected void ClearProviderServiceImageSession()
        {
            this.Session["providerserviceimage"] = null;
        }

        protected void SetProviderServiceImageSession(byte[] image)
        {
            this.Session["providerserviceimage"] = image;
        }

        protected byte[] GetProviderServiceImageSession()
        {
            var image = (byte[])this.Session["providerserviceimage"];
            return image;
        }

        protected FabiaProviderViewModel GetNewProviderSession()
        {
            if (this.Session["newprovider"] == null)
                return new FabiaProviderViewModel();
            return (FabiaProviderViewModel)this.Session["newprovider"];
        }

        protected  void SetNewProviderSession(FabiaProviderViewModel objProviderVM)
        {
            this.Session["newprovider"] = objProviderVM;
        }

    }
}
