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
    public class UsuarioDAL
    {
        public static string NonBlankValueOf(string strTestString)
        {
            if (String.IsNullOrEmpty(strTestString))
                return " ";
            else
                return strTestString.Trim();
        }
        public static List<UsuarioEntity> GetAll()
        {
            List<UsuarioEntity> list = new List<UsuarioEntity>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT USUARIO , SUSCRIPCION , PWD , NOMBRE , APELLIDO , EMAIL , ESTADO , producto FROM USUARIOS_TMP_GLOBAL ORDER BY USUARIO";
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadUser(reader));
                }
                conn.Close();
            }

            return list;
        }

        public static UsuarioEntity GetById(string id)
        {
            UsuarioEntity user = null;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT USUARIO , SUSCRIPCION , PWD , NOMBRE , APELLIDO , EMAIL , ESTADO , producto
                                FROM USUARIOS_TMP_GLOBAL 
                                WHERE USUARIO = @usuario";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("usuario", NonBlankValueOf(id));

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = LoadUser(reader);
                }
                conn.Close();
            }

            return user;

        }

        public static bool Exists(string id)
        {
            int nrorecord = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT Count(*)
                                FROM USUARIOS_TMP_GLOBAL 
                                WHERE USUARIO = @usuario";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("usuario", NonBlankValueOf(id));

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
                                FROM USUARIOCLIENTE 
                                WHERE Id = @usuario";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("usuario", NonBlankValueOf(id));

                nrorecord = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }

            return nrorecord > 0;

        }

        public static Mensaje Create(UsuarioEntity user)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {


                    int j;
                    bool result = Int32.TryParse(user.PCodigoSuscripcion, out j);
                    if (true == result)
                        result = true;
                    else
                        result = false;
                    if (result == true)
                    {
                        string sql = @"INSERT INTO USUARIOS_TMP_GLOBAL (USUARIO,SUSCRIPCION, PWD, NOMBRE,APELLIDO,EMAIL,ESTADO,producto) 
                                    VALUES (@usuario, @suscripcion, @pwd,@nombre,@apellido,@email,@estado,@producto)
                              ";

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        conn.Open();
                        cmd.Parameters.AddWithValue("@usuario", NonBlankValueOf(user.PID));
                        cmd.Parameters.AddWithValue("@suscripcion", NonBlankValueOf(user.PCodigoSuscripcion));
                        cmd.Parameters.AddWithValue("@pwd", NonBlankValueOf(user.PClave));
                        cmd.Parameters.AddWithValue("@nombre", NonBlankValueOf(user.PNombre));
                        cmd.Parameters.AddWithValue("@apellido", NonBlankValueOf(user.PApellido));
                        cmd.Parameters.AddWithValue("@email", NonBlankValueOf(user.PEmail));
                        cmd.Parameters.AddWithValue("@estado", user.PEstado);
                        cmd.Parameters.AddWithValue("@producto", user.PProducto);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        CreateUser();

                        return new Mensaje(true, "Se crearon correctamente");
                    }

                    else
                    {
                        return new Mensaje(false, "Existio un error, campos deben ser enteros");
                    }
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
                return new Mensaje(false, "Error Crear Usuario");

            }

        }
        public static void CreateUser()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    string sql = @"INSERT INTO UsuarioCliente (Id,Nombre,Apellido,Clave,Sesiones,Activo,SuscripcionId,ClienteId,SesionesUsadas,Mail,NombreLogin,CampoConvivencia,EsAdministradorS,EsAdministradorC,publicidad,ProductoId) select distinct(ltrim(g.USUARIO)),ltrim(g.NOMBRE),ltrim(g.APELLIDO),cast(ltrim(g.PWD) as varchar),CAST( 1000 as int),1,cast(g.SUSCRIPCION as int),(select  distinct  cast(s.ClienteId as int)from Suscripcion s where  s.Id=g.suscripcion) as ClienteId ,CAST(0 AS INT), CAST(g.EMAIL AS VARCHAR(200)) ,g.USUARIO ,g.USUARIO , 0 , 0  , CAST(4 AS INT),producto from USUARIOS_TMP_GLOBAL  g where  Not exists (select distinct(i.CampoConvivencia) from UsuarioCliente i where ltrim(rtrim(g.USUARIO))=rtrim(ltrim(i.CampoConvivencia))) ";
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
                sw.WriteLine("Usuario Original "+ex.ToString());
                //Close the file
                sw.Close();
               

            }
        }

        public static Mensaje Update(UsuarioEntity user)
        {
            try {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["default"].ToString()))
                {
                    conn.Open();


                    string sql = @"UPDATE USUARIOS_TMP_GLOBAL 
                                SET 
                                    pwd=@pwd,
                                    nombre=@nombre,
                                    apellido=@apellido,
                                    email=@email,
                                    estado=@estado,
                                    producto=@producto
                                    WHERE USUARIO = @usuario";

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@usuario", NonBlankValueOf(user.PID));
                    cmd.Parameters.AddWithValue("@suscripcion", NonBlankValueOf(user.PCodigoSuscripcion));
                    cmd.Parameters.AddWithValue("@pwd", NonBlankValueOf(user.PClave));
                    cmd.Parameters.AddWithValue("@nombre", NonBlankValueOf(user.PNombre));
                    cmd.Parameters.AddWithValue("@apellido", NonBlankValueOf(user.PApellido));
                    cmd.Parameters.AddWithValue("@email", NonBlankValueOf(user.PEmail));
                    cmd.Parameters.AddWithValue("@estado", user.PEstado);
                    cmd.Parameters.AddWithValue("@producto", user.PProducto);
                    cmd.ExecuteNonQuery();


                    //sql = @"UPDATE UsuarioCliente 
                    //            SET SuscripcionId=@suscripcion,
                    //                ClienteId=@clienteid,
                    //                Clave=@pwd,
                    //                Nombre=@nombre,
                    //                Apellido=@apellido,
                    //                Mail=@email,
                    //                Activo=@estado,
                    //                ProductoId=@producto
                    //                WHERE Id = @usuario";


                    sql = @"UPDATE UsuarioCliente 
                                SET  Clave=@pwd,
                                    Nombre=@nombre,
                                    Apellido=@apellido,
                                    Mail=@email,
                                    Activo=@estado,
                                    ProductoId=@producto
                                    WHERE Id = @usuario";

                    SqlCommand cmd2 = new SqlCommand(sql, conn);

                    cmd2.Parameters.AddWithValue("@usuario", NonBlankValueOf(user.PID));
                    cmd2.Parameters.AddWithValue("@clienteid", NonBlankValueOf(user.PClienteID));
                    cmd2.Parameters.AddWithValue("@suscripcion", NonBlankValueOf(user.PCodigoSuscripcion));
                    cmd2.Parameters.AddWithValue("@pwd", NonBlankValueOf(user.PClave));
                    cmd2.Parameters.AddWithValue("@nombre", NonBlankValueOf(user.PNombre));
                    cmd2.Parameters.AddWithValue("@apellido", NonBlankValueOf(user.PApellido));
                    cmd2.Parameters.AddWithValue("@email", NonBlankValueOf(user.PEmail));
                    cmd2.Parameters.AddWithValue("@estado", user.PEstado);
                    cmd2.Parameters.AddWithValue("@producto", user.PProducto);
                    cmd2.ExecuteNonQuery();
                    conn.Close();

                }

                return new Mensaje(true, "actualizaron");
            }
            catch (SqlException ex)
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("C:\\Log.txt");
                //Write a second line of text
                sw.WriteLine(ex.ToString());
                //Close the file
                sw.Close();
                return new Mensaje(false, "Error Actualizar Usuario");

            }
        }


        private static UsuarioEntity LoadUser(IDataReader reader)
        {
            UsuarioEntity user = new UsuarioEntity();

            user.PID = Convert.ToString(reader["USUARIO"]);
            user.PCodigoSuscripcion = Convert.ToString(reader["SUSCRIPCION"]);
            user.PClave = Convert.ToString(reader["PWD "]);
            user.PNombre = Convert.ToString(reader["NOMBRE"]);
            user.PApellido = Convert.ToString(reader["APELLIDO"]);
            user.PEmail = Convert.ToString(reader["EMAIL"]);
            user.PEstado = Convert.ToInt32(reader["ESTADO"]);
            user.PProducto = Convert.ToInt32(reader["producto"]);
          
            return user;
        }
    }
}