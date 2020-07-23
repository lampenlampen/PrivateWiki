using System;
using System.Threading.Tasks;
using FluentResults;

namespace PrivateWiki.Services.SerializationService
{
	public interface ISerializationService<T>
	{
		public Type Input { get; }

		public string Output { get; }

		Task<string> Serialize(T data);

		Task<Result<T>> Deserialize(string data);
	}

	public interface IJsonSerializationService<T> : ISerializationService<T>
	{
	}
}