using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.logica
{
    public class Cliente : Persona
    {
        public string strTelefono { get; set; }
        public string strDireccion { get; set; }
        public List<Mascota> lstMascotas { get; set; }

        public Cliente(string strId, string strNombre, string strCorreo, string strTelefono, string strDireccion)
            : base(strId, strNombre, strCorreo)
        {
            this.strTelefono = strTelefono;
            this.strDireccion = strDireccion;
            this.lstMascotas = new List<Mascota>();
        }

        public override string obtenerRol() => "Cliente";

        public override void mostrarInformacion()
        {
            base.mostrarInformacion();
            Console.WriteLine($"    Telefono: {strTelefono} | Direccion: {strDireccion} | Mascotas: {lstMascotas.Count}");
        }
    }
}
