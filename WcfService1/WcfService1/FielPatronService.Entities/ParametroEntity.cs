using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WcfService1.FielPatronService.Entities
{
    [DataContract]
    public class ParametroEntity
    {
        [DataMember]
        String es_RangoIP;
       

        public ParametroEntity() { }

        public ParametroEntity(string es_RangoIP) {
            this.es_RangoIP = es_RangoIP;
            
        }

        public string Es_RangoIP
        {
            get
            {
                return es_RangoIP;
            }

            set
            {
                es_RangoIP = value;
            }
        }
    }
}
