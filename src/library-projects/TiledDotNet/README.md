# TiledDotNet

# TiledDotNet

**TiledDotNet** is a .NET library for loading and working with [Tiled](https://www.mapeditor.org/) tilemap data. It is compatible with both .NET 8 and .NET Standard 2.0 projects.

## Features

- Parses Tiled JSON tilemaps and tilesets.
- Supports multiple layer types: tile, object, image, and group layers.
- Exposes strongly-typed models for tilemaps, tilesets, layers, and properties.
- Handles tile flipping, parallax, and custom properties.

## Installation

Add a project reference to `TiledDotNet` in your project file:
```xml
<ProjectReference Include="..\TiledDotNet\TiledDotNet.csproj" />
```

Or include the compiled DLL in your project.

## Usage Example
```csharp
using TiledDotNet.Models; 
using TiledDotNet.Converters;

// Load a Tiled JSON file as a string
string json = File.ReadAllText("path/to/tilemap.tmj");

// Convert JSON to DTO 
var converter = new TiledTilemapJsonConverterService(); 
var tilemapDTO = converter.GetTilemapDTOFromJsonFile(json);

// Map DTO to strongly-typed model (example)

```

## Project Compatibility

- .NET 8
- .NET Standard 2.0

For more details, see the `TiledDotNet` source in `Library/TiledDotNet`.
