using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ClownSchool
{
    public class SkeletonRenderer
    {
        public KinectContext Context { get; private set; }

        public Texture2D CurrentBitmap { get; private set; }
        
        private readonly Color centerPointBrush = Color.Blue;
        private readonly Color trackedJointBrush = Color.Yellow;
        private readonly Color inferredJointBrush = Color.Black;

        private Vector2 jointOrigin;
        public Texture2D JointSprite;

        public SkeletonRenderer(KinectContext context)
        {
            Context = context;            
        }

        public void LoadContent(ContentManager content)
        {
            JointSprite = Assets.JointSprite;            
            this.jointOrigin = new Vector2(this.JointSprite.Width / 2, this.JointSprite.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Skeleton skel in Context.Skeletons)
            {
                if (skel.TrackingState == SkeletonTrackingState.Tracked)
                {
                    DrawJoints(skel, spriteBatch);
                }
            }
        }

        private void DrawJoints(Skeleton skeleton, SpriteBatch spriteBatch)
        {                        
            // Render Joints
            foreach (Joint j in skeleton.Joints)
            {
                Color jointColor = Color.Green;
                if (j.TrackingState != JointTrackingState.Tracked)
                {
                    jointColor = Color.Yellow;
                }

                //spriteBatch.Draw(this.JointSprite, this.Context.SkeletonPointToScreen(j.Position), null, jointColor, 0.0f, this.jointOrigin, 1.0f, SpriteEffects.None, 0.0f);
            }
        }
    }
}