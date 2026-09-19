using System;
using System.Collections.Generic;
using System.Linq;
using ClinicaVeterinaria.logica;

namespace ClinicaVeterinaria
{
    internal class Program
    {
        static void Main()
        {
            GestorClinica objGestor = new GestorClinica();
            datosIniciales(objGestor);

            bool blnContinuar = true;
            while (blnContinuar)
            {
                mostrarEncabezado();
                mostrarMenu();

                string strOpcion = Console.ReadLine();
                Console.WriteLine();

                if (strOpcion == null)
                {
                    // Entrada estandar cerrada (EOF): se termina el programa.
                    break;
                }

                try
                {
                    switch (strOpcion)
                    {
                        case "1": mostrarPersonasPolimorfismo(objGestor); break;
                        case "2": registrarMascota(objGestor); break;
                        case "3": crearTicket(objGestor); break;
                        case "4": objGestor.mostrarTickets(); break;
                        case "5": asignarTicket(objGestor); break;
                        case "6": registrarHallazgo(objGestor); break;
                        case "7": resolverTicket(objGestor); break;
                        case "8": escalarTicket(objGestor); break;
                        case "9": cerrarTicket(objGestor); break;
                        case "10": consultarBitacora(objGestor); break;
                        case "11": objGestor.generarMetricas(); break;
                        case "12": objGestor.mostrarFlujo(); break;
                        case "13": blnContinuar = false; break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Opcion invalida.");
                            Console.ResetColor();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ResetColor();
                }

                if (blnContinuar)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione ENTER para continuar...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Hasta pronto.");
        }

        static void datosIniciales(GestorClinica objGestor)
        {
            objGestor.lstVeterinarios.Add(new Veterinario("V1", "Dra. Sofia Ramirez", "sofia.ramirez@clinicavet.com", "Medicina General", 3, 3));
            objGestor.lstVeterinarios.Add(new Veterinario("V2", "Dr. Diego Morales", "diego.morales@clinicavet.com", "Cirugia", 5, 2));
            objGestor.lstVeterinarios.Add(new Veterinario("V3", "Dra. Paola Ixcot", "paola.ixcot@clinicavet.com", "Dermatologia", 4, 2));

            Cliente objCliente1 = new Cliente("C1", "Ana Garcia", "ana.garcia@correo.com", "5555-1111", "Zona 10, Ciudad de Guatemala");
            Cliente objCliente2 = new Cliente("C2", "Luis Fernandez", "luis.fernandez@correo.com", "5555-2222", "Zona 7, Ciudad de Guatemala");
            objGestor.lstClientes.Add(objCliente1);
            objGestor.lstClientes.Add(objCliente2);

            objGestor.lstMascotas.Add(new Mascota("M1", "Rocky", "Perro", "Labrador", 4, objCliente1));
            objGestor.lstMascotas.Add(new Mascota("M2", "Michi", "Gato", "Comun Europeo", 2, objCliente2));
        }

        static void mostrarEncabezado()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=======================================================");
            Console.WriteLine("   SISTEMA DE CONTROL CLINICO VETERINARIO");
            Console.WriteLine("=======================================================");
            Console.ResetColor();
        }

        static void mostrarMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1.  Ver personas del sistema (polimorfismo)");
            Console.WriteLine("2.  Registrar mascota");
            Console.WriteLine("3.  Crear ticket (solicitar consulta, asigna automaticamente)");
            Console.WriteLine("4.  Ver tickets");
            Console.WriteLine("5.  Asignar / reasignar ticket");
            Console.WriteLine("6.  Registrar hallazgo clinico en ticket");
            Console.WriteLine("7.  Resolver ticket (diagnostico y tratamiento)");
            Console.WriteLine("8.  Escalar ticket");
            Console.WriteLine("9.  Cerrar ticket");
            Console.WriteLine("10. Consultar historial clinico (bitacora)");
            Console.WriteLine("11. Generar metricas");
            Console.WriteLine("12. Ver flujo de estados");
            Console.WriteLine("13. Salir");
            Console.ResetColor();
            Console.Write("Seleccione una opcion: ");
        }

        static void mostrarPersonasPolimorfismo(GestorClinica objGestor)
        {
            List<Persona> lstPersonas = new List<Persona>();
            lstPersonas.AddRange(objGestor.lstVeterinarios);
            lstPersonas.AddRange(objGestor.lstClientes);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Personas del sistema:");
            Console.ResetColor();
            foreach (Persona objPersona in lstPersonas)
                objPersona.mostrarInformacion();
        }

        static void registrarMascota(GestorClinica objGestor)
        {
            Cliente objCliente = solicitarCliente(objGestor);
            if (objCliente == null) return;

            Console.Write("ID de la mascota: ");
            string strId = Console.ReadLine();
            Console.Write("Nombre: ");
            string strNombre = Console.ReadLine();
            Console.Write("Especie (Perro, Gato, Ave, etc.): ");
            string strEspecie = Console.ReadLine();
            Console.Write("Raza: ");
            string strRaza = Console.ReadLine();
            Console.Write("Edad (anios): ");
            int.TryParse(Console.ReadLine(), out int intEdad);

            Mascota objMascota = new Mascota(strId, strNombre, strEspecie, strRaza, intEdad, objCliente);
            objGestor.lstMascotas.Add(objMascota);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Mascota registrada correctamente.");
            Console.ResetColor();
        }

        static void crearTicket(GestorClinica objGestor)
        {
            Mascota objMascota = solicitarMascota(objGestor);
            if (objMascota == null) return;

            Console.Write("Motivo de la consulta: ");
            string strMotivo = Console.ReadLine();
            Console.Write("Sintomas: ");
            string strSintomas = Console.ReadLine();
            Console.Write("Tipo de consulta (Medicina General, Cirugia, Dermatologia, Vacunacion, etc.): ");
            string strTipoConsulta = Console.ReadLine();
            Console.Write("Prioridad (Baja, Media, Alta, Emergencia): ");
            string strPrioridad = Console.ReadLine();

            Ticket objTicket = objGestor.crearTicket(strMotivo, strSintomas, strTipoConsulta, strPrioridad, objMascota);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Ticket #{objTicket.intNumero} creado. Estado actual: {objTicket.strEstado}.");
            Console.ResetColor();
        }

        static void asignarTicket(GestorClinica objGestor)
        {
            Ticket objTicket = solicitarTicket(objGestor);
            if (objTicket == null) return;

            Console.Write("Codigo del veterinario (Enter para asignacion automatica): ");
            string strIdVeterinario = Console.ReadLine();

            Ticket objResultado = string.IsNullOrWhiteSpace(strIdVeterinario)
                ? objGestor.asignarTicket(objTicket.intNumero)
                : objGestor.asignarTicket(objTicket.intNumero, strIdVeterinario);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Ticket #{objResultado.intNumero} asignado a {objResultado.objVeterinarioAsignado.strNombre}.");
            Console.ResetColor();
        }

        static void registrarHallazgo(GestorClinica objGestor)
        {
            Ticket objTicket = solicitarTicket(objGestor);
            if (objTicket == null) return;

            Console.Write("Tipo de hallazgo: ");
            string strTipo = Console.ReadLine();
            Console.Write("Descripcion: ");
            string strDescripcion = Console.ReadLine();
            Console.Write("Gravedad (Baja, Media, Alta, Critica): ");
            string strGravedad = Console.ReadLine();

            objGestor.registrarHallazgo(objTicket.intNumero, strTipo, strDescripcion, strGravedad);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Hallazgo registrado en la bitacora del ticket.");
            Console.ResetColor();
        }

        static void resolverTicket(GestorClinica objGestor)
        {
            Ticket objTicket = solicitarTicket(objGestor);
            if (objTicket == null) return;

            Console.Write("Diagnostico: ");
            string strDiagnostico = Console.ReadLine();
            Console.Write("Tratamiento aplicado: ");
            string strTratamiento = Console.ReadLine();

            objGestor.resolverTicket(objTicket.intNumero, strDiagnostico, strTratamiento);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Ticket #{objTicket.intNumero} resuelto.");
            Console.ResetColor();
        }

        static void escalarTicket(GestorClinica objGestor)
        {
            Ticket objTicket = solicitarTicket(objGestor);
            if (objTicket == null) return;

            Console.Write("Motivo de la escalacion: ");
            string strMotivo = Console.ReadLine();

            objGestor.escalarTicket(objTicket.intNumero, strMotivo);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Ticket #{objTicket.intNumero} escalado. Prioridad actual: {objTicket.strPrioridad}.");
            Console.ResetColor();
        }

        static void cerrarTicket(GestorClinica objGestor)
        {
            Ticket objTicket = solicitarTicket(objGestor);
            if (objTicket == null) return;

            objGestor.cerrarTicket(objTicket.intNumero);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Ticket #{objTicket.intNumero} cerrado.");
            Console.ResetColor();
        }

        static void consultarBitacora(GestorClinica objGestor)
        {
            Ticket objTicket = solicitarTicket(objGestor);
            if (objTicket == null) return;

            objTicket.mostrarBitacora();
        }

        static Ticket solicitarTicket(GestorClinica objGestor)
        {
            Console.Write("Numero de ticket: ");
            if (!int.TryParse(Console.ReadLine(), out int intNumero))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Numero invalido.");
                Console.ResetColor();
                return null;
            }
            return objGestor.buscarTicket(intNumero);
        }

        static Cliente solicitarCliente(GestorClinica objGestor)
        {
            if (objGestor.lstClientes.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay clientes registrados.");
                Console.ResetColor();
                return null;
            }

            for (int i = 0; i < objGestor.lstClientes.Count; i++)
                Console.WriteLine($"  {i}. {objGestor.lstClientes[i].strNombre}");
            Console.Write("Seleccione el cliente (indice): ");

            if (!int.TryParse(Console.ReadLine(), out int intIndice) || intIndice < 0 || intIndice >= objGestor.lstClientes.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Seleccion fuera de rango.");
                Console.ResetColor();
                return null;
            }
            return objGestor.lstClientes[intIndice];
        }

        static Mascota solicitarMascota(GestorClinica objGestor)
        {
            if (objGestor.lstMascotas.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No hay mascotas registradas.");
                Console.ResetColor();
                return null;
            }

            for (int i = 0; i < objGestor.lstMascotas.Count; i++)
            {
                Mascota m = objGestor.lstMascotas[i];
                Console.WriteLine($"  {i}. {m.strNombre} ({m.strEspecie}) - Dueno: {m.objDueno.strNombre}");
            }
            Console.Write("Seleccione la mascota (indice): ");

            if (!int.TryParse(Console.ReadLine(), out int intIndice) || intIndice < 0 || intIndice >= objGestor.lstMascotas.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Seleccion fuera de rango.");
                Console.ResetColor();
                return null;
            }
            return objGestor.lstMascotas[intIndice];
        }
    }
}
