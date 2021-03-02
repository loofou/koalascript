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

			SetVarRecursively(ref document, 0);
		}

		void SetVarRecursively(ref BsonDocument document, int depth)
		{
			if(depth == Path.Count - 1)
			{
				document[Path[depth]] = Value;
				return;
			}

			string name = Path[depth];
			if(document.ContainsKey(name))
			{
				//Throws exception, so will break out of the method
				if(!document[name].IsDocument) HandleError(depth);

				BsonDocument subDoc = document[name].AsDocument;
				SetVarRecursively(ref subDoc, depth + 1);
			}
			else
			{
				BsonDocument subDoc = new();
				document[name] = subDoc;

				SetVarRecursively(ref subDoc, depth + 1);
			}
		}

		void HandleError(int depth)
		{
			List<string> errorPathList = new(depth + 1);
			for(int j = 0; j < depth + 1; ++j)
			{
				errorPathList.Add(Path[j]);
			}

			string fullPath = string.Join("/", Path.Select(s => s));
			string errorPath = string.Join("/", errorPathList.Select(s => s));
			throw new DataException($"Can't set variable at '{fullPath}'! '{errorPath}' is not a document!");
		}
	}

	public class InitCollectionInstruction : IKoalaInstruction
	{
		public string CollectionName { get; }
		public IEnumerable<MakeDatasetInstruction> MakeDatasetInstructions { get; }
		public IEnumerable<EnsureIndexInstruction> EnsureIndexInstructions { get; }

		public InitCollectionInstruction(string collectionName
									   , IEnumerable<MakeDatasetInstruction> makeDatasetInstructions
									   , IEnumerable<EnsureIndexInstruction> ensureIndexInstructions)
		{
			CollectionName = collectionName;
			MakeDatasetInstructions = makeDatasetInstructions;
			EnsureIndexInstructions = ensureIndexInstructions;
		}

		public void Run(UltraLiteDatabase db)
		{
			UltraLiteCollection<BsonDocument> collection = db.GetCollection(CollectionName);
			foreach(MakeDatasetInstruction makeDatasetInstruction in MakeDatasetInstructions)
			{
				makeDatasetInstruction.Run(ref collection);
			}

			foreach(EnsureIndexInstruction ensureIndexInstruction in EnsureIndexInstructions)
			{
				ensureIndexInstruction.Run(ref collection);
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

	public class EnsureIndexInstruction : IKoalaInstruction
	{
		public List<string> Path { get; }
		public bool Unique { get; }

		public EnsureIndexInstruction(IEnumerable<string> path, bool unique = false)
		{
			Unique = unique;
			Path = path.ToList();
		}

		public void Run(ref UltraLiteCollection<BsonDocument> col)
		{
			string field = string.Join(".", Path);
			col.EnsureIndex(field, Unique);
		}
	}

}
