/**
 * Mascota atendida en la clinica, asociada a un Cliente (dueno).
 */
public class Mascota
{
    private String id;
    private String nombre;
    private String especie;
    private String raza;
    private int edad;
    private Cliente dueno;

    public Mascota(String id, String nombre, String especie, String raza, int edad, Cliente dueno)
    {
        this.id = id;
        this.nombre = nombre;
        this.especie = especie;
        this.raza = raza;
        this.edad = edad;
        this.dueno = dueno;
        dueno.agregarMascota(this);
    }

    public String getId()
    {
        return id;
    }

    public String getNombre()
    {
        return nombre;
    }

    public String getEspecie()
    {
        return especie;
    }

    public String getRaza()
    {
        return raza;
    }

    public int getEdad()
    {
        return edad;
    }

    public Cliente getDueno()
    {
        return dueno;
    }

    public void mostrarInformacion()
    {
        System.out.println(id + " - " + nombre + " (" + especie + ", " + raza + ", " + edad + " anios) - Dueno: " + dueno.getNombre());
    }
}
