using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Bang.Actions;
using ClownSchool.Tweening;

namespace ClownSchool.Entity {
    public class Clock : BaseEntity {

        public int StartValue { get; private set; }
        public float Value { get; set; }

        public bool Paused { get; set; }

        private float secondTimer = 1f;

        public static Point ClockSize = new Point(110, 110);

        public Clock(int posX, int posY, int seconds) {
            X = posX;
            Y = posY;

            Size = ClockSize;

            StartValue = seconds;
            Value = StartValue;

            Paused = false;

            collidable = false;
        }

        public override void Init() {
            
        }

        public void Switch() {
            Paused = !Paused;
        }

        public void AddTime() {
            Value += 5f;
            var plusFive = new SimpleGraphic(Assets.ClockAddFive, X, Y, 92, 67);
            Screen.AddEntity(plusFive);
            plusFive.Actions.AddAction(new TweenPositionTo(plusFive, new Vector2(plusFive.X, plusFive.Y + 100), 1f, Bounce.EaseOut), true);
            plusFive.Actions.AddAction(new CallFunction(delegate() { Screen.RemoveEntity(plusFive); }), false);
        }

        public void SubtractTime() {
            Value -= 5f;
            var minusFive = new SimpleGraphic(Assets.ClockSubtractFive, X, Y, 92, 67);
            Screen.AddEntity(minusFive);
            minusFive.Actions.AddAction(new TweenPositionTo(minusFive, new Vector2(minusFive.X, minusFive.Y + 100), 1f, Bounce.EaseOut), true);
            minusFive.Actions.AddAction(new CallFunction(delegate() { Screen.RemoveEntity(minusFive); }), false);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            if (!Paused) {
                Value -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Value < 0) {
                    Value = 0;
                    Paused = true;
                }

                secondTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (secondTimer <= 0) {
                    secondTimer = 1f;

                    if (Value < 10 && Value > 0) {
                        Assets.TimeShort.Play();
                    }                    
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {          
            for (int i = 0; i <= 360; i++) {
                if ((360 / (float)StartValue) * (Value - 1) >= i) {
                    var destRect = new Rectangle(this.X + (Size.X / 2), this.Y + (Size.Y / 2), 1, 50);
                    spriteBatch.Draw(Assets.ClockFillSprite, destRect, null, Color.White, -MathHelper.ToRadians(i + 180), new Vector2(0, 0), SpriteEffects.None, 0);
                }                
            }            
            
            spriteBatch.Draw(Assets.ClockFrameSprite, BoundingBox, Color.White);

            foreach (var num in FontNumber.FromInteger((int)Value, X + 30, Y + 35, new Point(27, 40), "00", FontNumber.FontNumberColor.Red)) {
                num.Draw(spriteBatch);
            }
        }

        public override void Delete() {            
        }
    }
}
