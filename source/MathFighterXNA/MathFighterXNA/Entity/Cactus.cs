using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool.Entity {
    public class Cactus : BaseEntity {

        public float Rotation;

        public Cactus(int posX, int posY, float rotation) {
            X = posX;
            Y = posY;

            Size = new Point(110, 241);
            Offset = new Point(20, 20);

            Rotation = rotation;
       
            CollisionType = "cactus";
        }

        public override void Init() {
            
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var collidingBalloon = GetFirstCollidingEntity("dragged_number");
            if (collidingBalloon != null) {
                Assets.BalloonPop.Play();
                Screen.RemoveEntity(collidingBalloon);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.CactusSprite, new Rectangle((int)X, (int)Y, 150, 261), null, Color.White, MathHelper.ToRadians(Rotation), new Vector2(0, 0), SpriteEffects.None, 0);
        }

        public override void Delete() {            
        }
    }
}
