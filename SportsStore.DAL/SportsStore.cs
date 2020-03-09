using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SportsStore.DAL {
    public class SportsStore {
        string _connStr = ConfigurationManager.ConnectionStrings["StudentsConn"].ToString();

        public void Update(Students.Contracts.Student student) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"UPDATE Cursus SET Naam = " + student.Naam + " WHERE Id = '" + student.Id + "'";
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

        public int Insert(Students.Contracts.Student student) {
            Int32 id = 0;
            string sql = "INSERT INTO Cursus ( Naam ) VALUES ( @Naam ); "
                + "SELECT CAST(scope_identity() AS int);";

            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Naam", SqlDbType.VarChar);
                try {
                    conn.Open();
                    cmd.Parameters["@Naam"].Value = student.Naam;
                    id = (Int32)cmd.ExecuteScalar();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public int Insert(List<Students.Contracts.Student> students) {
            Int32 id = 0;
            string sql = "INSERT INTO Cursus ( Naam ) VALUES ( @Naam ); "
                + "SELECT CAST(scope_identity() AS int);";

            using (SqlConnection conn = new SqlConnection(_connStr)) {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Naam", SqlDbType.VarChar);
                try {
                    conn.Open();
                    foreach (var student in students) {
                        cmd.Parameters["@Naam"].Value = student.Naam;
                        id = (Int32)cmd.ExecuteScalar();
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return id;
        }

        public void Delete(Students.Contracts.Student student) {
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = $"DELETE FROM Cursus WHERE Id = " + student.Id;
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

        public bool Select(Students.Contracts.Student student) {
            int count = 0;
            SqlConnection conn = new SqlConnection(_connStr);
            try {
                conn.Open();
                string sql = "SELECT Id Naam FROM Cursus WHERE Id = " + student.Id;

                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        student.Id = (int)dataReader["Id"];
                        student.Naam = (string)dataReader["Naam"];
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
