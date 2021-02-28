using System.Collections.Generic;
using MasterMemory;
using MessagePack;

namespace KoalaLiteDb.Tests.Data
{

	[MemoryTable("Config"), MessagePackObject(true)]
	public class Config
	{
		[PrimaryKey] public string ConfigId { get; }
		public Dictionary<string, object> Settings { get; }

		public Config(string configId) {
			ConfigId = configId;
			Settings = new Dictionary<string, object>();
		}
	}

}
