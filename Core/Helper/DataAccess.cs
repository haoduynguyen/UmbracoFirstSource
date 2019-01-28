using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Customs.Helper
{
    public class DataAccess
    {
        string connectionString;
        SqlConnection myConnection;
        public SqlCommand myCommand = new SqlCommand();
        SqlDataReader reader;


        public DataAccess(string connectionStringKey = "umbracoDbDSN")
        {
            this.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
        }

        public void Open()
        {
            if (myConnection == null)
            {
                myConnection = new SqlConnection(connectionString);
            }            
            myConnection.Open();
        }

        public void BuildCommand(bool isStoreProcedure = false)
        {
            myCommand = new SqlCommand();
            myCommand.Connection = myConnection;
            if (isStoreProcedure)
            {
                myCommand.CommandType = CommandType.StoredProcedure;
            }
        }

        public void SetSQLQuery(String query)
        {            
            myCommand.CommandText = query;
        }


        public void SetCommand(string sqlQuery, bool isStoreProcedure = false)
        {
            myCommand = new SqlCommand(sqlQuery, myConnection);
            if (isStoreProcedure)
            {
                myCommand.CommandType = CommandType.StoredProcedure;
            }
        }

        public void AddParameter(SqlParameter param)
        {
            myCommand.Parameters.Add(param);
        }

        public void AddParameter(string paramName, object paramValue)
        {
            myCommand.Parameters.AddWithValue(paramName, paramValue);
        }
        public SqlParameter AddParameter(string paramName, SqlDbType type)
        {
            return myCommand.Parameters.Add(paramName, type);
        }
       

        public void Close()
        {
            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }
            if (myCommand != null)
            {
                myCommand.Dispose();
            }
            if (myConnection != null)
            {
                myConnection.Close();
            }
        }

        public void ExecuteReader()
        {
            myCommand.Connection = myConnection;
            reader = myCommand.ExecuteReader();
        }

        public int ExecuteNonQuery()
        {
            myCommand.Connection = myConnection;
            return myCommand.ExecuteNonQuery();
        }

        public object ExecuteScalar()
        {
            myCommand.Connection = myConnection;
            return myCommand.ExecuteScalar();
        }

        public DataTable Query(string sql)
        {

            DataTable result = new DataTable();
            try
            {                
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = sql;
                this.Open();
                myCommand.Connection = myConnection;                
                result.Load(myCommand.ExecuteReader());

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                this.Close();
            }
            return result;
        }

        public int InsertTable(string tablename, Dictionary<string, object> data, bool identify = true)
        {
            string sql = "", insertkeysql = "", insertvaluesql = "", comma = "";
            int newid = 0;
            try
            {
                myCommand.Parameters.Clear();
                foreach (KeyValuePair<string, object> kVal in data)
                {
                    insertkeysql += comma + "[" + kVal.Key + "]";
                    insertvaluesql += comma + "@" + kVal.Key + "";
                    comma = " , ";
                    if (kVal.Value == null || string.IsNullOrWhiteSpace(kVal.Value.ToString()))
                        myCommand.Parameters.Add("@" + kVal.Key, SqlDbType.DateTime).Value = DBNull.Value;
                    if (kVal is DateTime)
                        myCommand.Parameters.Add("@" + kVal.Key, SqlDbType.DateTime).Value = kVal.Value;
                    else                    
                        myCommand.Parameters.AddWithValue("@" + kVal.Key, kVal.Value);
                }
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "INSERT INTO " + tablename + " (" + insertkeysql + ") VALUES (" + insertvaluesql + ");" + (identify ? "SELECT SCOPE_IDENTITY();" : "");
                this.Open();
                myCommand.Connection = myConnection;                
                if (identify)
                {
                    newid = Convert.ToInt32(myCommand.ExecuteScalar().ToString());
                }
                else
                {
                    newid = myCommand.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                this.Close();
            }
            return newid;
        }

        public int UpdateTable(string tablename, Dictionary<string, object> data, Dictionary<string, object> wheresqlarr)
        {
            string setsql = "", comma = "", wheresql = "";
            int newid = 0;
            try
            {
                myCommand.Parameters.Clear();
                foreach (KeyValuePair<string, object> kVal in data)
                {
                    setsql += comma + "" + kVal.Key + " = @" + kVal.Key;
                    comma = ", ";
                    myCommand.Parameters.AddWithValue("@" + kVal.Key, kVal.Value);
                }
                /*Where */
                comma = "";
                foreach (KeyValuePair<string, object> kvalue in wheresqlarr)
                {
                    wheresql += comma + "" + kvalue.Key + " = @" + kvalue.Key;
                    comma = " AND ";
                    myCommand.Parameters.AddWithValue("@" + kvalue.Key, kvalue.Value);                    
                }
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = "UPDATE " + tablename + " SET " + setsql + " WHERE " + wheresql;
                this.Open();
                myCommand.Connection = myConnection;   
                newid = Convert.ToInt32(myCommand.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                this.Close();
            }
            return newid;
        }
        public int UpdateTable(string tablename, Dictionary<string, object> data, string wheresql)
        {
            string setsql = "", comma = "";
            int newid = 0;
            try
            {
                myCommand.CommandType = CommandType.Text;
                myCommand.Parameters.Clear();
                foreach (KeyValuePair<string, object> kVal in data)
                {
                    setsql += comma + "[" + kVal.Key + "] = @" + kVal.Key;
                    comma = ", ";
                    myCommand.Parameters.AddWithValue("@" + kVal.Key, kVal.Value);
                }
                myCommand.CommandText = "UPDATE " + tablename + " SET " + setsql + " WHERE " + wheresql;
                this.Open();
                myCommand.Connection = myConnection;   
                newid = Convert.ToInt32(myCommand.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                this.Close();
            }
            return newid;
        }

        public SqlDataReader Reader { get { return reader; } }
    }
}