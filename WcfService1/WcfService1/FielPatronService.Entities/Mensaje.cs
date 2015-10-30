using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfService1.FielPatronService.Entities
{
    [DataContract]
    public class Mensaje
    {
        [DataMember]
        Boolean Estado;
        [DataMember]
        String Descripcion;
        public Boolean PEstado
        {
            get { return Estado; }
            set { Estado = value; }
        }

        public String PDescripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public Mensaje() { }

        public Mensaje(Boolean estado, string descripcion) {

            this.Estado = estado;
            this.Descripcion = descripcion;
        
        }
    }
}