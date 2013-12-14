using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ClownSchool.Bang.Actions;
using ClownSchool.Entity;
using ClownSchool.Entity.Menu;

namespace ClownSchool.Screens {
    public class MessageScreen : GameScreen {


        public Player Player;

        public MessageScreen(KinectContext context): base(context) {

        }

        public void Message() {

            var posMenu = new Point(300, (MainGame.Height / 2) - 250);
            var posResume = new Point(MainGame.Width - 600, (MainGame.Height / 2) - 250);

            var mi = new MenuItem(Assets.SignMenu, posMenu.X, posMenu.Y, delegate() { Manager.SwitchScreen(new MenuScreen(Context)); });
            AddEntity(mi);

            mi = new MenuItem(Assets.SignResume, posResume.X, posMenu.Y, delegate() { Manager.RemoveScreen(this); });
            AddEntity(mi);
            
        }



        public override void Draw(SpriteBatch spriteBatch) {           
            spriteBatch.Draw(Assets.PauseBackground, new Rectangle(0, 0, MainGame.Width, MainGame.Height), new Color(255, 255, 255, 200));



            base.Draw(spriteBatch);
        }

    }
}
