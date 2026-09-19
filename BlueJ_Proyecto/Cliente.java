import java.util.ArrayList;

/**
 * Dueno de una o mas mascotas que solicita consultas veterinarias.
 */
public class Cliente extends Persona
{
    private String telefono;
    private String direccion;
    private ArrayList<Mascota> mascotas;

    public Cliente(String id, String nombre, String correo, String telefono, String direccion)
    {
        super(id, nombre, correo);
        this.telefono = telefono;
        this.direccion = direccion;
        this.mascotas = new ArrayList<>();
    }

    public String getTelefono()
    {
        return telefono;
    }

    public String getDireccion()
    {
        return direccion;
    }

    public ArrayList<Mascota> getMascotas()
    {
        return mascotas;
    }

    public void agregarMascota(Mascota mascota)
    {
        mascotas.add(mascota);
    }

    public String obtenerRol()
    {
        return "Cliente";
    }

    public void mostrarInformacion()
    {
        System.out.println("[" + obtenerRol() + "] " + getId() + " - " + getNombre() + " (" + getCorreo() + ")");
        System.out.println("    Telefono: " + telefono + " | Direccion: " + direccion + " | Mascotas: " + mascotas.size());
    }
}
