# MonoTiled

MonoTiled is a C# library for loading and rendering Tiled maps in MonoGame projects. It provides a simple and efficient way to integrate Tiled maps into your game, allowing you to focus on game development rather than map handling.

## Features

- Easy integration of Tiled maps into MonoGame projects
- Support for multiple layers, including tile layers and object layers
- Automatic handling of tile animations and properties
- Simple API for loading and rendering maps
- Extensible architecture for custom map processing

## Usage

To use MonoTiled in your MonoGame project, follow these steps:

1. Install the MonoTiled NuGet package.
2. Create a new instance of the `TiledMap` class.
3. Load your Tiled map file using the `Load` method.
4. Render the map in your game's `Draw` method.

```csharp
using MonoTiled;

// Create a new TiledMap instance
TiledMap map = new TiledMap();

// Load the Tiled map file
map.Load("Content/Maps/MyMap.tmx");

// In your game's Draw method, render the map
map.Draw(spriteBatch);
```