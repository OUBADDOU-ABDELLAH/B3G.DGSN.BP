using B3G.DGSN.REPOSITORY.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3G.DGSN.REPOSITORY
{
    public class CRUDMethodes<T> : IGeneriqueCRUD<T> where T : class
    {
        private DBContextDGSN _context;
        public CRUDMethodes(DBContextDGSN context) {
        _context= context;
        }
        public async Task<T> Add(T session)
        {
            _context.Set<T>().AddAsync(session);
            await _context.SaveChangesAsync();
            return session;

        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public bool isSessionExist(string id)
        {
            return (_context.Sessions?.Any(e => e.state == id)).GetValueOrDefault();
        }

   
    }
}
