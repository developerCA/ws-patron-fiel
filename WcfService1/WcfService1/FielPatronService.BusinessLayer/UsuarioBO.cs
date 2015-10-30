using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfService1.FielPatronService.Entities;
using WcfService1.FielPatronService.DataAccess;

namespace WcfService1.FielPatronService.BusinessLayer
{
    public class UsuarioBO
    {
        public static List<UsuarioEntity> GetAll()
        {
            return UsuarioDAL.GetAll();
        }

        public static UsuarioEntity GetById(string id)
        {

            return UsuarioDAL.GetById(id);
        }

        public  Mensaje Save(UsuarioEntity user)
        {

            if (UsuarioDAL.Exists(user.PID))
            {
                if (UsuarioDAL.ExistsOriginal(user.PID) == false) {
                    UsuarioDAL.CreateUser();
                }
                return UsuarioDAL.Update(user);

            }
            else
            {
                UsuarioDAL.Create(user);
                return new Mensaje();
            }

        }
    }
}