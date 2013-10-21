using System;
using ClownSchool.Screens;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ClownSchool.Tweening;
using FarseerPhysics.Dynamics;

namespace ClownSchool.Entity {

    public class EquationInput : BaseEntity {

        public List<NumberSlot> Slots;

        public NumberSlot FirstEquationSlot;
        public NumberSlot SecondEquationSlot;

        public NumberSlot FirstProductSlot;
        public NumberSlot SecondProductSlot;

        public bool IsEquationSet {
            get {
                return (FirstEquationSlot.Balloon != null && SecondEquationSlot.Balloon != null);
            }
        }

        public bool IsAnswerSet {
            get {
                if (Product.ToString().Length > 1) {
                    return (FirstProductSlot.Balloon != null && SecondProductSlot.Balloon != null);
                } else {
                    return (FirstProductSlot.Balloon != null || SecondProductSlot.Balloon != null);
                }                
            }
        }

        public bool IsAnswerCorrect {
            get {
                if (!IsEquationSet || !IsAnswerSet) return false;

                return Product == Answer;
            }
        }

        public int Product {
            get {
                return FirstEquationSlot.Number * SecondEquationSlot.Number;
            }
        }

        public int Answer {
            get {
                string num = "";
                if (FirstProductSlot.HasNumber)
                    num += FirstProductSlot.Number.ToString();

                if (SecondProductSlot.HasNumber)
                    num += SecondProductSlot.Number.ToString();

                return Convert.ToInt32(num);
            }
        }

        public EquationInput(int posX, int posY) {
            X = posX;
            Y = posY;

            Size = new Point(337, 300);
            collidable = false;

            Slots = new List<NumberSlot>();
        }

        public override void Init() {
            FirstEquationSlot = new NumberSlot(this, 50, 80, false);
            SecondEquationSlot = new NumberSlot(this, 250, 80, false);

            FirstProductSlot = new NumberSlot(this, 50, 234, true);
            SecondProductSlot = new NumberSlot(this, 250, 234, true);

            Slots.Add(FirstEquationSlot);
            Slots.Add(SecondEquationSlot);

            Slots.Add(FirstProductSlot);
            Slots.Add(SecondProductSlot);

            foreach (NumberSlot slot in Slots) {
                Screen.AddEntity(slot);
            }
        }

        public void PopBalloons() {
            FirstEquationSlot.PopBalloon();
            SecondEquationSlot.PopBalloon();
            FirstProductSlot.PopBalloon();
            SecondProductSlot.PopBalloon();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);

            foreach (NumberSlot slot in Slots) {
                slot.Update(gameTime);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(Assets.EquationInputSprite, BoundingBox, Color.White);

            foreach (NumberSlot slot in Slots) {
                slot.Draw(spriteBatch);
            }
        }

        public override void Delete() {
            foreach (NumberSlot slot in Slots) {
                Screen.RemoveEntity(slot);
            }
        }
    }
}