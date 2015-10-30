using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfService1.FielPatronService.Entities;
using WcfService1.FielPatronService.DataAccess;

namespace WcfService1.FielPatronService.BusinessLayer
{
    public class RangoBO
    {
        public static List<RangoEntity> GetAll()
        {
            return RangoDAL.GetAll();


        }

        public  RangoEntity GetById(string id)
        {

            return RangoDAL.GetById(id);
        }

        public  RangoEntity GetByClienteId(string id)
        {

            return RangoDAL.GetByClienteId(id);
        }

        public  Mensaje Save(RangoEntity rango,UsuarioEntity usuario)
        {

            if (RangoDAL.Exists(rango.PRangoId))

            {
                
                return RangoDAL.Update(rango,usuario);
                 
            }
            else
            {
                RangoDAL.Create(rango,usuario);
                return new Mensaje();
            }

        }
    }
}