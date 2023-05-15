using B3G.DGSN.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3G.DGSN.SERVICE.INTERFACES
{
    public interface ISessionService
    {
        public void AddSession(Session session);
        public bool IsSessionExist(string id);
    }
}
