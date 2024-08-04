using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDIConverterWeb.Data
{
    public class EDIDataContextFactory : IDesignTimeDbContextFactory<EDIDbContext>
    {
        public EDIDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}EDIConverterWeb.Web"))
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new EDIDbContext(config.GetConnectionString("ConStr"));

        }
    }
}
