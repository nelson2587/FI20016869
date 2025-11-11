# 📘 Proyecto BooksConsole

## 👤 Información del estudiante
**Nombre:** Nelson Rodriguez Lopez

**Carné:** FI20016869


---


## ⚙️ Comandos utilizados (CLI .NET)

Durante el desarrollo del proyecto se emplearon los siguientes comandos en la **CLI de .NET** desde Visual Studio Code:

```bash
# Crear la solución y el proyecto
dotnet new sln -n BooksSolution
dotnet new console -n BooksConsole
dotnet sln add .\BooksConsole\BooksConsole.csproj

# Agregar paquetes de Entity Framework Core y SQLite
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0

# Restaurar dependencias y compilar
dotnet restore
dotnet build

# Crear y aplicar migraciones
dotnet ef migrations add InitialCreate
dotnet ef database update

# Ejecutar la aplicación
dotnet run
```

---

## 🌐 Fuentes y sitios web consultados

Durante el desarrollo se consultaron las siguientes páginas para resolver errores, entender comandos y aplicar configuraciones:

- [Documentación oficial de .NET CLI](https://learn.microsoft.com/es-es/dotnet/core/tools/)
- [Documentación de Entity Framework Core](https://learn.microsoft.com/es-es/ef/core/)
- [Tutorial de EF Core con SQLite (Microsoft Docs)](https://learn.microsoft.com/es-es/ef/core/get-started/overview/first-app)
- [Stack Overflow - SQLite Error 1: 'table already exists'](https://stackoverflow.com/questions/24100656/sqlite-error-1-table-already-exists)
- [C# CSV File Reader Example (CSVHelper)](https://joshclose.github.io/CsvHelper/)
- [Docs de Visual Studio Code - .NET Setup](https://code.visualstudio.com/docs/languages/dotnet)

---

## 🤖 Prompts utilizados con Chatbots de IA

Durante el desarrollo se utilizaron herramientas de inteligencia artificial para resolver errores y generar código compatible con **.NET 8.0**, **Entity Framework Core 9.0** y **SQLite 3**.

### 🔹 ChatGPT (OpenAI)
**Enlace de consulta:**  
[https://chat.openai.com/](https://chat.openai.com/)

**Prompts utilizados:**
- *“BooksConsole error con 5 errores (14,7s)… ¿cómo solucionarlo?”*  
  → ChatGPT indicó que faltaban referencias a las clases `Title` y `Tag` y explicó cómo estructurar correctamente las entidades en la carpeta `Models`.
  
- *“Error SQLite 1: ‘no such table: Authors’”*  
  → Explicó que se debía ejecutar los comandos `dotnet ef migrations add InitialCreate` y `dotnet ef database update` para crear el esquema antes de ejecutar `dotnet run`.

- *“Configurar proyecto para usar .NET 8.0 en lugar de 9.0”*  
  → Recomendó modificar el `TargetFramework` en el archivo `.csproj` a `net8.0`.

- *“Cómo generar archivos TSV a partir de una base de datos en C#”*  
  → ChatGPT proporcionó un ejemplo usando `StreamWriter` y la función `string.Join("\t", valores)` para generar los archivos `.tsv`.

---
¿Cómo cree que resultaría el uso de la estrategia de Code First para crear y actualizar una base de datos de tipo NoSQL (como por ejemplo MongoDB)? ¿Y con Database First? ¿Cree que habría complicaciones con las Foreign Keys?

El uso de la estrategia Code First en bases de datos NoSQL como MongoDB sería funcional, pero con limitaciones, ya que este tipo de bases no manejan esquemas estrictos ni llaves foráneas. Con Code First se podrían definir clases y estructuras desde el código, pero las “migraciones” no serían automáticas como en Entity Framework, sino que habría que realizarlas manualmente mediante scripts o procesos personalizados. En cambio, con Database First, donde el modelo se genera a partir de la base de datos existente, sería más complicado porque MongoDB no tiene un esquema fijo, por lo que obtener un modelo consistente sería difícil. Además, en NoSQL no existen las Foreign Keys, por lo que las relaciones se deben manejar desde la lógica de la aplicación mediante referencias o documentos embebidos, lo que puede generar problemas de consistencia o duplicación de datos.

---
¿Cuál carácter, además de la coma (,) y el Tab (\t), se podría usar para separar valores en un archivo de texto con el objetivo de ser interpretado como una tabla (matriz)? ¿Qué extensión le pondría y por qué? Por ejemplo: Pipe (|) con extensión .pipe.

En cuanto a los separadores de archivos de texto, además de la coma (,) y el tabulador (\t), se puede usar el pipe (|), ya que es un carácter poco común en textos y facilita la lectura. A este tipo de archivos se le podría asignar la extensión .psv (pipe-separated values), porque describe claramente el formato y mantiene compatibilidad con herramientas de análisis y hojas de cálculo.

---
## 🧠 Conclusión

Este proyecto permitió aplicar los conocimientos del patrón **Code First** con **Entity Framework Core 9.0**, la creación y migración de bases de datos **SQLite 3**, y la lectura/escritura de archivos **CSV/TSV** utilizando el **framework .NET 8.0**.  
El uso de herramientas como **Visual Studio Code**, **DB Browser for SQLite** y **chatbots de IA** facilitó la resolución de errores, la automatización de migraciones y la generación del código de forma más eficiente.

---

📅 **Última actualización:** 9 de noviembre de 2025
