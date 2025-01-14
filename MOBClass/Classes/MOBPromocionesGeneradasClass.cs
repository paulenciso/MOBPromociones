using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBClass
{
    public class MOBPromocionesGeneradasClass
    {
        /// <summary>
        /// Método que registra un promocion generada por un cliente
        /// </summary>
        /// <param name="_promocionGenerada">Objeto de tipo MOBPromocionesGeneradas</param>
        /// <returns>ID de la promocion generada</returns>
        public static int RegistrarMOBPromocionesGeneradas(MOBPromocionesGeneradas _promocionGenerada)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                ninadbCore.MOBPromocionesGeneradas.Add(_promocionGenerada);
                ninadbCore.SaveChanges();

                return _promocionGenerada.id_promocion_generada;
            }
        }

        /// <summary>
        /// Método para modificar un promocion generada
        /// </summary>
        /// <param name="_promocionGenerada">Objeto de tipo MOBPromocionesGeneradas</param>
        /// <returns>Número de elementos modificados</returns>
        public static int ModificarMOBpromocionGenerada(MOBPromocionesGeneradas _promocionGenerada)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                MOBPromocionesGeneradas promocionGenerada = ninadbCore.MOBPromocionesGeneradas.Find(_promocionGenerada.id_promocion_generada);

                if (promocionGenerada.fecha_canje != _promocionGenerada.fecha_canje) promocionGenerada.fecha_canje = _promocionGenerada.fecha_canje;
                if (promocionGenerada.entregado != _promocionGenerada.entregado) promocionGenerada.entregado = _promocionGenerada.entregado;
                if (promocionGenerada.id_ticket != _promocionGenerada.id_ticket) promocionGenerada.id_ticket = _promocionGenerada.id_ticket;

                return ninadbCore.SaveChanges();
            }
        }

        /// <summary>
        /// Método para obtener un promocion gGenerada mediante su ID
        /// </summary>
        /// <param name="_idpromocionGenerada">ID del promocion generada a buscar</param>
        /// <returns>Objeto de tipo MOBPromocionesGeneradas</returns>
        public static MOBPromocionesGeneradas ObtenerMOBpromocionGenerada(int _idpromocionGenerada)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.MOBPromocionesGeneradas.Find(_idpromocionGenerada);
            }
        }

        /// <summary>
        /// Método para obtener los promocion generadas
        /// </summary>
        /// <returns>Listado de objetos de tipo MOBPromocionesGeneradas</returns>
        public static List<MOBPromocionesGeneradas> ObtenerMOBPromocionesGeneradas()
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.MOBPromocionesGeneradas.ToList();
            }
        }
    }
}
