using System;

namespace ClinicaVeterinaria.logica
{
    public abstract class Persona
    {
        public string strId { get; set; }
        public string strNombre { get; set; }
        public string strCorreo { get; set; }
        public bool blnActivo { get; set; }

        protected Persona(string strId, string strNombre, string strCorreo)
        {
            this.strId = strId;
            this.strNombre = strNombre;
            this.strCorreo = strCorreo;
            this.blnActivo = true;
        }

        public abstract string obtenerRol();

        public virtual void mostrarInformacion()
        {
            Console.WriteLine($"[{obtenerRol()}] {strId} - {strNombre} ({strCorreo})");
        }
    }
}
