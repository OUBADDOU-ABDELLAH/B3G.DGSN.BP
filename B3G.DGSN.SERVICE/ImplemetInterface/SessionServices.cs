using B3G.DGSN.DOMAIN;

using B3G.DGSN.REPOSITORY.Interface;
using B3G.DGSN.SERVICE.INTERFACES;









namespace B3G.DGSN.SERVICE.ImplemetInterface
{
    public class SessionServices : ISessionService
    {
        private IGeneriqueCRUD<Session> _repo;




        public SessionServices(IGeneriqueCRUD<Session> CrudMethodes)
        {
            _repo = CrudMethodes;
        }
        public void AddSession(Session session)
        {
            if (session != null)
            {
                _repo.Add(session);

            }
        }

        public bool IsSessionExist(string id) => _repo.isSessionExist(id);


    }
}
