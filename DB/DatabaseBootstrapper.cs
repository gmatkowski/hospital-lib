using System.Data.Entity;

namespace Hospital.DB
{
    public class DatabaseBootstrapper
    {

        public void Configure(DbContext context)
        {
            if (context.Database.Exists())
                return;

            context.Database.Create();
        }
    }
}
