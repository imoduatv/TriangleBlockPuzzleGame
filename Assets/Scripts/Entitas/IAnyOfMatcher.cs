namespace Entitas
{
	public interface IAnyOfMatcher : ICompoundMatcher, IMatcher
	{
		INoneOfMatcher NoneOf(params int[] indices);

		INoneOfMatcher NoneOf(params IMatcher[] matchers);
	}
}
