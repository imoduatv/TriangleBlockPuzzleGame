using System.Linq;

namespace Entitas
{
	public class GroupSingleEntityException : EntitasException
	{
		public GroupSingleEntityException(Group group)
			: base("Cannot get the single entity from " + group + "!\nGroup contains " + group.count + " entities:", string.Join("\n", (from e in @group.GetEntities()
				select e.ToString()).ToArray()))
		{
		}
	}
}
