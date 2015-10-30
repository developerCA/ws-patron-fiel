using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfService1.FielPatronService.Entities
{
    public class SuscripcionEntity
    {
        String CodigoSuscripcion;
          String CodigoCliente;
        DateTime FechaSuscripcion;
          DateTime FechaFin;
          int Estado;

        public String PCodigoSuscripcion
        {
            get { return CodigoSuscripcion; }
            set { CodigoSuscripcion = value; }
        }
      

        public String PCodigoCliente
        {
            get { return CodigoCliente; }
            set { CodigoCliente = value; }
        }
        

        public DateTime PFechaSuscripcion
        {
            get { return FechaSuscripcion; }
            set { FechaSuscripcion = value; }
        }
      

        public DateTime PFechaFin
        {
            get { return FechaFin; }
            set { FechaFin = value; }
        }
        

        public int PEstado
        {
            get { return Estado; }
            set { Estado = value; }
        }

        public SuscripcionEntity() { }

        public SuscripcionEntity(string codigosuscripcion, string codigocliente, DateTime fechasuscripcion,DateTime fechafin, int estado) {
            this.CodigoSuscripcion = codigosuscripcion;
            this.CodigoCliente = codigocliente;
            this.FechaSuscripcion = fechasuscripcion;
            this.FechaFin = fechafin;
            this.Estado = estado;
        }

       
    }
}
