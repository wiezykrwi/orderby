using System.Linq;

using Raven.Client.Documents.Indexes;

using Shared;

namespace Query
{
	public class DataIndex : AbstractIndexCreationTask<Data>
	{
		public DataIndex()
		{
			Map = data =>
				from item in data
				select new
				{
					item.String01,
					item.String02,
					item.String03,
					item.String04,
					item.String05,
					item.String06,
					item.String07,
					item.String08,
					item.String09,
					item.String10
				};
		}
	}
}