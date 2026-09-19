using System;

namespace ClinicaVeterinaria.logica
{
    public class Ticket
    {
        public int intNumero { get; set; }
        public string strMotivo { get; set; }
        public string strSintomas { get; set; }
        public string strTipoConsulta { get; set; }
        public string strPrioridad { get; set; }
        public string strEstado { get; set; }
        public DateTime dtFechaCreacion { get; set; }
        public string strDiagnostico { get; set; }
        public string strTratamiento { get; set; }
        public bool blnEscalado { get; set; }
        public Mascota objMascota { get; set; }
        public Veterinario objVeterinarioAsignado { get; set; }
        public Bitacora objBitacora { get; set; }

        public Ticket(int intNumero, string strMotivo, string strSintomas, string strTipoConsulta, string strPrioridad, Mascota objMascota)
        {
            this.intNumero = intNumero;
            this.strMotivo = strMotivo;
            this.strSintomas = strSintomas;
            this.strTipoConsulta = strTipoConsulta;
            this.strPrioridad = strPrioridad;
            this.strEstado = "Abierto";
            this.dtFechaCreacion = DateTime.Now;
            this.blnEscalado = false;
            this.objMascota = objMascota;
            this.objBitacora = new Bitacora();
            objBitacora.registrarEvento($"Ticket creado para la mascota {objMascota.strNombre} (motivo: {strMotivo})");
        }

        public void asignar(Veterinario objVeterinario)
        {
            if (objVeterinarioAsignado != null)
            {
                objVeterinarioAsignado.liberarCarga();
                objBitacora.registrarEvento($"Reasignado de {objVeterinarioAsignado.strNombre} a {objVeterinario.strNombre}");
            }
            else
            {
                objBitacora.registrarEvento($"Asignado al veterinario {objVeterinario.strNombre}");
            }
            objVeterinarioAsignado = objVeterinario;
            objVeterinario.aumentarCarga();
            strEstado = "Asignado";
        }

        public void registrarHallazgo(string strTipo, string strDescripcion, string strGravedad)
        {
            objBitacora.registrarEvento($"Hallazgo [{strTipo}] ({strGravedad}): {strDescripcion}");
        }

        public void resolver(string strDiagnostico, string strTratamiento)
        {
            this.strDiagnostico = strDiagnostico;
            this.strTratamiento = strTratamiento;
            strEstado = "Resuelto";
            objBitacora.registrarEvento($"Consulta resuelta - Diagnostico: {strDiagnostico} | Tratamiento: {strTratamiento}");
        }

        public void cerrar()
        {
            strEstado = "Cerrado";
            objVeterinarioAsignado?.liberarCarga();
            objBitacora.registrarEvento("Ticket cerrado");
        }

        public void escalar(string strMotivo, Veterinario objVeterinarioSenior)
        {
            string strPrioridadAnterior = strPrioridad;
            strPrioridad = "Emergencia";
            blnEscalado = true;
            strEstado = "Escalado";
            objBitacora.registrarEvento($"Escalado ({strPrioridadAnterior} -> {strPrioridad}): {strMotivo}");
            if (objVeterinarioSenior != null && objVeterinarioSenior != objVeterinarioAsignado)
            {
                asignar(objVeterinarioSenior);
                strEstado = "Escalado";
            }
        }

        public void mostrarResumen()
        {
            Console.WriteLine($"Ticket #{intNumero} - {strMotivo}");
            Console.WriteLine($"  Mascota: {objMascota.strNombre} ({objMascota.strEspecie}) - Dueno: {objMascota.objDueno.strNombre}");
            Console.WriteLine($"  Tipo de consulta: {strTipoConsulta} | Prioridad: {strPrioridad} | Estado: {strEstado}");
            Console.WriteLine($"  Sintomas: {strSintomas}");
            Console.WriteLine($"  Fecha de creacion: {dtFechaCreacion:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"  Veterinario asignado: {(objVeterinarioAsignado != null ? objVeterinarioAsignado.strNombre : "Sin asignar")}");
            if (!string.IsNullOrEmpty(strDiagnostico))
                Console.WriteLine($"  Diagnostico: {strDiagnostico} | Tratamiento: {strTratamiento}");
            Console.WriteLine($"  Escalado: {(blnEscalado ? "Si" : "No")}");
        }

        public void mostrarBitacora()
        {
            mostrarResumen();
            objBitacora.mostrarBitacora();
        }
    }
}
