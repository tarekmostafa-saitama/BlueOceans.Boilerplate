//namespace Infrastructure.Repositories;

//using Application.Repositories;
//using Application.Specifications;
//using Dapper;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//public class DapperRepository<TEntity> : IRepository<TEntity> where TEntity : class
//{
//    private readonly IDbConnection _dbConnection;

//    public DapperRepository(string connectionString)
//    {
//        _dbConnection = new SqlConnection(connectionString);
//    }

//    public async Task<TEntity> GetByIdAsync(int id, bool trackChanges = true, params Expression<Func<TEntity, object>>[] includes)
//    {
//        var query = BuildSelectQuery(includes);
//        return await _dbConnection.QueryFirstOrDefaultAsync<TEntity>(query, new { Id = id });
//    }

//    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> criteria, bool trackChanges = true, params Expression<Func<TEntity, object>>[] includes)
//    {
//        var query = BuildSelectQuery(includes) + " WHERE " + GetSqlExpression(criteria);
//        return await _dbConnection.QueryFirstOrDefaultAsync<TEntity>(query);
//    }

//    public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = true, params Expression<Func<TEntity, object>>[] includes)
//    {
//        var query = BuildSelectQuery(includes);
//        return await _dbConnection.QueryAsync<TEntity>(query);
//    }

//    public async Task<IEnumerable<TEntity>> GetAsync(ISpecification<TEntity> specification, bool trackChanges = true, params Expression<Func<TEntity, object>>[] includes)
//    {
//        var query = BuildSelectQuery(includes) + " WHERE " + GetSqlExpression(specification.Criteria);
//        return await _dbConnection.QueryAsync<TEntity>(query, specification.Parameters);
//    }

//    public void Add(TEntity entity)
//    {
//        var query = BuildInsertQuery();
//        _dbConnection.Execute(query, entity);
//    }

//    public void Update(TEntity entity)
//    {
//        var query = BuildUpdateQuery();
//        _dbConnection.Execute(query, entity);
//    }

//    public void Remove(TEntity entity)
//    {
//        var query = BuildDeleteQuery();
//        _dbConnection.Execute(query, entity);
//    }

//    private string BuildSelectQuery(Expression<Func<TEntity, object>>[] includes)
//    {
//        var tableName = typeof(TEntity).Name;
//        var selectQuery = $"SELECT * FROM {tableName}";

//        if (includes != null && includes.Any())
//        {
//            var navigationProperties = includes.Select(expr => GetPropertyName(expr)).ToArray();
//            var joins = string.Join(" ", navigationProperties.Select(prop => $"LEFT JOIN {prop} ON {tableName}.Id = {prop}.{tableName}Id"));
//            selectQuery = $"{selectQuery} {joins}";
//        }

//        return selectQuery;
//    }

//    private string BuildInsertQuery()
//    {
//        var tableName = typeof(TEntity).Name;
//        var properties = GetPropertyNamesForInsert();
//        var columns = string.Join(", ", properties);
//        var values = string.Join(", ", properties.Select(p => $"@{p}"));
//        return $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
//    }

//    private string BuildUpdateQuery()
//    {
//        var tableName = typeof(TEntity).Name;
//        var properties = GetPropertyNamesForUpdate();
//        var setClause = string.Join(", ", properties.Select(p => $"{p} = @{p}"));
//        return $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";
//    }

//    private string BuildDeleteQuery()
//    {
//        var tableName = typeof(TEntity).Name;
//        return $"DELETE FROM {tableName} WHERE Id = @Id";
//    }

//    private string GetPropertyName(Expression<Func<TEntity, object>> expr)
//    {
//        if (expr.Body is MemberExpression memberExpr)
//        {
//            return memberExpr.Member.Name;
//        }
//        else if (expr.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression operand)
//        {
//            return operand.Member.Name;
//        }
//        throw new ArgumentException("Invalid expression");
//    }

//    private string GetSqlExpression(Expression<Func<TEntity, bool>> criteria)
//    {
//        return ExpressionParser<TEntity>.ToSql(criteria);
//    }

//    private string[] GetPropertyNamesForInsert()
//    {
//        // Exclude the Id property when inserting
//        return typeof(TEntity)
//            .GetProperties()
//            .Where(prop => prop.Name != "Id")
//            .Select(prop => prop.Name)
//            .ToArray();
//    }

//    private string[] GetPropertyNamesForUpdate()
//    {
//        // Include all properties except Id in the update query
//        return typeof(TEntity)
//            .GetProperties()
//            .Where(prop => prop.Name != "Id")
//            .Select(prop => prop.Name)
//            .ToArray();
//    }
//}//namespace Infrastructure.Repositories;

//using Application.Repositories;
//using Application.Specifications;
//using Dapper;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//public class DapperRepository<TEntity> : IRepository<TEntity> where TEntity : class
//{
//    private readonly IDbConnection _dbConnection;

//    public DapperRepository(string connectionString)
//    {
//        _dbConnection = new SqlConnection(connectionString);
//    }

//    public async Task<TEntity> GetByIdAsync(int id, bool trackChanges = true, params Expression<Func<TEntity, object>>[] includes)
//    {
//        var query = BuildSelectQuery(includes);
//        return await _dbConnection.QueryFirstOrDefaultAsync<TEntity>(query, new { Id = id });
//    }

//    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> criteria, bool trackChanges = true, params Expression<Func<TEntity, object>>[] includes)
//    {
//        var query = BuildSelectQuery(includes) + " WHERE " + GetSqlExpression(criteria);
//        return await _dbConnection.QueryFirstOrDefaultAsync<TEntity>(query);
//    }

//    public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = true, params Expression<Func<TEntity, object>>[] includes)
//    {
//        var query = BuildSelectQuery(includes);
//        return await _dbConnection.QueryAsync<TEntity>(query);
//    }

//    public async Task<IEnumerable<TEntity>> GetAsync(ISpecification<TEntity> specification, bool trackChanges = true, params Expression<Func<TEntity, object>>[] includes)
//    {
//        var query = BuildSelectQuery(includes) + " WHERE " + GetSqlExpression(specification.Criteria);
//        return await _dbConnection.QueryAsync<TEntity>(query, specification.Parameters);
//    }

//    public void Add(TEntity entity)
//    {
//        var query = BuildInsertQuery();
//        _dbConnection.Execute(query, entity);
//    }

//    public void Update(TEntity entity)
//    {
//        var query = BuildUpdateQuery();
//        _dbConnection.Execute(query, entity);
//    }

//    public void Remove(TEntity entity)
//    {
//        var query = BuildDeleteQuery();
//        _dbConnection.Execute(query, entity);
//    }

//    private string BuildSelectQuery(Expression<Func<TEntity, object>>[] includes)
//    {
//        var tableName = typeof(TEntity).Name;
//        var selectQuery = $"SELECT * FROM {tableName}";

//        if (includes != null && includes.Any())
//        {
//            var navigationProperties = includes.Select(expr => GetPropertyName(expr)).ToArray();
//            var joins = string.Join(" ", navigationProperties.Select(prop => $"LEFT JOIN {prop} ON {tableName}.Id = {prop}.{tableName}Id"));
//            selectQuery = $"{selectQuery} {joins}";
//        }

//        return selectQuery;
//    }

//    private string BuildInsertQuery()
//    {
//        var tableName = typeof(TEntity).Name;
//        var properties = GetPropertyNamesForInsert();
//        var columns = string.Join(", ", properties);
//        var values = string.Join(", ", properties.Select(p => $"@{p}"));
//        return $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
//    }

//    private string BuildUpdateQuery()
//    {
//        var tableName = typeof(TEntity).Name;
//        var properties = GetPropertyNamesForUpdate();
//        var setClause = string.Join(", ", properties.Select(p => $"{p} = @{p}"));
//        return $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";
//    }

//    private string BuildDeleteQuery()
//    {
//        var tableName = typeof(TEntity).Name;
//        return $"DELETE FROM {tableName} WHERE Id = @Id";
//    }

//    private string GetPropertyName(Expression<Func<TEntity, object>> expr)
//    {
//        if (expr.Body is MemberExpression memberExpr)
//        {
//            return memberExpr.Member.Name;
//        }
//        else if (expr.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression operand)
//        {
//            return operand.Member.Name;
//        }
//        throw new ArgumentException("Invalid expression");
//    }

//    private string GetSqlExpression(Expression<Func<TEntity, bool>> criteria)
//    {
//        return ExpressionParser<TEntity>.ToSql(criteria);
//    }

//    private string[] GetPropertyNamesForInsert()
//    {
//        // Exclude the Id property when inserting
//        return typeof(TEntity)
//            .GetProperties()
//            .Where(prop => prop.Name != "Id")
//            .Select(prop => prop.Name)
//            .ToArray();
//    }

//    private string[] GetPropertyNamesForUpdate()
//    {
//        // Include all properties except Id in the update query
//        return typeof(TEntity)
//            .GetProperties()
//            .Where(prop => prop.Name != "Id")
//            .Select(prop => prop.Name)
//            .ToArray();
//    }
//}

