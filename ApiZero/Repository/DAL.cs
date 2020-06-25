using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ApiZero.Repository
{
    public class DAL
    {
        private string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BDStrConn"].ConnectionString;

        public bool ValidaUsuario(string usuario, string senha)
        {
            bool valida = false;

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
  
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select usuario, senha from usuarios where usuario = @usuario and senha = @senha";
                    command.Parameters.AddWithValue("usuario", usuario);
                    command.Parameters.AddWithValue("senha", senha);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                        valida = true;
                }

                connection.Close();
            }

            return valida;
        }


    }
}