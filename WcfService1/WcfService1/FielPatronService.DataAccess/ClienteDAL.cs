using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WcfService1.FielPatronService.Entities;
using System.IO;

namespace WcfService1.FielPatronService.DataAccess
{
    public class ClienteDAL
    {
        public static string NonBlankValueOf(string strTestString)
        {
            if (String.IsNullOrEmpty(strTestString))
                return " ";
            else
                return strTestString.Trim();
        }
        public static List<ClienteEntity> GetAll()
        {
            List<ClienteEntity> list = new List<ClienteEntity>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT CODIGO_CLIENTE_GLOBAL,IDENTIFICACION,nombres FROM CLIENTE_TMP_GLOBAL ORDER BY nombres";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadCustomer(reader));
                }

                conn.Close();
            }

            return list;
        }

        public static ClienteEntity GetById(string id)
        {
            ClienteEntity customer = null;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT CODIGO_CLIENTE_GLOBAL,IDENTIFICACION,nombres
                                FROM CLIENTE_TMP_GLOBAL 
                                WHERE CODIGO_CLIENTE_GLOBAL = @customerId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("customerId", NonBlankValueOf(id));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    customer = LoadCustomer(reader);
                }
                conn.Close();
            }

            return customer;

        }

        public static bool Exists(string id)
        {
            int nrorecord = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT Count(*)
                                FROM CLIENTE_TMP_GLOBAL 
                                WHERE CODIGO_CLIENTE_GLOBAL = @customerId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("customerId", NonBlankValueOf(id));

                nrorecord = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }

            return nrorecord > 0;

        }


        public static bool ExistsOriginal(string id)
        {
            int nrorecord = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT Count(*)
                                FROM CLIENTE 
                                WHERE Id = @customerId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("customerId", id);

                nrorecord = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }

            return nrorecord > 0;

        }

        public static Mensaje Create(ClienteEntity customer)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    string sql = @"INSERT INTO CLIENTE_TMP_GLOBAL (CODIGO_CLIENTE_GLOBAL,IDENTIFICACION,nombres) 
                                    VALUES (@codigocliente, @identificacion, @nombres)
                              ";

                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@codigocliente", NonBlankValueOf(customer.PCodigo_Cliente));
                    cmd.Parameters.AddWithValue("@identificacion", NonBlankValueOf(customer.PIdentificacion));
                    cmd.Parameters.AddWithValue("@nombres", NonBlankValueOf(customer.PNombres));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException ex) {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("C:\\Log.txt");
              //Write a second line of text
                sw.WriteLine(ex.ToString());
                //Close the file
                sw.Close();
                return new Mensaje(false, "Error Crear Cliente");

            }
            CreateCliente();
            return new Mensaje(true,"Se creo Cliente correctamente");
        }

        public static void CreateCliente()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    string sql = @"INSERT INTO Cliente(Id,RUC,Nombre,Direccion,Telefono,Sesiones,Activo,SesionActiva,CODIGO_CLIENTE_GLOBAL) ( SELECT cast(g.CODIGO_CLIENTE_GLOBAL  as int ) ,ltrim(rtrim(g.IDENTIFICACION)) , ltrim(rtrim(g.Nombres)), null,null,CAST (10000 AS INT), 1 , 0 ,g.CODIGO_CLIENTE_GLOBAL FROM CLIENTE_TMP_GLOBAL G  WHERE ISNUMERIC(g.CODIGO_CLIENTE_GLOBAL)=1 AND not exists(select * from Cliente c where C.CODIGO_CLIENTE_GLOBAL= G.CODIGO_CLIENTE_GLOBAL ))";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("C:\\Log.txt");
                //Write a second line of text
                sw.WriteLine("Cliente Original " + ex.ToString());
                //Close the file
                sw.Close();

            }
        }

        public static Mensaje Update(ClienteEntity customer)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    conn.Open();

                    string sql = @"UPDATE CLIENTE_TMP_GLOBAL SET  
                                           nombres=@nombres
                                    WHERE CODIGO_CLIENTE_GLOBAL = @codigocliente";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@codigocliente", NonBlankValueOf(customer.PCodigo_Cliente));
                    cmd.Parameters.AddWithValue("@nombres", NonBlankValueOf(customer.PNombres));
                    cmd.ExecuteNonQuery();


                    sql = @"UPDATE Cliente SET  
                                           Nombre=@nombres
                                    WHERE Id = @codigocliente";

                    SqlCommand cmd2 = new SqlCommand(sql, conn);

                    cmd2.Parameters.AddWithValue("@codigocliente", NonBlankValueOf(customer.PCodigo_Cliente));
                    cmd2.Parameters.AddWithValue("@nombres", NonBlankValueOf(customer.PNombres));
                    cmd2.ExecuteNonQuery();

                    conn.Close();

                }
            }
            catch (SqlException ex)
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("C:\\Log.txt");
                //Write a second line of text
                sw.WriteLine(ex.ToString());
                //Close the file
                sw.Close();
                return new Mensaje(false, "Error Actualizar Cliente");

            }

            return new Mensaje(true, "Se actualizo Cliente correctamente");
        }


        private static ClienteEntity LoadCustomer(IDataReader reader)
        {
            ClienteEntity customer = new ClienteEntity();

            customer.PCodigo_Cliente = Convert.ToString(reader["CODIGO_CLIENTE_GLOBAL"]);
            customer.PIdentificacion = Convert.ToString(reader["IDENTIFICACION"]);
            customer.PNombres = Convert.ToString(reader["nombres"]);
           

            return customer;
        }

    }
}