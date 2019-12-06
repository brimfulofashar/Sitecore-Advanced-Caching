using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;

namespace Foundation.HtmlCache.Extensions
{
    public static class EFExtensions
    {
        public static EntityOp<TEntity> Upsert<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            return new UpsertOp<TEntity>(context, entity);
        }
    }

    public abstract class EntityOp<TEntity, TRet>
    {
        public readonly DbContext Context;
        public readonly TEntity Entity;
        public readonly string TableName;

        private readonly List<string> keyNames = new List<string>();
        public IEnumerable<string> KeyNames { get { return keyNames; } }

        private readonly List<string> excludeProperties = new List<string>();

        private static string GetMemberName<T>(Expression<Func<TEntity, T>> selectMemberLambda)
        {
            var member = selectMemberLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException("The parameter selectMemberLambda must be a member accessing labda such as x => x.Id", "selectMemberLambda");
            }
            return member.Member.Name;
        }

        public EntityOp(DbContext context, TEntity entity)
        {
            Context = context;
            Entity = entity;

            object[] mappingAttrs = typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false);
            TableAttribute tableAttr = null;
            if (mappingAttrs.Length > 0)
            {
                tableAttr = mappingAttrs[0] as TableAttribute;
            }

            if (tableAttr == null)
            {
                throw new ArgumentException("TEntity is missing TableAttribute", "entity");
            }

            TableName = tableAttr.Name;
        }

        public abstract TRet Execute(string suffix);
//        public void Run()
//        {
//            Execute();
//        }

        public EntityOp<TEntity, TRet> Key<TKey>(Expression<Func<TEntity, TKey>> selectKey)
        {
            keyNames.Add(GetMemberName(selectKey));
            return this;
        }

        public EntityOp<TEntity, TRet> ExcludeField<TField>(Expression<Func<TEntity, TField>> selectField)
        {
            excludeProperties.Add(GetMemberName(selectField));
            return this;
        }

        public IEnumerable<PropertyInfo> ColumnProperties
        {
            get
            {
                return typeof(TEntity).GetProperties().Where(pr => !excludeProperties.Contains(pr.Name));
            }
        }
    }

    public abstract class EntityOp<TEntity> : EntityOp<TEntity, int>
    {
        public EntityOp(DbContext context, TEntity entity) : base(context, entity) { }

        public sealed override int Execute(string suffix)
        {
            ExecuteNoRet(suffix);
            return 0;
        }

        protected abstract void ExecuteNoRet(string suffix);
    }

    public class UpsertOp<TEntity> : EntityOp<TEntity>
    {
        public UpsertOp(DbContext context, TEntity entity) : base(context, entity) { }

        protected override void ExecuteNoRet(string suffix)
        {
            StringBuilder sql = new StringBuilder();

            int notNullFields = 0;
            var valueKeyList = new List<string>();
            var columnList = new List<string>();
            var valueList = new List<object>();
            foreach (var p in ColumnProperties)
            {
                columnList.Add(p.Name);
                var val = p.GetValue(Entity, null);
                if (val != null)
                {
                    valueKeyList.Add("{" + (notNullFields++) + "}");
                    valueList.Add(val);
                }
                else
                {
                    valueKeyList.Add("null");
                }
            }
            var columns = columnList.ToArray();

            sql.Append("merge into ");
            sql.Append(TableName);
            sql.Append(" as T ");

            sql.Append("using");
            sql.Append(TableName + "_" + suffix);
            sql.Append(" as S ");

            sql.Append("on (");
            var mergeCond = string.Join(" and ", KeyNames.Select(kn => "T." + kn + "=S." + kn));
            sql.Append(mergeCond);
            sql.Append(") ");

            sql.Append("when matched then update set ");
            sql.Append(string.Join(",", columns.Select(c => "T." + c + "=S." + c).ToArray()));

            sql.Append(" when not matched then insert (");
            sql.Append(string.Join(",", columns));
            sql.Append(") values (S.");
            sql.Append(string.Join(",S.", columns));
            sql.Append(");");

            Context.Database.ExecuteSqlCommand(sql.ToString(), valueList.ToArray());

            /*
                merge into CacheItems as T 
                using CacheItems_395eb099a7a54363b464e19389df848f as S 
                on (T.ItemId = S.ItemId) 
                when matched 
                then update set T.ItemId=S.ItemId
                when not matched 
                then insert (Id,ItemId) values (S.Id,S.ItemId)
                OUTPUT $action,inserted.*;
             */
        }
    }
}