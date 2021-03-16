using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBank.Core.Implementation.Data;

namespace TinyBank.Core.Tests
{
    public class TinyBankFixture : IDisposable
    {
        public TinyBankDbContext DbContext { get; private set; }

        public TinyBankFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            var connectionString = config.GetConnectionString("tinyBank");

            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(connectionString);

            DbContext = new TinyBankDbContext(builder.Options);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
