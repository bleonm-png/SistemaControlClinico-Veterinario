# Sistema de Control Clinico Veterinario

Proyecto de diseno y desarrollo de un sistema de control clinico veterinario: diagramas UML (flujo, casos de uso, clases) y su implementacion en C# y Java.

## Contenido

- **Diagramas de flujo**: diagrama de flujo del ciclo de vida de un ticket (caso clinico), de Abierto a Cerrado, con la ruta de escalamiento a Emergencia.
- **Diagramas de caso de uso**: Crear Ticket, Asignar Ticket, Resolver Ticket, Escalar Ticket, Generar Metricas, Consultar Historial Clinico.
- **Diagramas de clase**: diagrama de clases del sistema.
- **ClinicaVeterinaria**: proyecto en C# (.NET), con las clases del sistema en la carpeta `logica`.
- **BlueJ_Proyecto**: version en Java del mismo modelo de clases, para el entorno BlueJ.
- **ANALISIS.md**: analisis completo del sistema.

## Casos de uso

1. **Crear Ticket**: el cliente reporta un problema de salud de su mascota (motivo, sintomas, tipo de consulta, prioridad); el sistema numera el ticket e incluye la asignacion automatica de un veterinario.
2. **Asignar Ticket**: el sistema asigna (o reasigna) el ticket al veterinario disponible con menor carga que pueda atender el tipo de consulta (misma especialidad o General), o de forma manual a un veterinario especifico por codigo.
3. **Resolver Ticket**: el veterinario registra el diagnostico y el tratamiento aplicado; el sistema valida que el ticket este Asignado o Escalado antes de resolverlo.
4. **Escalar Ticket**: sube la prioridad del ticket a Emergencia y lo reasigna al veterinario de mayor experiencia disponible; ocurre manualmente o automaticamente al registrar un hallazgo clinico de gravedad Alta o Critica.
5. **Generar Metricas**: el administrador obtiene indicadores de tickets por estado, por prioridad, tickets escalados y carga por veterinario.
6. **Consultar Historial Clinico**: el cliente o el veterinario buscan un ticket por numero y revisan el historial completo de eventos registrados.


