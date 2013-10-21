using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using ClownSchool.Tweening;
using Microsoft.Xna.Framework;

namespace ClownSchool.Entity {
    public class Camera : BaseEntity {

        public string SaveTo { get; set; }

        private Tweener flash { get; set; }

        public Camera(string saveTo) {
            SaveTo = saveTo;

            ZDepth = 1000;

            collidable = false;
        }

        public override void Init() {
            
        }

        public void TakePicture(int score) {
            Assets.CameraClick.Play();
            var filename = Path.Combine(SaveTo, score.ToString());
            Screen.Manager.Game.SaveScreenshot(filename);

            flash = new Tweener(0f, 1f, 0.2f, Linear.EaseIn);
            flash.Ended += delegate() {                                              
                flash = new Tweener(1f, 0f, 1f, Linear.EaseIn);
                flash.Ended += delegate() { Screen.RemoveEntity(this); };
            };

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            if (flash != null) {
                flash.Update(gameTime);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (flash != null) {
                spriteBatch.Draw(Assets.CameraFlash, new Rectangle(0, 0, MainGame.Width, MainGame.Height), new Color(flash.Position, flash.Position, flash.Position, flash.Position));
            }            
        }

        public override void Delete() {
            
        }
    }
}
