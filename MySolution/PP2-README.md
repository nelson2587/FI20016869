# Práctica Programada 2
**Tema:** Operaciones binarias, aritmética básica y cambio de bases (BIN/OCT/DEC/HEX)  
--

## 👤 Autor
- **Nombre:** Nelson Rodriguez Lopez 
- **Carné:**  FI20016869


---

## Q1) ¿Qué necesito instalar antes de empezar?
**R:**  
- **.NET 8 SDK:** https://dotnet.microsoft.com/download/dotnet/8.0  
- **Visual Studio Code** + extensiones **C#** (o *C# Dev Kit*).  
- (Opcional) Git para versionamiento.

Para comprobar:
```bash
dotnet --version  # Debe mostrar 8.x
```

---

## Q2) ¿Cómo creo la solución y el proyecto MVC?
**R:** En una carpeta de trabajo (por ejemplo, `PP2`):
```bash
mkdir PP2 && cd PP2
dotnet new sln -n MySolution
dotnet new mvc -n MyProject
dotnet sln MySolution.sln add MyProject/MyProject.csproj
```

---

## Q3) ¿Cómo debe quedar la estructura de carpetas?
**R:**
```
PP2/
├─ MySolution.sln
└─ MyProject/
   ├─ Controllers/
   │  └─ HomeController.cs
   ├─ Models/
   │  ├─ BinaryStringAttribute.cs
   │  ├─ BinaryInput.cs
   │  ├─ NumberBases.cs
   │  ├─ BinaryResults.cs
   │  └─ BinaryPageViewModel.cs
   ├─ Services/
   │  └─ BinaryService.cs
   ├─ Views/
   │  └─ Home/
   │     └─ Index.cshtml
   ├─ wwwroot/
   │  └─ css/site.css   (puede que ya exista; lo editamos)
   ├─ Program.cs
   └─ MyProject.csproj
```

> Nota: los **nombres deben ser exactos** (evitar typos como `BinaryImput.cs`).

---

## Q4) ¿Dónde va cada archivo y qué hace?
**R:**
- `Models/BinaryStringAttribute.cs`: Data Annotation para validar strings binarios (solo 0/1, largo > 0, ≤ 8 y múltiplo de 2).
- `Models/BinaryInput.cs`: Modelo de entrada con dos propiedades **A** y **B** con la anotación anterior.
- `Models/NumberBases.cs`: Representación de un número en BIN/OCT/DEC/HEX.
- `Models/BinaryResults.cs`: Contenedor de resultados (A, B, AND, OR, XOR, Suma, Producto) en las 4 bases.
- `Models/BinaryPageViewModel.cs`: ViewModel de la página, une `BinaryInput` + `BinaryResults`.
- `Services/BinaryService.cs`: Lógica de negocio (AND/OR/XOR carácter por carácter, suma, multiplicación, conversiones).
- `Controllers/HomeController.cs`: Orquestación MVC, recibe POST, valida el modelo y llama al servicio.
- `Views/Home/Index.cshtml`: Formulario + tabla única con los resultados.

---

## Q5) ¿Cómo se validan los datos?
**R:** Con **Data Annotations** aplicadas a `BinaryInput` usando `BinaryStringAttribute`:
- Solo **0** y **1** (regex `^[01]+$`).
- Largo **> 0**.
- Largo **≤ 8** (simula un byte).
- Largo **múltiplo de 2**: 2, 4, 6 u 8 caracteres.

**Ejemplos válidos:** `00`, `01`, `0000`, `101010`, `01010101`  
**Ejemplos no válidos:** `""` (vacío), `0` (no múltiplo de 2), `000` (no múltiplo de 2), `101010101` (> 8), `1F100000` (carácter inválido).

---

## Q6) ¿Cómo se hacen las operaciones?
**R:** En `BinaryService`:
- **Bit a bit (strings):** AND, OR, XOR **iterando carácter por carácter** (sin convertir a int para estas).
- **Aritmética:** `a + b` y `a * b` convertidos a enteros con `Convert.ToInt32(bin, 2)`.
- **Cambio de bases:** con `Convert.ToString(valor, base)` (`2`, `8`, `10`, `16`).  
- **Visualización de A y B:** se rellenan a **8 bits** con `PadLeft(8, '0')` para mostrarlos siempre como un byte.

---

## Q7) ¿Cómo se muestran los resultados?
**R:** En **una única tabla** dentro de `Views/Home/Index.cshtml`, con columnas para **BIN**, **OCT**, **DEC** y **HEX** para cada ítem:
- `a`, `b` (mostrados con 8 bits)
- `a Y b`, `a O b`, `a XOR b`
- `a + b`, `a • b`

> Los resultados no se recortan a 8 bits (para no perder información en suma/producto).

---

## Q8) ¿Cómo ejecuto el proyecto?
**R:** Desde la carpeta del proyecto:
```bash
cd PP2/MyProject
dotnet run
```
Abre la URL **HTTPS** que muestre la consola (por ejemplo `https://localhost:7058/`).

> Alternativa desde la raíz `PP2`:  
> `dotnet run --project MyProject/MyProject.csproj`

---

## Q9) ¿Cómo le pongo un diseño más atractivo?
**R:** Usé **Bootstrap 5** + **Bootstrap Icons** + **modo oscuro** con un botón simple:
1. En `_Layout.cshtml`, incluye los **CDN** de Bootstrap + Icons y un script para alternar tema (dark/light).
2. En `Index.cshtml`, utiliza **cards**, **input-group** y **tabla estilizada**.
3. En `wwwroot/css/site.css`, define una clase `.mono` (fuente monoespaciada) y algunos ajustes de tabla y navbar.

> Con eso la página queda moderna, legible y con dark mode para trabajar a gusto.

---

## Q10) ¿Cómo pruebo rápido que todo funciona?
**R:** Casos simples:
- `a = 1010`, `b = 11` → se validan (múltiplos de 2) y verás AND/OR/XOR + suma y producto en todas las bases.
- Probar errores: `a = 1` (falla por largo no múltiplo de 2), `b = 101010101` (falla por > 8), `a = 11002` (carácter inválido).

---

## Q11) Pregunta del enunciado: ¿Cuál es el número al **multiplicar** los **máximos permitidos**?
**R:** Máximo de 8 bits: `11111111` (255 decimal) para ambos.  
**Producto:** `255 × 255 = 65025`.

**En todas las bases:**
- **Binario:** `1111111000000001`
- **Octal:** `177001`
- **Decimal:** `65025`
- **Hexadecimal:** `FE01`

---

## Q12) ¿Puedo mover la lógica a otra capa?
**R:** Sí. La clase `BinaryService` ya representa una **capa de servicios**.  
Podrías crear un proyecto aparte (p. ej. `MyProject.Core` o `MyProject.Domain`) y referenciarlo desde `MyProject` para **separar UI y dominio**, facilitando **tests unitarios** (xUnit/NUnit) y mantenimiento.

---

## Q13) Errores comunes y cómo los solucioné
**R:**
- **“No se ha podido encontrar un proyecto para ejecutar”**: Estaba en `PP2/` (solución). Solución: entrar a `MyProject/` o usar `--project`.
- **CS0234/CS0246 (no encuentra clases/modelos)**: Archivos mal nombrados o con namespace distinto. Solución:  
  - Archivos exactos en `Models/`: `BinaryStringAttribute.cs`, `BinaryInput.cs`, `NumberBases.cs`, `BinaryResults.cs`, `BinaryPageViewModel.cs`  
  - Primeras líneas con `namespace MyProject.Models;` (o bloque `namespace MyProject.Models { ... }`).
  - En la Vista: `@model MyProject.Models.BinaryPageViewModel` en la 1a línea.
  - `Views/_ViewImports.cshtml` con `@using MyProject.Models` ayuda.
- **CS8954 (dos namespaces en el archivo)**: En `BinaryResults.cs` mezclé *file-scoped* con *block-scoped*. Solución: dejar **un solo** `namespace`.

---

## Pregunta solicitada 1
**¿Cuál es el número que resulta al multiplicar, si se introducen los valores máximos permitidos en a y b? Indíquelo en todas las bases (binaria, octal, decimal y hexadecimal).**

**Respuesta (razonamiento breve):**  
El máximo permitido por las reglas es 8 bits: `a = 11111111` y `b = 11111111` (ambos valen 255 en decimal).  
**Producto:** 255 × 255 = **65025**.

**Representaciones:**
- **Binario:** `1111111000000001`
- **Octal:** `177001`
- **Decimal:** `65025`
- **Hexadecimal:** `FE01`

## Pregunta solicitada 2
**¿Es posible hacer las operaciones en otra capa? Si sí, ¿en cuál sería?**

**Respuesta:**  
Sí. Conviene ubicar la lógica en una **capa de servicios / dominio** separada de la UI.  
- Por ejemplo, crear un proyecto de clase llamado **`MyProject.Core`** o **`MyProject.Domain`** que contenga la clase `BinaryService` (y cualquier lógica adicional).  
- El proyecto MVC (**`MyProject`**) referencia esa capa y **inyecta** el servicio (DI).  
- Beneficios: separación de responsabilidades (SRP), pruebas unitarias fáciles, mantenibilidad y posibilidad de reutilizar la lógica en otras interfaces (API, consola, tests).

