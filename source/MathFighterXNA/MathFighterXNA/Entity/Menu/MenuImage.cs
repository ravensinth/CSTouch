using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ClownSchool.Entity.Menu {
    public class MenuImage : BaseEntity {

        public Texture2D Graphic { get; set; }
        public Action OnClick { get; set; }

        public Menu Menu { get; set; }

        public MenuImage(Texture2D graphic, int posX, int posY, Point size, Action onClick) {
            onClick = OnClick;
            Graphic = graphic;

            X = posX;
            Y = posY;

            Size = size;
        }

        public MenuImage() { 
        
        }

        public override void Init() {
            
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(Graphic, new Rectangle((int)X, (int)Y, Size.X, Size.Y), Color.White);
        }

        public override void Delete() {

        }
    }
}
