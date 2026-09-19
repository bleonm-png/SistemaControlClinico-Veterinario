using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicaVeterinaria.logica
{
    public class GestorClinica
    {
        public List<Veterinario> lstVeterinarios { get; set; }
        public List<Cliente> lstClientes { get; set; }
        public List<Mascota> lstMascotas { get; set; }
        public List<Ticket> lstTickets { get; set; }
        private FlujoTicket objFlujo;
        private int intSiguienteNumero;

        public GestorClinica()
        {
            lstVeterinarios = new List<Veterinario>();
            lstClientes = new List<Cliente>();
            lstMascotas = new List<Mascota>();
            lstTickets = new List<Ticket>();
            objFlujo = new FlujoTicket();
            intSiguienteNumero = 1;
        }

        public Ticket crearTicket(string strMotivo, string strSintomas, string strTipoConsulta, string strPrioridad, Mascota objMascota)
        {
            Ticket objTicket = new Ticket(intSiguienteNumero, strMotivo, strSintomas, strTipoConsulta, strPrioridad, objMascota);
            intSiguienteNumero++;
            lstTickets.Add(objTicket);
            try
            {
                asignarTicket(objTicket.intNumero);
            }
            catch (InvalidOperationException)
            {
                // El ticket queda Abierto si no hay veterinario disponible por el momento.
            }
            return objTicket;
        }

        public Ticket asignarTicket(int intNumero)
        {
            Ticket objTicket = buscarTicket(intNumero);
            Veterinario objVeterinario = asignarVeterinarioAutomatico(objTicket.strTipoConsulta);
            if (objVeterinario == null)
                throw new InvalidOperationException("No hay veterinarios disponibles para este tipo de consulta.");
            objTicket.asignar(objVeterinario);
            return objTicket;
        }

        public Ticket asignarTicket(int intNumero, string strIdVeterinario)
        {
            Ticket objTicket = buscarTicket(intNumero);
            Veterinario objVeterinario = lstVeterinarios.FirstOrDefault(v => v.strId == strIdVeterinario);
            if (objVeterinario == null)
                throw new KeyNotFoundException("Veterinario no encontrado.");
            objTicket.asignar(objVeterinario);
            return objTicket;
        }

        public Ticket registrarHallazgo(int intNumero, string strTipo, string strDescripcion, string strGravedad)
        {
            Ticket objTicket = buscarTicket(intNumero);
            objTicket.registrarHallazgo(strTipo, strDescripcion, strGravedad);
            bool blnGravedadAlta = strGravedad.Equals("Alta", StringComparison.OrdinalIgnoreCase)
                || strGravedad.Equals("Critica", StringComparison.OrdinalIgnoreCase);
            if (blnGravedadAlta && objFlujo.puedeCambiarEstado(objTicket.strEstado, "Escalado"))
                escalarTicket(intNumero, $"Hallazgo clinico de gravedad {strGravedad}");
            return objTicket;
        }

        public Ticket escalarTicket(int intNumero, string strMotivo)
        {
            Ticket objTicket = buscarTicket(intNumero);
            if (!objFlujo.puedeCambiarEstado(objTicket.strEstado, "Escalado"))
                throw new InvalidOperationException($"No se puede escalar un ticket en estado '{objTicket.strEstado}'.");
            Veterinario objSenior = lstVeterinarios
                .Where(v => v.strEspecialidad.Equals(objTicket.strTipoConsulta, StringComparison.OrdinalIgnoreCase)
                    || v.strEspecialidad.Equals("General", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(v => v.intNivelExperiencia)
                .FirstOrDefault();
            objTicket.escalar(strMotivo, objSenior);
            return objTicket;
        }

        public Ticket resolverTicket(int intNumero, string strDiagnostico, string strTratamiento)
        {
            Ticket objTicket = buscarTicket(intNumero);
            if (!objFlujo.puedeCambiarEstado(objTicket.strEstado, "Resuelto"))
                throw new InvalidOperationException($"No se puede resolver un ticket en estado '{objTicket.strEstado}'.");
            objTicket.resolver(strDiagnostico, strTratamiento);
            return objTicket;
        }

        public Ticket cerrarTicket(int intNumero)
        {
            Ticket objTicket = buscarTicket(intNumero);
            if (!objFlujo.puedeCambiarEstado(objTicket.strEstado, "Cerrado"))
                throw new InvalidOperationException($"No se puede cerrar un ticket en estado '{objTicket.strEstado}'.");
            objTicket.cerrar();
            return objTicket;
        }

        private Veterinario asignarVeterinarioAutomatico(string strTipoConsulta)
        {
            return lstVeterinarios
                .Where(v => v.puedeAtender(strTipoConsulta))
                .OrderBy(v => v.intCargaActual)
                .FirstOrDefault();
        }

        public Ticket buscarTicket(int intNumero)
        {
            Ticket objTicket = lstTickets.FirstOrDefault(t => t.intNumero == intNumero);
            if (objTicket == null)
                throw new KeyNotFoundException($"No existe un ticket con numero {intNumero}.");
            return objTicket;
        }

        public void mostrarTickets()
        {
            if (lstTickets.Count == 0)
            {
                Console.WriteLine("No hay tickets registrados.");
                return;
            }
            foreach (var objTicket in lstTickets)
                objTicket.mostrarResumen();
        }

        public void mostrarFlujo()
        {
            objFlujo.mostrarFlujo();
        }

        public void generarMetricas()
        {
            Console.WriteLine($"Total de tickets: {lstTickets.Count}");

            string[] arrEstados = { "Abierto", "Asignado", "Escalado", "Resuelto", "Cerrado" };
            Console.WriteLine("Por estado:");
            foreach (var strEstado in arrEstados)
                Console.WriteLine($"  {strEstado}: {lstTickets.Count(t => t.strEstado == strEstado)}");

            string[] arrPrioridades = { "Baja", "Media", "Alta", "Emergencia" };
            Console.WriteLine("Por prioridad:");
            foreach (var strPrioridad in arrPrioridades)
                Console.WriteLine($"  {strPrioridad}: {lstTickets.Count(t => t.strPrioridad.Equals(strPrioridad, StringComparison.OrdinalIgnoreCase))}");

            Console.WriteLine($"Tickets escalados: {lstTickets.Count(t => t.blnEscalado)}");

            Console.WriteLine("Carga por veterinario:");
            foreach (var objVeterinario in lstVeterinarios)
                Console.WriteLine($"  {objVeterinario.strNombre} ({objVeterinario.strEspecialidad}): {objVeterinario.intCargaActual}/{objVeterinario.intCapacidadMaxima}");
        }
    }
}
