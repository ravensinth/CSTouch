using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool {
    public class TutorialHand : PlayerHand {
        public TutorialHand(Player player, JointType hand)
            : base(player) {

            
        }

        public void Grab() {
            IsGrabbing = true;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            Actions.Update(gameTime);
            Coroutines.Update();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            var glove = IsGrabbing ? Assets.GloveFist : Assets.Glove;

            spriteBatch.Draw(IsGrabbing ? Assets.GloveFist : Assets.Glove, new Rectangle((int)X, (int)Y, 56, 64), null, Color.White, 0, new Vector2(glove.Width / 2, glove.Height / 2), Hand == JointType.HandLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);      
        }
    }
}
