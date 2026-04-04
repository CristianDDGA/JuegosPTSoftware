### 📂 Árbol de Directorios (Ejemplo con Tic-Tac-Toe)

Así es como debe verse la estructura de carpetas en Visual Studio. Por favor, **respeta esta jerarquía** y crea tu propia carpeta (ej. `NReinas`, `Laberinto`) dentro de cada proyecto.

```text
PatronesBusquedaFISEI/
│
├── 📁 Application/                  # 🧠 El cerebro (Algoritmos)
│   ├── 📁 TicTacToe/                
│   │   ├── ITicTacToeUseCase.cs     # Contrato para la interfaz
│   │   └── TicTacToeUseCase.cs      # Lógica del Minimax
│   └── 📁 NReinas/                  # <-- (Jhon pondrá su Backtracking aquí)
│
├── 📁 Domain/                       # 🧱 Los cimientos (Reglas puras)
│   ├── 📁 TicTacToe/
│   │   ├── GameStatus.cs
│   │   ├── Player.cs
│   │   └── TicTacToeBoard.cs        # Matriz y validaciones
│   └── 📁 NReinas/                  # <-- (Jhon pondrá sus Entidades aquí)
│
├── 📁 Infrastructure/               # 🔌 Conexiones Externas (Archivos/txt)
│   ├── 📁 TicTacToe/                # (Vacío para este juego)
│   └── 📁 Laberinto/                # <-- (Alexis podría leer su matriz desde un .txt aquí)
│
└── 📁 Presentation/                 # 🎨 La pintura (WPF y Ventanas)
    ├── 📄 App.xaml                  # Configuración de arranque
    ├── 📄 MainMenuWindow.xaml       # ⚠️ MENÚ PRINCIPAL (Solo agregar el botón)
    ├── 📁 TicTacToe/
    │   └── TicTacToeWindow.xaml     # Ventana independiente de Cristian
    └── 📁 NReinas/                  # <-- (Jhon creará su NReinasWindow.xaml aquí)


# 🏛️ Proyecto de Búsqueda y Patrones de Software - FISEI

¡Bienvenidos al repositorio central del proyecto del Primer Parcial! 

Este proyecto contiene la resolución de los problemas suplementarios implementados en **C#** con **WPF** para la interfaz gráfica. Hemos configurado el proyecto utilizando **Clean Architecture** y principios **SOLID** para garantizar que todos podamos trabajar en paralelo sin generar conflictos de código.

---

## 👥 Equipo de Trabajo y Asignaciones
* **1. N Reinas (Backtracking):** Jhon Jumbo 
* **2. Caballo de Ajedrez (Backtracking/DFS):** Matías Morales 
* **3. Laberinto (BFS/A*):** Alexis López 
* **4. Agente Viajero (Branch and Bound):** Luis Miranda 
* **5. Tic-Tac-Toe Minimax:** Cristian Ango *(Base y Juego Completado ✅)*

---

## 🏗️ Estructura del Proyecto (Clean Architecture)
El proyecto está dividido en 4 capas estrictas. **Regla de oro: Las capas internas NO pueden ver a las externas.**

1. **`Domain` (Dominio):** Tus reglas puras, entidades y estados. (Ej: Validaciones del tablero, enums). **No tiene referencias a ninguna otra capa ni librerías visuales.**
2. **`Application` (Aplicación):** Tus Casos de Uso y la lógica de tus algoritmos de búsqueda (DFS, BFS, Backtracking, etc.). **Referencia a Domain.**
3. **`Infrastructure` (Infraestructura):** Para leer archivos externos o bases de datos (solo si tu problema lo requiere, como matrices desde un txt). **Referencia a Application.**
4. **`Presentation` (Presentación):** Tu interfaz gráfica (WPF). Aquí va tu archivo `.xaml` y `.cs`. **No pongas lógica de algoritmos aquí.**

---


git checkout -b feature/tu-nombre-juego
