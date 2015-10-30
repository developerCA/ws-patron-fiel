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
    public class SuscripcionDAL
    {

        public static string NonBlankValueOf(string strTestString)
        {
            if (String.IsNullOrEmpty(strTestString))
                return " ";
            else
                return strTestString.Trim();
        }
        public static List<SuscripcionEntity> GetAll()
        {
            List<SuscripcionEntity> list = new List<SuscripcionEntity>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT CODIGO,ICLIENTE,FECHA_SUSCRIPCION,FECHA_FIN,ESTADO FROM SUSCRIPCION_TMP_GLOBAL ORDER BY CODIGO";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadSuscription(reader));
                }
                conn.Close();
            }

            return list;
        }

        public static SuscripcionEntity GetById(string id)
        {
            SuscripcionEntity suscription = null;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT CODIGO,ICLIENTE,FECHA_SUSCRIPCION,FECHA_FIN,ESTADO
                                FROM SUSCRIPCION_TMP_GLOBAL 
                                WHERE CODIGO = @suscriptionId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("suscriptionId", NonBlankValueOf(id));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    suscription = LoadSuscription(reader);
                }
                conn.Close();
            }

            return suscription;

        }

        public static bool Exists(string id)
        {
            int nrorecord = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT Count(*)
                                FROM SUSCRIPCION_TMP_GLOBAL
                               WHERE CODIGO = @suscriptionId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("suscriptionId", NonBlankValueOf(id));

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
                                FROM SUSCRIPCION
                               WHERE Id = @suscriptionId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("suscriptionId", NonBlankValueOf(id));

                nrorecord = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }

            return nrorecord > 0;

        }

        public static Mensaje Create(SuscripcionEntity suscription)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    string sql = @"INSERT INTO SUSCRIPCION_TMP_GLOBAL (CODIGO,ICLIENTE,FECHA_SUSCRIPCION,FECHA_FIN,ESTADO) 
                                    VALUES (@codigo, @icliente, @fechasuscripcion,@fechafin,@estado)
                              ";

                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@codigo", NonBlankValueOf(suscription.PCodigoSuscripcion));
                    cmd.Parameters.AddWithValue("@icliente", NonBlankValueOf(suscription.PCodigoCliente));
                    cmd.Parameters.AddWithValue("@fechasuscripcion", suscription.PFechaSuscripcion);
                    cmd.Parameters.AddWithValue("@fechafin", suscription.PFechaFin);
                    cmd.Parameters.AddWithValue("@estado", suscription.PEstado);
                    cmd.ExecuteNonQuery();
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
                return new Mensaje(false, "Error Crear Suscripcion");

            }

            

                CreateSuscription();

                return new Mensaje(true, "Se creo Suscripcion correctamente"); 
        }

        public static void CreateSuscription()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    string sql = @" INSERT INTO Suscripcion (Id,ClienteId,FechaInicio,FechaFin,Activo,Sesiones,SesionesUsadas,AreasLimitadas) select cast(g.CODIGO as int),cast(g.ICLIENTE as int ),CONVERT(datetime, g.FECHA_SUSCRIPCION, 120) ,CONVERT(datetime, g.FECHA_FIN, 120),g.ESTADO ,CAST(0 AS INT),CAST(0 AS CHAR(1)) , 0 from SUSCRIPCION_TMP_GLOBAL g where ISNUMERIC(g.CODIGO)=1 AND ISNUMERIC(g.ICLIENTE)=1 AND not exists (select s.Id from Suscripcion  s where s.Id=g.CODIGO )";
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
                sw.WriteLine("Suscripcion Original " + ex.ToString());
                //Close the file
                sw.Close();


            }
        }

        public static Mensaje Update(SuscripcionEntity suscription)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    conn.Open();

                    string sql = @"UPDATE SUSCRIPCION_TMP_GLOBAL 
                               SET FECHA_SUSCRIPCION=@fechasuscripcion,
                                   FECHA_FIN=@fechafin,
                                   ESTADO= @estado 
                               WHERE CODIGO = @codigo";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@codigo", NonBlankValueOf(suscription.PCodigoSuscripcion));
                    cmd.Parameters.AddWithValue("@icliente", NonBlankValueOf(suscription.PCodigoCliente));
                    cmd.Parameters.AddWithValue("@fechasuscripcion", suscription.PFechaSuscripcion);
                    cmd.Parameters.AddWithValue("@fechafin", suscription.PFechaFin);
                    cmd.Parameters.AddWithValue("@estado", suscription.PEstado);

                    cmd.ExecuteNonQuery();

                    sql = @"UPDATE SUSCRIPCION
                               SET  FechaInicio=@fechasuscripcion,
                                   FechaFin=@fechafin,
                                   Activo= @estado 
                               WHERE Id = @codigo";

                    SqlCommand cmd2 = new SqlCommand(sql, conn);

                    cmd2.Parameters.AddWithValue("@codigo", NonBlankValueOf(suscription.PCodigoSuscripcion));
                    cmd2.Parameters.AddWithValue("@icliente", NonBlankValueOf(suscription.PCodigoCliente));
                    cmd2.Parameters.AddWithValue("@fechasuscripcion", suscription.PFechaSuscripcion);
                    cmd2.Parameters.AddWithValue("@fechafin", suscription.PFechaFin);
                    cmd2.Parameters.AddWithValue("@estado", suscription.PEstado);

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
                return new Mensaje(false, "Error Actualizar Suscripcion");

            }

            return new Mensaje(true, "Se actualizo Suscripcion correctamente");
        }


        private static SuscripcionEntity LoadSuscription(IDataReader reader)
        {
            SuscripcionEntity suscription = new SuscripcionEntity();
            suscription.PCodigoSuscripcion = Convert.ToString(reader["CODIGO"]);
            suscription.PCodigoCliente = Convert.ToString(reader["ICLIENTE"]);
            suscription.PFechaSuscripcion = Convert.ToDateTime(reader["FECHA_SUSCRIPCION"]);
            suscription.PFechaFin = Convert.ToDateTime(reader["FECHA_FIN"]);
            suscription.PEstado = Convert.ToInt32(reader["ESTADO"]);
            return suscription;
        }
    }
}