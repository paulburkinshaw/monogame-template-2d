using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoSprite
{
    public interface ISpriteRenderer
    {
        void Draw(Texture2D spritesheetTexture, Vector2 position, Rectangle sourceRectangle, float rotation = 0f, Vector2 origin = new Vector2());
    }

    public class SpriteRenderer : ISpriteRenderer
    {
        private readonly SpriteBatch _spriteBatch;     

        public SpriteRenderer(           
            SpriteBatch spriteBatch,        
            Rectangle windowSize
            )
        {          
            _spriteBatch = spriteBatch;                           
        }

        public void Draw(
            Texture2D spritesheetTexture, 
            Vector2 position, 
            Rectangle sourceRectangle, 
            float rotation = 0f, 
            Vector2 origin = new Vector2())
        {
            // the 4th parameter is the color tint, we use Color.White to draw the texture with its original colors
            // we could also use a different color tint if we wanted to
            // it works like a filter, for example Color.Red would make the texture appear redder
            // this is because the color values are multiplied together, so white (1,1,1) leaves the colors unchanged
            // while red (1,0,0) would remove the green and blue components, making the texture appear redder
            // to specify a color mask we could use new Color(r, g, b) where r, g, b are floats between 0 and 1
            // this would allow us to create custom color tints
            _spriteBatch.Draw(spritesheetTexture, position, sourceRectangle, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
        }
    }
}
