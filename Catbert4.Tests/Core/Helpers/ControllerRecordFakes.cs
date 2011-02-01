using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catbert4.Core.Domain;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;

namespace Catbert4.Tests.Core.Helpers
{
    public static class ControllerRecordFakes
    {
        public static void FakeUnits(int count, IRepository<Unit> repository)
        {
            var records = new List<Unit>();
            FakeUnits(count, repository, records);
        }

        public static void FakeUnits(int count, IRepository<Unit> repository, List<Unit> specificRecords)
        {
            var records = new List<Unit>();
            var specificRecordsCount = 0;
            if (specificRecords != null)
            {
                specificRecordsCount = specificRecords.Count;
                for (int i = 0; i < specificRecordsCount; i++)
                {
                    records.Add(specificRecords[i]);
                }
            }

            for (int i = 0; i < count; i++)
            {
                records.Add(CreateValidEntities.Unit(i + specificRecordsCount + 1));
            }

            var totalCount = records.Count;
            for (int i = 0; i < totalCount; i++)
            {
                records[i].SetIdTo(i + 1);
                int i1 = i;
                repository
                    .Expect(a => a.GetNullableById(i1 + 1))
                    .Return(records[i])
                    .Repeat
                    .Any();
            }
            repository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            repository.Expect(a => a.Queryable).Return(records.AsQueryable()).Repeat.Any();
            repository.Expect(a => a.GetAll()).Return(records).Repeat.Any();
        }
    }
}
