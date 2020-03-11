using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SportsStore.DAL
{
    public class Category
    {
        #region Fields
        string _connStr = ConfigurationManager.ConnectionStrings["SportsStoreConn"].ToString();
        #endregion

        /// <summary>
        /// Update record into database
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public void Update(Contracts.Category category) 
        {
            SqlConnection conn = new SqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = $"UPDATE category SET Name = '" + category.Name + "', CategoryId ='" + category.CategoryId + "' WHERE Id = " + category.Id;
                SqlCommand updateCommand = new SqlCommand(sql,conn);
                updateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// Insert new row in database
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int Insert(Contracts.Category category)
        {
            Int32 id = 0;
            string sql = "INSERT INTO category ( Name, CategoryId ) VALUES ( '" + category.Name + "', '" + category.CategoryId + "'  ); "
                + "SELECT CAST(scope_identity() AS int);";
            // output INSERTED.ID typisch Microsoft SQL Server
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand(sql,conn);
                try
                {
                    conn.Open();
                    id = (Int32)cmd.ExecuteScalar();
                    category.Id = id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public int Insert(List<Contracts.Category> categories)
        {
            Int32 id = 0;
            string sql = "INSERT INTO category ( Name, CategoryId ) OUTPUT INSERTED.ID VALUES ( @Name, @CategoryId )";
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.Add("@Name",SqlDbType.NVarChar);
                cmd.Parameters.Add("@CategoryId",SqlDbType.NVarChar);
                try
                {
                    conn.Open();
                    foreach (var category in categories)
                    {
                        cmd.Parameters["@Name"].Value = category.Name;
                        cmd.Parameters["@CategoryId"].Value = category.CategoryId;
                        id = (Int32)cmd.ExecuteScalar();
                        category.Id = id;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        /// <summary>
        /// Delete row from database
        /// </summary>
        /// <param name="category"></param>
        public void Delete(Contracts.Category category)
        {
            SqlConnection conn = new SqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = $"DELETE FROM category WHERE categoryId = " + category.Id;
                SqlCommand updateCommand = new SqlCommand(sql,conn);
                updateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// Get row from database
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool Exists(Contracts.Category category)
        {
            if (category.Id <= 0 && string.IsNullOrEmpty(category.Name)) return false;

            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = "SELECT Id, Name, CategoryId FROM category WHERE ";
                if (category.Id != 0)
                    sql += " Id = " + category.Id;
                else if (!string.IsNullOrEmpty(category.Name.ToString()))
                    sql += " Name = '" + category.Name + "'";
                SqlCommand command = new SqlCommand(sql,conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        category.Id = (int)dataReader[0];
                        ++count;
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return false;
        }

        /// <summary>
        /// Get row from database
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool Select(Contracts.Category category)
        {
            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = "SELECT Id, Name, CategoryId FROM category WHERE ";
                if (category.Id != 0)
                    sql += " Id = " + category.Id;
                else if (!string.IsNullOrEmpty(category.Name))
                    sql += " Name = '" + category.Name + "'";
                SqlCommand command = new SqlCommand(sql,conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        category.Id = (int)dataReader["Id"];
                        category.Name = (string)dataReader["Name"];
                        category.CategoryId = (int)dataReader["CategoryId"];
                        ++count;
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return false;
        }


    }
}
