using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService1.FielPatronService.Entities;
using WcfService1.FielPatronService.BusinessLayer;
namespace WcfService1.service
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "FielPatronService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione FielPatronService.svc o FielPatronService.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class FielPatronService : IFielPatronService
    {
        public Mensaje ActivarUsuario(ClienteEntity cliente, SuscripcionEntity suscripcion, List<UsuarioEntity> usuarios,RangoEntity rango, ParametroEntity parametro)
        {

            try
            {
                ClienteBO clienteBO = new ClienteBO();
                Mensaje a = clienteBO.Save(cliente);

                SuscripcionBO suscripcionBO = new SuscripcionBO();
                suscripcion.PEstado = 1;
                Mensaje b = suscripcionBO.Save(suscripcion);
                UsuarioBO usuarioBO = new UsuarioBO();
                int cont = 0;
                Mensaje c = new Mensaje();


                foreach (UsuarioEntity usuario in usuarios)
                {

                    usuario.PEstado = 1;
                    cont = cont + 1;

                    c = usuarioBO.Save(usuario);
                }

                string MensajeRango = "";
                if (parametro.Es_RangoIP.Equals("1"))
                {
                    RangoBO rangoBO = new RangoBO();


                    rango.PClienteId = Convert.ToInt32(cliente.PCodigo_Cliente);
                    rango.PSuscripcionId = Convert.ToInt32(suscripcion.PCodigoSuscripcion);
                    rango.PMail = usuarios[0].PEmail;
                    rango.PActivo = 1;
                    rango.PClave = usuarios[0].PClave;
                    Mensaje d = rangoBO.Save(rango, usuarios[0]);
                    MensajeRango = d.PDescripcion;
                }

                string mensaje = a.PDescripcion + " - " + b.PDescripcion + " - Se " + " " + c.PDescripcion + " " + cont + " Usuarios" + MensajeRango;
                Mensaje respuesta = new Mensaje(true, mensaje);
                return respuesta;
            }
            catch (TimeoutException timeProblem)
            {
                Console.WriteLine("The service operation timed out. " + timeProblem.Message);
                Console.ReadLine();
                // wcfClient.Abort();
                return null;
            }
           
            catch (FaultException unknownFault)
            {
                Console.WriteLine("An unknown exception was received. " + unknownFault.Message);
                Console.ReadLine();
                // wcfClient.Abort();
                return null;
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message + commProblem.StackTrace);
                Console.ReadLine();
                //wcfClient.Abort();
                return null;
            }

        }

        public Mensaje DesactivarUsuario(ClienteEntity cliente, SuscripcionEntity suscripcion, List<UsuarioEntity> usuarios, ParametroEntity parametro)
        {
            ClienteBO clienteBO = new ClienteBO();
            Mensaje a = clienteBO.Save(cliente);

            SuscripcionBO suscripcionBO = new SuscripcionBO();
            suscripcion.PEstado = 0;
            Mensaje b = suscripcionBO.Save(suscripcion);

            UsuarioBO usuarioBO = new UsuarioBO();
            int cont = 0;
            Mensaje c = new Mensaje();
            string valor="";
            foreach (UsuarioEntity usuario in usuarios)
            {
                usuario.PEstado = 0;
                cont = cont + 1;
                c = usuarioBO.Save(usuario);
                valor = c.PDescripcion;
            }
            string MensajeRango = "";
            if (parametro.Es_RangoIP.Equals("1"))
            {
                RangoBO rangoBO = new RangoBO();
                RangoEntity rango;
                rango = rangoBO.GetByClienteId(cliente.PCodigo_Cliente);
                rango.PActivo = 0;
                //rango.PClienteId = Convert.ToInt32(cliente.PCodigo_Cliente);
                //rango.PSuscripcionId = Convert.ToInt32(suscripcion.PCodigoSuscripcion);
                //rango.PMail = usuarios[0].PEmail;
                //rango.PClave = usuarios[0].PClave;
                Mensaje d = rangoBO.Save(rango, usuarios[0]);
                MensajeRango = d.PDescripcion;
            }

            string mensaje = a.PDescripcion + " - " + b.PDescripcion + " - Se " + " " + c.PDescripcion + " " + cont + " Usuarios" + MensajeRango;



            Mensaje respuesta = new Mensaje(true, mensaje);
            return respuesta;
        }

        public Mensaje CambiarClaveUsuario(ClienteEntity cliente, SuscripcionEntity suscripcion, List<UsuarioEntity> usuarios)
        {
            ClienteBO clienteBO = new ClienteBO();
            Mensaje a = clienteBO.Save(cliente);

            SuscripcionBO suscripcionBO = new SuscripcionBO();
            Mensaje b = suscripcionBO.Save(suscripcion);

            UsuarioBO usuarioBO = new UsuarioBO();
            int cont = 0;
            Mensaje c = new Mensaje();
            foreach (UsuarioEntity usuario in usuarios)
            {
                cont = cont + 1;
                c = usuarioBO.Save(usuario);
            }
            string mensaje = a.PDescripcion + " - " + b.PDescripcion + " - Se " + " " + c.PDescripcion + " " + cont + " Usuarios";
            Mensaje respuesta = new Mensaje(true, mensaje);
            return respuesta;
        }

        public Mensaje ModificarDatosUsuario(ClienteEntity cliente, SuscripcionEntity suscripcion, List<UsuarioEntity> usuarios)
        {
            ClienteBO clienteBO = new ClienteBO();
            Mensaje a = clienteBO.Save(cliente);

            SuscripcionBO suscripcionBO = new SuscripcionBO();
            Mensaje b = suscripcionBO.Save(suscripcion);

            UsuarioBO usuarioBO = new UsuarioBO();
            int cont = 0;
            Mensaje c = new Mensaje();
            foreach (UsuarioEntity usuario in usuarios)
            {
                cont = cont + 1;
                c = usuarioBO.Save(usuario);
            }
            string mensaje = a.PDescripcion + " - " + b.PDescripcion + " - Se " + " " + c.PDescripcion + " " + cont + " Usuarios";
            Mensaje respuesta = new Mensaje(true, mensaje);
            return respuesta;
        }

        
    }
}
