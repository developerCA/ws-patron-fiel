using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WcfService1.FielPatronService.Entities
{
    [DataContract]
    public class ClienteEntity
    {
        [DataMember]
        String Codigo_Cliente;
        [DataMember]
        String Identificacion;
        [DataMember]
        String Nombres;

        public String PNombres
        {
            get { return Nombres; }
            set { Nombres = value; }
        }

        public String PIdentificacion
        {
            get { return Identificacion; }
            set { Identificacion = value; }
        }    

        public String PCodigo_Cliente
        {
            get { return Codigo_Cliente; }
            set { Codigo_Cliente = value; }
        }

        public ClienteEntity() { }

        public ClienteEntity(string identificacion, string codigo_cliente, string nombres) {
            this.Identificacion = identificacion;
            this.Codigo_Cliente = codigo_cliente;
            this.Nombres = nombres;
        }
            
        
    }
}
