using System;

namespace ClinicaVeterinaria.logica
{
    public class Mascota
    {
        public string strId { get; set; }
        public string strNombre { get; set; }
        public string strEspecie { get; set; }
        public string strRaza { get; set; }
        public int intEdad { get; set; }
        public Cliente objDueno { get; set; }

        public Mascota(string strId, string strNombre, string strEspecie, string strRaza, int intEdad, Cliente objDueno)
        {
            this.strId = strId;
            this.strNombre = strNombre;
            this.strEspecie = strEspecie;
            this.strRaza = strRaza;
            this.intEdad = intEdad;
            this.objDueno = objDueno;
            objDueno.lstMascotas.Add(this);
        }

        public void mostrarInformacion()
        {
            Console.WriteLine($"{strId} - {strNombre} ({strEspecie}, {strRaza}, {intEdad} anios) - Dueno: {objDueno.strNombre}");
        }
    }
}
