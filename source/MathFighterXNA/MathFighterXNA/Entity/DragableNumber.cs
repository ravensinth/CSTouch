using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;
using ClownSchool.Entity.NumberState;

namespace ClownSchool {

    public class DragableNumber : BaseEntity {        

        public Player Owner { get; set; }
        public int Number { get; private set; }

        //States
        public INumberState State;
        public DefaultState DefaultState;
        public IdleState IdleState;

        private static Color SelectedColor = Color.White;
        private static Color UnselectedColor = new Color(200, 200, 200, 100);

        private bool selected { get; set; }

        public DragableNumber(Player owner, int posX, int posY, int number) {
            Owner = owner;
            Position = new Vector2((int)posX, (int)posY);
            Size = new Point(52, 64);
            Offset = new Point(5, 5);

            Number = number;

            CollisionType = "number";

            DefaultState = new DefaultState(this);
            IdleState = new IdleState(this);

            State = DefaultState;
        }

        public override void Init() {

        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var hand = (PlayerHand)GetFirstCollidingEntity("hand");
            if (hand != null) {
                State.OnHandCollide(hand);               
            }

            selected = hand != null && (hand.Player == Owner || Owner == null);

            State.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.BalloonSpritesheet, new Rectangle((int)X, (int)Y, selected ? (int)(62 * 1.2f) : 62, selected ? (int)(89 * 1.2f) : 89), new Rectangle(62 * (Number), 0, 62, 89), selected ? SelectedColor : UnselectedColor);

            State.Draw(spriteBatch);            
        }

        public override void Delete() {
        }
    }
}