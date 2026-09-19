using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicaVeterinaria.logica
{
    public class FlujoTicket
    {
        private Dictionary<string, List<string>> dicTransiciones;

        public FlujoTicket()
        {
            dicTransiciones = new Dictionary<string, List<string>>
            {
                { "Abierto", new List<string> { "Asignado" } },
                { "Asignado", new List<string> { "Escalado", "Resuelto" } },
                { "Escalado", new List<string> { "Asignado", "Resuelto" } },
                { "Resuelto", new List<string> { "Cerrado", "Asignado" } },
                { "Cerrado", new List<string>() }
            };
        }

        public bool puedeCambiarEstado(string strActual, string strNuevo)
        {
            return dicTransiciones.ContainsKey(strActual) && dicTransiciones[strActual].Contains(strNuevo);
        }

        public void mostrarFlujo()
        {
            Console.WriteLine("Flujo: Abierto -> Asignado -> Escalado -> Resuelto -> Cerrado");
            foreach (var par in dicTransiciones)
            {
                string strDestinos = par.Value.Count > 0 ? string.Join(", ", par.Value) : "(estado final)";
                Console.WriteLine($"  {par.Key} -> {strDestinos}");
            }
        }
    }
}
