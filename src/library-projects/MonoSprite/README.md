<h1 align="center">
MonoSprite
<br/>
<br/>
A lightweight custom Aseprite importer designed for use with <a href="https://www.monogame.net">MonoGame</a>

</h1>

**MonoSprite** is a lightweight custom Aseprite layer-based importer designed for use with the [MonoGame](https://www.monogame.net/) framework. It enables you to import and work with Aseprite spritesheets, supporting animation data defined on separate layers (rather than tag-based animations).

# Features

- **Aseprite JSON Import:** Parses Aseprite-exported JSON spritesheets.
- **Layer-Based Animations:** Supports animations defined by separate layers in Aseprite.
- **Tag-Based Animations:** Supports animations defined by separate tags in Aseprite.
- **Frame Data Extraction:** Retrieves frame rectangles, durations, and custom cel data.
- **Lightweight:** Minimal dependencies and easy integration with MonoGame projects.

# Usage

## Configure Sprite in Aseprite

### Animations in Layers
- Name your layers corresponding to the different sprite animations eg for a walk animation name your layer "Walk". 
- Specify whether an animation should loop by adding a comma followed by the word "loop" after the layer name, eg: `Walk,loop`.

### Animations in Tags
- Name your tags corresponding to the different sprite animations eg for a walk animation name your tag "Walk". 
- Specify whether an animation should loop by adding a comma followed by the word "loop" after the layer name, eg: `Walk,loop`.

### User Data
You can also embed user data in an individual cel of your Sprite. To do this right click on a frame cel and select Cel Properties and enter a string in the User Data text field.
This will then be added to the outputted `.json` file in the `meta.layers.cels` property, e.g.:
```json
"layers": [
   { "name": "Layer", "opacity": 255, "blendMode": "normal", "cels": [{ "frame": 3, "data": "myUserData" }] }
  ],
```

## Export Spritesheet
### Animations in Layers
- On the Export Sprite Sheet on the **Layout** tab make sure that Sheet Type is is set to "By Rows". 
- On the **Sprite** tab select Split Layers.
- On the **Borders** tab make sure Border Padding and Inner Padding is set to 0 and Spacing is set to 1, this will add a 1px border around each sprite in the exported sprite sheet image. 
- On the **Output** tab:
  -  Select Output File and JSON Data.
  -  Select Array from the dropdown selection.
  -  Select Layers on the Meta section. 
- Enter `{layer}|{frame}` in the **Item Filename**. This will set the the layer name and frame number in the`filename` property of each frame element in the `.json` file.  

### Animations in Tags
- On the Export Sprite Sheet on the **Layout** tab make sure that Sheet Type is is set to "By Rows". 
- On the **Sprite** select Split Tags
- On the **Borders** tab make sure Border Padding and Inner Padding is set to 0 and Spacing is set to 1, this will add a 1px border around each sprite in the exported sprite sheet image. 
- On the **Output** tab:
  -  Select **Output File** and **JSON Data**.
  -  Select **Array** from the dropdown selection.
  -  Select **Tags** on the Meta section. 
- Enter `{tag}|{frame}` in the **Item Filename**. This will set the the tag name and frame number in the`filename` property of each frame element in the `.json` file.  


If all of the above was configured correctly you should have 2 files exported: a sprite sheet image with each sprite animation on a seperate row and a `.json` file that specifies the size and `x,y` coordinates of each frame within the image, something like this:

```json
{ "frames": [
    {
      "filename": "Walk,loop|0",
      "frame": { "x": 0, "y": 0, "w": 64, "h": 64 },
      "rotated": false,
      "trimmed": false,
      "spriteSourceSize": { "x": 0, "y": 0, "w": 64, "h": 64 },
      "sourceSize": { "w": 64, "h": 64 },
      "duration": 100
    },
    {
      "filename": "Walk,loop|1",
      "frame": { "x": 65, "y": 0, "w": 64, "h": 64 },
      "rotated": false,
      "trimmed": false,
      "spriteSourceSize": { "x": 0, "y": 0, "w": 64, "h": 64 },
      "sourceSize": { "w": 64, "h": 64 },
      "duration": 100
    },
    {
      "filename": "Attack|0",
      "frame": { "x": 0, "y": 65, "w": 64, "h": 64 },
      "rotated": false,
      "trimmed": false,
      "spriteSourceSize": { "x": 0, "y": 0, "w": 64, "h": 64 },
      "sourceSize": { "w": 64, "h": 64 },
      "duration": 100
    },
    {
      "filename": "Attack|1",
      "frame": { "x": 65, "y": 65, "w": 64, "h": 64 },
      "rotated": false,
      "trimmed": false,
      "spriteSourceSize": { "x": 0, "y": 0, "w": 64, "h": 64 },
      "sourceSize": { "w": 64, "h": 64 },
      "duration": 100
    }
    ...
  ],
  "meta": {
  "app": "https://www.aseprite.org/",
  "version": "1.3.7-x64",
  "image": "Spriteheet1.png",
  "format": "RGBA8888",
  "size": { "w": 389, "h": 129 },
  "scale": "1",
  "layers": [
   { "name": "Ready,loop", "opacity": 255, "blendMode": "normal" },
   { "name": "Attack", "opacity": 255, "blendMode": "normal", "cels": [{ "frame": 3, "data": "isattacking" }] }
  ]
 }
}
```

## Load your spritesheet

```csharp
using MonoSprite;
using System.IO.Abstractions;

// Setup dependencies (example)
var fileSystem = new FileSystem();
var jsonConverter = new MonoSprite.Converters.AsepriteSpritesheetJsonConverterService();
var spritesheetService = new AsepriteSpritesheetService(fileSystem, jsonConverter);

// Load spritesheet
var asepriteSpritesheet = spritesheetService.GetAsepriteSpritesheet("path/to/spritesheet.json");
```

# Limitations

- Designed for integration with MonoGame, but can be adapted for other frameworks.

# License

MIT License

---

*This project is not affiliated with Aseprite or MonoGame.*