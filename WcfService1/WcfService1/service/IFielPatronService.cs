using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WcfService1.FielPatronService.Entities;

namespace WcfService1.service
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IFielPatronService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IFielPatronService
    {
        [OperationContract]
        Mensaje ActivarUsuario(ClienteEntity cliente,SuscripcionEntity suscripcion, List<UsuarioEntity> usuarios,RangoEntity rango, ParametroEntity parametro);

        [OperationContract]
        Mensaje DesactivarUsuario(ClienteEntity cliente, SuscripcionEntity suscripcion, List<UsuarioEntity> usuarios, ParametroEntity parametro);

        [OperationContract]
        Mensaje CambiarClaveUsuario(ClienteEntity cliente, SuscripcionEntity suscripcion, List<UsuarioEntity> usuarios);

        [OperationContract]
        Mensaje ModificarDatosUsuario(ClienteEntity cliente, SuscripcionEntity suscripcion, List<UsuarioEntity> usuarios );

    }
}
