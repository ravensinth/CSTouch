using System;
using Microsoft.Xna.Framework;

namespace ClownSchool.Bang.Actions {

    public class CallFunction : IAction {

        public Action Function { get; set;}      

        private bool isComplete { get; set; }
        private bool isBlocking { get; set; }

        public CallFunction(Action function) {
            Function = function;
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

        public void Update(GameTime gameTime) {
            Function();

            Complete();
        }

        public void Complete() {
            isComplete = true;
        }
    }
}
