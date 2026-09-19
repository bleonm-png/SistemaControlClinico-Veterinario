import java.util.ArrayList;
import java.util.NoSuchElementException;

/**
 * Coordina veterinarios, clientes, mascotas y tickets de la clinica.
 */
public class GestorClinica
{
    private ArrayList<Veterinario> veterinarios;
    private ArrayList<Cliente> clientes;
    private ArrayList<Mascota> mascotas;
    private ArrayList<Ticket> tickets;
    private FlujoTicket flujo;
    private int siguienteNumero;

    public GestorClinica()
    {
        veterinarios = new ArrayList<>();
        clientes = new ArrayList<>();
        mascotas = new ArrayList<>();
        tickets = new ArrayList<>();
        flujo = new FlujoTicket();
        siguienteNumero = 1;
    }

    public Ticket crearTicket(String motivo, String sintomas, String tipoConsulta, String prioridad, Mascota mascota)
    {
        Ticket ticket = new Ticket(siguienteNumero, motivo, sintomas, tipoConsulta, prioridad, mascota);
        siguienteNumero++;
        tickets.add(ticket);
        try
        {
            asignarTicket(ticket.getNumero());
        }
        catch (IllegalStateException ex)
        {
            // El ticket queda Abierto si no hay veterinario disponible por el momento.
        }
        return ticket;
    }

    public Ticket asignarTicket(int numero)
    {
        Ticket ticket = buscarTicket(numero);
        Veterinario veterinario = asignarVeterinarioAutomatico(ticket.getTipoConsulta());
        if (veterinario == null)
        {
            throw new IllegalStateException("No hay veterinarios disponibles para este tipo de consulta.");
        }
        ticket.asignar(veterinario);
        return ticket;
    }

    public Ticket asignarTicket(int numero, String idVeterinario)
    {
        Ticket ticket = buscarTicket(numero);
        Veterinario veterinario = null;
        for (Veterinario v : veterinarios)
        {
            if (v.getId().equals(idVeterinario))
            {
                veterinario = v;
                break;
            }
        }
        if (veterinario == null)
        {
            throw new NoSuchElementException("Veterinario no encontrado.");
        }
        ticket.asignar(veterinario);
        return ticket;
    }

    public Ticket registrarHallazgo(int numero, String tipo, String descripcion, String gravedad)
    {
        Ticket ticket = buscarTicket(numero);
        ticket.registrarHallazgo(tipo, descripcion, gravedad);
        boolean gravedadAlta = gravedad.equalsIgnoreCase("Alta") || gravedad.equalsIgnoreCase("Critica");
        if (gravedadAlta && flujo.puedeCambiarEstado(ticket.getEstado(), "Escalado"))
        {
            escalarTicket(numero, "Hallazgo clinico de gravedad " + gravedad);
        }
        return ticket;
    }

    public Ticket escalarTicket(int numero, String motivo)
    {
        Ticket ticket = buscarTicket(numero);
        if (!flujo.puedeCambiarEstado(ticket.getEstado(), "Escalado"))
        {
            throw new IllegalStateException("No se puede escalar un ticket en estado '" + ticket.getEstado() + "'.");
        }
        Veterinario senior = null;
        for (Veterinario v : veterinarios)
        {
            boolean compatible = v.getEspecialidad().equalsIgnoreCase(ticket.getTipoConsulta())
                || v.getEspecialidad().equalsIgnoreCase("General");
            if (compatible && (senior == null || v.getNivelExperiencia() > senior.getNivelExperiencia()))
            {
                senior = v;
            }
        }
        ticket.escalar(motivo, senior);
        return ticket;
    }

    public Ticket resolverTicket(int numero, String diagnostico, String tratamiento)
    {
        Ticket ticket = buscarTicket(numero);
        if (!flujo.puedeCambiarEstado(ticket.getEstado(), "Resuelto"))
        {
            throw new IllegalStateException("No se puede resolver un ticket en estado '" + ticket.getEstado() + "'.");
        }
        ticket.resolver(diagnostico, tratamiento);
        return ticket;
    }

    public Ticket cerrarTicket(int numero)
    {
        Ticket ticket = buscarTicket(numero);
        if (!flujo.puedeCambiarEstado(ticket.getEstado(), "Cerrado"))
        {
            throw new IllegalStateException("No se puede cerrar un ticket en estado '" + ticket.getEstado() + "'.");
        }
        ticket.cerrar();
        return ticket;
    }

    private Veterinario asignarVeterinarioAutomatico(String tipoConsulta)
    {
        Veterinario elegido = null;
        for (Veterinario v : veterinarios)
        {
            if (v.getEspecialidad().equalsIgnoreCase(tipoConsulta) && v.estaDisponible())
            {
                if (elegido == null || v.getCargaActual() < elegido.getCargaActual())
                {
                    elegido = v;
                }
            }
        }
        if (elegido == null)
        {
            for (Veterinario v : veterinarios)
            {
                if (v.getEspecialidad().equalsIgnoreCase("General") && v.estaDisponible())
                {
                    if (elegido == null || v.getCargaActual() < elegido.getCargaActual())
                    {
                        elegido = v;
                    }
                }
            }
        }
        return elegido;
    }

    public Ticket buscarTicket(int numero)
    {
        for (Ticket t : tickets)
        {
            if (t.getNumero() == numero)
            {
                return t;
            }
        }
        throw new NoSuchElementException("No existe un ticket con numero " + numero + ".");
    }

    public void mostrarTickets()
    {
        if (tickets.isEmpty())
        {
            System.out.println("No hay tickets registrados.");
            return;
        }
        for (Ticket t : tickets)
        {
            t.mostrarResumen();
        }
    }

    public void mostrarFlujo()
    {
        flujo.mostrarFlujo();
    }

    public void generarMetricas()
    {
        System.out.println("Total de tickets: " + tickets.size());

        String[] estados = { "Abierto", "Asignado", "Escalado", "Resuelto", "Cerrado" };
        System.out.println("Por estado:");
        for (String estado : estados)
        {
            int contador = 0;
            for (Ticket t : tickets)
            {
                if (t.getEstado().equals(estado))
                {
                    contador++;
                }
            }
            System.out.println("  " + estado + ": " + contador);
        }

        String[] prioridades = { "Baja", "Media", "Alta", "Emergencia" };
        System.out.println("Por prioridad:");
        for (String prioridad : prioridades)
        {
            int contador = 0;
            for (Ticket t : tickets)
            {
                if (t.getPrioridad().equalsIgnoreCase(prioridad))
                {
                    contador++;
                }
            }
            System.out.println("  " + prioridad + ": " + contador);
        }

        int escalados = 0;
        for (Ticket t : tickets)
        {
            if (t.isEscalado())
            {
                escalados++;
            }
        }
        System.out.println("Tickets escalados: " + escalados);

        System.out.println("Carga por veterinario:");
        for (Veterinario v : veterinarios)
        {
            System.out.println("  " + v.getNombre() + " (" + v.getEspecialidad() + "): " + v.getCargaActual() + "/" + v.getCapacidadMaxima());
        }
    }

    public ArrayList<Veterinario> getVeterinarios()
    {
        return veterinarios;
    }

    public ArrayList<Cliente> getClientes()
    {
        return clientes;
    }

    public ArrayList<Mascota> getMascotas()
    {
        return mascotas;
    }

    public ArrayList<Ticket> getTickets()
    {
        return tickets;
    }
}
