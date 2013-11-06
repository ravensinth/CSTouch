using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect.Toolkit.Interaction;

namespace ClownSchool {

    public class Player : BaseEntity {        
        public SkeletonPlayerAssignment SkeletonAssignment { get; set; }
        public KinectContext Context { get; private set; }

        public PlayerHand LeftHand { get; private set; }
        public PlayerHand RightHand { get; set; }

        public bool DrawHands { get; set; }

        public bool IsDragging {
            get {
                return LeftHand.DraggingBalloon != null || RightHand.DraggingBalloon != null;
            }
        }

        //public Skeleton Skeleton {
        //    get {
        //        switch (SkeletonAssignment) {
        //            case SkeletonPlayerAssignment.FirstSkeleton:
        //                return Context.GetFirstSkeleton();
        //            case SkeletonPlayerAssignment.LeftSkeleton:
        //                return Context.GetLeftSkeleton();
        //            case SkeletonPlayerAssignment.RightSkeleton:
        //                return Context.GetRightSkeleton();
        //        }

        //        return null;
        //    }
        //}

        public bool IsReady {
            get {
                return true;
            }
        }     

        public Player(KinectContext context, SkeletonPlayerAssignment assignment) {
            this.Context = context;
            this.SkeletonAssignment = assignment;            

            LeftHand = new PlayerHand(this, JointType.HandLeft);
            RightHand = new PlayerHand(this, JointType.HandRight);

            DrawHands = true;
        }

        public override void Init() {
            Screen.AddEntity(LeftHand);
            Screen.AddEntity(RightHand);
        }

        public override void Update(GameTime gameTime) {   
                LeftHand.Update(gameTime);
                RightHand.Update(gameTime);            
        }

        public override void Draw(SpriteBatch spriteBatch) {
                LeftHand.Draw(spriteBatch);
                RightHand.Draw(spriteBatch);                      
        }

        public override void Delete() {
            Screen.RemoveEntity(LeftHand);
            Screen.RemoveEntity(RightHand);
        }
    }
}
