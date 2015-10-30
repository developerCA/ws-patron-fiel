using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfService1.FielPatronService.Entities
{
   public class UsuarioEntity
    {
        String ID;
        String CodigoSuscripcion;
        String ClienteID;
        String Clave;
        String Nombre;
        String Apellido;
        String Email;

        int Estado;
        int Producto;

        public String PID
        {
            get { return ID; }
            set { ID = value; }
        }
      

        public String PCodigoSuscripcion
        {
            get { return CodigoSuscripcion; }
            set { CodigoSuscripcion = value; }
        }

        public String PClienteID
        {
            get { return ClienteID; }
            set { ClienteID = value; }
        }





        public String PClave
        {
            get { return Clave; }
            set { Clave = value; }
        }
      

        public String PNombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
       

        public String PApellido
        {
            get { return Apellido; }
            set { Apellido = value; }
        }
       

        public String PEmail
        {
            get { return Email; }
            set { Email = value; }
        }
       

        public int PEstado
        {
            get { return Estado; }
            set { Estado = value; }
        }
        

        public int PProducto
        {
            get { return Producto; }
            set { Producto = value; }
        }

        public UsuarioEntity() { }

        public UsuarioEntity(string id,string codigosuscripcion,string clienteid, string nombre, string apellido, string email, int estado, int producto) {
            this.ID = id;
            this.CodigoSuscripcion = codigosuscripcion;
            this.ClienteID = clienteid;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Email = email;
            this.Estado = estado;
            this.Producto = producto;
        }
    }
}
