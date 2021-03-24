using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;

using TinyBank.Core.Implementation.Config;
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

            var appConfig = config.ReadAppConfiguration();

            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(appConfig.DatabaseConnectionString);

            DbContext = new TinyBankDbContext(builder.Options);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
