using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ClownSchool {
    public class Animation {
        
        private float frameTimeElapsed;

        public bool Finished { get; private set; }        

        public Texture2D SpriteSheet { get; set; }

        public int FrameCount {
            get { return (int)SpriteSheet.Width / FrameWidth; }
        }

        public float FrameTime { get; set; }

        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }

        public string Name { get; set; }
        public bool LoopAnimation { get; set; }

        private int currentFrame;
        public int CurrentFrame {
            get {
                return currentFrame;
            }
            set {
                if (value <= FrameCount - 1) currentFrame = value;
            }
        }

        public Rectangle FrameRectangle {
            get { return new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight); }
        }

        public Animation(string animationName, Texture2D spriteSheet, int frameWidth) {
            this.Name = animationName;
            this.SpriteSheet = spriteSheet;
            this.FrameWidth = frameWidth;
            this.FrameHeight = spriteSheet.Height;

            this.FrameTime = 0.05f;
            this.frameTimeElapsed = 0;

            this.LoopAnimation = true;

            this.currentFrame = 0;
        }

        public void Play(bool loop) {
            LoopAnimation = loop;
            currentFrame = 0;
            Finished = false;
        }

        public void Update(GameTime gameTime) {
            frameTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimeElapsed >= FrameTime && !Finished) {
                if (currentFrame == FrameCount - 1) {
                    if (!LoopAnimation) {
                        Finished = true;
                        return;
                    } else {
                        currentFrame = 0;
                    }
                } else {
                    currentFrame++;
                }

                frameTimeElapsed = 0;
            }
        }
    }
}
