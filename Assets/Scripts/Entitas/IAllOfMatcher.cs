namespace Entitas
{
	public interface IAllOfMatcher : ICompoundMatcher, IMatcher
	{
		IAnyOfMatcher AnyOf(params int[] indices);

		IAnyOfMatcher AnyOf(params IMatcher[] matchers);

		INoneOfMatcher NoneOf(params int[] indices);

		INoneOfMatcher NoneOf(params IMatcher[] matchers);
	}
}
