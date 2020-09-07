using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentResults;
using PrivateWiki.DataModels;
using PrivateWiki.DataModels.Errors;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.DataModels.Settings;
using PrivateWiki.Services.SerializationService;
using PrivateWiki.ViewModels.Settings;
using Path = System.IO.Path;

namespace PrivateWiki.Services.AppSettingsService.BackupSyncSettingsService
{
	public interface IBackupSyncTargetsJsonSerializer : IJsonSerializationService<IEnumerable<IBackupSyncTarget>> {}
	
	public class BackupSyncTargetsJsonSerializer : IBackupSyncTargetsJsonSerializer
	{
		public Type Input { get; } = typeof(IList<IBackupSyncTarget>);
		public string Output { get; } = "json";

		public Task<string> Serialize(IEnumerable<IBackupSyncTarget> targets) => Task.Run(async () =>
		{
			var options = new JsonWriterOptions
			{
				Indented = true
			};

			using var stream = new MemoryStream();
			await using (var writer = new Utf8JsonWriter(stream, options))
			{
				writer.WriteStartObject();
				writer.WriteStartArray("targets");

				foreach (var target in targets)
				{
					writer.WriteStartObject();

					writer.WriteString("type", target.Type.ToString());
					writer.WriteString("id", target.Id.ToString());
					writer.WriteString("name", target.Name);
					writer.WriteString("description", target.Description);
					writer.WriteBoolean("is_enabled", target.IsEnabled);

					switch (target.Type)
					{
						case BackupSyncTargetType.LocalFileStorage:
							var target2 = (LfsBackupSyncTarget) target;

							writer.WriteString("path", target2.TargetPath);
							writer.WriteString("sync_frequency", target2.Frequency.ToString());
							writer.WriteBoolean("is_assets_sync_enabled", target2.IsAssetsSyncEnabled);

							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					writer.WriteEndObject();
				}

				writer.WriteEndArray();
				writer.WriteEndObject();
			}

			return Encoding.UTF8.GetString(stream.ToArray());
		});

		public Task<Result<IEnumerable<IBackupSyncTarget>>> Deserialize(string json)
		{
			return Task.Run<Result<IEnumerable<IBackupSyncTarget>>>(() =>
			{
				IList<IBackupSyncTarget> list = new List<IBackupSyncTarget>();

				using (var doc = JsonDocument.Parse(json))
				{
					var root = doc.RootElement;
					var targets = root.GetProperty("targets");

					foreach (var jsonTarget in targets.EnumerateArray())
					{
					}

					foreach (var jsonTarget in targets.EnumerateArray())
					{
						foreach (var property in jsonTarget.EnumerateObject())
						{
						}

						var prop = jsonTarget.GetProperty("type");

						if (!jsonTarget.TryGetProperty("type", out JsonElement jsonType))
						{
							return Result.Fail(new DeserializationError("Target has no type property.").WithMetadata("json", json));
						}

						if (!Enum.TryParse<BackupSyncTargetType>(jsonType.GetString(), out BackupSyncTargetType type))
						{
							return Result.Fail(new DeserializationError("Type has no valid type value").WithMetadata("json", json));
						}

						if (!jsonTarget.TryGetProperty("id", out JsonElement jsonId))
						{
							return Result.Fail(new DeserializationError("Target has no id property.").WithMetadata("json", json));
						}

						if (!Guid.TryParse(jsonId.GetString(), out Guid id))
						{
							return Result.Fail(new DeserializationError("Target has no valid id value.").WithMetadata("json", json));
						}

						if (!jsonTarget.TryGetProperty("name", out JsonElement jsonName))
						{
							return Result.Fail(new DeserializationError("Target has no name value.").WithMetadata("json", json));
						}

						var name = jsonName.GetString();

						if (!jsonTarget.TryGetProperty("description", out JsonElement jsonDescription))
						{
							return Result.Fail(new DeserializationError("Target has no description value.").WithMetadata("json", json));
						}

						var description = jsonDescription.GetString();

						if (!jsonTarget.TryGetProperty("is_enabled", out JsonElement jsonIsEnabled))
						{
							return Result.Fail(new DeserializationError("Target has no is_enabled element.").WithMetadata("json", json));
						}

						var isEnabled = jsonIsEnabled.GetBoolean();

						switch (type)
						{
							case BackupSyncTargetType.LocalFileStorage:
								if (!jsonTarget.TryGetProperty("is_assets_sync_enabled", out JsonElement jsonIsAssetsSyncEnabled))
								{
									return Result.Fail(new DeserializationError("Target has no is_assets_sync_enabled element.").WithMetadata("json", json));
								}

								var isAssetsSyncEnabled = jsonIsAssetsSyncEnabled.GetBoolean();

								if (!jsonTarget.TryGetProperty("sync_frequency", out JsonElement jsonSyncFrequency))
								{
									return Result.Fail(new DeserializationError("Target has no sync_frequency element.").WithMetadata("json", json));
								}

								if (!Enum.TryParse(jsonSyncFrequency.GetString(), out SyncFrequency syncFrequency))
								{
									return Result.Fail(new DeserializationError("Type has no valid sync_frequency value").WithMetadata("json", json));
								}

								if (!jsonTarget.TryGetProperty("path", out JsonElement jsonPath))
								{
									return Result.Fail(new DeserializationError("Target has no path value.").WithMetadata("json", json));
								}

								var path = jsonPath.GetString();

								var target = new LfsBackupSyncTarget(id, name, description, path, syncFrequency, isEnabled, isAssetsSyncEnabled);
								list.Add(target);

								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}
				}

				return Result.Ok((IEnumerable<IBackupSyncTarget>) list);
			});
		}
	}
}