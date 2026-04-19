using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoSprite
{
    public class AnimStartedEventArgs : EventArgs
    {
        public readonly Animation Animation;

        public AnimStartedEventArgs(Animation animation)
        {
            Animation = animation;
        }
    }

    public class AnimCompleteEventArgs : EventArgs
    {
        public readonly string AnimationName;

        public AnimCompleteEventArgs(string animationName)
        {
            AnimationName = animationName;
        }
    }

    public class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }
        public int Duration { get; set; }

        public AnimationFrame(Rectangle sourceRectangle, int duration)
        {
            SourceRectangle = sourceRectangle;
            Duration = duration;
        }
    }

    public class Animation
    {
        public string AnimationName { get; private set; }
        public List<AnimationFrame> Frames { get; private set; }
        public bool IsPlaying { get; private set; }  
        public bool IsLooping { get; private set; }

        public Animation(
            string animationName,          
            IEnumerable<AnimationFrame> frames,
            bool isLooping
           )
        {
            AnimationName = animationName;
            IsLooping = isLooping;
            Frames = frames.ToList();
        }

        public void Start()
        {
            IsPlaying = true;
            // OnAnimationStarted(new AnimationStartedEventArgs(this));
        }

        public void Stop()
        {
            IsPlaying = false;
        }
    }
}
