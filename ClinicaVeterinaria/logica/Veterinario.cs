using System;

namespace ClinicaVeterinaria.logica
{
    public class Veterinario : Persona
    {
        public string strEspecialidad { get; set; }
        public int intNivelExperiencia { get; set; }
        public int intCargaActual { get; set; }
        public int intCapacidadMaxima { get; set; }

        public Veterinario(string strId, string strNombre, string strCorreo, string strEspecialidad, int intNivelExperiencia, int intCapacidadMaxima)
            : base(strId, strNombre, strCorreo)
        {
            this.strEspecialidad = strEspecialidad;
            this.intNivelExperiencia = intNivelExperiencia;
            this.intCapacidadMaxima = intCapacidadMaxima;
            this.intCargaActual = 0;
        }

        public override string obtenerRol() => "Veterinario";

        public bool estaDisponible()
        {
            return blnActivo && intCargaActual < intCapacidadMaxima;
        }

        public bool puedeAtender(string strTipoConsulta)
        {
            if (!estaDisponible()) return false;
            return strEspecialidad.Equals(strTipoConsulta, StringComparison.OrdinalIgnoreCase)
                || strEspecialidad.Equals("General", StringComparison.OrdinalIgnoreCase);
        }

        public void aumentarCarga()
        {
            if (intCargaActual < intCapacidadMaxima) intCargaActual++;
        }

        public void liberarCarga()
        {
            if (intCargaActual > 0) intCargaActual--;
        }

        public override void mostrarInformacion()
        {
            base.mostrarInformacion();
            Console.WriteLine($"    Especialidad: {strEspecialidad} | Nivel: {intNivelExperiencia} | Carga: {intCargaActual}/{intCapacidadMaxima}");
        }
    }
}
