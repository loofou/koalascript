using System.Collections.Generic;
using UltraLiteDB;

namespace KoalaLiteDb.Lang
{

	public interface IKoalaInstruction
	{
		public void Run(DatabaseBuilder db);
	}

	public class SetVarInstruction : IKoalaInstruction
	{
		public List<string> Path { get; }
		public BsonValue Value { get; }

		public SetVarInstruction(List<string> path, BsonValue value)
		{
			Path = path;
			Value = value;
		}

		public void Run(DatabaseBuilder db)
		{
			//db.Append()

			for(int i = 1; i < Path.Count; ++i)
			{
				string name = Path[i];

				//collection.Exists(Query.)
			}
		}
	}

}
