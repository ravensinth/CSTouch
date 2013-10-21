using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClownSchool.Bang.Actions {
    public class WaitForPlayerCount : IAction {

        private bool isBlocking { get; set; }
        private bool isComplete { get; set; }

        private KinectContext context { get; set; }
        private int count { get; set; }

        public WaitForPlayerCount(int count, KinectContext context) {
            this.context = context;
            this.count = count;
        }

        public bool IsBlocking() {
            return isBlocking;
        }

        public bool IsComplete() {
            return isComplete;
        }

        public void Block() {
            isBlocking = true;
        }

        public void Unblock() {
            isBlocking = false;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            if (context.Skeletons.Count >= count) {
                Complete();
            }
        }

        public void Complete() {
            isComplete = true;
        }
    }
}
