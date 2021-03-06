﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ClownSchool.Entity.Menu {
    public class MenuButton : BaseEntity {

        public Texture2D Graphic { get; set; }
        public Action OnClick { get; set; }

        public Menu Menu { get; set; }

        private bool selected { get; set; }

        public MenuButton(Texture2D graphic, int posX, int posY, Action onClick) {
            Graphic = graphic;
            OnClick = onClick;

            X = posX;
            Y = posY;

            Size = new Point(130, 130);
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
                if (hand.Clicked) {
                    Assets.MenuChoose.Play();
                    OnClick();
                }

            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (selected) {
                spriteBatch.Draw(Assets.MenuButtonGlow, new Rectangle((int)X - 30, (int)Y - 30, Size.X + 60, Size.Y + 60), Color.White);
            }

            spriteBatch.Draw(Graphic, new Rectangle((int)X, (int)Y, Size.X, Size.Y), Color.White);
        }

        public override void Delete() {

        }
    }
}
