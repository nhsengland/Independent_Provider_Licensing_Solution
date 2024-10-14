using Database.LicenceHolder.Readonly.Entites;
using Microsoft.EntityFrameworkCore;

namespace Database.LicenceHolder.Readonly;

public class ReadonlyLicenceHolderDbContext(DbContextOptions<ReadonlyLicenceHolderDbContext> options) : DbContext(options)
{
    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Licence> Licences { get; set; }
}
