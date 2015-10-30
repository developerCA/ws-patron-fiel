using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfService1.FielPatronService.Entities;
using WcfService1.FielPatronService.DataAccess;

namespace WcfService1.FielPatronService.BusinessLayer
{
    public class SuscripcionBO
    {
        public static List<SuscripcionEntity> GetAll()
        {
            return SuscripcionDAL.GetAll();
        }

        public static SuscripcionEntity GetById(string id)
        {

            return SuscripcionDAL.GetById(id);
        }

        public  Mensaje Save(SuscripcionEntity suscription)
        {

            if (SuscripcionDAL.Exists(suscription.PCodigoSuscripcion))
            {
                if (SuscripcionDAL.ExistsOriginal(suscription.PCodigoSuscripcion) == false)
                {
                    SuscripcionDAL.CreateSuscription();
                }
                return SuscripcionDAL.Update(suscription);

            }
            else
            {
                SuscripcionDAL.Create(suscription);
                return new Mensaje();
            }

        }

    }
}