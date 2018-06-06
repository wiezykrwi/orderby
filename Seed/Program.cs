using System;
using System.Diagnostics;

using Raven.Client.Documents;

using Shared;

namespace Seed
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			var store = new DocumentStore
			{
				Urls = new[] { "http://localhost:8080" },
				Database = "orderby"
			};

			store.Initialize();

			var stopwatch = Stopwatch.StartNew();

			for (int i = 0; i < 1000; i++)
			{
				using (var session = store.OpenSession())
				{
					for (int j = 0; j < 1000; j++)
					{
						session.Store(new Data
						{
							String01 = Guid.NewGuid().ToString(),
							String02 = Guid.NewGuid().ToString(),
							String03 = Guid.NewGuid().ToString(),
							String04 = Guid.NewGuid().ToString(),
							String05 = Guid.NewGuid().ToString(),
							String06 = Guid.NewGuid().ToString(),
							String07 = Guid.NewGuid().ToString(),
							String08 = Guid.NewGuid().ToString(),
							String09 = Guid.NewGuid().ToString(),
							String10 = Guid.NewGuid().ToString()
						});
					}

					session.SaveChanges();
				}
			}

			Console.WriteLine($"Seeding finished in {stopwatch.Elapsed}.");
			Console.ReadKey(true);
		}
	}
}
