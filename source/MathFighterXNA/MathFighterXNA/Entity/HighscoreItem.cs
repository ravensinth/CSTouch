using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownSchool.Highscore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool.Entity {
    public class HighscoreItem : BaseEntity {

        public Score Score { get; set; }

        private float pictureSizeDivisor = 9f;

        private static float maxPictureSizeDivisor = 9f;
        private static float minPictureSizeDivisor = 2f;

        public HighscoreItem(int posX, int posY, Score score) {
            Score = score;
            X = posX;
            Y = posY;

            Size = new Point((int)(Score.Picture.Width / maxPictureSizeDivisor), (int)(Score.Picture.Height / maxPictureSizeDivisor));
            Offset = new Point(4, 10);

            CollisionType = "score_item";
        }

        public override void Init() {
            
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (GetFirstCollidingEntity("hand") != null) {
                if (pictureSizeDivisor > minPictureSizeDivisor) {
                    pictureSizeDivisor -= 0.2f;
                    ZDepth = 100;
                } else {
                    pictureSizeDivisor = minPictureSizeDivisor;                    
                }
            } else {
                if (pictureSizeDivisor < maxPictureSizeDivisor) {
                    pictureSizeDivisor += 0.2f;
                } else {
                    pictureSizeDivisor = maxPictureSizeDivisor;
                    ZDepth = 1;
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(Score.Picture, new Rectangle(X + 80, Y + 55, (int)(Score.Picture.Width / pictureSizeDivisor), (int)(Score.Picture.Height / pictureSizeDivisor)), null, Color.White, 0, new Vector2(Score.Picture.Width / 2, Score.Picture.Height / 2), SpriteEffects.None, 0);
            foreach (var num in FontNumber.FromInteger(Score.Value, X + 300, Y + 25, new Point(27, 40), "0000", FontNumber.FontNumberColor.Yellow)) {
                num.Draw(spriteBatch);
            }         
        }

        public override void Delete() {            
        }
    }
}
