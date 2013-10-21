using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace ClownSchool.Highscore {
    public class Score : IComparable<Score> {
        public Texture2D Picture { get; set; }
        public int Value { get; set; }

        public static Score LoadFromFile(string filename) {
            var score = new Score();
            var file = Path.GetFileName(filename);
            score.Value = int.Parse(file.Split('_')[0]);
            using (var fs = new FileStream(filename, FileMode.Open)) {
                score.Picture = Texture2D.FromStream(MainGame.graphics.GraphicsDevice, fs);
            }

            return score;
        }

        public int CompareTo(Score other) {
            return this.Value.CompareTo(other.Value) * -1;
        }
    }
}
