# MonoGame Single Player Template

A MonoGame single-player starter template with a clean, reusable project structure for 2D games.

## Solution Structure

```
/src
 ├─ MonoGame.SinglePlayer.Template          → Main MonoGame game project
 └─ MonoGame.SinglePlayer.Template.Content  → Content Pipeline project and game assets
```

## Project Structure

```
src/
  MonoGame.SinglePlayer.Template/
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

  MonoGame.SinglePlayer.Template.Content/
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