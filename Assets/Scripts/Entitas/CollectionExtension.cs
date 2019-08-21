using System.Collections.Generic;
using System.Linq;

namespace Entitas
{
	public static class CollectionExtension
	{
		public static Entity SingleEntity(this ICollection<Entity> collection)
		{
			if (collection.Count != 1)
			{
				throw new SingleEntityException(collection.Count);
			}
			return collection.First();
		}
	}
}
