using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;
using ClownSchool.Entity;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Linq;

namespace ClownSchool {

    public class PlayerHand : BaseEntity {

        public Vector2 Position { get; set; }        
        public Player Player { get; private set; }
        public JointType Hand { get; private set; }        
        public Balloon DraggingBalloon { get; set; }
        public bool HandClosed { get; set; }

        public bool IsGrabbing { get; set; }
        public MouseState MouseState;

        public KinectContext Context {
            get {
                return Player.Context;
            }
        }

        public PlayerHand(Player player, JointType hand) {            
            this.Offset = new Point(-30, -30);
            this.Size = new Point(50, 50);

            Player = player;
            Hand = hand;

            CollisionType = "hand";

            ZDepth = 1000;
        }

        public override void Init() {
        }

        public override void Update(GameTime gameTime) {
            Matrix Scale = Matrix.CreateScale(1, 1, 1);
            MouseState = Mouse.GetState();

            var pos = new Vector2(MouseState.X, MouseState.Y);
            this.Position = Vector2.Transform(pos, Scale);
            X = Position.X;
            Y = Position.Y;

            this.HandClosed = MouseState.LeftButton == ButtonState.Pressed;
        }

        public override void Draw(SpriteBatch spriteBatch) {       
            if (Player.DrawHands) {
                var glove = IsGrabbing ? Assets.GloveFist : Assets.Glove;
                spriteBatch.Draw(glove, new Rectangle((int)Position.X, (int)Position.Y, 56, 64), null, Color.White, 0, new Vector2(glove.Width / 2, glove.Height / 2), Hand == JointType.HandLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
        }

        public void Grab(Balloon balloon) {
            balloon.AttachTo(this);
            this.DraggingBalloon = balloon;

            Assets.BalloonGrab.Play();  
        }

        public override void Delete() {
        }
    }
}