namespace PrivateWiki.Services.StorageServices.Sql.Sqlite
{
	public interface IConverter<TInput, TResult>
	{
		TResult Convert(TInput input);
	}
}