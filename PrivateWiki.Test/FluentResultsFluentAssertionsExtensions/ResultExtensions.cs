using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using FluentResults;

namespace PrivateWiki.Test.FluentResultsFluentAssertionsExtensions
{
	public static class ResultExtensions
	{
		public static ResultAssertions<T> Should<T>(this Result<T> result) => new ResultAssertions<T>(result);
	}

	public class ResultAssertions<T> : ReferenceTypeAssertions<Result<T>, ResultAssertions<T>>
	{
		protected override string Identifier => "result";

		public ResultAssertions(Result<T> instance)
		{
			Subject = instance;
		}

		public AndConstraint<ResultAssertions<T>> HaveError<TError>(string because = "", params object[] becauseArgs) where TError : Error
		{
			Execute.Assertion
				.BecauseOf(because, becauseArgs)
				.Given(() => Subject)
				.ForCondition(x => x.HasError<TError>())
				.FailWith("Expected {context:result} to have Error {0}{reason}", _ => typeof(TError));

			return new AndConstraint<ResultAssertions<T>>(this);
		}
	}
}