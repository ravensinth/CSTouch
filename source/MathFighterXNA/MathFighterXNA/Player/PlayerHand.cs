using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using ClownSchool.Entity;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Linq;

namespace ClownSchool {

    public class PlayerHand : BaseEntity {

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

            if (Context.UserInfos.TryGetValue(Player.Skeleton.TrackingId, out userInfo)) {
                return (from InteractionHandPointer hp in userInfo.HandPointers where hp.HandType == (Hand == JointType.HandLeft ? InteractionHandType.Left : InteractionHandType.Right) select hp).FirstOrDefault();
            }

            return null;         
        }

        public override void Update(GameTime gameTime) {
            if (!Player.IsReady)
                return;

                #region dragging
                if (Configuration.GRABBING_ENABLED) {
                    var handPointer = getHandPointer();

                    if (handPointer != null) {
                        if (handPointer.HandEventType == InteractionHandEventType.Grip) {
                            IsGrabbing = true;
                        } else if (handPointer.HandEventType == InteractionHandEventType.GripRelease) {
                            IsGrabbing = false;

                            if (this.DraggingBalloon != null) {
                                DraggingBalloon.Cut();
                                DraggingBalloon = null;
                            }
                        }
                    }
                }
                #endregion
                           
                this.Position = Context.SkeletonPointToScreen(Player.Skeleton.Joints[Hand].Position);                 
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!Player.IsReady)
                return;

            if (Player.DrawHands) {
                var glove = IsGrabbing ? Assets.GloveFist : Assets.Glove;
                spriteBatch.Draw(glove, new Rectangle(X, Y, 56, 64), null, Color.White, 0, new Vector2(glove.Width / 2, glove.Height / 2), Hand == JointType.HandLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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