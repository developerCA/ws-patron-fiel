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
    public class RangoDAL
    {
        public static string NonBlankValueOf(string strTestString)
        {
            if (String.IsNullOrEmpty(strTestString))
                return " ";
            else
                return strTestString.Trim();
        }
        public static List<RangoEntity> GetAll()
        {
            List<RangoEntity> list = new List<RangoEntity>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT Id,IpInicio,IpFinal,Sesiones,ClienteId,SuscripcionId,SesionesUsadas,Clave,Mail,Activo FROM Rango";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadRango(reader));
                }

                conn.Close();
            }

            return list;
        }

        public static RangoEntity GetById(string id)
        {
            RangoEntity rango = null;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT Id,IpInicio,IpFinal,Sesiones,ClienteId,SuscripcionId,SesionesUsadas,Clave,Mail,Activo FROM Rango where Id = @rangoId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("rangoId", NonBlankValueOf(id));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rango = LoadRango(reader);
                }
                conn.Close();
            }

            return rango;

        }

        public static  RangoEntity GetByClienteId(string id)
        {
            

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                RangoEntity rango = new RangoEntity();
                conn.Open();

                string sql = @"SELECT Id,IpInicio,IpFinal,Sesiones,ClienteId,SuscripcionId,SesionesUsadas,Clave,Mail,Activo FROM Rango where ClienteId = @rangoId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("rangoId", NonBlankValueOf(id));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    rango = LoadRango(reader);
                }
                conn.Close();
                return rango;
            }

           

        }

        public static bool Exists(string id)
        {
            int nrorecord = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT Count(*) FROM Rango where Id = @rangoId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("rangoId", NonBlankValueOf(id));

                nrorecord = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }

            return nrorecord > 0;

        }

        public static int Ultimo()
        {
            int nrorecord = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT max(Id) FROM Rango";

                SqlCommand cmd = new SqlCommand(sql, conn);
                nrorecord = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }

            return nrorecord ;

        }




        public static Mensaje Create(RangoEntity rango,UsuarioEntity usuario)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    string sql = @"INSERT INTO Rango(IpInicio,IpFinal,Sesiones,ClienteId,SuscripcionId,SesionesUsadas,Clave,Mail,Activo) 
                                    VALUES (@IpInicio, @IpFinal, @Sesiones,@ClienteId,@SuscripcionId,@SesionesUsadas,@Clave,@Mail,@Activo)
                              ";

                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@IpInicio", NonBlankValueOf(rango.PIpInicio));
                    cmd.Parameters.AddWithValue("@IpFinal", NonBlankValueOf(rango.PIpFinal));
                    cmd.Parameters.AddWithValue("@Sesiones", rango.PSesiones);
                    cmd.Parameters.AddWithValue("@ClienteId", rango.PClienteId);
                    cmd.Parameters.AddWithValue("@SuscripcionId", rango.PSuscripcionId);
                    cmd.Parameters.AddWithValue("@SesionesUsadas", rango.PSesionesUsadas);
                    cmd.Parameters.AddWithValue("@Clave", NonBlankValueOf(rango.PClave));
                    cmd.Parameters.AddWithValue("@Mail", NonBlankValueOf(rango.PMail));
                    cmd.Parameters.AddWithValue("@Activo", rango.PActivo);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    sql = @"INSERT INTO UsuarioRango(Id,RangoId,Clave) 
                                    VALUES (@Id, @RangoId, @Clave)
                              ";

                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand(sql, conn);

                    cmd2.Parameters.AddWithValue("@Id", NonBlankValueOf(usuario.PID));
                    cmd2.Parameters.AddWithValue("@RangoId", Convert.ToString(Ultimo()));
                    cmd2.Parameters.AddWithValue("@Clave", rango.PClave);
                    cmd2.ExecuteNonQuery();
                    conn.Close();

                    return new Mensaje(true, "Se creo Rango correctamente");
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
          
           
        }

    

        public static Mensaje Update(RangoEntity rango, UsuarioEntity usuario)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    conn.Open();

                    string sql = @"UPDATE Rango SET  
                                    IpInicio=@IpInicio,
                                    IpFinal=@IpFinal,
                                    Sesiones=@Sesiones,
                                    ClienteId=@ClienteId,
                                    SuscripcionId=@SuscripcionId,
                                    SesionesUsadas=@SesionesUsadas,
                                    Clave=@Clave,
                                    Mail=@Mail,
                                    Activo=@Activo
                                    WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@IpInicio", NonBlankValueOf(rango.PIpInicio));
                    cmd.Parameters.AddWithValue("@IpFinal", NonBlankValueOf(rango.PIpFinal));

                    cmd.Parameters.AddWithValue("@Sesiones",rango.PSesiones);
                    cmd.Parameters.AddWithValue("@ClienteId", rango.PClienteId);
                    cmd.Parameters.AddWithValue("@SuscripcionId", rango.PSuscripcionId);
                    cmd.Parameters.AddWithValue("@SesionesUsadas", rango.PSesionesUsadas);

                    cmd.Parameters.AddWithValue("@Clave", NonBlankValueOf(rango.PClave));
                    cmd.Parameters.AddWithValue("@Mail", NonBlankValueOf(rango.PMail));
                    cmd.Parameters.AddWithValue("@Activo", rango.PActivo);
                    cmd.Parameters.AddWithValue("@Id", rango.PRangoId);
                    cmd.ExecuteNonQuery();

                    sql = @"UPDATE UsuarioRango SET  
                                    RangoId = @RangoId,
                                    Clave=@Clave
                                    where Id=@Id ";

                    SqlCommand cmd2 = new SqlCommand(sql, conn);

                    cmd2.Parameters.AddWithValue("@RangoId", NonBlankValueOf(rango.PRangoId));
                    cmd2.Parameters.AddWithValue("@Clave", NonBlankValueOf(rango.PClave));

                    cmd2.Parameters.AddWithValue("@Id", NonBlankValueOf(usuario.PID));
                  
                    cmd2.ExecuteNonQuery();

                    conn.Close();
                    return new Mensaje(true, "Se actualizo Rango correctamente");

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
                return new Mensaje(false, "Error Actualizar Rango");

            }

           
        }


        public static RangoEntity LoadRango(SqlDataReader reader)
        {
            RangoEntity rango = new RangoEntity();
            rango.PRangoId = Convert.ToString(reader["Id"]);
            rango.PIpInicio = Convert.ToString(reader["IpInicio"]);
            rango.PIpFinal = Convert.ToString(reader["IpFinal"]);
            rango.PSesiones = Convert.ToInt32(reader["Sesiones"]);
            rango.PClienteId = Convert.ToInt32(reader["ClienteId"]);
            rango.PSuscripcionId = Convert.ToInt32(reader["SuscripcionId"]);
            rango.PSesionesUsadas = Convert.ToInt32(reader["SesionesUsadas"]);
            rango.PClave = Convert.ToString(reader["Clave"]);
            rango.PMail = Convert.ToString(reader["Mail"]);
            rango.PActivo= Convert.ToInt32(reader["Activo"]);


            return rango;
        }

    }
}