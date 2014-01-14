using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace com.metafour.aims.services.NCLoader
{
    class Database
    {
        private SqlConnection conn = new SqlConnection();
        private SqlCommand cmd = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();

        #region Singleton

        private static Database instance;

        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Database();
                }
                return instance;
            }
        }

        #endregion

        #region Properties

        private string m_sUser;
        private string m_sPassword;
        private string m_sDatabase;
        private string m_sServer;
        private int m_nTimeout;
        private string connString;

        public string User
        {
            get
            {
                return m_sUser;
            }
            set
            {
                m_sUser = value;
            }
        }

        public string Password
        {
            get
            {
                return m_sPassword;
            }
            set
            {
                m_sPassword = value;
            }
        }

        public string DB
        {
            get
            {
                return m_sDatabase;
            }
            set
            {
                m_sDatabase = value;
            }
        }


        public string Server
        {
            get
            {
                return m_sServer;
            }
            set
            {
                m_sServer = value;
            }
        }

        public int TimeOut
        {
            get
            {
                return m_nTimeout;
            }
            set
            {
                m_nTimeout = value;
            }
        }

        #endregion

        public Database()
        {

        }

        public string GetConnectionString()
        {
            string connString = "Data Source=" + Database.Instance.Server + ";Initial Catalog=" + Database.Instance.DB + ";User ID=" + Database.Instance.User + ";Password=" + Database.Instance.Password;
            return connString;
        }

        public SqlConnection GetConnection()
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn = new SqlConnection(GetConnectionString());
                conn.Open();
            }
            else
            {
                return conn;
            }

            return conn;
        }

        public void CloseConnection()
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }


        public void GetDataTable(string sql, ref DataTable dt)
        {
            try
            {
                da = new SqlDataAdapter(sql, GetConnection());
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToFile(ex.Message.ToString());
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                da = new SqlDataAdapter(sql, GetConnection());
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToFile(ex.Message.ToString());
            }
            finally
            {
                CloseConnection();
            }
            return dt;
        }

        public Int32 ExecuteQuery(string sql)
        {
            try
            {
                Int32 returnVal = 0;
                cmd.CommandText = sql;
                cmd.Connection = GetConnection();
                returnVal = cmd.ExecuteNonQuery();
                return returnVal;
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToFile(ex.Message.ToString());
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Int32 ExecuteStoredProcedure_GetIdentity(string sStoredProcedure, List<SqlParameter> parameter, SqlParameter op = null)
        {
            try
            {
                Int32 result = -1;
                DataTable dt = new DataTable();
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = GetConnection();
                oCmd.CommandText = sStoredProcedure;
                oCmd.CommandType = CommandType.StoredProcedure;
                if (parameter != null && parameter.Count != 0)
                {
                    oCmd.Parameters.AddRange(parameter.ToArray());
                }

                SqlDataReader reader = oCmd.ExecuteReader();
                if (op != null)
                {
                    if (oCmd.Parameters[op.ParameterName].Value != DBNull.Value)
                    {
                        result = (int)oCmd.Parameters[op.ParameterName].Value;
                    }

                }
                else
                {
                    if (oCmd.Parameters["@Key"].Value != DBNull.Value)
                    {
                        result = (int)oCmd.Parameters["@Key"].Value;
                    }

                }

                return result;

            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToFile(ex.Message.ToString());
                return -1;
            }
            finally
            {
                CloseConnection();
            }

        }



        public void ExecuteStoredProcedure(string sStoredProcedure, List<SqlParameter> parameter)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = GetConnection();
                oCmd.CommandText = sStoredProcedure;
                oCmd.CommandType = CommandType.StoredProcedure;
                if (parameter != null && parameter.Count != 0)
                {
                    oCmd.Parameters.AddRange(parameter.ToArray());
                }
                //oCmd.CommandTimeout = 300;
                SqlDataReader reader = oCmd.ExecuteReader();
                //Int32 result = Convert.ToInt32(oCmd.Parameters["@Key"]);            
                //return result;
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToFile(ex.Message.ToString());
                //return -1;
            }
            finally
            {
                CloseConnection();
            }

        }

        public DataTable ExecuteStoredProcedure_DataTable(string sStoredProcedure, List<SqlParameter> parameter)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = GetConnection();
                oCmd.CommandText = sStoredProcedure;
                oCmd.CommandType = CommandType.StoredProcedure;
                if (parameter != null && parameter.Count != 0)
                {
                    oCmd.Parameters.AddRange(parameter.ToArray());
                }
                //oCmd.CommandTimeout = 300;
                SqlDataReader reader = oCmd.ExecuteReader();
                dt.Load(reader);
                return dt;
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToFile(ex.Message.ToString());
                return null;
            }
            finally
            {
                CloseConnection();
            }

        }

        public Int32 ExecuteScalar(string sql)
        {
            try
            {
                Int32 retunVal = default(Int32);
                cmd.CommandText = sql;
                cmd.Connection = GetConnection();
                retunVal = Convert.ToInt32(cmd.ExecuteScalar());
                return retunVal;
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToFile(ex.Message.ToString());
                return 0;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
