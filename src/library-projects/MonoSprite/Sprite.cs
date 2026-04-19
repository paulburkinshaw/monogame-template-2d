using MonoSprite.Extensions;
using Microsoft.Xna.Framework;
using System;

namespace MonoSprite
{
    public interface ISprite
    {
        Color[,] GetColourDataForCurrentFrame();
        void Update(string animationKey, float elapsedMs);
        void UpdateAnimation();
        void DrawSprite();
    }

    // Rename to Sprite after moving all state/animation logic from Sprite and SpriteAnimation here
    public class Sprite
    {
        private readonly Spritesheet _spritesheet;
        private int _currentFrameNumber;
        private Animation _currentAnimation;
        private float _timeSinceLastFrame;
        private ISpriteRenderer _spriteRenderer;

        public Animation CurrentAnimation => _currentAnimation;

        public event EventHandler<AnimStartedEventArgs> AnimationStarted = delegate { };
        public event EventHandler<AnimCompleteEventArgs> AnimationComplete = delegate { };

        protected virtual void OnAnimationStarted(AnimStartedEventArgs args)
        {
            if (AnimationStarted != null)
                AnimationStarted(this, args);
        }

        protected virtual void OnAnimationComplete(AnimCompleteEventArgs args)
        {
            if (AnimationComplete != null)
                AnimationComplete(this, args);
        }

        public Sprite(
            Spritesheet spriteSheet,
            string initialAnimationName,
            ISpriteRenderer spriteRenderer
            )
        {
            _spritesheet = spriteSheet;
            _currentFrameNumber = 0;
            _timeSinceLastFrame = 0;
            _currentAnimation = _spritesheet.Animations[initialAnimationName];
            _spriteRenderer = spriteRenderer;        
        }

        public void Update(string animationKey, float elapsedMs)
        {
            if (_spritesheet.Animations.TryGetValue(animationKey, out Animation animation))
            {
                if (_currentAnimation.AnimationName != animationKey)
                {
                    StopAnimation();
                    ResetAnimation();
                }

                if (!animation.IsPlaying)
                {                  
                    _currentAnimation = animation;
                    StartAnimation();                 
                }

                UpdateAnimation(elapsedMs);
            }
            else
            {
                StopAnimation();
                ResetAnimation();
            }

        }

        /// <summary>
        /// Update the current frame based on elapsed game time and frame duration.
        /// This allows the animation to be frame rate independent
        /// and for the animation to progress at the same speed regardless of the frame rate
        /// </summary>
        private void UpdateAnimation(float elapsedMs)
        {
            // If the game is running at 30fps, ElapsedGameTimeMs will be approx 33.33ms
            // _timeSinceLastFrameMs will accumulate these values until it reaches the frame duration
            // If the frame duration is 100ms, the frame will update every 6 frames at 60fps and every 3 frames at 30fps
            // _timeSinceLastFrame += _gameSettings.ElapsedGameTimeMs;
            _timeSinceLastFrame += elapsedMs;

            if (_timeSinceLastFrame >= _currentAnimation.Frames[_currentFrameNumber].Duration)
            {
                _currentFrameNumber++;

                if (_currentFrameNumber == _currentAnimation.Frames.Count)
                {
                    OnAnimationComplete(new AnimCompleteEventArgs(_currentAnimation.AnimationName));

                    // If looping, reset to first frame, otherwise stay on last frame and stop playing
                    if (_currentAnimation.IsLooping)
                        _currentFrameNumber = 0;
                    else
                        _currentFrameNumber--;
                }

                _timeSinceLastFrame = 0;
            }
        }

        public Color[,] GetColourDataForCurrentFrame()
        {     
            return _spritesheet.SpritesheetTexture.GetColourData(_currentAnimation.Frames[_currentFrameNumber].SourceRectangle);       
        }

        public void Draw(Vector2 position, float rotation = 0f, Vector2 origin = new Vector2())
        {
            _spriteRenderer.Draw(_spritesheet.SpritesheetTexture, position, _currentAnimation.Frames[_currentFrameNumber].SourceRectangle, rotation, origin);
        }

        private void StartAnimation()
        {
            _currentAnimation.Start();
             OnAnimationStarted(new AnimStartedEventArgs(_currentAnimation));
        }

        private void StopAnimation()
        {
            _currentAnimation.Stop();
            OnAnimationComplete(new AnimCompleteEventArgs(_currentAnimation.AnimationName));
        }

        private void ResetAnimation()
        {
            _currentFrameNumber = 0;
        }
    }
}
