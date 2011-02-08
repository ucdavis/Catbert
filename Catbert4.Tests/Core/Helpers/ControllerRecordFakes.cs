using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catbert4.Core.Domain;
using Rhino.Mocks;
using UCDArch.Core.DomainModel;
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
                repository
                    .Expect(a => a.GetById(i1 + 1))
                    .Return(records[i])
                    .Repeat
                    .Any();
            }
            repository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            repository.Expect(a => a.Queryable).Return(records.AsQueryable()).Repeat.Any();
            repository.Expect(a => a.GetAll()).Return(records).Repeat.Any();
        }

        public static void FakeRoles(int count, IRepository<Role> repository)
        {
            var records = new List<Role>();
            FakeRoles(count, repository, records);
        }

        public static void FakeRoles(int count, IRepository<Role> repository, List<Role> specificRecords)
        {
            var records = new List<Role>();
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
                records.Add(CreateValidEntities.Role(i + specificRecordsCount + 1));
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
                repository
                    .Expect(a => a.GetById(i1 + 1))
                    .Return(records[i])
                    .Repeat
                    .Any();
            }
            repository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
            repository.Expect(a => a.Queryable).Return(records.AsQueryable()).Repeat.Any();
            repository.Expect(a => a.GetAll()).Return(records).Repeat.Any();
        }

        public static void FakeUsers(int count, IRepository<User> repository)
        {
            var records = new List<User>();
            FakeUsers(count, repository, records);
        }

        public static void FakeUsers(int count, IRepository<User> repository, List<User> specificRecords)
        {
            var records = new List<User>();
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
                records.Add(CreateValidEntities.User(i + specificRecordsCount + 1, true));
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

        public static void FakePermissions(int count, IRepository<Permission> repository)
        {
            var records = new List<Permission>();
            FakePermissions(count, repository, records);
        }

        public static void FakePermissions(int count, IRepository<Permission> repository, List<Permission> specificRecords)
        {
            var records = new List<Permission>();
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
                records.Add(CreateValidEntities.Permission(i + specificRecordsCount + 1));
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

        public static void FakeUnitAssociations(int count, IRepository<UnitAssociation> repository)
        {
            var records = new List<UnitAssociation>();
            FakeUnitAssociations(count, repository, records);
        }

        public static void FakeUnitAssociations(int count, IRepository<UnitAssociation> repository, List<UnitAssociation> specificRecords)
        {
            var records = new List<UnitAssociation>();
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
                records.Add(CreateValidEntities.UnitAssociation(i + specificRecordsCount + 1));
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

        public static void FakeApplications(int count, IRepository<Application> repository)
        {
            var records = new List<Application>();
            FakeApplications(count, repository, records);
        }

        public static void FakeApplications(int count, IRepository<Application> repository, List<Application> specificRecords)
        {
            var records = new List<Application>();
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
                records.Add(CreateValidEntities.Application(i + specificRecordsCount + 1));
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

        public static void FakeSchools(int count, IRepository<School > repository)
        {
            var records = new List<School>();
            FakeSchools(count, repository, records);
        }

        public static void FakeSchools(int count, IRepository<School> repository, List<School> specificRecords)
        {
            var records = new List<School>();
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
                records.Add(CreateValidEntities.School(i + specificRecordsCount + 1));
            }

            var totalCount = records.Count;
            for (int i = 0; i < totalCount; i++)
            {
                records[i].SetIdTo((i + 1).ToString());
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

        public static void FakeApplicationRoles(int count, IRepository<ApplicationRole> repository)
        {
            var records = new List<ApplicationRole>();
            FakeApplicationRoles(count, repository, records);
        }

        public static void FakeApplicationRoles(int count, IRepository<ApplicationRole> repository, List<ApplicationRole> specificRecords)
        {
            var records = new List<ApplicationRole>();
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
                records.Add(CreateValidEntities.ApplicationRole(i + specificRecordsCount + 1));
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

    //public abstract class ControllerRecordFakes2<T> where T : DomainObjectWithTypedId<int>
    //{
    //    public void FakeRecords(int count, IRepository<T> repository)
    //    {
    //        var records = new List<T>();
    //        FakeRecords(count, repository, records);
    //    }
        
    //    public void FakeRecords(int count, IRepository<T> repository, List<T> specificRecords)
    //    {
    //        var records = new List<T>();
    //        var specificRecordsCount = 0;
    //        if (specificRecords != null)
    //        {
    //            specificRecordsCount = specificRecords.Count;
    //            for (int i = 0; i < specificRecordsCount; i++)
    //            {
    //                records.Add(specificRecords[i]);
    //            }
    //        }

    //        for (int i = 0; i < count; i++)
    //        {
    //            //records.Add(CreateValidEntities.ApplicationRole(i + specificRecordsCount + 1));
    //            records.Add(GetValid(i + specificRecordsCount + 1));
    //        }

    //        var totalCount = records.Count;
    //        for (int i = 0; i < totalCount; i++)
    //        {
    //            records[i].SetIdTo(i + 1);
    //            int i1 = i;
    //            repository
    //                .Expect(a => a.GetNullableById(i1 + 1))
    //                .Return(records[i])
    //                .Repeat
    //                .Any();
    //        }
    //        repository.Expect(a => a.GetNullableById(totalCount + 1)).Return(null).Repeat.Any();
    //        repository.Expect(a => a.Queryable).Return(records.AsQueryable()).Repeat.Any();
    //        repository.Expect(a => a.GetAll()).Return(records).Repeat.Any();
    //    }

    //    protected abstract T GetValid(int count);
    //}

    //public class FakeRoles : ControllerRecordFakes2<Role>
    //{
    //    protected override Role GetValid(int count)
    //    {
    //        return CreateValidEntities.Role(count);
    //    }
    //}


}
