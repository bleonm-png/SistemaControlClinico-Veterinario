/**
 * Clase abstracta base para las personas del sistema (Cliente, Veterinario).
 */
public abstract class Persona
{
    private String id;
    private String nombre;
    private String correo;

    public Persona(String id, String nombre, String correo)
    {
        this.id = id;
        this.nombre = nombre;
        this.correo = correo;
    }

    public String getId()
    {
        return id;
    }

    public String getNombre()
    {
        return nombre;
    }

    public void setNombre(String nombre)
    {
        this.nombre = nombre;
    }

    public String getCorreo()
    {
        return correo;
    }

    public void setCorreo(String correo)
    {
        this.correo = correo;
    }

    public abstract String obtenerRol();

    public abstract void mostrarInformacion();
}
