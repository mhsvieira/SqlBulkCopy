using ApiZero.Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ApiZero.Repository
{
    public class DAL_BRQ
    {
        string ConnectionString = string.Empty;

        // Método construtor da classe, definindo a string de conexão
        public DAL_BRQ()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["BRQBDStrConn"].ConnectionString;
        }

        // Chamada da gravação registros em lote no banco
        public bool GravarLoteRegistrosBanco(List<Quota> quotas)
        {
            try
            {
                var dataTable = ConverteListaToDataTable(quotas);

                var gravaLote = GravaLoteRegistros(dataTable, "Quotas");

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " - Erro método GravarLoteRegistrosBanco");
            }
        }

        // Converte List<Quota> para um DataTable
        public DataTable ConverteListaToDataTable<T>(List<T> quotas)
        {
            try
            {
                DataTable dataTable = new DataTable(typeof(T).Name);

                //busca todas as propriedades da classe Quota
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo prop in Props)
                {
                    //configura os nomes das colunas e os nomes das propriedades
                    dataTable.Columns.Add(prop.Name);
                }

                foreach (T quota in quotas)
                {
                    var values = new object[Props.Length];

                    for (int i = 0; i < Props.Length; i++)
                    {
                        //insere os valores das propriedades no datatable
                        values[i] = Props[i].GetValue(quota, null);
                    }

                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " - Erro método ConverteListaToDataTable");
            }
        }

        // grava registros em lote no banco
        public bool GravaLoteRegistros(DataTable dataTable, string tablename)
        {
            try
            {
                if (dataTable.Rows.Count > 0)
                {
                    using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                        {
                            sqlBulkCopy.DestinationTableName = tablename;
                            connection.Open();
                            sqlBulkCopy.WriteToServer(dataTable);
                            connection.Close();
                        }
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " - Erro método GravaLoteRegistros");
            }
        }

        // Recupera registros recém gravados para geração do arquivo de saída, filtrando pelo nome do arquivo que será gerado
        public List<Quota> RecuperarRegistrosBanco(string pathArquivo)
        {
            try
            {
                List<Quota> lstQuotas = new List<Quota>();

                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT ID, NUMBER, AMOUNT, STARTDATE, FINALDATE, OWNERNAME, USERNAME, [PERCENTAGE], MAILED FROM QUOTAS WHERE PATHFILENAME = @PATHFILENAME";
                        command.Parameters.AddWithValue("@PATHFILENAME", pathArquivo);

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Quota quota = new Quota()
                            {
                                Id = (int)reader["ID"],
                                Number = reader["NUMBER"].ToString(),
                                Amount = reader["AMOUNT"].ToString(),
                                StartDate = reader["STARTDATE"].ToString(),
                                FinalDate = reader["FINALDATE"].ToString(),
                                OwnerName = reader["OWNERNAME"].ToString(),
                                Username = reader["USERNAME"].ToString(),
                                Percentage = reader["PERCENTAGE"].ToString(),
                                Mailed = reader["MAILED"].ToString()
                            };

                            lstQuotas.Add(quota);
                        }
                    }

                    connection.Close();
                }

                return lstQuotas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " - Erro método RecuperarRegistrosBanco");
            }
        }
    }
}
