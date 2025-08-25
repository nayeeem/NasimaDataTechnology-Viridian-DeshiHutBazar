using System.Linq;
using Model;

namespace Data
{
    public class PostAddressRepository : WebBusinessEntityContext, IPostAddressRepository
    {
        public PostAddress GetPostAddressByID(long addrId)
        {
            return _Context.Addresses.SingleOrDefault(a => a.AddressID == addrId && a.IsActive);
        }

        public void UpdatePostAddress(PostAddress address)
        {
            if (address != null)
            {
                _Context.Entry<PostAddress>(address).State = System.Data.Entity.EntityState.Modified;
                _Context.SaveChanges();
            }
        }

        public void AddPostAddress(PostAddress address)
        {
            if (address != null)
            {
                _Context.Addresses.Add(address);
                _Context.SaveChanges();
            }
        }
    }
}
