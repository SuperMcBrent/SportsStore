using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SportsStore.DAL {
    class OrderLine {
        #region Fields
        string _connStr = ConfigurationManager.ConnectionStrings["SportsStoreConn"].ToString();
        #endregion

        /// <summary>
        /// Update record into database
        /// </summary>
        /// <param name="orderline"></param>
        /// <returns></returns>
        public void Update(Contracts.OrderLine orderline) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"UPDATE Orderline SET OrderId = '" + orderline.OrderId + "', ProductId ='" + orderline.ProductId + "' WHERE Id = " + orderline.Id;
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
        /// <param name="orderline"></param>
        /// <returns></returns>
        public int Insert(Contracts.OrderLine orderline) {
            Int32 id = 0;
            string sql = "INSERT INTO Orderline ( OrderId, ProductId ) VALUES ( '" + orderline.OrderId + "', '" + orderline.ProductId + "' ); "
                + "SELECT CAST(scope_identity() AS int);";
            // output INSERTED.ID typisch Microsoft SQL Server
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try {
                    conn.Open();
                    id = (Int32)cmd.ExecuteScalar();
                    orderline.Id = id;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public int Insert(List<Contracts.OrderLine> orderlines) {
            Int32 id = 0;
            string sql = "INSERT INTO Orderline ( OrderId, ProductId ) OUTPUT INSERTED.ID VALUES ( @OrderId, @ProductId )";
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@OrderId", SqlDbType.NVarChar);
                cmd.Parameters.Add("@ProductId", SqlDbType.NVarChar);
                try {
                    conn.Open();
                    foreach (var orderline in orderlines) {
                        cmd.Parameters["@OrderId"].Value = orderline.OrderId;
                        cmd.Parameters["@ProductId"].Value = orderline.ProductId;
                        id = (Int32)cmd.ExecuteScalar();
                        orderline.Id = id;
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
        /// <param name="orderline"></param>
        public void Delete(Contracts.OrderLine orderline) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"DELETE FROM Orderline WHERE Id = " + orderline.Id;
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
        /// <param name="orderline"></param>
        /// <returns></returns>
        public bool Exists(Contracts.OrderLine orderline) {
            if (orderline.Id <= 0 && string.IsNullOrEmpty(orderline.OrderId.ToString())) return false;

            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, OrderId, ProductId FROM Orderline WHERE ";
                if (orderline.Id != 0)
                    sql += " Id = " + orderline.Id;
                else if (!string.IsNullOrEmpty(orderline.OrderId.ToString()))
                    sql += " OrderId = '" + orderline.OrderId.ToString() + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        orderline.Id = (int)dataReader[0];
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
        /// <param name="orderline"></param>
        /// <returns></returns>
        public bool Select(Contracts.OrderLine orderline) {
            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, Name, PostalCode FROM Orderline WHERE ";
                if (orderline.Id != 0)
                    sql += " Id = " + orderline.Id;
                else if (!string.IsNullOrEmpty(orderline.OrderId.ToString()))
                    sql += " OrderId = '" + orderline.OrderId + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        orderline.Id = (int)dataReader["Id"];
                        orderline.OrderId = (int)dataReader["OrderId"];
                        orderline.ProductId = (int)dataReader["ProductId"];
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
