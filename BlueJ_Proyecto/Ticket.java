import java.util.Date;

/**
 * Caso clinico (ticket) abierto por un Cliente para una Mascota.
 */
public class Ticket
{
    private int numero;
    private String motivo;
    private String sintomas;
    private String tipoConsulta;
    private String prioridad;
    private String estado;
    private Date fechaCreacion;
    private String diagnostico;
    private String tratamiento;
    private boolean escalado;
    private Mascota mascota;
    private Veterinario veterinarioAsignado;
    private Bitacora bitacora;

    public Ticket(int numero, String motivo, String sintomas, String tipoConsulta, String prioridad, Mascota mascota)
    {
        this.numero = numero;
        this.motivo = motivo;
        this.sintomas = sintomas;
        this.tipoConsulta = tipoConsulta;
        this.prioridad = prioridad;
        this.estado = "Abierto";
        this.fechaCreacion = new Date();
        this.escalado = false;
        this.mascota = mascota;
        this.bitacora = new Bitacora();
        bitacora.registrarEvento("Ticket creado para la mascota " + mascota.getNombre() + " (motivo: " + motivo + ")");
    }

    public void asignar(Veterinario veterinario)
    {
        if (veterinarioAsignado != null)
        {
            veterinarioAsignado.liberarCarga();
            bitacora.registrarEvento("Reasignado de " + veterinarioAsignado.getNombre() + " a " + veterinario.getNombre());
        }
        else
        {
            bitacora.registrarEvento("Asignado al veterinario " + veterinario.getNombre());
        }
        veterinarioAsignado = veterinario;
        veterinario.aumentarCarga();
        estado = "Asignado";
    }

    public void registrarHallazgo(String tipo, String descripcion, String gravedad)
    {
        bitacora.registrarEvento("Hallazgo [" + tipo + "] (" + gravedad + "): " + descripcion);
    }

    public void resolver(String diagnostico, String tratamiento)
    {
        this.diagnostico = diagnostico;
        this.tratamiento = tratamiento;
        estado = "Resuelto";
        bitacora.registrarEvento("Consulta resuelta - Diagnostico: " + diagnostico + " | Tratamiento: " + tratamiento);
    }

    public void cerrar()
    {
        estado = "Cerrado";
        if (veterinarioAsignado != null)
        {
            veterinarioAsignado.liberarCarga();
        }
        bitacora.registrarEvento("Ticket cerrado");
    }

    public void escalar(String motivo, Veterinario veterinarioSenior)
    {
        String prioridadAnterior = prioridad;
        prioridad = "Emergencia";
        escalado = true;
        estado = "Escalado";
        bitacora.registrarEvento("Escalado (" + prioridadAnterior + " -> " + prioridad + "): " + motivo);
        if (veterinarioSenior != null && veterinarioSenior != veterinarioAsignado)
        {
            asignar(veterinarioSenior);
            estado = "Escalado";
        }
    }

    public int getNumero()
    {
        return numero;
    }

    public String getMotivo()
    {
        return motivo;
    }

    public String getSintomas()
    {
        return sintomas;
    }

    public String getTipoConsulta()
    {
        return tipoConsulta;
    }

    public String getPrioridad()
    {
        return prioridad;
    }

    public String getEstado()
    {
        return estado;
    }

    public Date getFechaCreacion()
    {
        return fechaCreacion;
    }

    public String getDiagnostico()
    {
        return diagnostico;
    }

    public String getTratamiento()
    {
        return tratamiento;
    }

    public boolean isEscalado()
    {
        return escalado;
    }

    public Mascota getMascota()
    {
        return mascota;
    }

    public Veterinario getVeterinarioAsignado()
    {
        return veterinarioAsignado;
    }

    public Bitacora getBitacora()
    {
        return bitacora;
    }

    public void mostrarResumen()
    {
        System.out.println("Ticket #" + numero + " - " + motivo);
        System.out.println("  Mascota: " + mascota.getNombre() + " (" + mascota.getEspecie() + ") - Dueno: " + mascota.getDueno().getNombre());
        System.out.println("  Tipo de consulta: " + tipoConsulta + " | Prioridad: " + prioridad + " | Estado: " + estado);
        System.out.println("  Sintomas: " + sintomas);
        System.out.println("  Fecha de creacion: " + fechaCreacion);
        System.out.println("  Veterinario asignado: " + (veterinarioAsignado != null ? veterinarioAsignado.getNombre() : "Sin asignar"));
        if (diagnostico != null && !diagnostico.isEmpty())
        {
            System.out.println("  Diagnostico: " + diagnostico + " | Tratamiento: " + tratamiento);
        }
        System.out.println("  Escalado: " + (escalado ? "Si" : "No"));
    }

    public void mostrarBitacora()
    {
        mostrarResumen();
        bitacora.mostrarBitacora();
    }
}
