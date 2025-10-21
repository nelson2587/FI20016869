# Proyecto PP3 – Minimal API con .NET 8  
**Curso:** Programación Avanzada en Web  
**Estudiante:** Nelson Rodriguez Lopez
** Carnet:** FI20016869
**Universidad:** Fidélitas  
**Lenguaje:** C#  
**Entorno:** Visual Studio Code  
**Framework:** .NET 8.0  

---

## 🎯 Objetivo del Proyecto
Aplicar los conocimientos adquiridos al utilizar un **Minimal API** con la herramienta del **Framework ASP.NET Core (.NET 8.0)**.  
El sistema expone cuatro endpoints principales:

- `/` → Redirige al Swagger UI.  
- `/include` → Inserta una palabra en una posición específica.  
- `/replace` → Reemplaza palabras según su longitud.  
- `/erase` → Elimina palabras según su longitud.  

Cada endpoint puede devolver resultados en formato **JSON** o **XML**, según el encabezado `xml: true` del request.

---

## ⚙️ Requisitos previos

Antes de comenzar, asegurarse de tener instalado:

1. **.NET SDK 8.0 o superior**  
   Descargar desde [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)

   Verificar la instalación con:  
   ```bash
   dotnet --version
   ```
   Si devuelve algo como `8.0.100` o superior, está correcto.

2. **Visual Studio Code**  
   Descargar desde [https://code.visualstudio.com/](https://code.visualstudio.com/)

   Instalar las extensiones recomendadas:
   - `C# Dev Kit`
   - `C# Extensions`
   - `NuGet Package Manager GUI`

3. **Postman (opcional)**  
   Para realizar pruebas HTTP/REST con los endpoints.

---

## 🧩 Estructura del Proyecto

```
PP3/
 ├── MyProject/
 │   ├── MyProject.csproj
 │   ├── Program.cs
 │   └── Properties/
 │       └── launchSettings.json
 ├── MySolution.sln
 └── README.md
```

---

## 🛠️ Configuración paso a paso

### 1️⃣ Crear la solución y el proyecto
En la terminal:
```bash
dotnet new sln -n PP3
dotnet new web -n MyProject
dotnet sln add MyProject/MyProject.csproj
```

### 2️⃣ Instalar Swagger
```bash
cd MyProject
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
```

---

## 🧰 Código principal (`Program.cs`)

Asegúrese de tener el código **completo y actualizado** proporcionado por el profesor o el ejemplo funcional.  
**Importante:** El archivo **no debe contener** esta línea:
```csharp
using Microsoft.AspNetCore.OpenApi;
```
Swagger funciona únicamente con:
```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
app.UseSwagger();
app.UseSwaggerUI();
```

---

## 🚫 Errores comunes y cómo resolverlos

### ❌ Error CS0234  
> “El tipo o el nombre del espacio de nombres 'OpenApi' no existe en el espacio de nombres 'Microsoft.AspNetCore'”

**Solución:**
- Eliminar esta línea al inicio del archivo `Program.cs`:
  ```csharp
  using Microsoft.AspNetCore.OpenApi;
  ```

---

### ❌ Error CS1503  
> “Argumento 3: no se puede convertir de ‘string’ a ‘System.Type?’” o viceversa.

**Causa:** El método `.Produces()` no coincide con las sobrecargas de .NET 8.  
**Solución:**  
- Eliminar o simplificar las llamadas:
  ```csharp
  .Produces(typeof(ResultDto), 200)
  .Produces(typeof(object), 400)
  ```

---

### ❌ Error con `WithOpenApi`
> “RouteHandlerBuilder does not contain a definition for 'WithOpenApi'”

**Solución:**
- Eliminar todas las llamadas a `.WithOpenApi(...)`.  
- Swagger seguirá funcionando normalmente con las líneas base mostradas arriba.

---

## ▶️ Cómo ejecutar el proyecto

1. Abrir **Visual Studio Code** en la carpeta del proyecto:
   ```bash
   cd C:\Users\nelson\PP3
   code .
   ```

2. Ejecutar los siguientes comandos en la terminal:
   ```bash
   dotnet clean
   dotnet build
   dotnet run
   ```

3. Esperar el mensaje:
   ```
   Now listening on: https://localhost:5001
   ```
4. Abrir el navegador y acceder a:
   👉 [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## 🧪 Cómo probar los endpoints

### 1️⃣ `/include`
**Método:** POST  
**Ruta:** `/include/{position}`

Ejemplo:
```
POST https://localhost:5001/include/0?value=Hello
Form-data: text=This is a test
```

Resultado JSON:
```json
{
  "ori": "This is a test",
  "new": "Hello This is a test"
}
```

---

### 2️⃣ `/replace`
**Método:** PUT  
**Ruta:** `/replace/{length}`

Ejemplo:
```
PUT https://localhost:5001/replace/4?value=hi
Form-data: text=This test will show replace
```

Resultado JSON:
```json
{
  "ori": "This test will show replace",
  "new": "hi hi will show replace"
}
```

---

### 3️⃣ `/erase`
**Método:** DELETE  
**Ruta:** `/erase/{length}`

Ejemplo:
```
DELETE https://localhost:5001/erase/4
Form-data: text=This test will show erase
```

Resultado JSON:
```json
{
  "ori": "This test will show erase",
  "new": "will show erase"
}
```

---

## 💡 Consejos de estudiante

✅ Usa `dotnet clean` y `dotnet build` cada vez que modifiques el código.  
✅ Si Swagger no abre, revisa el puerto (`http://localhost:5000` o `https://localhost:5001`).  
✅ Guarda el archivo antes de ejecutar `dotnet run`.  
✅ Si aparecen errores `CS0234` o `CS1503`, revisa los `using` al inicio del archivo.  

---

## 💬 Conclusión

Este proyecto permite comprender la estructura de un **Minimal API** en ASP.NET Core, trabajando con parámetros, validaciones, y respuestas en formato JSON y XML.  
Además, enseña a resolver los errores más frecuentes que ocurren al programar en **Visual Studio Code con .NET 8**.

---
¿Es posible enviar valores en el Body (por ejemplo, en el Form) del Request de tipo GET?

Técnicamente se puede poner datos en el body de un GET, pero no es recomendado y la mayoría de servidores (como en ASP.NET Core) no lo procesan. Lo mejor es usar POST o PUT si para enviar datos al servidor.

¿Qué ventajas y desventajas observa con el Minimal API si se compara con la opción de utilizar Controllers?

Las Minimal APIs son para proyectos pequeños o prototipos rápidos porque el código es más simple y directo. Pero si el sistema crece, los Controllers son mejor opción porque permiten organizar mejor el código, aplicar validaciones, filtros, seguridad y las versiones. 



---

**Autor:**  
👨‍💻 *Nelson Rodriguez Lopez*  
Estudiante de Ingeniería en Sistemas – Universidad Fidélitas  
Costa Rica 
