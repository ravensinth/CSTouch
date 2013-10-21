using ClownSchool.Entity;
using Microsoft.Xna.Framework;
using ClownSchool.Tweening;

namespace ClownSchool.Bang.Actions {
    public class EndEquationInput : IAction {

        public EquationInput Input { get; set; }        

        private bool isComplete { get; set; }

        public EndEquationInput(EquationInput input) {
            Input = input;
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
            if (Input.IsAnswerCorrect) {
                Input.Actions.InsertAfter(this, new TweenPositionTo(Input, new Vector2(Input.X, -300), 2f, Back.EaseOut), true);
                Assets.AnswerCorrect.Play();
            } else {
                Input.PopBalloons();
                Input.Actions.InsertAfter(this, new TweenPositionTo(Input, new Vector2(Input.X, 1000), 2f, Back.EaseOut), true);
                Assets.AnswerWrong.Play();
            }

            Complete();
        }

        public void Complete() {
            isComplete = true;
        }
    }
}
