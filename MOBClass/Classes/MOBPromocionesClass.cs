using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBClass
{
    public class MOBPromocionesClass
    {
        /// <summary>
        /// Método que registra una promocion
        /// </summary>
        /// <param name="_promocion">Objeto de tipo MOBPromociones</param>
        /// <returns>ID de la promocion registrada</returns>
        public static int RegistrarMOBPromociones(MOBPromociones _promocion)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                ninadbCore.MOBPromociones.Add(_promocion);
                ninadbCore.SaveChanges();

                return _promocion.id_promocion;
            }
        }

        /// <summary>
        /// Método para modificar una promocion
        /// </summary>
        /// <param name="_promocion">Objeto de tipo MOBPromociones</param>
        /// <returns>Número de elementos modificados</returns>
        public static int ModificarMOBPromocion(MOBPromociones _promocion)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                MOBPromociones promocion = ninadbCore.MOBPromociones.Find(_promocion.id_promocion);

                if (!promocion.descripcion.Equals(_promocion.descripcion)) promocion.descripcion = _promocion.descripcion;
                if (!promocion.especificaciones.Equals(_promocion.especificaciones)) promocion.especificaciones = _promocion.especificaciones;
                if (promocion.cantidad_premios != _promocion.cantidad_premios) promocion.cantidad_premios = _promocion.cantidad_premios;
                if (!promocion.imagen_nombre.Equals(_promocion.imagen_nombre)) promocion.imagen_nombre = _promocion.imagen_nombre;
                if (promocion.premios_restantes != _promocion.premios_restantes) promocion.premios_restantes = _promocion.premios_restantes;

                return ninadbCore.SaveChanges();
            }
        }

        /// <summary>
        /// Método para obtener una promocion mediante su ID
        /// </summary>
        /// <param name="_idPromocion">ID de la promocion a buscar</param>
        /// <returns>Objeto de tipo MOBPromociones</returns>
        public static MOBPromociones ObtenerMOBPromocion(int _idPromocion)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.MOBPromociones.Find(_idPromocion);
            }
        }

        /// <summary>
        /// Método para obtener las promociones registradas
        /// </summary>
        /// <returns>Listado de objetos de tipo MOBPromociones</returns>
        public static List<MOBPromociones> ObtenerMOBPromociones()
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.MOBPromociones.ToList();
            }
        }

        public static MOBPromociones SeleccionarPromocionAleatoria()
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.spMOBSeleccionarPromocionAleatoria().FirstOrDefault();
            }
        }
    }
}
