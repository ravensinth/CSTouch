using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClownSchool.Entity.Menu {
    public class Menu : BaseEntity {

        public List<MenuItem> Items = new List<MenuItem>();

        public Menu() {
            collidable = false;
        }

        public void AddItem(MenuItem item) {
            item.Menu = this;
            Items.Add(item);
        }

        public override void Init() {
            foreach (var itm in Items) {
                Screen.AddEntity(itm);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            foreach (var itm in Items) {
                itm.Update(gameTime);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            foreach (var itm in Items) {
                itm.Draw(spriteBatch);
            }
        }

        public override void Delete() {
            foreach (var itm in Items) {
                Screen.RemoveEntity(itm);
            }
        }
    }
}
