using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Interaction;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool {

    public class KinectContext {
        
        public Texture2D CurrentBitmap { get; private set; }      
        public Dictionary<int, UserInfo> UserInfos { get; private set; }
        private GraphicsDevice graphicsDevice { get; set; }

        public KinectContext(GraphicsDevice device) {
            graphicsDevice = device;
            UserInfos = new Dictionary<int, UserInfo>();
        }

        public void Initialize() {            
            this.CurrentBitmap = Assets.SplashLogo;
        }

    }
}