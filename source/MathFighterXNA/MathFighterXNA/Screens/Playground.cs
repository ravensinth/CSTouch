using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;

namespace ClownSchool.Screens {

    public class Playground : GameScreen {
        public Player Player { get; set; }        
        
        public Playground(KinectContext context) : base(context) {
        }

        public override void Init() {
            Player = new Player(Context, SkeletonPlayerAssignment.FirstSkeleton);
            AddEntity(Player);
            
            for (int i = 1; i <= 10; i++) {
                AddEntity(new DragableNumber(Player, (60 * i) - 30, 20, i));
            }                       
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);               
        }        

        public override void Draw(SpriteBatch spriteBatch) {
            foreach (var ent in Entities) {
                ent.Draw(spriteBatch);
            }                
        }
    }
}