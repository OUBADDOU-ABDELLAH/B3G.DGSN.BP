using B3G.DGSN.DOMAIN;
using Microsoft.EntityFrameworkCore;


namespace B3G.DGSN.REPOSITORY
{
    public class DBContextDGSN : DbContext
    {

        public DBContextDGSN(DbContextOptions<DBContextDGSN> options) : base(options)
        {
        }
        public DbSet<Session> Sessions { get; set; }

    }
}
