using Microsoft.Xna.Framework;
using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Bang.Actions;
using FarseerPhysics.Dynamics;

namespace ClownSchool.Entity {

    public class NumberSlot : BaseEntity {

        public Balloon Balloon { get; set; }
        public EquationInput Owner { get; set; }
        public Player Player { get; set; }
               
        public bool Reassignable { get; set; }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public bool IsLocked { get; set; }

        public int Number {
            get {
                if (Balloon != null) {
                    return Balloon.Number;
                }

                return -1;
            }
        }

        public bool HasNumber {
            get {
                return Number != -1;
            }
        }

        private bool allowGrab = true;

        public Point NumberPosition {
            get {
                return new Point((int)this.X, (int)this.BoundingBox.Center.Y - 170);
            }
        }

        public NumberSlot(EquationInput owner, int offsetX, int offsetY, bool reassignable, bool isLocked) {
            Owner = owner;

            OffsetX = offsetX;
            OffsetY = offsetY;

            Size = new Point(44, 44);

            Reassignable = reassignable;
            IsLocked = isLocked;

            CollisionType = "slot";
        }

        public override void Init() {            
        }

        public bool TryAttach(Balloon balloon) {
            PlayerHand hand = (PlayerHand)balloon.AttachedEntity;

            if ((Reassignable || Balloon == null) && (Player == null || hand.Player == Player) && (IsLocked == false)) {
                if (Balloon != null) {
                    Balloon.Loose();
                }

                balloon.AttachTo(this);
                Balloon = balloon;

                Balloon.BalloonBody.CollidesWith = Category.None;

                hand.DraggingBalloon = null;

                Assets.BalloonDrop.Play();

                //MakeUncollidableUntilHandLeaves();

                return true;
            }

            return false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            X = Owner.X + OffsetX;
            Y = Owner.Y + OffsetY;

            if (!allowGrab)
                return;

            //PlayerHand hand = (PlayerHand)GetFirstCollidingEntity("hand");           
            //if (hand != null && hand.Player == this.Player && hand.IsGrabbing && hand.DraggingBalloon == null && this.Balloon != null) {
            //    //MakeUncollidableUntilHandLeaves();

            //    this.Balloon.AttachTo(hand);
            //    hand.DraggingBalloon = this.Balloon;

            //    this.Balloon = null;
            //}
        }

        private void MakeUncollidableUntilHandLeaves() {
            allowGrab = false;
            Actions.AddAction(new WaitForCondition(delegate() { return GetFirstCollidingEntity("hand") == null; }), true);
            Actions.AddAction(new CallFunction(delegate() { Debug.WriteLine("allowGrab"); allowGrab = true; }), true);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            if (Balloon != null) {
                spriteBatch.Draw(Assets.RopeKnot, new Rectangle(BoundingBox.Center.X, BoundingBox.Center.Y, 11, 12), null, Color.White, 0, new Vector2(5.5f, 6f), SpriteEffects.None, 0);
            }

            if (((this.Player != null && this.Player.IsDragging) || (this.Player == null && Screen.SomePlayerIsDragging)) && (Balloon == null || Reassignable)) {
                spriteBatch.Draw(Assets.IndicatorYellow, new Rectangle(BoundingBox.Center.X, BoundingBox.Center.Y, 50, 50), null, new Color(150, 150, 150, 100), 0, new Vector2(25, 25), SpriteEffects.None, 0);
            }
        }

        public void PopBalloon() {
            if (Balloon != null) {
                Balloon.Pop();
            }
        }

        public override void Delete() {
            if (Balloon != null) {
                Screen.RemoveEntity(Balloon);            
            }
        }
    }
}