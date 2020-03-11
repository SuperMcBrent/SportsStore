using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SportsStore.DAL {
    public class Cart {
        #region Fields
        string _connStr = ConfigurationManager.ConnectionStrings["SportsStoreConn"].ToString();
        #endregion

        /// <summary>
        /// Update record into database
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public void Update(Contracts.Cart cart) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"UPDATE cart SET NumberOfItems = '" + cart.NumberOfItems + "', TotalValue ='" + cart.TotalValue + "' WHERE Id = " + cart.Id;
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
        /// <param name="cart"></param>
        /// <returns></returns>
        public int Insert(Contracts.Cart cart) {
            Int32 id = 0;
            string sql = "INSERT INTO cart ( NumberOfItems, TotalValue ) VALUES ( '" + cart.NumberOfItems + "', '" + cart.TotalValue + "' ); "
                + "SELECT CAST(scope_identity() AS int);";
            // output INSERTED.ID typisch Microsoft SQL Server
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try {
                    conn.Open();
                    id = (Int32)cmd.ExecuteScalar();
                    cart.Id = id;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public int Insert(List<Contracts.Cart> carts) {
            Int32 id = 0;
            string sql = "INSERT INTO cart ( NumberOfItems, TotalValue ) OUTPUT INSERTED.ID VALUES ( @NumberOfItems, @TotalValue )";
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@NumberOfItems", SqlDbType.NVarChar);
                cmd.Parameters.Add("@TotalValue", System.Data.SqlDbType.NVarChar);
                try {
                    conn.Open();
                    foreach (var cart in carts) {
                        cmd.Parameters["@NumberOfItems"].Value = cart.NumberOfItems;
                        cmd.Parameters["@TotalValue"].Value = cart.TotalValue;
                        id = (Int32)cmd.ExecuteScalar();
                        cart.Id = id;
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
        /// <param name="cart"></param>
        public void Delete(Contracts.Cart cart) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"DELETE FROM cart WHERE cartId = " + cart.Id;
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
        /// <param name="cart"></param>
        /// <returns></returns>
        public bool Exists(Contracts.Cart cart) {
            if (cart.Id <= 0 && string.IsNullOrEmpty(cart.NumberOfItems.ToString())) return false;

            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, NumberOfItems, TotalValue FROM cart WHERE ";
                if (cart.Id != 0)
                    sql += " Id = " + cart.Id;
                else if (!string.IsNullOrEmpty(cart.NumberOfItems.ToString()))
                    sql += " Name = '" + cart.NumberOfItems.ToString() + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        cart.Id = (int)dataReader[0];
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
        /// <param name="cart"></param>
        /// <returns></returns>
        public bool Select(Contracts.Cart cart) {
            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, Name, PostalCode FROM cart WHERE ";
                if (cart.Id != 0)
                    sql += " Id = " + cart.Id;
                else if (!string.IsNullOrEmpty(cart.NumberOfItems.ToString()))
                    sql += " Name = '" + cart.NumberOfItems.ToString() + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        cart.Id = (int)dataReader["Id"];
                        cart.NumberOfItems = (int)dataReader["NumberOfItems"];
                        cart.TotalValue = (decimal)dataReader["TotalValue"];
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
