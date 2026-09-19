using System;
using System.Collections.Generic;

namespace ClinicaVeterinaria.logica
{
    public class Bitacora
    {
        public List<string> lstRegistros { get; set; }

        public Bitacora()
        {
            lstRegistros = new List<string>();
        }

        public void registrarEvento(string strEvento)
        {
            lstRegistros.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} - {strEvento}");
        }

        public void mostrarBitacora()
        {
            Console.WriteLine("  Historial clinico:");
            if (lstRegistros.Count == 0)
            {
                Console.WriteLine("    (sin eventos registrados)");
                return;
            }
            foreach (var strEvento in lstRegistros)
                Console.WriteLine($"    - {strEvento}");
        }
    }
}
