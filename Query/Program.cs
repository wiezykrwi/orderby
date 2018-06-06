using System;
using System.Linq;
using System.Threading;

using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;

using Shared;

namespace Query
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

			new DataIndex().Execute(store);

			WaitForIndexing(store);

			using (var session = store.OpenSession())
			{
				var results = session.Query<Data, DataIndex>().Statistics(out QueryStatistics stats).OrderBy(x => x.String05).Skip(10).Take(10).ToList();
				Console.WriteLine($"Stats.DurationInMs: {stats.DurationInMs}");
				foreach (var result in results)
				{
					Console.WriteLine(result.String01);
				}
			}

			Console.ReadKey(true);
		}

		private static void WaitForIndexing(IDocumentStore store)
		{
			var admin = store.Maintenance.ForDatabase(store.Database);
			
			while (true)
			{
				var databaseStatistics = admin.Send(new GetStatisticsOperation());
				var indexes = databaseStatistics.Indexes.Where(x => x.State != IndexState.Disabled);

				if (indexes.All(x => x.IsStale == false && x.Name.StartsWith(Raven.Client.Constants.Documents.Indexing.SideBySideIndexNamePrefix) == false))
				{
					return;
				}

				if (databaseStatistics.Indexes.Any(x => x.State == IndexState.Error))
				{
					throw new Exception("Indexing error");
				}

				Thread.Sleep(1000);
			}
		}
	}
}
