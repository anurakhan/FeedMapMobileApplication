using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace FeedMapWebApiApp.Models
{
    public class DataAccess
    {
        public string ConnectionString
        {
            get
            {
                return m_configuration.GetConnectionString("FeedMapDataBase");
            }
        }

        IConfiguration m_configuration;

        public DataAccess(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        #region execute query provided connection
        public void ExecuteNonQuery(SqlConnection connection, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public T ExecuteScalar<T>(SqlConnection connection, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                return (T)cmd.ExecuteScalar();
            }
        }

        public DataTable FillTable(SqlConnection connection, string query)
        {
            DataTable tbl = new DataTable();
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                using (SqlDataAdapter adt = new SqlDataAdapter(cmd))
                {
                    adt.Fill(tbl);
                }
            }
            return tbl;
        }
        #endregion

        #region execute query without provided connection
        public void ExecuteNonQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                ExecuteNonQuery(conn, query);
            }
        }

        public T ExecuteScalar<T>(string query)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                return ExecuteScalar<T>(conn, query);
            }
        }

        public DataTable FillTable(string query)
        {
            DataTable tbl;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                tbl = FillTable(conn, query);
            }
            return tbl;
        }
        #endregion

        #region execute query with params
        public void ExecuteNonQuery(string query, List<SqlParameter> sqlParams)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(sqlParams.ToArray());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable FillTable(string query, List<SqlParameter> sqlParams)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(sqlParams.ToArray());
                    using (SqlDataAdapter adt = new SqlDataAdapter(cmd))
                    {
                        adt.Fill(tbl);
                    }
                }
            }
            return tbl;
        }
        public T ExecuteScalar<T>(string query, List<SqlParameter> sqlParams)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(sqlParams.ToArray());
                    return (T)cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        #region GetCommand
        public SqlCommand GetCommand(string text, CommandType cmdType, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.CommandType = cmdType;
            return cmd;
        }
        public SqlCommand GetCommand(string text, CommandType cmdType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.Transaction = tran;
            cmd.CommandType = cmdType;
            return cmd;
        }
        #endregion

        #region build sql param
        public SqlParameter BuildSqlParam(string name, SqlDbType dbType, object value)
        {
            SqlParameter parameter = new SqlParameter(name, dbType);
            parameter.Value = value;
            return parameter;
        }
        public SqlParameter BuildSqlParam(string name, SqlDbType dbType, object value, int size)
        {
            SqlParameter parameter = new SqlParameter(name, dbType, size);
            parameter.Value = value;
            return parameter;
        }
        #endregion
    }
    public static class SqlHelper
    {
        public static void Add(this List<SqlParameter> sqlCol, string name, SqlDbType dbType, object value)
        {
            SqlParameter parameter = new SqlParameter(name, dbType);
            parameter.Value = value;
            sqlCol.Add(parameter);
        }
    }
}
