using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownSchool.Entity;

namespace ClownSchool.Bang.Actions {
    public class WaitForEquationInput : IAction {

        public EquationInput Input { get; set; }
        public EquationInputType Type { get; set; }

        private bool isComplete { get; set; }

        public WaitForEquationInput(EquationInput input, EquationInputType type) {
            Input = input;
            Type = type;
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

        public void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            if ((Type == EquationInputType.Equation && Input.IsEquationSet) || (Type == EquationInputType.Product && Input.IsAnswerSet)) {
                Complete();
            }
        }

        public void Complete() {
            isComplete = true;
        }
    }

    public enum EquationInputType {
        Equation,
        Product        
    }
}
