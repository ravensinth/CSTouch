using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ClownSchool.Highscore {
    public class ScoreList : List<Score> {
        public static ScoreList LoadFromDirectory(string path) {
            var list = new ScoreList();

            foreach (var file in Directory.GetFiles(path)) {
                list.Add(Score.LoadFromFile(file));
            }

            list.Sort();

            return list;
        }
    }
}
