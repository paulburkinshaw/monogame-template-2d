# MonoGame Template TwoD

A MonoGame starter template with a clean, reusable project structure for 2D games.

---

# Getting Started

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [MonoGame](https://www.monogame.net/downloads/) (install the `dotnet-mgcb` and `dotnet-mgcb-editor` tools)
- An IDE with C# support (Visual Studio, Rider, or VS Code with the C# extension)

## Using this Template

1. **Clone or download** this repository.
2. **Copy** the `MonoGame.Template.TwoD.sln` file, along with the `src/MonoGame.Template.TwoD`, `src/MonoGame.Template.TwoD.Content`, `src/library-projects/`, and `.config` folders, into your new project location. Once copied you should have a structure like:
    ```
    /your-game-folder
    ├─ MonoGame.Template.TwoD.sln
    ├─ .config
    └─ src/
        ├─ MonoGame.Template.TwoD
        ├─ MonoGame.Template.TwoD.Content
        └─ library-projects/
            ├─ MonoSprite
            ├─ MonoTiled
            └─ TiledDotNet
    ```
3. **Rename** the folders, solution, project files, and namespaces to match your game name (find/replace `MonoGame.Template.TwoD` across the solution). Remembering to update the filenames as well as the namespaces inside the code files.
4. **Update `gameSettings.json`** with your desired resolution, font name, and content strings.
5. **Build and run** — you should see the menu screen with your configured start prompt.

## What the Template Gives You Out of the Box

The template runs immediately after cloning — no additional setup required beyond the prerequisites. Hit run and you'll see a working game with:

- A menu screen that transitions to a playing state on Space/Enter
- A player sprite loaded from an Aseprite spritesheet, controllable via keyboard
- A Tiled tilemap loaded and rendered
- Internal resolution rendering scaled up to the window size (correct pixel art scaling)
- A DI-wired game loop ready to extend

---

# Solution Structure

```
/src
 ├─ MonoGame.Template.TwoD          → Main MonoGame game project
 ├─ MonoGame.Template.TwoD.Content  → Content Pipeline project and game assets
 └─ library-projects/
     ├─ MonoSprite                   → Sprite and Aseprite spritesheet support
     ├─ MonoTiled                    → Tilemap rendering
     └─ TiledDotNet                  → Tiled JSON deserialization
```

> **Note:** The library projects (`MonoSprite`, `MonoTiled`, `TiledDotNet`) are currently included directly in this solution. They will be extracted into their own repositories and published as NuGet packages in a future update.

# Project Structure

```
src/
  MonoGame.Template.TwoD/
    Program.cs
    TemplateGame.cs
    gameSettings.json
    Core/
      GameSettings.cs
      GameSettingsConfig.cs
    Rendering/
      GameRenderer.cs
      UIRenderer.cs
      DebugRenderer.cs
    UI/
      UIService.cs
    Input/
      IInputSource.cs
      KeyboardInputSource.cs
      ControllerInputSource.cs
    States/
      IGameState.cs
      GameStateMachine.cs
      MenuState.cs
      PlayingState.cs
    World/
      IGameWorld.cs
      GameWorld.cs
    Gameplay/
      GameEntities/
        IEntity.cs
        Entity.cs
        EntityService.cs
        IHasInputSource.cs
        IHasTransform.cs
        IRenderable.cs
        IUpdatable.cs
        Transform.cs
        Player.cs

  MonoGame.Template.TwoD.Content/
    Content.mgcb
    Fonts/
    Spritesheets/
    Tilesets/
    Tilemaps/
```

- `TemplateGame.cs` is the main MonoGame game class. Handles DI setup, content loading, and delegates update/draw to the state machine.
- `gameSettings.json` drives resolution, animation settings, tilemap type, active language, and localised content strings.
- `GameSettingsConfig.cs` is the JSON deserialization DTO for `gameSettings.json`.
- `GameRenderer.cs` renders game entities and tilemaps to an off-screen render target, then scales to the window.
- `UIRenderer.cs` renders UI elements (fonts, HUD) to a separate off-screen render target.
- `UIService.cs` manages sprite font registration and retrieval by name.
- `DebugRenderer.cs` contains debug-only rendering helpers.
- `GameStateMachine.cs` manages high-level game states. States receive the machine via `Enter()` and trigger transitions via `ChangeState()`.
- `MenuState.cs` is the initial state — displays a start prompt and transitions to `PlayingState` on Space/Enter.
- `PlayingState.cs` runs the main game loop — processes input, updates entities, and renders.
- `GameWorld.cs` is the simulation container for entities and the active tilemap.
- `EntityService.cs` manages entity registration and querying by capability (updatable, renderable, has input source).

---

# gameSettings.json

`gameSettings.json` is the central configuration file for the template. It is loaded at startup and drives the following:

| Section | Purpose |
|---|---|
| `language` | Active language key (e.g. `"en"`). Used to select the correct content strings at runtime. |
| `internalSize` | The resolution the game renders at internally (e.g. 640x360 for pixel art). |
| `windowSize` | The resolution of the game window. Should be a whole-number multiple of `internalSize`. |
| `animationSettings.targetFramesPerSecond` | Target update rate for sprite animations. |
| `tilemapSettings.tilemapType` | Tilemap rendering mode (`0` = Static, `1` = Dynamic). |
| `uiSettings.menuFontName` | The name of the SpriteFont asset used for menu text. Must match the font registered in `LoadContent`. |
| `contentSettings.languages` | Localised content strings keyed by language code. Add a new language key alongside `"en"` to support additional languages. |

### Example

```json
{
  "language": "en",
  "internalSize": { "width": 640, "height": 360 },
  "windowSize": { "width": 1920, "height": 1080 },
  "animationSettings": { "targetFramesPerSecond": 30 },
  "tilemapSettings": { "tilemapType": 1 },
  "uiSettings": { "menuFontName": "MenuFont" },
  "contentSettings": {
    "languages": {
      "en": { "menuScreenMessage": "Press Space or Enter to start game" },
      "es": { "menuScreenMessage": "Pulsa la barra espaciadora o Enter para comenzar." }
    }
  }
}
```

---

# Graphics and Rendering

## InternalSize, WindowSize and Aspect Ratio

The `GameSettings` class defines the `InternalSize` and `WindowSize` properties which control the game's rendering resolution and how it scales to the window. 

### InternalSize

The `InternalSize` is the resolution at which the game is rendered internally. This is where you define the base resolution for your game. For example, if you want a pixel art game with a retro feel, you might choose a smaller internal resolution like 320x180 (which is a 16:9 aspect ratio).

### WindowSize

The `WindowSize` is the resolution of the game window. This is how large the game will appear on the player's screen. You can choose to scale the internal resolution up to fit the window size while maintaining the aspect ratio. For example, if your internal size is 320x180, you could set the window size to 1280x720 (which is 4 times the internal resolution) to maintain a crisp pixel art look without distortion.

### Aspect Ratio

The aspect ratio is the ratio of the width to the height of the game window. Maintaining a consistent aspect ratio is important to ensure that the game looks correct on different screen sizes. For example, a 16:9 aspect ratio means that for every 16 units of width, there are 9 units of height. By keeping the internal size and window size in the same aspect ratio, you can avoid stretching or squashing the game's visuals.

### Example Configurations

#### 16:9 Aspect Ratio with 40x23 Tiles

**InternalSize**: 640x360 
- 40 tiles x 16 pixels = 640, 23 tiles x 16 pixels = 368 but but 368 is not a standard resolution so we use 360 instead.
- This will leave 8 pixels of unused space vertically, but it's better than stretching to 368 which would distort the pixels.

**WindowSize**: 1920x1080 
- 3x scale of the internal size (640x3=1920, 360x3=1080)
- Maintains a true 16:9 aspect ratio and provides a crisp pixel art look without distortion.

#### 16:9 Aspect Ratio with 20x11 Tiles

**InternalSize**: 320x180 
- 20 tiles x 16 pixels = 320, 11.5 tiles x 16 pixels = 184 but 184 is not a standard resolution so we use 180 instead.
- This will leave 4 pixels of unused space vertically.

**WindowSize**: 1280x720
- 4x scale of the internal size (320x4=1280, 180x4=720)
- Maintains a true 16:9 aspect ratio and provides a crisp pixel art look without distortion.
