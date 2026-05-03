# MonoGame Template TwoD

A MonoGame starter template with a clean, reusable project structure for 2D games.

# Solution Structure

```
/src
 ├─ MonoGame.Template.TwoD          → Main MonoGame game project
 └─ MonoGame.Template.TwoD.Content  → Content Pipeline project and game assets
```

# Project Structure

```
src/
  MonoGame.Template.TwoD/
    Program.cs
    TemplateGame.cs
    Core/
      GameSettings.cs
    Rendering/
      Camera2D.cs
      GameRenderer.cs
      DebugRenderer.cs
    Input/
      InputService.cs
      KeyboardInputSource.cs
    States/
      IGameState.cs
      GameStateMachine.cs
      PlayState.cs
    World/
      SceneService.cs
      World.cs
    Gameplay/
      Entities/
        IEntity.cs

  MonoGame.Template.TwoD.Content/
    Content.mgcb
    Spritesheets/
    Tilesets/
    Tilemaps/
```

- `TemplateGame.cs` is the main MonoGame game class.
- `GameRenderer.cs` handles game rendering and presentation.
- `DebugRenderer.cs` contains debug-only rendering helpers.
- `GameStateMachine.cs` manages high-level game states.
- `World.cs` is the simulation container for game objects and world state.

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
