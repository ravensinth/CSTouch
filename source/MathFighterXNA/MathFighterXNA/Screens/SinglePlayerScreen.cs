using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;
using System.Collections.Generic;
using System;


namespace ClownSchool.Screens {

    public class SinglePlayerScreen : CoopPlayerScreen {

        public SinglePlayerScreen(KinectContext context)
            : base(context) {                
        }

        public override void AddPlayers() {
            PlayerOne = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            PlayerTwo = PlayerOne;

            AddEntity(PlayerOne);
        }

        public override void Init() {
            base.Init();

            NeededPlayerCount = 1;
        }

    }
}