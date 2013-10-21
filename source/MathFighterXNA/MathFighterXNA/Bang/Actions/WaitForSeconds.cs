using System;
using Microsoft.Xna.Framework;

namespace ClownSchool.Bang.Actions {

    public class WaitForSeconds : IAction {

        public float Amount { get; set; }

        float currentTime;

        private bool isComplete { get; set; }

        public WaitForSeconds(float amount) {
            Amount = amount;
            currentTime = Amount;
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
            currentTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime <= 0) {
                Complete();
            }
        }

        public void Complete() {
            isComplete = true;
        }
    }
}
