using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool.Entity {
    public class FontNumber : BaseEntity {

        public int Value { get; set; }
        private FontNumberColor FontColor { get; set; }
        private Texture2D FontTexture { get; set; }


        public FontNumber(int value, int posX, int posY, Point size, FontNumberColor color) {
            X = posX;
            Y = posY;

            Value = value;

            Size = size;

            switch (color) {
                case FontNumberColor.Red:
                    FontTexture = Assets.FontNumberRed;
                    break;
                case FontNumberColor.Yellow:
                    FontTexture = Assets.FontNumberYellow;
                    break;
                default:
                    FontTexture = Assets.FontNumberRed;
                    break;
            }
        }

        public override void Init() {
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
            spriteBatch.Draw(FontTexture, BoundingBox, new Rectangle(Value * 87, 0, 87, 100), Color.White);
        }

        public static List<FontNumber> FromInteger(int number, int posX, int posY, Point size, string format, FontNumberColor color) {
            var numbers = new List<FontNumber>();

            if (number < 0)
                number = 0;

            var numString = number.ToString(format);

            for (int i = 0; i < numString.Length; i++) {
                numbers.Add(new FontNumber(int.Parse(numString[i].ToString()), posX + (i * size.X), posY, size, color));
            }

            return numbers;
        }

        public override void Delete() {
        }

        public enum FontNumberColor {
            Red,
            Yellow
        }
    }
}
