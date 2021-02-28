// <auto-generated />
#pragma warning disable CS0105
using KoalaLiteDb.Tests.Data;
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using System.Collections.Generic;
using System;
using KoalaLiteDb.Tables;

namespace KoalaLiteDb
{
   public sealed class ImmutableBuilder : ImmutableBuilderBase
   {
        MemoryDatabase memory;

        public ImmutableBuilder(MemoryDatabase memory)
        {
            this.memory = memory;
        }

        public MemoryDatabase Build()
        {
            return memory;
        }

        public void ReplaceAll(System.Collections.Generic.IList<Config> data)
        {
            var newData = CloneAndSortBy(data, x => x.ConfigId, System.StringComparer.Ordinal);
            var table = new ConfigTable(newData);
            memory = new MemoryDatabase(
                table
            
            );
        }

        public void RemoveConfig(string[] keys)
        {
            var data = RemoveCore(memory.ConfigTable.GetRawDataUnsafe(), keys, x => x.ConfigId, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.ConfigId, System.StringComparer.Ordinal);
            var table = new ConfigTable(newData);
            memory = new MemoryDatabase(
                table
            
            );
        }

        public void Diff(Config[] addOrReplaceData)
        {
            var data = DiffCore(memory.ConfigTable.GetRawDataUnsafe(), addOrReplaceData, x => x.ConfigId, System.StringComparer.Ordinal);
            var newData = CloneAndSortBy(data, x => x.ConfigId, System.StringComparer.Ordinal);
            var table = new ConfigTable(newData);
            memory = new MemoryDatabase(
                table
            
            );
        }

    }
}