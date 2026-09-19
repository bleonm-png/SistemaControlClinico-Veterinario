import java.util.ArrayList;
import java.util.Date;

/**
 * Historial clinico de eventos de un ticket.
 */
public class Bitacora
{
    private ArrayList<String> registros;

    public Bitacora()
    {
        registros = new ArrayList<>();
    }

    public void registrarEvento(String evento)
    {
        registros.add("[" + new Date() + "] " + evento);
    }

    public ArrayList<String> getRegistros()
    {
        return registros;
    }

    public void mostrarBitacora()
    {
        System.out.println("  Historial clinico:");
        if (registros.isEmpty())
        {
            System.out.println("    (sin eventos registrados)");
            return;
        }
        for (String evento : registros)
        {
            System.out.println("    - " + evento);
        }
    }
}
