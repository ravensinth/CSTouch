using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
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

        private MouseState OldMouseState;
        private MouseState MouseState;

        public MenuItem(Texture2D graphic, int posX, int posY, Action onClick) {
            Graphic = graphic;
            OnClick = onClick;

            X = posX;
            Y = posY;

            Size = new Point(200, 107);

            RenderBalloons = false;
            RenderRopes = false;
        }

        public MenuItem() { 
        
        }

        public override void Init() {
            
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var _selected = selected;
            var hand = (PlayerHand)GetFirstCollidingEntity("hand");
            selected = hand != null;

            if (!_selected && selected) {
                Assets.MenuOver.Play();
            }

            if (selected) {
                OldMouseState = MouseState;
                MouseState = hand.MouseState;                
                if (selected && MouseState.LeftButton == ButtonState.Released && OldMouseState.LeftButton == ButtonState.Pressed) {
                    Assets.MenuChoose.Play();
                    OnClick();
                }
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
        }

        public override void Delete() {

        }
    }
}
