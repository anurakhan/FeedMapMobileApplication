using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FeedMapDAL.Helper;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;
using Microsoft.Extensions.Configuration;

namespace FeedMapDAL.Repository.Concrete
{
    public class UserDataRepository : IUserDataRepository
    {
        DataAccess m_DataAccess;

        public UserDataRepository(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration.GetConnectionString("FeedMapDataBase"));
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            string sql = " SELECT USER_ID, USER_USERNAME, USER_PASSWORDHASH, USER_PASSWORDSALT ";
            sql += " FROM Users ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0) return null;

            List<UserDTO> retLst = new List<UserDTO>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new UserDTO()
                {
                    Id = (int)row["USER_ID"],
                    UserName = (string)row["USER_USERNAME"],
                    PasswordHash = (byte[])row["USER_PASSWORDHASH"],
                    PasswordSalt = (byte[])row["USER_PASSWORDSALT"]
                });
            }

            return retLst;
        }


        public UserDTO GetUser(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT USER_ID, USER_USERNAME, USER_PASSWORDHASH, USER_PASSWORDSALT ";
            sql += " FROM Users ";
            sql += " WHERE USER_ID = @id ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0) return null;

            UserDTO ret = new UserDTO()
            {
                Id = (int)retTbl.Rows[0]["USER_ID"],
                UserName = (string)retTbl.Rows[0]["USER_USERNAME"],
                PasswordHash = (byte[])retTbl.Rows[0]["USER_PASSWORDHASH"],
                PasswordSalt = (byte[])retTbl.Rows[0]["USER_PASSWORDSALT"]
            };

            return ret;
        }

        public UserDTO GetUserByUserName(string userName)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@username", SqlDbType.VarChar, (object)userName);

            string sql = " SELECT USER_ID, USER_USERNAME, USER_PASSWORDHASH, USER_PASSWORDSALT ";
            sql += " FROM Users ";
            sql += " WHERE USER_USERNAME = @username ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0) return null;

            UserDTO ret = new UserDTO()
            {
                Id = (int)retTbl.Rows[0]["USER_ID"],
                UserName = (string)retTbl.Rows[0]["USER_USERNAME"],
                PasswordHash = (byte[])retTbl.Rows[0]["USER_PASSWORDHASH"],
                PasswordSalt = (byte[])retTbl.Rows[0]["USER_PASSWORDSALT"]
            };

            return ret;
        }

        public int Post(UserDTO user)
        {
            using (SqlConnection conn = new SqlConnection(m_DataAccess.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_USER", CommandType.StoredProcedure, conn))
                {
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@username", SqlDbType.VarChar, user.UserName));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@passwordhash", SqlDbType.Binary, user.PasswordHash));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@passwordsalt", SqlDbType.Binary, user.PasswordSalt));
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    return (int)cmd.Parameters["@id"].Value;
                }
            }
        }

        public void Update(UserDTO user, int id)
        {
            throw new NotImplementedException();
        }
    }
}
