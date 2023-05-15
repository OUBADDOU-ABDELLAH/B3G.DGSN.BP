
namespace B3G.DGSN.REPOSITORY.Interface
{
    public interface IGeneriqueCRUD<T>
    {
        Task<T> GetByIdAsync(string id);
        Task<T> Add(T session);


        public bool isSessionExist(string id);
      
    }
}
