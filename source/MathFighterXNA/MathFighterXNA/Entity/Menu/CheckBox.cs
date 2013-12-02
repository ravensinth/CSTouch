using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ClownSchool.Entity.Menu {
    public class CheckBox : BaseEntity {


        public Point SizeChecked { get; set; }
        public Point SizeUnchecked { get; set; }
        public Texture2D GraphicChecked { get; set; }
        public Texture2D GraphicUnchecked { get; set; }
        public Action OnClick { get; set; }
        public bool Checked { get; set; }
        public Settings.type SettingType { get; set; }

        public Menu Menu { get; set; }

        private bool selected { get; set; }

        public CheckBox(Settings.type settingType, Texture2D graphicChecked, Texture2D graphicUnchecked, int posX, int posY) {
            GraphicChecked = graphicChecked;
            GraphicUnchecked = graphicUnchecked;
            SettingType = settingType;

            X = posX;
            Y = posY;

            Size = new Point(196, 149);
            SizeChecked = new Point(221, 149);
            SizeUnchecked = new Point(196, 149);
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
                    Settings.ChangeSetting(SettingType);
                }
            }
            Checked = Settings.GetValueByType(SettingType);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (selected) {
                spriteBatch.Draw(Assets.MenuSignGlow, new Rectangle((int)X - 30, (int)Y - 30, SizeUnchecked.X + 60, SizeUnchecked.Y + 60), Color.White);
            }
            if (Checked) {
                spriteBatch.Draw(GraphicChecked, new Rectangle((int)X, (int)Y, SizeChecked.X, SizeChecked.Y), Color.White);
            }
            else {
                spriteBatch.Draw(GraphicUnchecked, new Rectangle((int)X, (int)Y, SizeUnchecked.X, SizeUnchecked.Y), Color.White);
            }

            
        }

        public override void Delete() {

        }
    }
}
