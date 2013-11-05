using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;
using ClownSchool.Entity;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Linq;
using System.Diagnostics;

namespace ClownSchool {

    public class PlayerHand : BaseEntity {

        public Vector2 Position { get; set; }        
        public Player Player { get; private set; }
        public JointType Hand { get; private set; }        
        public Balloon DraggingBalloon { get; set; }

        public bool IsGrabbing { get; set; }

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

        private InteractionHandPointer getHandPointer() {   
            UserInfo userInfo;

            //if (Context.UserInfos.TryGetValue(Player.Skeleton.TrackingId, out userInfo)) {
            //    return (from InteractionHandPointer hp in userInfo.HandPointers where hp.HandType == (Hand == JointType.HandLeft ? InteractionHandType.Left : InteractionHandType.Right) select hp).FirstOrDefault();
            //}

            return null;         
        }

        public override void Update(GameTime gameTime) {
            //########################### Das kommt auf jeden Fall nicht hier hin
            //float scaleX = 1324f;
            //float scaleY = 768f;


            Matrix Scale = Matrix.CreateScale(1, 1, 1);

            var pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            this.Position = Vector2.Transform(pos, Scale);
            Debug.WriteLine("MouseX: " + Mouse.GetState().X + " MausY: " + Mouse.GetState().Y);
            Debug.WriteLine(this.Position);

        }

        public override void Draw(SpriteBatch spriteBatch) {
            //if (!Player.IsReady)
            //    return;
            
            if (Player.DrawHands) {
                var glove = IsGrabbing ? Assets.GloveFist : Assets.Glove;
                spriteBatch.Draw(glove, new Rectangle((int)Position.X, (int)Position.Y, 56, 64), null, Color.White, 0, new Vector2(glove.Width / 2, glove.Height / 2), Hand == JointType.HandLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                Debug.WriteLine("X: " + X + ", Y: " + Y);
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