# Sistema de Gestión de Direcciones en .NET

## Tabla de Contenidos
- [Introducción](#introducción)
- [Instalación](#instalación)
- [Adiciones](#adiciones)
- [Ideas Inconclusas](#ideas-inconclusas)

## Introducción
Hola profe, este es mi intento de integrar addresses al proyecto base, este proyecto usa MySQL (más adelante podrá ver cómo instalarlo).

Lamentablemente no puedo hostearlo así que te doy todas las indicaciones para que lo puedas instalar y correr.

Ante cualquier error contácteme.

Muchas gracias, Maximiliano Maese.

## Instalación
Para instalar y ejecutar este proyecto, siga los siguientes pasos:
1. Clone el repositorio con `git clone https://github.com/mxmaese/asp.net_final_project.git`.
2. Ejecute `dotnet restore` en la carpeta final Project dentro del proyecto clonado para instalar las dependencias.
3. [Aquí](https://github.com/mxmaese/asp.net_final_project/blob/main/Data%20base/test_final_project.sql) puede encontrar el SQL para importar la base de datos.
4. Configure el Usuario, Contraseña, Base de datos y Host.
5. Inicie la aplicación con `dotnet run`.

## Adiciones
- **Sistema de Direcciones:** Puede crear, leer, actualizar y eliminar direcciones.
- **Sistema "AntiFraude":** Evita que las personas puedan crear o editar direcciones sin estar logueados o que estando logueados no puedan editar los de otras personas.
- **Sistema de Registro de Usuario/Empleado:** Puede registrarse como usuario para poder tener acceso a las direcciones o como Empleado para seleccionar tipos de perfiles.
- **Validación de Datos Frontend y Backend:** Valida los datos ingresados en el cliente y el servidor para asegurar que no haya ninguna inconcordancia.
- **Validación de Datos vía API:** Asegura la precisión de los datos de direcciones ingresados.
- **Dualidad para Elegir la Dirección:** Permite al usuario elegir entre ingresar los datos por medio de campos o por un mapa interactivo.
- **Sistema de Errores Personalizado:** Al aparecer errores te muestra una pantalla personalizada.

## Ideas Inconclusas
- **Mejorar el Sistema de Errores:** La idea era que aparezca arriba a la derecha una vista parcial que muestra el error más bonito.
- **Error al Loguearse:** Que cuando insertas mal la contraseña aparezca un error ahí y que no te lleve a otra vista.
