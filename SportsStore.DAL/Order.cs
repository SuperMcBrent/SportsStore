using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SportsStore.DAL {
    class Order {
        #region Fields
        string _connStr = ConfigurationManager.ConnectionStrings["SportsStoreConn"].ToString();
        #endregion

        /// <summary>
        /// Update record into database
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public void Update(Contracts.Order order) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"UPDATE Order SET " +
                    "DeliveryDate = '" + order.DeliveryDate + "', " +
                    "Giftwrapping ='" + order.Giftwrapping + "' " +
                    "OrderDate ='" + order.OrderDate + "' " +
                    "OrderId ='" + order.OrderId + "' " +
                    "ShippingCity ='" + order.ShippingCity + "' " +
                    "ShippingStreet ='" + order.ShippingStreet + "' " +
                    "WHERE Id = " + order.Id;
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
        /// <param name="order"></param>
        /// <returns></returns>
        public int Insert(Contracts.Order order) {
            Int32 id = 0;
            string sql = "INSERT INTO Order ( DeliveryDate, Giftwrapping, OrderDate, OrderId, ShippingCity, ShippingStreet ) " +
                "VALUES ( '" + order.DeliveryDate + "', '" + order.Giftwrapping
                + "', '" + order.OrderDate + "', '" + order.OrderId
                + "', '" + order.ShippingCity + "', '" + order.ShippingStreet + "' ); "
                + "SELECT CAST(scope_identity() AS int);";
            // output INSERTED.ID typisch Microsoft SQL Server
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try {
                    conn.Open();
                    id = (Int32)cmd.ExecuteScalar();
                    order.Id = id;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public int Insert(List<Contracts.Order> orders) {
            Int32 id = 0;
            string sql = "INSERT INTO Order ( DeliveryDate, Giftwrapping, OrderDate, OrderId, ShippingCity, ShippingStreet ) OUTPUT INSERTED.ID " +
                "VALUES ( @DeliveryDate, @Giftwrapping, @OrderDate, @OrderId, @ShippingCity, @ShippingStreet )";
            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@DeliveryDate", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Giftwrapping", SqlDbType.NVarChar);
                cmd.Parameters.Add("@OrderDate", SqlDbType.NVarChar);
                cmd.Parameters.Add("@OrderId", SqlDbType.NVarChar);
                cmd.Parameters.Add("@ShippingCity", SqlDbType.NVarChar);
                cmd.Parameters.Add("@ShippingStreet", SqlDbType.NVarChar);
                try {
                    conn.Open();
                    foreach (var order in orders) {
                        cmd.Parameters["@DeliveryDate"].Value = order.DeliveryDate;
                        cmd.Parameters["@Giftwrapping"].Value = order.Giftwrapping;
                        cmd.Parameters["@OrderDate"].Value = order.OrderDate;
                        cmd.Parameters["@OrderId"].Value = order.OrderId;
                        cmd.Parameters["@ShippingCity"].Value = order.ShippingCity;
                        cmd.Parameters["@ShippingStreet"].Value = order.ShippingStreet;
                        id = (Int32)cmd.ExecuteScalar();
                        order.Id = id;
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
        /// <param name="order"></param>
        public void Delete(Contracts.Order order) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"DELETE FROM Order WHERE Id = " + order.Id;
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
        /// <param name="order"></param>
        /// <returns></returns>
        public bool Exists(Contracts.Order order) {
            if (order.Id <= 0 && string.IsNullOrEmpty(order.OrderId.ToString())) return false;

            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, OrderId FROM Order WHERE ";
                if (order.Id != 0)
                    sql += " Id = " + order.Id;
                else if (!string.IsNullOrEmpty(order.OrderId.ToString()))
                    sql += " OrderId = '" + order.OrderId.ToString() + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        order.Id = (int)dataReader[0];
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
        /// <param name="order"></param>
        /// <returns></returns>
        public bool Select(Contracts.Order order) {
            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id, OrderId FROM Order WHERE ";
                if (order.Id != 0)
                    sql += " Id = " + order.Id;
                else if (!string.IsNullOrEmpty(order.OrderId.ToString()))
                    sql += " OrderId = '" + order.OrderId + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        order.Id = (int)dataReader["Id"];
                        order.DeliveryDate = (DateTime)dataReader["DeliveryDate"];
                        order.Giftwrapping = (string)dataReader["Giftwrapping"];
                        order.OrderDate = (DateTime)dataReader["OrderDate"];
                        order.OrderId = (int)dataReader["OrderId"];
                        order.ShippingCity = (string)dataReader["ShippingCity"];
                        order.ShippingStreet = (string)dataReader["ShippingStreet"];
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
