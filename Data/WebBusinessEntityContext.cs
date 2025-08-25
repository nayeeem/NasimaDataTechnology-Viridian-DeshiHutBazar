using Model;
namespace Data
{
    public class WebBusinessEntityContext
    {
        public readonly WebDeshiHutBazarEntityContext _Context;

        public WebBusinessEntityContext()
        {
            _Context = new WebDeshiHutBazarEntityContext();
        }
      
    }
}
