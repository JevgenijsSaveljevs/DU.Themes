using AutoMapper;
using DU.Themes.Entities;
using DU.Themes.Models;
using DU.Themes.Models.Filter;
using DU.Themes.Validation;
using FluentValidation;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace DU.Themes
{
    /// <summary>
    /// Application Extensions for Casting, Validating,
    /// Searching by Id, Transaction Opening, Touching entity
    /// </summary>
    public static partial class Extensions
    {


        /// <summary>
        /// Extensions for finding special entity by Id
        /// </summary>
        /// <typeparam name="T">any entity defined in <see cref="DbContext"/> from <see cref="Models"/></typeparam>
        /// <param name="source">sortable source</param>
        /// <param name="Id">Entity Id</param>
        /// <returns>If source contains entity with given id, then return entity, otherwise default value (for many cases it's null)</returns>
        public static T ById<T>(this IQueryable<T> source, long Id)
            where T : EntityBase
        {
            return source.FirstOrDefault(x => x.Id == Id);
        }

        /// <summary>
        /// Extensions for finding special entity by Id
        /// </summary>
        /// <typeparam name="T">any entity defined in <see cref="DbContext"/> from <see cref="Models"/></typeparam>
        /// <param name="source">sortable source</param>
        /// <param name="Id">Entity Id</param>
        /// <returns>If source contains entity with given id, then return entity, otherwise default value (for many cases it's null)</returns>
        public static T ById<T>(this DbQuery<T> source, long Id)
            where T : EntityBase
        {
            return source.FirstOrDefault(x => x.Id == Id);
        }

        public static TDomain GetByModelId<TDomain, TModel>(this TModel model, DbContext context)
            where TDomain : EntityBase
            where TModel : ModelBase
        {
            if (model != null)
            {
                return context.Set<TDomain>().FirstOrDefault(x => x.Id == model.Id);
            }

            return null;
        }

        public static TDomain GetById<TDomain, TModel>(this TModel model, DbContext context)
            where TDomain : class, IDentifiable 
            where TModel : IDentifiable
        {
            if (model != null)
            {
                return context.Set<TDomain>().FirstOrDefault(x => x.Id == model.Id);
            }

            return null;
        }

        public static TDomain GetUserByModelId<TDomain, TModel>(this TModel model, DbContext context)
           where TDomain : IdentityUser<long, UserLogin, UserRole, UserClaim>
           where TModel : ModelBase
        {
            if (model != null)
            {
                return context.Set<TDomain>().FirstOrDefault(x => x.Id == model.Id);
            }

            return null;
        }

        /// <summary>
        /// Extensions for finding special entity by Id
        /// </summary>
        /// <typeparam name="T">any entity defined in <see cref="DbContext"/> from <see cref="Models"/></typeparam>
        /// <param name="source">sortable source</param>
        /// <param name="Id">Entity Id</param>
        /// <returns>If source contains entity with given id, then returns Task of entity entity, otherwise Task of default value(for many cases it's null)</returns>
        public static Task<T> ByIdAsync<T>(this IDbSet<T> source, long Id)
            where T : EntityBase
        {
            return source.FirstOrDefaultAsync(x => x.Id == Id);
        }

        /// <summary>
        /// Performs Validation based on FluentValidation rules from <see cref="DU.Themes.ValidaitonApiFilter"/>
        /// <para>Throws <see cref="ValidationException"/> if any mistake occurs</para>
        /// </summary>
        /// <typeparam name="T">type  derived from <see cref="EntityBase"/> class</typeparam>
        /// <param name="entity">Entity to be valdiated</param>
        /// <param name="validator">Validator instance</param>
        public static void Validate<T>(this T entity, ValidatorBase<T> validator)
            where T : EntityBase
        {
            var result = validator.Validate(entity);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

        /// <summary>
        /// Opens Transaction, by default in <see cref="IsolationLevel.ReadCommitted"/>
        /// </summary>
        /// <param name="ctx">context</param>
        /// <param name="isolation">Isolation Level</param>
        /// <returns><see cref="DbContextTransaction"/> instance</returns>
        public static DbContextTransaction BeginTran(
            this DbContext ctx,
            IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            lock ("transaction")
            {
                return ctx.Database.BeginTransaction(isolation);
            }

        }

        /// <summary>
        /// Assigns <see cref="EntityBase.TouchTime"/> to <see cref="DateTime.UtcNow"/>
        /// </summary>
        /// <typeparam name="T">Type derived from <see cref="EntityBase"/></typeparam>
        /// <param name="entity">Entity tobe "Touched"</param>
        public static void Touch<T>(this T entity)
            where T : EntityBase
        {
            entity.TouchTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Converts entity from one type to another, i.e. "Maps" it based on mappings or <see cref="Profile"/>.
        /// If Source entity doesn't have Destinations type properties those properties have default value
        /// e.g. <see cref="int"/> -> 0, <see cref="DateTime"/> -> <see cref="DateTime.MinValue"/>, reference type -> <see cref="null"/>
        /// </summary>
        /// <typeparam name="TSource">Source entity type</typeparam>
        /// <typeparam name="TDestination">Destinatio nentity type</typeparam>
        /// <param name="source">source entity</param>
        /// <returns>entity of Destination type with properties obtained from entity of source type</returns>
        public static TDestination CastTo<TSource, TDestination>(this TSource source)
            where TDestination : class
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination CastTo<TSource, TDestination>(this TSource source, DbContext context)
            where TDestination : class
        {
            return Mapper.Map<TSource, TDestination>(source, (opts) => { opts.Items.Add("Context", context); });
        }

        public static void UpdateFrom<TSource, TDestination>(this TSource toUpdate, TDestination newValues)
        {
            Mapper.Map<TDestination, TSource>(newValues, toUpdate);
        }


        public static string FromResource<T>(this T source)
            where T : struct
        {
            var propName = $"{typeof(T).Name}_{source.ToString()}";

            return DU.Themes.Models.ModelResources.ResourceManager.GetString(propName);
        }

        public static void TrimStrings<T>(this T entity)
            where T : class, new()
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.PropertyType == typeof(string));

            foreach (var prop in props)
            {
                var value = prop.GetValue(entity) as string;
                try
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.TrimEnd().TrimStart();

                        prop.SetMethod?.Invoke(entity, new[]
                        {
                            value
                        });
                    }

                }

                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// Extensions for finding special entity by Id
        /// </summary>
        /// <typeparam name="T">any entity defined in <see cref="DbContext"/> from <see cref="Models"/></typeparam>
        /// <param name="source">sortable source</param>
        /// <param name="Id">Entity Id</param>
        /// <returns>If source contains entity with given id, then return entity, otherwise default value (for many cases it's null)</returns>
        public static IQueryable<T> Sort<T>(this IDbSet<T> source, FilterBase filter)
            where T : class
        {

            var type = typeof(T);
            var method = filter.SortByAscending ? "OrderBy" : "OrderByDescending";


            var parameter = Expression.Parameter(type, "p");

            var keys = filter.SortKey.Split('.');

            var propertyType = typeof(T);
            MemberExpression me = null;

            foreach (var propertyKey in keys)
            {
                if (me == null)
                {
                    me = Expression.Property(parameter, propertyKey);
                }
                else
                {
                    me = Expression.Property(me, propertyKey);
                }

                propertyType = me.Type;
            }

            var orderByExp = Expression.Lambda(me, parameter);

            // OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression <Func<TSource, TKey>> keySelector)

            MethodCallExpression resultExp = Expression.Call(
                    type: typeof(Queryable),
                    methodName: method,
                    typeArguments: new Type[] { type, propertyType }, // OrderBy<TSource, TKey>
                    arguments: new[]
                    {
                        source.Expression,               // param1: this IQueryable <TSource> source
                        Expression.Quote(orderByExp)     // param2: Expression <Func<TSource, TKey>> keySelector
                    });

            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// Extensions for finding special entity by Id
        /// </summary>
        /// <typeparam name="T">any entity defined in <see cref="DbContext"/> from <see cref="Models"/></typeparam>
        /// <param name="source">sortable source</param>
        /// <param name="Id">Entity Id</param>
        /// <returns>If source contains entity with given id, then return entity, otherwise default value (for many cases it's null)</returns>
        public static IQueryable<T> Sort<T>(this IDbSet<T> source, string propertyName, bool asc)
            where T : class
        {

            var type = typeof(T);
            var method = asc ? "OrderBy" : "OrderByDescending";


            var parameter = Expression.Parameter(type, "p");

            var keys = propertyName.Split('.');

            var propertyType = typeof(T);
            MemberExpression me = null;

            foreach (var propertyKey in keys)
            {
                if (me == null)
                {
                    me = Expression.Property(parameter, propertyKey);
                }
                else
                {
                    me = Expression.Property(me, propertyKey);
                }

                propertyType = me.Type;
            }

            var orderByExp = Expression.Lambda(me, parameter);

            // OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression <Func<TSource, TKey>> keySelector)

            MethodCallExpression resultExp = Expression.Call(
                    type: typeof(Queryable),
                    methodName: method,
                    typeArguments: new Type[] { type, propertyType }, // OrderBy<TSource, TKey>
                    arguments: new[]
                    {
                        source.Expression,               // param1: this IQueryable <TSource> source
                        Expression.Quote(orderByExp)     // param2: Expression <Func<TSource, TKey>> keySelector
                    });

            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// Extensions for finding special entity by Id
        /// </summary>
        /// <typeparam name="T">any entity defined in <see cref="DbContext"/> from <see cref="Models"/></typeparam>
        /// <param name="source">sortable source</param>
        /// <param name="Id">Entity Id</param>
        /// <returns>If source contains entity with given id, then return entity, otherwise default value (for many cases it's null)</returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string propertyName, bool asc)
            where T : class
        {

            var type = typeof(T);
            var method = asc ? "OrderBy" : "OrderByDescending";


            var parameter = Expression.Parameter(type, "p");

            var keys = propertyName.Split('.');

            var propertyType = typeof(T);
            MemberExpression me = null;

            foreach (var propertyKey in keys)
            {
                if (me == null)
                {
                    me = Expression.Property(parameter, propertyKey);
                }
                else
                {
                    me = Expression.Property(me, propertyKey);
                }

                propertyType = me.Type;
            }

            var orderByExp = Expression.Lambda(me, parameter);

            // OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression <Func<TSource, TKey>> keySelector)

            MethodCallExpression resultExp = Expression.Call(
                    type: typeof(Queryable),
                    methodName: method,
                    typeArguments: new Type[] { type, propertyType }, // OrderBy<TSource, TKey>
                    arguments: new[]
                    {
                        source.Expression,               // param1: this IQueryable <TSource> source
                        Expression.Quote(orderByExp)     // param2: Expression <Func<TSource, TKey>> keySelector
                    });

            return source.Provider.CreateQuery<T>(resultExp);
        }


        public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchText, Expression<Func<T, bool>> predicate)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return source;
            }
            else
            {
                return source.Where(predicate);
            }

        }

        public static string SafeTrim(this string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return q;
            }

            return q.Trim();
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        public static IQueryable<T> AsEagerRequests<T>(this DbSet<T> source)
            where T : Request
        {
            return source
                .Include(x => x.Start)
                .Include(x => x.End)
                .Include(x => x.Student)
                .Include(x => x.Teacher)
                .Include(x => x.Reviewer);
        }

        public static IQueryable<T> AsEagerThemes<T>(this DbSet<T> source)
            where T : Theme
        {
            return source
                .Include(x => x.Start)
                .Include(x => x.End)
                .Include(x => x.Student)
                .Include(x => x.Teacher)
                .Include(x => x.Reviewer);
        }

        ////public static string Compute(this Request request)
        ////{
        ////    var props = new string[]
        ////    {
        ////        $"{nameof(Request.Teacher)}{request.Teacher?.Id.ToString()}",
        ////        $"{nameof(Request.Student)}{request.Student?.Id.ToString()}",
        ////        $"{nameof(Request.Reviewer)}{request.Reviewer?.Id.ToString()}",
        ////        $"{nameof(Request.Status)}{request.Status.ToString()}",
        ////    };

        ////    return string.Join(" :: ", props);
        ////}

        ////public static string Compute(this RequestModel request)
        ////{
        ////    var props = new string[]
        ////    {
        ////        $"{nameof(Request.Teacher)}{request.Teacher?.Id.ToString()}",
        ////        $"{nameof(Request.Student)}{request.Student?.Id.ToString()}",
        ////        $"{nameof(Request.Reviewer)}{request.Reviewer?.Id.ToString()}",
        ////        $"{nameof(Request.Status)}{request.Status.ToString()}",
        ////    };

        ////    return string.Join(" :: ", props);
        ////}
    }
}