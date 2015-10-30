using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfService1.FielPatronService.Entities;
using WcfService1.FielPatronService.DataAccess;

namespace WcfService1.FielPatronService.BusinessLayer
{
    public class ClienteBO
    {
        public static List<ClienteEntity> GetAll()
        {
            return ClienteDAL.GetAll();
        }

        public static ClienteEntity GetById(string id)
        {

            return ClienteDAL.GetById(id);
        }

        public  Mensaje Save(ClienteEntity customer)
        {

            if (ClienteDAL.Exists(customer.PCodigo_Cliente))

            {
                if (ClienteDAL.ExistsOriginal(customer.PCodigo_Cliente) == false)
                {
                    ClienteDAL.CreateCliente();
                }
                return ClienteDAL.Update(customer);
                 
            }
            else
            {
                ClienteDAL.Create(customer);
                return new Mensaje();
            }

        }
    }
}