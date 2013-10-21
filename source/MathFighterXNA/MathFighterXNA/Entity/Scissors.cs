using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Tweening;

namespace ClownSchool.Entity {
    public class Scissors : BaseEntity {

        private ScissorPosition position;

        private bool selected = false;

        private float rotation = 30f;
        private static float rotationAmount = 2.5f;

        private static Color SelectedColor = Color.White;
        private static Color NotSelectedColor = new Color(150, 150, 150, 150);

        public Scissors(int posX, int posY, ScissorPosition position) {
            X = posX;
            Y = posY;

            this.position = position;
            Size = new Point(200, 100);
            Offset = new Point(-100, -50);

            collidable = true;
            CollisionType = "scissors";
        }

        public override void Init() {
            
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            var hand = (PlayerHand)GetFirstCollidingEntity("hand");
            if (hand != null) {
                selected = true;

                Close();
            } else {
                selected = false;

                Open();
            }
        }

        private bool snipped = false;
        private void Open() {
            if (rotation < 30f) {
                snipped = false;
                rotation += rotationAmount;
            } else {
                rotation = 30f;
            }
        }
        
        private void Close() {
            if (rotation > 5f) {
                rotation -= rotationAmount;
            } else {
                rotation = 5f;
                if (!snipped) {
                    Cut();
                    snipped = true;
                }                
            }
        }

        public void Cut() {
            Assets.ScissorsSnip.Play();

            var hand = (PlayerHand)GetFirstCollidingEntity("hand");
            if (hand != null && hand.DraggingBalloon != null) {
                hand.DraggingBalloon.Cut();
                hand.DraggingBalloon = null;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            var color = selected ? SelectedColor : NotSelectedColor;
            
            var bottom = Assets.ScissorBottomRight;
            var top = Assets.ScissorTopRight;
            var bottomOrigin = new Vector2(105, 10);
            var topOrigin = new Vector2(108, 34);

            var rotationBottomMultiplier = 1;
            var rotationTopMultiplier = -1;

            if (position == ScissorPosition.Left) {
                bottom = Assets.ScissorBottomLeft;
                top = Assets.ScissorTopLeft;
                bottomOrigin = new Vector2(95, 10);
                topOrigin = new Vector2(91, 34);

                rotationBottomMultiplier *= -1;
                rotationTopMultiplier *= -1;
            }

            spriteBatch.Draw(bottom, new Rectangle(X, Y, 200, 50), null, color, MathHelper.ToRadians(rotation * rotationBottomMultiplier), bottomOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(top, new Rectangle(X, Y, 200, 50), null, color, MathHelper.ToRadians(rotation * rotationTopMultiplier), topOrigin, SpriteEffects.None, 0);

            if(Screen.SomePlayerIsDragging)
                spriteBatch.Draw(Assets.IndicatorYellow, new Rectangle(BoundingBox.Center.X - 125, BoundingBox.Center.Y - 125, 250, 250), new Color(255, 0, 0, 100));
        }

        public override void Delete() {
            
        }

        public enum ScissorPosition {
            Left,
            Right
        }
    }
}
