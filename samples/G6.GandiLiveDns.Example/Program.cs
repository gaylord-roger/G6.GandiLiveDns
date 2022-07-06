using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace G6.GandiLiveDns.Tests
{	
	class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                    .AddUserSecrets<Program>()
                                    .Build();

            var dns = new GandiLiveDns
            {
                Apikey = configuration["ApiKey"] // Your Gandi ApiKey (get it from your gandi account)
            };
            var domain = configuration["Domain"];// The domain you want to manage

            var entries = await dns.GetDomainRecords(domain, default);
            foreach (var entry in entries)
            {
                Console.WriteLine($"{entry.rrset_name} {entry.rrset_type} {entry.rrset_values.FirstOrDefault() ?? ""}");
            }

            var uniqueName = Guid.NewGuid().ToString().TrimStart('{').TrimEnd('}');
            await dns.PostDomainRecord(domain, uniqueName, "A", new[] { "8.8.8.8" }, 300, default);
            await dns.PutDomainRecord(domain, uniqueName, "A", new[] { "9.9.9.9" }, 500, default);
            await dns.DeleteDomainRecord(domain, uniqueName, "A", default);
        }
    }
}