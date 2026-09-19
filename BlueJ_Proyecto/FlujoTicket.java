import java.util.ArrayList;
import java.util.HashMap;

/**
 * Valida las transiciones de estado permitidas para un ticket (consulta).
 */
public class FlujoTicket
{
    private HashMap<String, ArrayList<String>> transiciones;

    public FlujoTicket()
    {
        transiciones = new HashMap<>();

        ArrayList<String> desdeAbierto = new ArrayList<>();
        desdeAbierto.add("Asignado");
        transiciones.put("Abierto", desdeAbierto);

        ArrayList<String> desdeAsignado = new ArrayList<>();
        desdeAsignado.add("Escalado");
        desdeAsignado.add("Resuelto");
        transiciones.put("Asignado", desdeAsignado);

        ArrayList<String> desdeEscalado = new ArrayList<>();
        desdeEscalado.add("Asignado");
        desdeEscalado.add("Resuelto");
        transiciones.put("Escalado", desdeEscalado);

        ArrayList<String> desdeResuelto = new ArrayList<>();
        desdeResuelto.add("Cerrado");
        desdeResuelto.add("Asignado");
        transiciones.put("Resuelto", desdeResuelto);

        transiciones.put("Cerrado", new ArrayList<>());
    }

    public boolean puedeCambiarEstado(String estadoActual, String estadoNuevo)
    {
        return transiciones.containsKey(estadoActual) && transiciones.get(estadoActual).contains(estadoNuevo);
    }

    public void mostrarFlujo()
    {
        System.out.println("Flujo: Abierto -> Asignado -> Escalado -> Resuelto -> Cerrado");
        for (String estado : transiciones.keySet())
        {
            ArrayList<String> destinos = transiciones.get(estado);
            String texto = destinos.isEmpty() ? "(estado final)" : String.join(", ", destinos);
            System.out.println("  " + estado + " -> " + texto);
        }
    }
}
