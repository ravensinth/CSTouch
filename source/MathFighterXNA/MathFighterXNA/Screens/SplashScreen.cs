using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ClownSchool.Tweening;
using System.Diagnostics;

namespace ClownSchool.Screens {
    public class SplashScreen : GameScreen {

        public Texture2D Graphic { get; set; }
        public float Duration { get; set; }

        private float alpha = 0f;

        private Tweener alphaTweener;

        private int tweenCount = 0;

        public bool TweenerFinished {
            get {
                return tweenCount >= 2;
            }
        }

        public SplashScreen(KinectContext context, Texture2D graphic, float duration) : base(context) {
            Graphic = graphic;
            Duration = duration;

            alphaTweener = new Tweener(alpha, 1, duration, Linear.EaseIn);
            alphaTweener.Ended += delegate() { alphaTweener.Reverse(); tweenCount++; };
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);
            
            alphaTweener.Update(gameTime);
            alpha = alphaTweener.Position;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);

            spriteBatch.Draw(Assets.PauseBackground, new Rectangle(0, 0, MainGame.Width, MainGame.Height), Color.Black);

            int logoWidth = 700;
            int logoHeight = 714;

            spriteBatch.Draw(Graphic, new Rectangle((MainGame.Width / 2) - logoWidth / 2, (MainGame.Height / 2) - logoHeight / 2, logoWidth, logoHeight), new Color(alpha, alpha, alpha, alpha));
        }
    }
}
