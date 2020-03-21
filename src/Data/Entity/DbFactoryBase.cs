using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using System.Linq;
using System.Text;
using System.Reflection;
using System;

namespace API.Data
{
    public abstract class DbFactoryBase
    {
        private readonly IConfiguration _config;

        internal string DbConnectionString => _config.GetConnectionString("SQLDBConnectionString");

        public DbFactoryBase(IConfiguration config)
        {
            _config = config;
        }

        internal IDbConnection DbConnection => new SqlConnection(_config.GetConnectionString("SQLDBConnectionString"));

        public virtual async Task<IEnumerable<T>> DbQueryAsync<T>(string sql, object parameters = null)
        {
            using (IDbConnection dbCon = DbConnection)
            {
                if (parameters == null)
                    return await dbCon.QueryAsync<T>(sql);

                return await dbCon.QueryAsync<T>(sql, parameters);
            }
        }
        public virtual async Task<T> DbQuerySingleAsync<T>(string sql, object parameters)
        {
            using (IDbConnection dbCon = DbConnection)
            {
                return await dbCon.QueryFirstOrDefaultAsync<T>(sql, parameters);
            }
        }

        public virtual async Task<bool> DbExecuteAsync<T>(string sql, object parameters)
        {
            using (IDbConnection dbCon = DbConnection)
            {
                return await dbCon.ExecuteAsync(sql, parameters) > 0;
            }
        }

        public virtual async Task<bool> DbExecuteScalarAsync(string sql, object parameters)
        {
            using (IDbConnection dbCon = DbConnection)
            {
                return await dbCon.ExecuteScalarAsync<bool>(sql, parameters);
            }
        }

        public virtual async Task<T> DbExecuteScalarDynamicAsync<T>(string sql, object parameters = null)
        {
            using (IDbConnection dbCon = DbConnection)
            {
                if (parameters == null)
                    return await dbCon.ExecuteScalarAsync<T>(sql);

                return await dbCon.ExecuteScalarAsync<T>(sql, parameters);
            }
        }

        public virtual async Task<(IEnumerable<T> Data, int RecordCount)> DbQueryMultipleAsync<T>(string sql, object parameters = null)
        {
            IEnumerable<T> data = null;
            int totalRecords = 0;

            using (IDbConnection dbCon = DbConnection)
            {
                using (GridReader results = await dbCon.QueryMultipleAsync(sql, parameters))
                {
                    data = await results.ReadAsync<T>();
                    totalRecords = await results.ReadSingleAsync<int>();
                }
            }

            return (data, totalRecords);
        }


        public virtual async Task<(IEnumerable<TParent> Data, int RecordCount)> DbQueryMultipleAsync<TParent,TChild>(string sql, object parameters = null)
        {
            IEnumerable<TParent> data = null;
            int totalRecords = 0;

            using (IDbConnection dbCon = DbConnection)
            {
                using (GridReader results = await dbCon.QueryMultipleAsync(sql, parameters))
                {
                    data = await results.ReadAsync<TParent>();
                    totalRecords = await results.ReadSingleAsync<int>();
                }
            }

            return (data, totalRecords);
        }


        /// <summary>
        /// Uses DbFactory base methods calculate the pagination
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="urlSearchParams"></param>
        /// <param name="builder"></param>
        /// <param name="orderCriteria"></param>
        /// <returns></returns>
        public virtual async Task<(IEnumerable<TParent> Data, Pagination Pagination)> DbQueryPagedAsync<TParent>(UrlQuerySearchParameters urlSearchParams,
            SqlBuilder builder
            , string orderCriteria = "Id")
        {
            IEnumerable<TParent> data = null;
            int recordCount = 0;
            
            var param = new DynamicParameters();
            param.Add("Limit", urlSearchParams.PageSize);
            param.Add("Offset", urlSearchParams.PageNumber);
            string tableName = typeof(TParent).Name;
            if (builder == null)            
                builder = new SqlBuilder(); 
            
            //orderCriteria = $"[{tableName}].[" + orderCriteria+"]";

            //orderCriteria += $"[{tableName}]" + $".[{orderCriteria}]";
            //For the left join queries we should define TableName.ColumnName so ...
            StringBuilder sb = new StringBuilder();            
            foreach (var property in typeof(TParent).GetProperties())
            {
                builder.Select($"[{tableName}].[{property.Name}]");               
            }

            string sql = $@"SELECT /**select**/ FROM {tableName} /**where**/
                                  ORDER BY {orderCriteria} DESC 
                                  OFFSET @Limit * (@Offset -1) ROWS FETCH NEXT @Limit ROWS ONLY";
            

            if (urlSearchParams.SearchFields.Count > 0)
            {
                foreach (var listItem in urlSearchParams.SearchFields)
                {
                    if (typeof(TParent).GetProperty(listItem.Column).PropertyType == typeof(DateTime?)
                        || typeof(TParent).GetProperty(listItem.Column).PropertyType == typeof(DateTime))
                    {

                        string[] dates = listItem.SearchValue.Split("-");
                        if (dates.Length == 3)                        
                            builder.Where($"DATEPART(yy, {listItem.Column}) =  {dates[0]} AND DATEPART(mm, {listItem.Column}) = {dates[1]} AND DATEPART(dd, {listItem.Column}) = {dates[2]}");
                    }                        
                    else
                        builder.Where($"{listItem.Column} LIKE '{listItem.SearchValue}%'");
                } 
            }
           

            using (IDbConnection dbCon = DbConnection)
            {
                if (urlSearchParams.IncludeCount)
                {
                    sql += $@" Select COUNT(Id) from {tableName} /**where**/";
                    var query = builder.AddTemplate(sql, param).RawSql;

                    var pagedRows = await DbQueryMultipleAsync<TParent>(query, param);
                    data = pagedRows.Data;
                    recordCount = pagedRows.RecordCount;
                }
                else
                {
                    var query = builder.AddTemplate(sql, param).RawSql;
                    data = await DbQueryAsync<TParent>(query, param);
                }
            }

            var metadata = new Pagination
            {
                PageNumber = urlSearchParams.PageNumber,
                PageSize = urlSearchParams.PageSize,
                TotalRecords = recordCount
            };

            return (data, metadata);
        }
    }
}
