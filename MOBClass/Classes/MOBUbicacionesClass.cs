using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBClass
{
    public class MOBUbicacionesClass
    {
        /// <summary>
        /// Método que registra una ubicacion
        /// </summary>
        /// <param name="_ubicacion">Objeto de tipo MOBUbicaciones</param>
        /// <returns>ID de la ubicacion registrada</returns>
        public static int RegistrarMOBUbicaciones(MOBUbicaciones _ubicacion)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                ninadbCore.MOBUbicaciones.Add(_ubicacion);
                ninadbCore.SaveChanges();

                return _ubicacion.id_ubicacion;
            }
        }

        /// <summary>
        /// Método para modificar una ubicacion
        /// </summary>
        /// <param name="_ubicacion">Objeto de tipo MOBUbicaciones</param>
        /// <returns>Número de elementos modificados</returns>
        public static int ModificarMOBUbicacion(MOBUbicaciones _ubicacion)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                MOBUbicaciones ubicacion = ninadbCore.MOBUbicaciones.Find(_ubicacion.id_ubicacion);

                if (!ubicacion.nombre.Equals(_ubicacion.nombre)) ubicacion.nombre = _ubicacion.nombre;
                if (!ubicacion.descripcion.Equals(_ubicacion.descripcion)) ubicacion.descripcion = _ubicacion.descripcion;
                if (!ubicacion.codigoURL.Equals(_ubicacion.codigoURL)) ubicacion.codigoURL = _ubicacion.codigoURL;
                if (ubicacion.id_ubicacion != _ubicacion.id_ubicacion) ubicacion.id_ubicacion = _ubicacion.id_ubicacion;

                return ninadbCore.SaveChanges();
            }
        }

        /// <summary>
        /// Método para obtener una ubicacion mediante su ID
        /// </summary>
        /// <param name="_idUbicacion">ID de la ubicacion a buscar</param>
        /// <returns>Objeto de tipo MOBUbicaciones</returns>
        public static MOBUbicaciones ObtenerMOBUbicacion(int _idUbicacion)
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.MOBUbicaciones.Find(_idUbicacion);
            }
        }

        /// <summary>
        /// Método para obtener las ubicaciones registrados
        /// </summary>
        /// <returns>Listado de objetos de tipo MOBUbicaciones</returns>
        public static List<MOBUbicaciones> ObtenerMOBUbicaciones()
        {
            using (ninadbEntities ninadbCore = new ninadbEntities())
            {
                return ninadbCore.MOBUbicaciones.ToList();
            }
        }
    }
}
