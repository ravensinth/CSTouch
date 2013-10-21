using System;
using Microsoft.Xna.Framework;

namespace ClownSchool.Bang.Actions {

    public class WaitForCondition : IAction {

        public Func<bool> Condition { get; set; }

        private bool isComplete { get; set; }

        public WaitForCondition(Func<bool> condition) {
            Condition = condition;
        }

        public bool IsBlocking() {
            return true;
        }

        public bool IsComplete() {
            return isComplete;
        }

        public void Block() {
        }

        public void Unblock() {
        }

        public void Update(GameTime gameTime) {
            if (Condition())
                Complete();
        }

        public void Complete() {
            isComplete = true;
        }
    }
}
