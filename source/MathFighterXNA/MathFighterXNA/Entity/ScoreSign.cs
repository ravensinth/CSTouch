using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ClownSchool.Bang.Actions;
using ClownSchool.Tweening;

namespace ClownSchool.Entity {
    public class ScoreSign : BaseEntity {

        public int Value { get; set; }

        public ScoreSign(int posX, int posY) {
            X = posX;
            Y = posY;

            collidable = false;

            Size = Size = new Point(200, 107);
        }

        public void AddPoints() {
            Value += 5;
            var plusFive = new SimpleGraphic(Assets.ClockAddFive, (int)X + 50, (int)Y + 30, 92, 67);
            Screen.AddEntity(plusFive);
            plusFive.Actions.AddAction(new TweenPositionTo(plusFive, new Vector2(plusFive.X, plusFive.Y + 100), 1f, Bounce.EaseOut), true);
            plusFive.Actions.AddAction(new CallFunction(delegate() { Screen.RemoveEntity(plusFive); }), false);
        }

        public void SubtractPoints() {
            Value -= 5;
            var minusFive = new SimpleGraphic(Assets.ClockSubtractFive, X, Y, 92, 67);
            Screen.AddEntity(minusFive);
            minusFive.Actions.AddAction(new TweenPositionTo(minusFive, new Vector2(minusFive.X, minusFive.Y + 100), 1f, Bounce.EaseOut), true);
            minusFive.Actions.AddAction(new CallFunction(delegate() { Screen.RemoveEntity(minusFive); }), false);
        }

        public override void Init() {
           
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.ScoreSign, BoundingBox, Color.White);

            foreach (var num in FontNumber.FromInteger(Value, X + 45, Y + 60, new Point(27, 40), "0000", FontNumber.FontNumberColor.Yellow)) {
                num.Draw(spriteBatch);
            }
        }

        public override void Delete() {
            throw new NotImplementedException();
        }
    }
}
