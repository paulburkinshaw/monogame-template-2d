using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoSprite
{
    public interface ISpriteService
    {
        Spritesheet LoadSpritesheet(string spritesheetFilepath, string spritesheetName);

        /// <summary>
        /// Create a new SpriteInstance from a spritesheet file path and name
        /// This method loads the spritesheet if not already loaded, creates a SpriteRenderer, and then creates the SpriteInstance
        /// </summary>
        /// <param name="spritesheetFilepath"></param>
        /// <param name="spritesheetName"></param>
        /// <param name="initialAnimationName"></param>
        /// <param name="spriteBatch"></param>
        /// <returns></returns>
        Sprite CreateSpriteInstance(
            string spritesheetFilepath,
            string spritesheetName,
            string initialAnimationName,
            SpriteBatch spriteBatch);

        /// <summary>
        /// Create a new SpriteInstance from an existing Spritesheet instance
        /// This method creates a SpriteRenderer, and then creates the SpriteInstance
        /// this overload is useful when you want to share a spritesheet among multiple sprite instances
        /// we could also have an overload that takes a dictionary of spritesheets to avoid loading the same spritesheet multiple times
        /// </summary>
        /// <param name="spritesheet"></param>
        /// <param name="initialAnimationName"></param>
        /// <param name="spriteBatch"></param>
        /// <returns></returns>
        Sprite CreateSpriteInstance(
           Spritesheet spritesheet,
           string initialAnimationName,      
           SpriteBatch spriteBatch);
    }
    public class SpriteService : ISpriteService
    {
        private readonly IAsepriteSpritesheetService _asepriteSpritesheetService;
        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;

        public SpriteService(
            IAsepriteSpritesheetService asepriteSpritesheetService,
            ContentManager content,
            GraphicsDevice graphicsDevice,
            SpriteBatch spriteBatch)
        {
            _asepriteSpritesheetService = asepriteSpritesheetService;
            _content = content;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
        }

        public Spritesheet LoadSpritesheet(string spritesheetFilepath, string spritesheetName)
        {
            // Get Aseprite spritesheet from file
            var asepriteSpritesheet = _asepriteSpritesheetService.GetAsepriteSpritesheet($@"Content\{spritesheetFilepath}\{spritesheetName}.json");

            // Load spritesheet texture from file
            var spritesheetTexture = _content.Load<Texture2D>($@"{spritesheetFilepath}\{asepriteSpritesheet.SpritesheetImageName}");

            var animations = new List<Animation>();

            foreach (var asepriteAnimation in asepriteSpritesheet.Animations)
            {
                var frames = asepriteAnimation.Frames.Select(frame => new AnimationFrame(new Rectangle(
                                                                                             frame.SourceRectangle.X,
                                                                                             frame.SourceRectangle.Y,
                                                                                             frame.SourceRectangle.Width,
                                                                                             frame.SourceRectangle.Height),
                                                                                             frame.Duration));

                animations.Add(new Animation(
                    animationName: asepriteAnimation.Name,
                    frames: frames,
                    isLooping: asepriteAnimation.Loop)
                    );
            }

            var spritesheet = new Spritesheet(
                spritesheetTexture: spritesheetTexture,
                spritesheetName: asepriteSpritesheet.SpritesheetImageName,
                animations: animations.ToDictionary(a => a.AnimationName, a => a),
                graphicsDevice: _graphicsDevice,
                spriteBatch: _spriteBatch
             );

            return spritesheet;
        }

        // Create a new SpriteInstance from a spritesheet file path and name
        // This method loads the spritesheet if not already loaded, creates a SpriteRenderer, and then creates the SpriteInstance
        public Sprite CreateSpriteInstance(
            string spritesheetFilepath,
            string spritesheetName,
            string initialAnimationName,
            SpriteBatch spriteBatch)
        {
            // Load or retrieve the shared Spritesheet
            var spritesheet = LoadSpritesheet(spritesheetFilepath, spritesheetName);

            // Create the SpriteRenderer
            var spriteRenderer = new SpriteRenderer(spriteBatch);

            // Create and return the SpriteInstance, passing the renderer if needed
            var spriteInstance = new Sprite(
                spritesheet,
                initialAnimationName,            
                spriteRenderer
            );

            return spriteInstance;
        }



        // Create a new SpriteInstance from an existing Spritesheet instance
        // This method creates a SpriteRenderer, and then creates the SpriteInstance
        // this overload is useful when you want to share a spritesheet among multiple sprite instances
        // we could also have an overload that takes a dictionary of spritesheets to avoid loading the same spritesheet multiple times
        public Sprite CreateSpriteInstance(
           Spritesheet spritesheet,          
           string initialAnimationName,
           SpriteBatch spriteBatch)
        {
            // Create the SpriteRenderer
            var spriteRenderer = new SpriteRenderer(spriteBatch);

            // Create and return the SpriteInstance, passing the renderer if needed
            var spriteInstance = new Sprite(
                spritesheet,
                initialAnimationName,
                spriteRenderer
            );

            return spriteInstance;
        }

    }
}

