/**
 * Veterinario que atiende tickets (consultas) segun su especialidad.
 */
public class Veterinario extends Persona
{
    private String especialidad;
    private int nivelExperiencia;
    private int cargaActual;
    private int capacidadMaxima;

    public Veterinario(String id, String nombre, String correo, String especialidad, int nivelExperiencia, int capacidadMaxima)
    {
        super(id, nombre, correo);
        this.especialidad = especialidad;
        this.nivelExperiencia = nivelExperiencia;
        this.capacidadMaxima = capacidadMaxima;
        this.cargaActual = 0;
    }

    public String getEspecialidad()
    {
        return especialidad;
    }

    public int getNivelExperiencia()
    {
        return nivelExperiencia;
    }

    public int getCargaActual()
    {
        return cargaActual;
    }

    public int getCapacidadMaxima()
    {
        return capacidadMaxima;
    }

    public boolean estaDisponible()
    {
        return cargaActual < capacidadMaxima;
    }

    public boolean puedeAtender(String tipoConsulta)
    {
        if (!estaDisponible())
        {
            return false;
        }
        return especialidad.equalsIgnoreCase(tipoConsulta) || especialidad.equalsIgnoreCase("General");
    }

    public void aumentarCarga()
    {
        if (cargaActual < capacidadMaxima)
        {
            cargaActual++;
        }
    }

    public void liberarCarga()
    {
        if (cargaActual > 0)
        {
            cargaActual--;
        }
    }

    public String obtenerRol()
    {
        return "Veterinario";
    }

    public void mostrarInformacion()
    {
        System.out.println("[" + obtenerRol() + "] " + getId() + " - " + getNombre() + " (" + getCorreo() + ")");
        System.out.println("    Especialidad: " + especialidad + " | Nivel: " + nivelExperiencia + " | Carga: " + cargaActual + "/" + capacidadMaxima);
    }
}
