using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBClass
{
    public class MOBClientesClass
    {
        /// <summary>
        /// Método que registra un cliente
        /// </summary>
        /// <param name="_cliente">Objeto de tipo MOBClientes</param>
        /// <returns>ID del cliente registrado</returns>
        public static int RegistrarMOBClientes(MOBClientes _cliente)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                ninadbCore.MOBClientes.Add(_cliente);
                ninadbCore.SaveChanges();

                return _cliente.id_cliente;
            }
        }

        /// <summary>
        /// Método para modificar un cliente
        /// </summary>
        /// <param name="_cliente">Objeto de tipo MOBClientes</param>
        /// <returns>Número de elementos modificados</returns>
        public static int ModificarMOBcliente(MOBClientes _cliente)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                MOBClientes cliente = ninadbCore.MOBClientes.Find(_cliente.id_cliente);

                if (!cliente.correo.Equals(_cliente.correo)) cliente.correo = _cliente.correo;
                if (!cliente.tokenValidacion.Equals(_cliente.tokenValidacion)) cliente.tokenValidacion = _cliente.tokenValidacion;
                if (cliente.fecha_validacion != _cliente.fecha_validacion) cliente.fecha_validacion = _cliente.fecha_validacion;
                if (cliente.validado != _cliente.validado) cliente.validado = _cliente.validado;

                return ninadbCore.SaveChanges();
            }
        }

        /// <summary>
        /// Método para obtener un cliente mediante su ID
        /// </summary>
        /// <param name="_idcliente">ID del cliente a buscar</param>
        /// <returns>Objeto de tipo MOBClientes</returns>
        public static MOBClientes ObtenerMOBCliente(int _idcliente)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.MOBClientes.Find(_idcliente);
            }
        }

        /// <summary>
        /// Método para obtener los clientes registrados
        /// </summary>
        /// <returns>Listado de objetos de tipo MOBClientes</returns>
        public static List<MOBClientes> ObtenerMOBClientes()
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.MOBClientes.ToList();
            }
        }
    }
}
