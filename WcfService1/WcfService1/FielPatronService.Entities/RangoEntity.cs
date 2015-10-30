using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WcfService1.FielPatronService.Entities
{
    [DataContract]
    public class RangoEntity
    {
        [DataMember]
        String RangoId;
        [DataMember]
        String IpInicio;
        [DataMember]
        String IpFinal;
        [DataMember]
        int Sesiones;
        [DataMember]
        int ClienteId;
        [DataMember]
        int SuscripcionId;
        [DataMember]
        int SesionesUsadas;
        [DataMember]
        String Clave;
        [DataMember]
        String Mail;
        [DataMember]
        int Activo;

        public string PIpInicio
        {
            get
            {
                return IpInicio;
            }

            set
            {
                IpInicio = value;
            }
        }
        public string PIpFinal
        {
            get
            {
                return IpFinal;
            }

            set
            {
                IpFinal = value;
            }
        }

        public int PClienteId
        {
            get
            {
                return ClienteId;
            }

            set
            {
                ClienteId = value;
            }
        }

        public int PSesiones
        {
            get
            {
                return Sesiones;
            }

            set
            {
                Sesiones = value;
            }
        }

      

        public int PSuscripcionId
        {
            get
            {
                return SuscripcionId;
            }

            set
            {
                SuscripcionId = value;
            }
        }

        public int PSesionesUsadas
        {
            get
            {
                return SesionesUsadas;
            }

            set
            {
                SesionesUsadas = value;
            }
        }

        public string PClave
        {
            get
            {
                return Clave;
            }

            set
            {
                Clave = value;
            }
        }

        public string PMail
        {
            get
            {
                return Mail;
            }

            set
            {
                Mail = value;
            }
        }

        public int PActivo
        {
            get
            {
                return Activo;
            }

            set
            {
                Activo = value;
            }
        }

        public string PRangoId
        {
            get
            {
                return RangoId;
            }

            set
            {
                RangoId = value;
            }
        }

        public RangoEntity() { }

        public RangoEntity(string RangoId,string IpInicio, string IpFinal, int Sesiones, int ClienteId,int SuscripcionId, int SesionesUsadas,string Clave, string Mail, int Activo) {
            this.RangoId = RangoId;
            this.IpInicio = IpInicio;
            this.IpFinal = IpFinal;
            this.Sesiones = Sesiones;
            this.ClienteId = ClienteId;
            this.SuscripcionId = SuscripcionId;
            this.SesionesUsadas = SesionesUsadas;
            this.Clave = Clave;
            this.Mail = Mail;
            this.Activo = Activo;
        }
            
        
    }
}
