# Analisis del Sistema de Control Clinico Veterinario

## 1. Introduccion

Una clinica veterinaria necesita llevar control de las consultas que solicitan sus clientes para sus mascotas: quien la solicito, que veterinario la atendio, que se encontro y que tratamiento se aplico, y en que estado se encuentra cada caso. Este proyecto disena e implementa un sistema que administra ese ciclo de vida completo usando el modelo de "ticket" (uno por consulta o caso clinico), inspirado en un sistema de tickets de soporte tecnico pero adaptado al dominio veterinario.

## 2. Objetivos

### 2.1 Objetivo general

Disenar e implementar un sistema que permita registrar, asignar, atender, escalar y cerrar consultas veterinarias (tickets), manteniendo un historial clinico auditable y generando metricas de operacion para la clinica.

### 2.2 Objetivos especificos

- Modelar los actores del sistema (clientes, veterinarios, administrador) y las entidades del dominio (mascota, ticket, bitacora) mediante un diagrama de clases orientado a objetos.
- Definir el ciclo de vida de un ticket como una maquina de estados explicita y validada.
- Implementar el sistema en C# (.NET) con una interfaz de consola, y una version equivalente en Java para su uso educativo en BlueJ.
- Documentar el analisis mediante diagramas de flujo y de casos de uso.

## 3. Alcance

El sistema cubre:

- Registro de clientes, mascotas y veterinarios (datos de demostracion cargados al iniciar el programa).
- Creacion de tickets (consultas) asociados a una mascota, con asignacion automatica de veterinario.
- Asignacion manual o automatica de veterinario, segun especialidad y carga de trabajo.
- Registro de hallazgos clinicos durante la atencion, con escalamiento automatico si la gravedad es Alta o Critica.
- Escalamiento manual de un ticket (aumento de prioridad y reasignacion a un veterinario de mayor experiencia).
- Resolucion del ticket (diagnostico y tratamiento) y cierre del caso.
- Consulta del historial (bitacora) de eventos de un ticket.
- Generacion de metricas operativas (tickets por estado, por prioridad, tickets escalados, carga por veterinario).

Queda fuera del alcance: persistencia en base de datos, interfaz grafica/web, autenticacion de usuarios y facturacion; el sistema es una aplicacion de consola con datos en memoria, orientada a demostrar el modelo de dominio y las reglas de negocio.

## 4. Actores del sistema

| Actor | Descripcion |
|---|---|
| **Cliente** | Dueno de una o mas mascotas. Solicita consultas (crea tickets) y puede consultar el historial clinico de sus mascotas. |
| **Veterinario** | Atiende los tickets que se le asignan: registra hallazgos, escala casos graves y resuelve el ticket con un diagnostico y tratamiento. |
| **Gestor de Consultas (sistema)** | Actor interno: representa la logica del sistema (`GestorClinica`) que asigna tickets a veterinarios automaticamente segun especialidad y carga. |
| **Administrador** | Consulta las metricas operativas de la clinica. |

## 5. Requerimientos funcionales

| Codigo | Requerimiento |
|---|---|
| RF-01 | El sistema debe permitir registrar mascotas asociadas a un cliente existente. |
| RF-02 | El sistema debe permitir crear un ticket para una mascota, indicando motivo, sintomas, tipo de consulta y prioridad. |
| RF-03 | Al crear un ticket, el sistema debe intentar asignarlo automaticamente a un veterinario disponible cuya especialidad coincida con el tipo de consulta, o que tenga especialidad "General". |
| RF-04 | El sistema debe permitir asignar o reasignar manualmente un ticket a un veterinario especifico por codigo. |
| RF-05 | El sistema debe permitir registrar hallazgos clinicos sobre un ticket, indicando tipo, descripcion y gravedad. |
| RF-06 | Si un hallazgo tiene gravedad Alta o Critica, el sistema debe escalar el ticket automaticamente. |
| RF-07 | El sistema debe permitir escalar manualmente un ticket, elevando su prioridad a Emergencia y reasignandolo al veterinario de mayor nivel de experiencia disponible para esa especialidad. |
| RF-08 | El sistema debe permitir resolver un ticket registrando diagnostico y tratamiento, solo si el ticket esta en estado Asignado o Escalado. |
| RF-09 | El sistema debe permitir cerrar un ticket solo si esta en estado Resuelto. |
| RF-10 | El sistema debe validar toda transicion de estado contra un flujo de estados definido, rechazando transiciones no permitidas. |
| RF-11 | El sistema debe permitir consultar el historial completo (bitacora) de eventos de un ticket. |
| RF-12 | El sistema debe permitir generar metricas: tickets por estado, tickets por prioridad, tickets escalados y carga de trabajo por veterinario. |

## 6. Casos de uso (descripcion narrativa)

Los diagramas correspondientes estan en la carpeta `Diagramas de caso de uso` (un diagrama general y uno por cada caso de uso).

### CU-01 Crear Ticket

- **Actor principal:** Cliente.
- **Precondicion:** El cliente y la mascota ya estan registrados en el sistema.
- **Flujo principal:**
  1. El cliente selecciona la mascota para la cual solicita la consulta.
  2. Ingresa motivo, sintomas, tipo de consulta y prioridad.
  3. El sistema numera el ticket consecutivamente y registra su creacion en la bitacora.
  4. El sistema intenta asignar automaticamente un veterinario disponible (*«include» Asignar Ticket*).
- **Flujo alternativo:** Si no hay veterinario disponible, el ticket queda en estado Abierto sin asignar.
- **Excepcion:** Si la seleccion de mascota esta fuera de rango, el sistema notifica el error y no crea el ticket.

### CU-02 Asignar Ticket

- **Actor principal:** Gestor de Consultas (sistema); puede iniciarse tambien manualmente.
- **Flujo principal:**
  1. Se busca el ticket por numero.
  2. Se busca el veterinario con menor carga actual que pueda atender el tipo de consulta (misma especialidad o "General").
  3. Se asigna el veterinario al ticket, se incrementa su carga y el ticket pasa a estado Asignado.
  4. Se registra el evento en la bitacora del ticket.
- **Flujo alternativo:** Puede indicarse manualmente el codigo de un veterinario especifico en lugar de la busqueda automatica.
- **Excepciones:** Ticket no encontrado; no hay veterinario disponible para esa especialidad (el ticket queda sin asignar).

### CU-03 Resolver Ticket

- **Actor principal:** Veterinario.
- **Precondicion:** El ticket esta en estado Asignado o Escalado (validado por `FlujoTicket`).
- **Flujo principal:**
  1. Se busca el ticket por numero.
  2. El veterinario puede registrar hallazgos adicionales (*«include» Registrar hallazgo en ticket*).
  3. Se valida que el cambio de estado a Resuelto sea permitido.
  4. Se ingresa el diagnostico y el tratamiento aplicado; el ticket pasa a estado Resuelto.
  5. El caso puede cerrarse a continuacion (*«include» Cerrar ticket*).
- **Excepciones:** Ticket no encontrado; intento de resolver un ticket en un estado que no lo permite (p. ej. Abierto o Cerrado).
- **Extension:** Reabrir ticket (vuelve a estado Asignado) si el cliente indica que la mascota no se recupero.

### CU-04 Escalar Ticket

- **Actor principal:** Veterinario; tambien puede ocurrir automaticamente (ver RF-06).
- **Precondicion:** El ticket esta en un estado desde el cual se permite escalar (Abierto no lo permite; Asignado o Escalado si).
- **Flujo principal:**
  1. Se busca el ticket por numero.
  2. Se valida el cambio de estado a Escalado.
  3. Se eleva la prioridad del ticket a Emergencia y se deja constancia del motivo.
  4. Se reasigna el ticket al veterinario con mayor nivel de experiencia compatible con el tipo de consulta (*«include» Asignar Ticket*).
- **Excepciones:** Ticket no encontrado; cambio de estado invalido.

### CU-05 Generar Metricas

- **Actor principal:** Administrador.
- **Flujo principal:** El sistema calcula y muestra: total de tickets, tickets por cada estado, tickets por cada prioridad, cantidad de tickets escalados y la carga (tickets asignados / capacidad maxima) de cada veterinario.

### CU-06 Consultar Historial Clinico

- **Actores principales:** Cliente, Veterinario.
- **Flujo principal:**
  1. Se busca el ticket por numero.
  2. Se muestra el resumen del ticket (mascota, tipo de consulta, prioridad, estado, diagnostico si existe).
  3. Se listan todos los eventos registrados en su bitacora, en orden cronologico.
- **Excepcion:** Ticket no encontrado.

## 7. Diagrama de flujo

Ver `Diagramas de flujo/Diagrama_Flujo.drawio`. Representa el ciclo de vida completo de un ticket desde que el cliente reporta el problema de su mascota hasta el cierre del caso y la generacion de metricas, incluyendo los puntos de decision para asignacion, escalamiento y confirmacion de recuperacion por parte del cliente.

## 8. Diagrama de clases

Ver `Diagramas de clase/Diagrama_Clases.drawio` (y su equivalente funcional en `BlueJ_Proyecto`, mediante `package.bluej`).

### 8.1 Clases principales

- **`Persona` (abstracta):** superclase de `Cliente` y `Veterinario`. Define identidad (`strId`, `strNombre`, `strCorreo`) y el metodo abstracto `obtenerRol()`.
- **`Cliente`:** dueno de mascotas; datos de contacto (telefono, direccion) y su lista de mascotas.
- **`Veterinario`:** especialidad, nivel de experiencia y capacidad de atencion (carga actual / carga maxima). Determina si puede atender un tipo de consulta.
- **`Mascota`:** especie, raza, edad y su dueno (`Cliente`).
- **`Ticket`:** el caso clinico. Contiene los datos de la consulta, su estado, su prioridad, el diagnostico/tratamiento una vez resuelto, y una `Bitacora` propia (composicion).
- **`Bitacora`:** historial de eventos con marca de tiempo, asociado 1 a 1 (por composicion) a cada `Ticket`.
- **`FlujoTicket`:** valida las transiciones de estado permitidas (tabla de transiciones).
- **`GestorClinica`:** clase controladora / fachada del sistema; mantiene las listas de veterinarios, clientes, mascotas y tickets, y coordina las reglas de negocio (asignacion automatica, validacion de flujo, metricas).

### 8.2 Relaciones principales

- Herencia: `Cliente` y `Veterinario` heredan de `Persona`.
- Asociacion: `Mascota` — `Cliente` (muchas mascotas, un dueno); `Ticket` — `Mascota` (muchos tickets, una mascota); `Ticket` — `Veterinario` (muchos tickets, un veterinario asignado como maximo).
- Composicion: `Ticket` ◆— `Bitacora` (la bitacora no existe sin su ticket).
- Asociacion/agregacion: `GestorClinica` mantiene las colecciones de `Veterinario`, `Cliente`, `Mascota` y `Ticket`.
- Dependencia: `GestorClinica` usa `FlujoTicket` para validar cada cambio de estado.

## 9. Maquina de estados del ticket

| Estado actual | Estados destino permitidos |
|---|---|
| Abierto | Asignado |
| Asignado | Escalado, Resuelto |
| Escalado | Asignado, Resuelto |
| Resuelto | Cerrado, Asignado (reapertura) |
| Cerrado | (estado final, sin transiciones) |

Esta tabla se implementa identicamente en `FlujoTicket` (C#) y `FlujoTicket` (Java), de modo que ambas versiones del sistema comparten exactamente las mismas reglas de negocio.

## 10. Decisiones de diseno

- **Validacion centralizada del flujo:** a diferencia de dejar que cada clase `Ticket` valide sus propias transiciones, la validacion se centraliza en `GestorClinica` usando `FlujoTicket`. Esto evita que un estado invalido se genere por una via distinta a la clase controladora, y mantiene a `Ticket` como una clase mas simple (solo ejecuta la accion y registra el evento).
- **Escalamiento automatico:** un hallazgo clinico de gravedad Alta o Critica dispara el escalamiento automaticamente (RF-06), igual que en el sistema de referencia (tickets de soporte) un error de impacto alto disparaba la escalacion. Esto refleja una regla de negocio real: un hallazgo grave no deberia depender de que el veterinario recuerde escalar manualmente.
- **Asignacion por menor carga:** el algoritmo de asignacion automatica prioriza al veterinario disponible con menor carga actual entre quienes pueden atender la especialidad solicitada (o "General"), buscando distribuir el trabajo de forma equilibrada.
- **Reasignacion en escalamiento:** al escalar, el sistema busca al veterinario de mayor nivel de experiencia compatible con la especialidad, ya que un caso de emergencia amerita ser revisado por el profesional mas experimentado disponible.
- **Paridad C# / Java:** ambas implementaciones comparten exactamente el mismo modelo de clases, la misma nomenclatura (traducida a la convencion de cada lenguaje) y las mismas reglas de negocio, para que el diagrama de clases sea valido para ambas versiones del codigo.

## 11. Entregables de este proyecto

1. Este documento de analisis (`ANALISIS.md`).
2. Diagramas de flujo y de casos de uso en formato draw.io (`Diagramas de flujo/`, `Diagramas de caso de uso/`).
3. Diagrama de clases en formato draw.io (`Diagramas de clase/`) y version equivalente para BlueJ (`BlueJ_Proyecto/`).
4. Codigo fuente en C# (`ClinicaVeterinaria/`), compilable con `dotnet build` / ejecutable con `dotnet run`.
5. Codigo fuente en Java (`BlueJ_Proyecto/`), abrible directamente en BlueJ.
6. Repositorio publicado en GitHub con todas las carpetas anteriores.
7. Video explicando la funcionalidad del codigo: pendiente de grabar.
