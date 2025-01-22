using Microsoft.EntityFrameworkCore;
using PersonalShop.DataAccessLayer;

namespace PersonalShop.Tests.Application.Fixture
{
    public class DbFixture : IDisposable
    {
        public ApplicationDbContext DbContext { get; private set; }
        private bool _disposed;

        public DbFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            DbContext = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                DbContext.Dispose();
            }

            _disposed = true;
        }
    }
}
