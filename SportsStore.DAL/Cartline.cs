using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace SportsStore.DAL
{
    public class CartLine
    {
        #region Fields
        string _connStr = ConfigurationManager.ConnectionStrings["SportsStoreConn"].ToString();
        #endregion

        /// <summary>
        /// Update record into database
        /// </summary>
        /// <param name="cartLine"></param>
        /// <returns></returns>
        public void Update(Contracts.CartLine cartLine)
        {
            SqlConnection conn = new SqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = $"UPDATE CartLine SET Id = '" + cartLine.Id + "', Quantity ='" + cartLine.Quantity + "' WHERE Id = " + cartLine.Id;
                SqlCommand updateCommand = new SqlCommand(sql, conn);
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
        /// <param name="cartLine"></param>
        /// <returns></returns>
        public int Insert(Contracts.CartLine cartLine)
        {
            Int32 id = 0;
            string sql = "INSERT INTO CartLine ( Id, Quantity ) VALUES ( '" + cartLine.Id + "', '" + cartLine.Quantity + "' ); "
                + "SELECT CAST(scope_identity() AS int);";
            // output INSERTED.ID typisch Microsoft SQL Server
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    id = (Int32)cmd.ExecuteScalar();
                    cartLine.Id = id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }
        public int Insert(List<Contracts.CartLine> cartLines)
        {
            Int32 id = 0;
            string sql = "INSERT INTO CartLine ( Id, Quantity ) OUTPUT INSERTED.ID VALUES ( @Id, @Quantity )";
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters.Add("@Quanity", System.Data.SqlDbType.Int);
                try
                {
                    conn.Open();
                    foreach (var cartLine in cartLines)
                    {
                        cmd.Parameters["@Id"].Value = cartLine.Id;
                        cmd.Parameters["@Quantity"].Value = cartLine.Quantity;
                        id = (Int32)cmd.ExecuteScalar();
                        cartLine.Id = id;
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
        /// <param name="cartLine"></param>
        public void Delete(Contracts.CartLine cartLine)
        {
            SqlConnection conn = new SqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = $"DELETE FROM CartLine WHERE CartLineId = " + cartLine.Id;
                SqlCommand updateCommand = new SqlCommand(sql, conn);
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
        /// <param name="cartLine"></param>
        /// <returns></returns>
        public bool Exists(Contracts.CartLine cartLine)
        {
            if (cartLine.Id <= 0 && cartLine.Quantity<=0) return false;

            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = "SELECT Id, Quantity FROM CartLine WHERE ";
                if (cartLine.Id != 0)
                    sql += " Id = " + cartLine.Id;
                else if (!(cartLine.Quantity <= 0))
                    sql += " Quantity = '" + cartLine.Quantity + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cartLine.Id = (int)dataReader[0];
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
        /// <param name="cartLine"></param>
        /// <returns></returns>
        public bool Select(Contracts.CartLine cartLine)
        {
            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = "SELECT Id, Quantity FROM CartLine WHERE ";
                if (cartLine.Id != 0)
                    sql += " Id = " + cartLine.Id;
                else if (!(cartLine.Quantity <= 0))
                    sql += " Quantity = '" + cartLine.Quantity + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cartLine.Id = (int)dataReader["Id"];
                        cartLine.Quantity = (int)dataReader["Quantity"];
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
