# Sistema de Gestión de Direcciones en .NET

## Tabla de Contenidos
- [Introducción](#introducción)
- [Instalación](#instalación)
- [Adiciones](#adiciones)
- [Ideas Inconclusas](#ideas-inconclusas)
## Introducción
Hola profe, este es mi intengo de integrar addresses al proyecto base, este proyecto usa Mysql (mas adelante podra ver como instalarlo).

Lamentablemente no puede hostearlo asi que te doy todas las indicaciones para que lo puedas instalar y correr

Ante cualquier error contacteme

Muchas gracias, Maximiliano Maese

## Instalación
Para instalar y ejecutar este proyecto, siga los siguientes pasos:
1. Clone el repositorio con `git clone https://github.com/mxmaese/asp.net_final_proyect.git`.
2. Ejecute `dotnet restore` en la carpeta final Proyect dentro del proyecto clonado para instalar las dependencias.
3. [Aquí](https://github.com/mxmaese/asp.net_final_proyect/blob/main/Data%20base/test_final_proyect.sql) puede encontrar el sql para importar la base de datos
4. Configure el Usuario,Contraceña,Base de datos y Host
5. Inicie la aplicación con `dotnet run`.

## Adiciones
- **Sistema de addresses:** Puede crear, leer, actualizar y eliminar direcciones.
- **Sistema "AntiFraude":** Evita que las personas puedan crear o editar addresses sin estar logueados o que estando logueados no puedas editar los de otras personas.
- **Sistema de registro de Usuario/Empleado:**: Puede registrarse como usuario para poder tener acceso a los address o como Empleado para seleccionar tipos de perfiles.
- **Validación de Datos Frontend y Backend:** Valida los datos ingresados en el cliente y el servidor para ver que no haya ninguna inconcordancia.
- **Validación de Datos via API:** Asegura la precisión de los datos de direcciones ingresados.
- **Dualidad para elejir la direccion:** Permite al usuario elejir entre ingresar los datos por medio de campos o por un mapa interactivo.
- **Sistema de errores personalizado:** Al aparecer errores te muestra una pantalla personalizada.

## Ideas inconclusas
- **Mejorar el sistema de errorres:** La idea era que aparezca arriba a la derecha una vista parcial que muestra el error mas bonito
- **Error al loguearse:** Que cuando insertas mal la contraceña aparezca un error ahi y que no te lleve a otra vista no me muestra Ideas inconclusas en la tabla de contenidos y como hago un link a una parte del repositorio.
- **Error al loguearse:** Que cuando insertas mal la contraceña aparezca un error ahi y que no te lleve a otra vista.
