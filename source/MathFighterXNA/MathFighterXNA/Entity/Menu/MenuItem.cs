using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ClownSchool.Entity.Menu {
    public class MenuItem : BaseEntity {

        public Texture2D Graphic { get; set; }
        public Action OnClick { get; set; }

        public Menu Menu { get; set; }

        public bool RenderBalloons { get; set; }
        public bool RenderRopes { get; set; }

        private bool selected { get; set; }

        private float hoverTime = 0f;
        private float maxHoverTime = 2f;

        public MenuItem(Texture2D graphic, int posX, int posY, Action onClick) {
            Graphic = graphic;
            OnClick = onClick;

            X = posX;
            Y = posY;

            Size = new Point(200, 107);

            RenderBalloons = false;
            RenderRopes = false;
        }

        public override void Init() {
            
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
                      
            var _selected = selected;
            selected = GetFirstCollidingEntity("hand") != null;

            if (!_selected && selected) {
                Assets.MenuOver.Play();
            }

            if (selected) {
                hoverTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (hoverTime >= maxHoverTime) {
                    hoverTime = 0;
                    Assets.MenuChoose.Play();
                    
                    if (OnClick != null) {
                        OnClick();
                    }
                }
            } else {
                hoverTime = 0;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (RenderBalloons) {
                spriteBatch.Draw(Assets.MenuBalloons, new Rectangle((int)X - 55, (int)Y - 190, 150, 195), Color.White);
                spriteBatch.Draw(Assets.MenuBalloons, new Rectangle((int)X + 123, (int)Y - 190, 150, 195), Color.White);
            }

            if (RenderRopes) {
                spriteBatch.Draw(Assets.MenuRope, new Rectangle((int)X + 10, (int)Y + 95, 3, 115), Color.White);
                spriteBatch.Draw(Assets.MenuRope, new Rectangle((int)X + 188, (int)Y + 95, 3, 115), Color.White);
            }

            if (selected) {
                spriteBatch.Draw(Assets.MenuSignGlow, new Rectangle((int)X - 30, (int)Y - 30, Size.X + 60, Size.Y + 60), Color.White);
            }

            spriteBatch.Draw(Graphic, new Rectangle((int)X, (int)Y, Size.X, Size.Y), Color.White);

            if (hoverTime > 0 && hoverTime <= maxHoverTime) {
                PlayerHand hand = (PlayerHand)GetFirstCollidingEntity("hand");
                if (hand == null)
                    return;

                for (int i = 0; i <= 360; i++) {
                    var destRect = new Rectangle((int)hand.X - 50, (int)hand.Y - 50, 1, 20);

                    var asset = Assets.CirclePartFilled;
                    if ((360 / maxHoverTime) * hoverTime <= i) {
                        asset = Assets.CirclePartEmpty;
                    }

                    spriteBatch.Draw(asset, destRect, null, Color.White, MathHelper.ToRadians(i), new Vector2(0, 20), SpriteEffects.None, 0);
                }
            }
        }

        public override void Delete() {

        }
    }
}
