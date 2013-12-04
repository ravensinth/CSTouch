﻿using System;
using ClownSchool.Screens;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ClownSchool.Tweening;
using FarseerPhysics.Dynamics;
using System.Diagnostics;

namespace ClownSchool.Entity {

    public class EquationInput : BaseEntity {

        public List<NumberSlot> Slots;

        public NumberSlot FirstEquationSlot;
        public NumberSlot SecondEquationSlot;
        public NumberSlot ThirdEquationSlot;
        public NumberSlot FourthEquationSlot;

        public NumberSlot FirstProductSlot;
        public NumberSlot SecondProductSlot;

        public Settings.type CurrentOperator;

        public bool IsEquationSet {
            get {
                return (FirstEquationSlot.Balloon != null && SecondEquationSlot.Balloon != null);
            }
        }

        public bool IsAnswerSet {
            get {                
                switch (CurrentOperator) {
                    case Settings.type.Multiplication:
                        if (Product.ToString().Length > 1) {
                            return (FirstProductSlot.Balloon != null && SecondProductSlot.Balloon != null);
                        }
                        break;
                
                    case Settings.type.Addition:
                        if (Sum.ToString().Length > 1) {
                            return (FirstProductSlot.Balloon != null && SecondProductSlot.Balloon != null);
                        }
                        break;
                                
                    case Settings.type.Subtraction:
                        if (Difference.ToString().Length > 1) {
                            return (FirstProductSlot.Balloon != null && SecondProductSlot.Balloon != null);
                        }
                        break;                                                         
                }
                return (FirstProductSlot.Balloon != null || SecondProductSlot.Balloon != null);             
            }
        }

        public bool IsAnswerCorrect {
            get {
                if (!IsEquationSet || !IsAnswerSet) return false;

                switch (CurrentOperator) {
                    case Settings.type.Multiplication:
                        return Product == Answer;

                    case Settings.type.Addition:
                        return Sum == Answer;
                }
                switch (CurrentOperator) {
                    case Settings.type.Subtraction:
                        return Difference == Answer;
                }
                return false;
            }
        }

        public int Product {
            get {
                return FirstEquationSlot.Number * SecondEquationSlot.Number;
            }
        }

        public int Sum {
            get {
                string num1 = "";
                num1 += FirstEquationSlot.Number.ToString();
                num1 += ThirdEquationSlot.Number.ToString();

                string num2 = "";
                num2 += SecondEquationSlot.Number.ToString();
                num2 += FourthEquationSlot.Number.ToString();

                return Convert.ToInt32(num1) + Convert.ToInt32(num2);
            }
        }

        public int Difference {
            get {
                string num1 = "";
                num1 += FirstEquationSlot.Number.ToString();
                num1 += ThirdEquationSlot.Number.ToString();

                string num2 = "";
                num2 += SecondEquationSlot.Number.ToString();
                num2 += FourthEquationSlot.Number.ToString();

                return Convert.ToInt32(num1) - Convert.ToInt32(num2);
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
            FirstEquationSlot = new NumberSlot(this, 20, 80, false);
            SecondEquationSlot = new NumberSlot(this, 220, 80, false);
            ThirdEquationSlot = new NumberSlot(this, 80, 80, false);
            FourthEquationSlot = new NumberSlot(this, 280, 80, false);

            FirstProductSlot = new NumberSlot(this, 50, 234, true);
            SecondProductSlot = new NumberSlot(this, 250, 234, true);

            Slots.Add(FirstEquationSlot);
            Slots.Add(SecondEquationSlot);
            Slots.Add(ThirdEquationSlot);
            Slots.Add(FourthEquationSlot);

            Slots.Add(FirstProductSlot);
            Slots.Add(SecondProductSlot);

            foreach (NumberSlot slot in Slots) {
                Screen.AddEntity(slot);
            }
            SetCurrentOperator();
        }

        private void SetCurrentOperator() {
            var rand = new Random();            

            switch (rand.Next(0, 3)) {
                case 0:
                    CurrentOperator = Settings.type.Addition;
                    break;
                case 1:
                    CurrentOperator = Settings.type.Multiplication;
                    break;
                case 2:
                    CurrentOperator = Settings.type.Subtraction;
                    break;
            }
            if (Settings.GetValueByType(CurrentOperator) == false) {
                SetCurrentOperator();
            }
        }

        public void PopBalloons() {
            FirstEquationSlot.PopBalloon();
            SecondEquationSlot.PopBalloon();
            ThirdEquationSlot.PopBalloon();
            FourthEquationSlot.PopBalloon();
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
            Rectangle operatorRec = new Rectangle(600, 200, 100, 100);
            Texture2D operatorImage; 
            switch (CurrentOperator) { 
                case Settings.type.Addition:
                    operatorImage = Assets.OperatorPlus;
                    break;
                case Settings.type.Subtraction:
                    operatorImage = Assets.OperatorMinus;
                    break;
                case Settings.type.Multiplication:
                    operatorImage = Assets.OperatorTimes;
                    break;
                default:
                    operatorImage = Assets.NumberSlotSprite;
                    break;
            }
            spriteBatch.Draw(operatorImage, operatorRec, Color.White);
        }

        public override void Delete() {
            foreach (NumberSlot slot in Slots) {
                Screen.RemoveEntity(slot);
            }
        }
    }
}