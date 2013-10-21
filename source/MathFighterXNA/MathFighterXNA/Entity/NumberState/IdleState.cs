using ClownSchool.Tweening;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool.Entity.NumberState {

    public class IdleState : INumberState {
        public DragableNumber Owner;

        public IdleState(DragableNumber owner) {
            Owner = owner;
        }

        void INumberState.OnHandCollide(PlayerHand hand) {
        }

        void INumberState.OnSlotCollide(NumberSlot slot) {
        }

        void INumberState.Update(Microsoft.Xna.Framework.GameTime gameTime) {           

        }

        void INumberState.Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
        }
    }
}
