using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SportsStore.DAL {
    public class City {
        #region Fields
        string _connStr = ConfigurationManager.ConnectionStrings["SportsStoreConn"].ToString();
        #endregion

        /// <summary>
        /// Update record into database
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public void Update(Contracts.City city) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"UPDATE City SET Name = '" + city.Name + "', PostalCode ='" + city.PostalCode + "' WHERE Id = " + city.Id;
                SqlCommand updateCommand = new SqlCommand(sql, conn);
                updateCommand.ExecuteNonQuery();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                throw;
            } finally {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// Insert new row in database
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public int Insert(Contracts.City city) {
            Int32 id = 0;
            string sql = "INSERT INTO City ( Name, PostalCode ) VALUES ( '" + city.Name + "', '" + city.PostalCode + "' ); "
                + "SELECT CAST(scope_identity() AS int);";
            // output INSERTED.ID typisch Microsoft SQL Server
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try {
                    conn.Open();
                    id = (Int32)cmd.ExecuteScalar();
                    city.Id = id;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public int Insert(List<Contracts.City> citys) {
            Int32 id = 0;
            string sql = "INSERT INTO City ( Name, PostalCode ) OUTPUT INSERTED.ID VALUES ( @Name, @PostalCode )";
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters.Add("@PostalCode", System.Data.SqlDbType.NVarChar);
                try {
                    conn.Open();
                    foreach (var city in citys) {
                        cmd.Parameters["@Name"].Value = city.Name;
                        cmd.Parameters["@PostalCode"].Value = city.PostalCode;
                        id = (Int32)cmd.ExecuteScalar();
                        city.Id = id;
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        /// <summary>
        /// Delete row from database
        /// </summary>
        /// <param name="city"></param>
        public void Delete(Contracts.City city) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"DELETE FROM City WHERE CityId = " + city.Id;
                SqlCommand updateCommand = new SqlCommand(sql, conn);
                updateCommand.ExecuteNonQuery();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                throw;
            } finally {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// Get row from database
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public bool Exists(Contracts.City city) {
            if (city.Id <= 0 && string.IsNullOrEmpty(city.Name)) return false;

            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, Name, PostalCode FROM City WHERE ";
                if (city.Id != 0)
                    sql += " Id = " + city.Id;
                else if (!string.IsNullOrEmpty(city.Name))
                    sql += " Name = '" + city.Name + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        city.Id = (int)dataReader[0];
                        ++count;
                    }
                    return true;
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                throw;
            } finally {
                conn.Close();
                conn.Dispose();
            }
            return false;
        }

        /// <summary>
        /// Get row from database
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public bool Select(Contracts.City city) {
            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, Name, PostalCode FROM City WHERE ";
                if (city.Id != 0)
                    sql += " Id = " + city.Id;
                else if (!string.IsNullOrEmpty(city.Name))
                    sql += " Name = '" + city.Name + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        city.Id = (int)dataReader["Id"];
                        city.Name = (string)dataReader["Name"];
                        city.PostalCode = (string)dataReader["PostalCode"];
                        ++count;
                    }
                    return true;
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                throw;
            } finally {
                conn.Close();
                conn.Dispose();
            }
            return false;
        }
    }
}
