using System.Collections.Generic;
using System.Data;
using System.Linq;
using UltraLiteDB;

namespace KoalaLiteDb.Lang
{

	public interface IKoalaInstruction { }

	public class SetVarInstruction : IKoalaInstruction
	{
		public List<string> Path { get; }
		public BsonValue Value { get; }

		public SetVarInstruction(IEnumerable<string> path, BsonValue value)
		{
			Path = path.ToList();
			Value = value;
		}

		public void Run(ref BsonDocument document)
		{
			if(Path.Count == 1)
			{
				document[Path[0]] = Value;
				return;
			}

			SetVarRecursively(ref document, Path);
		}

		void SetVarRecursively(ref BsonDocument document, List<string> path)
		{
			if(path.Count == 1)
			{
				document[path[0]] = Value;
				return;
			}

			for(int i = 0; i < path.Count; ++i)
			{
				string name = path[i];
				if(document.ContainsKey(name))
				{
					if(document[name].IsDocument)
					{
						BsonDocument subDoc = document[name].AsDocument;
						List<string> subPath = new(path);
						subPath.RemoveAt(0);
						SetVarRecursively(ref subDoc, subPath);
					}
					else
					{
						List<string> errorPathList = new(i);
						for(int j = 0; j < i; ++j)
						{
							errorPathList.Add(Path[j]);
						}

						string fullPath = string.Join("/", Path.Select(s => s));
						string errorPath = string.Join("/", errorPathList.Select(s => s));
						throw new DataException($"Can't set variable at {fullPath}! {errorPath} is not a document!");
					}
				}
				else
				{
					BsonDocument subDoc = new();
					document[name] = subDoc;

					List<string> subPath = new(path);
					subPath.RemoveAt(0);
					SetVarRecursively(ref subDoc, subPath);
				}
			}
		}
	}

	public class InitCollectionInstruction : IKoalaInstruction
	{
		public string CollectionName { get; }
		public IEnumerable<MakeDatasetInstruction> Instructions { get; }

		public InitCollectionInstruction(string collectionName, IEnumerable<MakeDatasetInstruction> instructions)
		{
			CollectionName = collectionName;
			Instructions = instructions;
		}

		public void Run(UltraLiteDatabase db)
		{
			UltraLiteCollection<BsonDocument> collection = db.GetCollection(CollectionName);
			foreach(MakeDatasetInstruction makeDatasetInstruction in Instructions)
			{
				makeDatasetInstruction.Run(ref collection);
			}
		}
	}

	public class MakeDatasetInstruction : IKoalaInstruction
	{
		public BsonValue DatasetName { get; }
		public IEnumerable<SetVarInstruction> Instructions { get; }

		public MakeDatasetInstruction(string datasetName, IEnumerable<SetVarInstruction> instructions)
		{
			DatasetName = datasetName;
			Instructions = instructions;
		}

		public void Run(ref UltraLiteCollection<BsonDocument> col)
		{
			BsonDocument document = col.FindById(DatasetName);
			if(document == null)
			{
				document = new BsonDocument { ["_id"] = DatasetName };
			}

			foreach(SetVarInstruction instruction in Instructions)
			{
				instruction.Run(ref document);
			}

			col.Upsert(document);
		}
	}

}
