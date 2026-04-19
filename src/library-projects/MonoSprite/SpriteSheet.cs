using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoSprite
{
    public class Spritesheet
    {
        public Texture2D SpritesheetTexture { get; }
        public string SpritesheetName { get; }
        public IDictionary<string, Animation> Animations { get; private set; }

        public Spritesheet(
            Texture2D spritesheetTexture,
            string spritesheetName,
            IDictionary<string, Animation> animations,
            GraphicsDevice graphicsDevice,
            SpriteBatch spriteBatch
        )
        {
            SpritesheetTexture = spritesheetTexture;
            SpritesheetName = spritesheetName;
            Animations = animations;         
        }  
    }
}
