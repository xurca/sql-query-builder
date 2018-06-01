using SqlQueryBuilder.Generics;
using System;
using System.Linq.Expressions;

namespace SqlQueryBuilder
{
	public class SqlQueryBuilder : SqlQueryBuilderBase
	{
		public SqlQueryBuilder(ITableNameResolver tableNameResolver, IColumnNameResolver columnNameResolver) : base(tableNameResolver, columnNameResolver)
		{
		}

		private SqlQueryBuilder(SqlQueryBuilderBase sqlQueryBuilderBase) : base(sqlQueryBuilderBase)
		{
		}

		public SqlQueryBuilder<TTable> From<TTable>()
		{
			return UpdateAndExpand<TTable>(sqlBuilder => sqlBuilder.AddFrom<TTable>());
		}

		public SqlQueryBuilder<TTable> Insert<TTable>(Expression<Func<TTable, string>> stringExpression, params object[] parameters)
		{
			return UpdateAndExpand<TTable>(sqlBuilder => sqlBuilder.AddInsert<TTable>(ParseStringFormatExpression(stringExpression.Body), parameters));
		}

		public SqlQueryBuilder<TTable> Update<TTable>(Expression<Func<TTable, string>> stringExpression, params object[] parameters)
		{
			return UpdateAndExpand<TTable>(sqlBuilder => sqlBuilder.AddUpdate<TTable>(ParseStringFormatExpression(stringExpression.Body), parameters));
		}

		private SqlQueryBuilder<TNew> UpdateAndExpand<TNew>(Action<SqlQueryBuilder> updateAction)
		{
			var clone = Clone();
			updateAction(clone);

			return new SqlQueryBuilder<TNew>(clone);
		}

		private SqlQueryBuilder Clone()
		{
			return new SqlQueryBuilder(this);
		}
	}
}