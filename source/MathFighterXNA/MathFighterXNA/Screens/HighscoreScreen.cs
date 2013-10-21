using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownSchool.Highscore;
using Microsoft.Xna.Framework;
using ClownSchool.Entity;
using ClownSchool.Entity.Menu;

namespace ClownSchool.Screens {
    public class HighscoreScreen : GameScreen {

        public ScoreList Scores { get; private set; }
        public Player Player { get; set; }


        public HighscoreScreen(KinectContext context, string scorePath)
            : base(context) {

                Scores = ScoreList.LoadFromDirectory(scorePath);                
        }

        public override void Init() {
            base.Init();

            AddCurtain();

            Player = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            Player.DrawHands = true;

            AddEntity(Player);

            var posX = 400;
            var posY = 235;

            var width = Assets.HighscoreBoard.Width / 2;
            var height = Assets.HighscoreBoard.Height / 2;

            var board = new SimpleGraphic(Assets.HighscoreBoard, (MainGame.Width / 2) - width / 2, (MainGame.Height / 2) - (height / 2) - 50, width, height);
            AddEntity(board);

            foreach (Score s in Scores.Take(3)) {
                var hi = new HighscoreItem(posX, posY, s);
                AddEntity(hi);

                posY += 145;
            }

            var menuSign = new MenuItem(Assets.SignMenu, (MainGame.Width / 2) - 100, (MainGame.Height) - 125, delegate() { Manager.SwitchScreen(new MenuScreen(Context)); });
            AddEntity(menuSign);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {                      
            base.Draw(spriteBatch);
        }
    }
}
