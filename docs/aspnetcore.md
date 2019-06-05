# Esquio on ASP.NET Core

*Esquio* ha sido desde un principio pensado para ser usado en nuestros proyectos de ASP.NET Core, tanto para los desarrollos con MVC, Razor como para aplicaciones SPA con el backend en .NET Core. Para ello, se han creado un conjunto de extensiones que nos permitirán trabajar con *Esquio* de una forma sencilla, desde **TagHelpers**, **Filtros** hasta nuevos servicios que iremos presentando a continuación.

> En samples/WebApp puede encontrar un ejemplo completo del uso de *Esquio* en AspNetCore.

## Setup 

Para integrar *Esquio* en ASP.NET Core empezaremos por instalar el paquete de NuGet Esquio.AspNetCore.

>**[CLI]** dotnet package add Esquio.AspNetCore 

>**[PowerShell]** install-package Esquio.AspNetCore

Una vez instalado, incluiremos los servicios de *Esquio* dentro del contenedor de dependencias de AspNetCore.

```csharp
             sevices
                .AddEsquio()
                .AddAspNetCoreDefaultServices()
                .AddConfigurationStore(Configuration, "Esquio")
```

Los métodos **AddEsquio** y **AddAspNetCoreDefaultServices** nos permiten agregar el conjunto de servicios que *Esquio* necesita para funcionar. 

A mayores, el método **AddConfigurationStore** registra el almacén de configuración a utilizar, en este caso basado en el sistema de configuración por defecto de AspNetCore.

> Puede leer más sobre  los diferentes almancenes de configuración de *Esquio* en [Configuración de Esquio](./configuration.md).

## Primeros pasos

Despues de agregar los diferentes servicios de *Esquio*  ya podremos empezar a utilizarlo en nuestros aplicaciones AspNetCore. Haremos aquí un pequeño repaso de las diferentes opciones que tenemos.

### Action Filters

**Esquio.AspNetCore** nos proporciona diferentes Action Filters que nos permitirán agregar condiciones a las acciones que se pueden ejecutar en nuestros controladores habilitando asi características de una forma sencilla.

El filtro **ActionFilter** nos permite deshabilitar una acción de un controlador en base al estado de una o multiples Feature.

```csharp
    public class DemoController : Controller
    {
        [FeatureFilter(Names = "TheFeatureName")]
        public IActionResult SomeAction()
        {
            return View();
        }
    }
```

La propiedad **Names** del atributo **FeatureFilter** nos permitirá establecer una o más Features que tendrán que estar activas para que esta accion se pueda ejecutar.

> En el caso de especificar multiples Feature sepárelas usando el carácter ';'.

En el caso de que alguna de las Feature no esté activa por defecto el request devolverá un *404 NotFound* como resultado de la llamada. En el caso de querer cambiar este resultado puede hacer uso del método **AddMvcFallbackAction** que *Esquio* nos proporciona. A continuación vemos como devolver una redirección