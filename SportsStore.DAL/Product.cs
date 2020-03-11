using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SportsStore.DAL {
    class Product {
        #region Fields
        string _connStr = ConfigurationManager.ConnectionStrings["SportsStoreConn"].ToString();
        #endregion

        /// <summary>
        /// Update record into database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public void Update(Contracts.Product product) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"UPDATE Product SET " +
                    "Description = '" + product.Description + "', " +
                    "Name ='" + product.Name +
                    "Price ='" + product.Price +
                    "ProductId ='" + product.ProductId +
                    "' WHERE Id = " + product.Id;
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
        /// <param name="product"></param>
        /// <returns></returns>
        public int Insert(Contracts.Product product) {
            Int32 id = 0;
            string sql = "INSERT INTO Product ( Description, Name, Price, ProductId ) VALUES " +
                "( '" + product.Description + "', '" + product.Name + "', '" + product.Price + "', '" + product.ProductId + "' ); "
                + "SELECT CAST(scope_identity() AS int);";
            // output INSERTED.ID typisch Microsoft SQL Server
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try {
                    conn.Open();
                    id = (Int32)cmd.ExecuteScalar();
                    product.Id = id;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public int Insert(List<Contracts.Product> products) {
            Int32 id = 0;
            string sql = "INSERT INTO Product ( Description, Name, Price, ProductId ) OUTPUT INSERTED.ID VALUES " +
                "( @Description, @Name, @Price, @ProductId )";
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Price", SqlDbType.NVarChar);
                cmd.Parameters.Add("@ProductId", SqlDbType.NVarChar);
                try {
                    conn.Open();
                    foreach (var product in products) {
                        cmd.Parameters["@Description"].Value = product.Description;
                        cmd.Parameters["@Name"].Value = product.Name;
                        cmd.Parameters["@Price"].Value = product.Price;
                        cmd.Parameters["@ProductId"].Value = product.ProductId;
                        id = (Int32)cmd.ExecuteScalar();
                        product.Id = id;
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
        /// <param name="product"></param>
        public void Delete(Contracts.Product product) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"DELETE FROM Product WHERE ProductId = " + product.Id;
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
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Exists(Contracts.Product product) {
            if (product.Id <= 0 && string.IsNullOrEmpty(product.Name)) return false;

            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, Name, PostalCode FROM Product WHERE ";
                if (product.Id != 0)
                    sql += " Id = " + product.Id;
                else if (!string.IsNullOrEmpty(product.Name))
                    sql += " Name = '" + product.Name + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        product.Id = (int)dataReader[0];
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
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Select(Contracts.Product product) {
            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, Name, PostalCode FROM Product WHERE ";
                if (product.Id != 0)
                    sql += " Id = " + product.Id;
                else if (!string.IsNullOrEmpty(product.Name))
                    sql += " Name = '" + product.Name + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        product.Id = (int)dataReader["Id"];
                        product.Description = (string)dataReader["Description"];
                        product.Name = (string)dataReader["Name"];
                        product.Price = (decimal)dataReader["Price"];
                        product.ProductId = (int)dataReader["ProductId"];
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
