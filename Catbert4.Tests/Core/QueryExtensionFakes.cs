﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UCDArch.Core.PersistanceSupport;

namespace Catbert4.Tests.Core
{
    public class QueryExtensionFakes : IQueryExtensionProvider
    {
        public IQueryable<T> Cache<T>(IQueryable<T> queryable, string region)
        {
            return queryable;
        }

        public IQueryable<TOriginal> Fetch<TOriginal, TRelated>(IQueryable<TOriginal> queryable, Expression<Func<TOriginal, TRelated>> relationshipProperty, params Expression<Func<TRelated, TRelated>>[] thenFetchRelationship)
        {
            return queryable;
        }

        public IQueryable<TOriginal> FetchMany<TOriginal, TRelated>(IQueryable<TOriginal> queryable, Expression<Func<TOriginal, IEnumerable<TRelated>>> relationshipCollection, params Expression<Func<TRelated, IEnumerable<TRelated>>>[] thenFetchManyRelationship)
        {
            return queryable;
        }

        public IEnumerable<T> ToFuture<T>(IQueryable<T> queryable)
        {
            return queryable;
        }
    }
}