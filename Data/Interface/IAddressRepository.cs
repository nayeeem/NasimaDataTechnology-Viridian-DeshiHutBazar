using Model;

namespace Data
{
    public interface IPostAddressRepository
    {
        PostAddress GetPostAddressByID(long addrId);

        void UpdatePostAddress(PostAddress address);

        void AddPostAddress(PostAddress address);
    }
}
