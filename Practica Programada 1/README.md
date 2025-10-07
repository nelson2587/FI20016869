# README — Suma de Números Naturales (Consola C#)

## 👤 Autor
- **Nombre:** Nelson Rodriguez Lopez 
- **Carné:**  FI20016869

## 🎯 Objetivo
Construir una aplicación de consola en C# que:

1. Implemente **dos métodos** para sumar los primeros *n* números naturales:
   - `SumFor(n)`: fórmula cerrada `n*(n+1)/2`.
   - `SumIte(n)`: suma **iterativa** de 1 a *n*.
2. **Valide ascendentemente** (desde 1 hasta un tope) con cada método, **deteniendo** al primer resultado inválido y **reportando el último válido**.
3. **Valide descendentemente** (desde el tope hasta 1) con cada método, **deteniendo** al primer resultado válido y **reportándolo**.
4. **Muestre** todos los resultados en la **consola**.



---

## 🧱 Requisitos
- **SDK .NET**  8 .
- Sistema operativo Windows.

---

## 🗂️ Estructura sugerida
```
SumasNaturales.App/
├─ Program.cs
├─ Sumas.cs           // (opcional) separar lógica
└─ README.md
```

---

## ⚙️ Pasos de instalación y ejecución
```bash
# 1) Crear proyecto
dotnet new console -n SumasNaturales.App
cd SumasNaturales.App

# 2) Reemplazar el contenido de Program.cs con tu implementación

# 3) Compilar
dotnet build -c Release

# 4) Ejecutar (tope por defecto = 2,000,000 sugerido)
dotnet run --project . -- 2000000
```

---

## 🔢 Interfaz de uso (CLI)
```bash
dotnet run -- <TOPE_MAXIMO>
```

- `<TOPE_MAXIMO>`: entero positivo que define el límite superior de búsqueda (ej.: `2000000`).

Si no se pasa argumento, puedes:
- Usar un **valor por defecto** (p. ej. `2_000_000`), o
- Mostrar ayuda y salir con código distinto de 0.

---

## 🧮 Métodos requeridos
```csharp
// Fórmula: O(1). Usar 'unchecked' para comparar comportamiento con overflow.
static int SumFor(int n)
{
    unchecked { return (n * (n + 1)) / 2; }
}

// Iterativa: O(n).
static int SumIte(int n)
{
    int acc = 0;
    unchecked
    {
        for (int i = 1; i <= n; i++)
            acc += i;
    }
    return acc;
}
```

---

## 🔎 Estrategias de validación

### Ascendente: “último válido”
Recorre `n = 1..Max` y **guarda el último (n, suma) con suma > 0**. Detente al primer inválido.

```csharp
static (int n, int sum) AscendenteUltimoValido(Func<int,int> f, int max)
{
    int lastN = 0, lastSum = 0;
    for (int n = 1; n <= max; n++)
    {
        int s = f(n);
        if (s > 0) { lastN = n; lastSum = s; }
        else break;
    }
    return (lastN, lastSum);
}
```

### Descendente: “primer válido”
Recorre `n = Max..1` y **detente en el primer (n, suma) con suma > 0**.

```csharp
static (int n, int sum) DescendentePrimerValido(Func<int,int> f, int max)
{
    for (int n = max; n >= 1; n--)
    {
        int s = f(n);
        if (s > 0) return (n, s);
    }
    return (0, 0);
}
```

---

## 📤 Salida esperada (formato de ejemplo)
```
• SumFor:
        ◦ Ascendente 1..Max  → n: 65535 → sum: 2147450880
        ◦ Descendente Max..1 → n: 65535 → sum: 2147450880

• SumIte:
        ◦ Ascendente 1..Max  → n: 65535 → sum: 2147450880
        ◦ Descendente Max..1 → n: 65535 → sum: 2147450880
```



## 🧪 Ejemplos de ejecución
```bash

dotnet run --project SumasNaturales.App -- 100000


dotnet run --project SumasNaturales.App -- 2000000

dotnet run --project SumasNaturales.App -- --quick 
dotnet run --project SumasNaturales.App -- --skip-desc-ite
```

---

## 🧠 Notas de diseño y criterios
- **Overflow**: El uso de `unchecked` permite observar el límite práctico en `int`. Si quieres evitarlo, usa `long` y ajusta el criterio de “válido”.
- **Criterio de validez**: Se usa `sum > 0`. Alternativas:
  - Verificar **monotonía**: `f(n) > f(n-1)` (puede ser más robusto).
  - Detectar overflow con `checked` y `try/catch`.
- **Rendimiento**:
  - `SumFor` es O(1) y muy rápido.
  - `SumIte` es O(n); usar topes altos puede tardar. Ajusta el tope si es necesario.

---
1) ¿Por qué difieren los resultados entre métodos y estrategias?
Diferencias entre métodos (fórmula vs. iterativa)

Tipo int y overflow: trabajas en unchecked con int (32-bits). La suma real de 1..n crece como ~n²/2, así que sobrepasa int.MaxValue pronto (el último n “exacto” sería 65 535 si hiciéramos la aritmética sin desbordes intermedios).

Orden de evaluación:

Fórmula: SumFor(n) = (n * (n + 1)) / 2. El producto n*(n+1) puede desbordarse antes de dividir entre 2. Ese desborde cambia el signo y el valor, así que “la frontera” donde sum > 0 deja de cumplirse no coincide con la frontera de la suma exacta.

Iterativa: SumIte(n) hace acc += i paso a paso. Aquí el desborde aparece cuando acc cruza int.MaxValue mientras acumula. Es otro patrón de desborde, por eso el “máximo n con sum > 0” puede ser distinto al de la fórmula.

Moraleja: aunque ambas implementan la misma suma matemática, en int y unchecked cada una “explota” de manera distinta por los intermedios (producto vs. sumas sucesivas).

Diferencias entre estrategias (ascendente vs. descendente)

Criterio de parada:

Ascendente: vas 1→Max y te quedas con el último válido (sum > 0). Cuando el valor se vuelve ≤ 0 (por overflow), paras. El reporte queda pegado al umbral donde el método deja de dar positivos.

Descendente: vas Max→1 y te quedas con el primer válido. Como el signo del resultado con overflow no es monotónico “natural” respecto a n (por el wrap-around), el primer positivo al bajar puede estar en un n diferente al “último positivo” que encontraste subiendo.

En resumen: el mismo criterio sum > 0 sobre datos con overflow hace que subir y bajar encuentren n distintos.

Si quieres que ambos métodos y ambas estrategias coincidan, no uses int en unchecked con esa expresión. Opciones: usar long para los intermedios (p. ej. (long)n*(n+1)/2), reordenar la fórmula para minimizar overflow ((n/2)*(n+1) si n es par, de lo contrario n*((n+1)/2)), o detectar overflow explícitamente (checked + try/catch).

2) ¿Qué pasa si aplico las mismas estrategias con SumRec?

Numéricamente, SumRec(n) acumula igual que la iterativa (suma 1+2+…+n), así que el patrón de overflow será muy parecido a SumIte (no al de la fórmula). Espera límites similares de “último n con sum > 0”.

Pero aparece otro límite: el pila de llamadas. La recursión profunda dispara StackOverflow alrededor de ~miles/decenas de miles de llamadas (depende del entorno). Por eso en tu código pusiste recCap (~12 000) como “límite seguro”.

Ascendente con SumRec: funciona hasta que n alcance recCap (o hasta que el overflow haga sum <= 0, lo que ocurra antes).

Descendente con SumRec: es la peor combinación si el max es grande: cada evaluación f(n) intenta una recursión de profundidad n. Con n muy alto, puedes reventar la pila casi de inmediato.

Conclusión: con SumRec, el comportamiento de overflow se parecerá al iterativo, pero el cuello de botella práctico será el tope recursivo, no el overflow del int.

Recomendación rápida para experimentarlo sin sorpresas

Mantén SumRec capado (como ya lo haces con --rec-max).

Para comparar solo la aritmética sin overflow de intermedios:

Cambia la fórmula a usar intermedios de 64-bits:

static int SumForSafe(int n) => (int)(((long)n * (n + 1)) / 2);


O reordena para evitar desbordes en el producto:

static int SumForReordered(int n)
{
    unchecked
    {
        if ((n & 1) == 0) return (n / 2) * (n + 1);
        else              return n * ((n + 1) / 2);
    }
}


Si también cambias SumIte/SumRec a long para el acumulador, verás que ascendente y descendente coinciden (porque ya no hay wrap-around que “rompa” la monotonicidad) y los tres métodos dan el mismo n y la misma suma.



