using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Entity;
using ClownSchool.Bang.Actions;
using ClownSchool.Tweening;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;
using ClownSchool.Entity.Menu;
using ClownSchool.Bang;
using FarseerPhysics.Dynamics;

namespace ClownSchool.Screens {

    public class CoopPlayerScreen : GameScreen {

        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public int NeededPlayerCount = 2;

        public Clock MainClock { get; set; }
        public ScoreSign Score { get; set; }

        public EquationInput Input { get; set; }

        private Dictionary<DragableNumber, Vector2> Numbers { get; set; }

        private bool Ended = false;

        private bool paused = false;

        public CoopPlayerScreen(KinectContext context)
            : base(context) {
        }

        public override void Init() {
            base.Init();

            Manager.FadeInSong(Assets.GameSong, true, 0.2f);

            AddCurtain();
            OpenCurtain();

            AddPlayers();

            MainClock = new Clock(20, 20, 90);
            MainClock.Paused = true;

            AddEntity(MainClock);

            Score = new ScoreSign(MainGame.Width - 190, 25);
            Score.Value = 0;
            AddEntity(Score);

            Coroutines.Start(LoadNumbersFromFile());

            AddInput();

            var posPause = new Vector2(MainGame.Width - 230, (MainGame.Height) - 300);

            var pause = new MenuButton(Assets.ButtonPause, (int)posPause.X, (int)posPause.Y, delegate() { paused = true; });
            AddEntity(pause);

            if (!Configuration.GRABBING_ENABLED) {
                AddEntity(new Scissors(120, MainGame.Height - 350, Scissors.ScissorPosition.Left));
                AddEntity(new Scissors(MainGame.Width - 120, MainGame.Height - 350, Scissors.ScissorPosition.Right));
            }
        }

        public virtual void AddPlayers() {
            PlayerOne = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            PlayerTwo = new Player(Context, SkeletonPlayerAssignment.RightSkeleton);

            AddEntity(PlayerOne);
            AddEntity(PlayerTwo);
        }

        private IEnumerator LoadNumbersFromFile() {
            Numbers = new Dictionary<DragableNumber, Vector2>();

            var rand = new Random();

            var values = new List<int>();
            for (int i = 0; i < 11; i++) {
                values.Add(i);
            }

            using (StreamReader reader = new StreamReader(@"BalloonArrangements\CoopPlayerScreen.csv")) {

                while (!reader.EndOfStream) {
                    string[] data = reader.ReadLine().Split(';');
                    if (data.Length == 2) {
                        int posX = int.Parse(data[0]);
                        int posY = int.Parse(data[1]);

                        var value = values[rand.Next(0, values.Count)];
                        values.Remove(value);

                        var num = new DragableNumber(null, posX, posY, value);
                        Numbers.Add(num, new Vector2(posX, posY));

                        num.ZDepth = -1;

                        yield return Pause(0.1f);
                        Assets.BalloonPlace.Play(0.5f, 0, 0);
                        AddEntity(num);

                        if (Numbers.Count > 1) {
                            var value2 = values[rand.Next(0, values.Count)];
                            values.Remove(value2);

                            int posX2 = MainGame.Width - posX - 62;
                            int posY2 = posY;

                            var num2 = new DragableNumber(null, posX2, posY2, value2);
                            Numbers.Add(num2, new Vector2(posX2, posY2));

                            num2.ZDepth = -1;

                            yield return Pause(0.1f);
                            Assets.BalloonPlace.Play(0.5f, 0, 0);
                            AddEntity(num2);
                        }
                    }
                }
            }
        }

        private static IEnumerator Pause(float time) {
            var watch = Stopwatch.StartNew();
            while (watch.Elapsed.TotalSeconds < time)
                yield return 0;
        }

        public void PauseClock() {
            MainClock.Paused = true;
        }

        public void ResumeCurrentClock() {
            MainClock.Paused = false;
        }

        public void ShuffleBalloons() {
            shuffleNumberPositions();

            foreach (var num in Numbers.Keys) {
                var posX = Numbers[num].X;
                var posY = Numbers[num].Y;

                var tweenTo = new Vector2(posX, posY);

                var that = num;

                num.State = num.IdleState;
                num.Actions.AddAction(new TweenPositionTo(num, tweenTo, 1.5f, Back.EaseInOut), true);
                num.Actions.AddAction(new CallFunction(delegate() { that.State = new ClownSchool.Entity.NumberState.DefaultState(that); }), true);
            }
        }

        private void shuffleNumberPositions() {
            var posList = new List<Vector2>(Numbers.Values.ToArray());

            var rand = new Random();

            foreach (var key in Numbers.Keys.ToArray()) {
                var randVector = posList[rand.Next(0, posList.Count)];
                posList.Remove(randVector);

                Numbers[key] = randVector;
            }
        }

        private void AddInput() {
            Input = new EquationInput((MainGame.Width / 2) - 337 / 2, MainGame.Height);

            //Input.CurrentPlayer = CurrentPlayer; 

            var center = new Vector2((MainGame.Width / 2) - (337 / 2), 200);

            Input.Actions.AddAction(new CallFunction(delegate() { PauseClock(); }), true);

            Input.Actions.AddAction(new CallFunction(delegate() { ShuffleBalloons(); }), true);
            Input.Actions.AddAction(new TweenPositionTo(Input, center, 2f, Tweening.Back.EaseOut), true);

            Input.Actions.AddAction(new CallFunction(delegate() { ResumeCurrentClock(); }), true);
            Input.Actions.AddAction(new WaitForEquationInput(Input, EquationInputType.Product), true);
            Input.Actions.AddAction(new CallFunction(delegate() { PauseClock(); }), true);

            Input.Actions.AddAction(new EndEquationInput(Input), true);

            Input.Actions.AddAction(new CallFunction(delegate() {
                if (!Input.IsAnswerCorrect) {
                    MainClock.SubtractTime();
                }
                else {
                    MainClock.AddTime();
                    Score.AddPoints();
                }
                RemoveEntity(Input);
                AddInput();
            }), true);

            AddEntity(Input);

            AddRandomBalloons(Input);

            Input.FirstEquationSlot.Player = Input.SecondEquationSlot.Player = null;
            Input.FirstProductSlot.Player = null;
            Input.SecondProductSlot.Player = null;
        }

        private void AddRandomBalloons(EquationInput input) {


            switch (input.CurrentOperator) {
                case Settings.type.Multiplication:
                    AddBalloonsMultipliction(input);
                    break;
                case Settings.type.Addition:
                    AddBalloonsAddition(input);
                    break;
                case Settings.type.Subtraction:
                    AddBalloonsSubtraction(input);
                    break;
            }



            //if ((input.CurrentOperator == Settings.type.Subtraction) && (ball1.Number < ball2.Number)) {
            //    RemoveEntity(ball1);
            //    RemoveEntity(ball2);
            //    RemoveEntity(ball3);
            //    RemoveEntity(ball4);
            //    AddRandomBalloons(input);
            //}
        }

        private void AddBalloonsSubtraction(EquationInput input) {
            var rand = new Random(); int Number1 = rand.Next(11, 100);
            int Number2 = rand.Next(10, Number1 - 1);
            var ball1 = new Balloon((int)input.X, (int)input.Y, GetIntArray(Number1)[0]);
            var ball2 = new Balloon((int)input.X, (int)input.Y, GetIntArray(Number2)[0]);
            var ball3 = new Balloon((int)input.X, (int)input.Y, GetIntArray(Number1)[1]);
            var ball4 = new Balloon((int)input.X, (int)input.Y, GetIntArray(Number2)[1]);

            AddEntity(ball1);
            AddEntity(ball2);
            AddEntity(ball3);
            AddEntity(ball4);

            ball1.AttachTo(Input.FirstEquationSlot);
            Input.FirstEquationSlot.Balloon = ball1;
            ball1.BalloonBody.CollidesWith = Category.None;

            ball2.AttachTo(Input.SecondEquationSlot);
            Input.SecondEquationSlot.Balloon = ball2;
            ball2.BalloonBody.CollidesWith = Category.None;

            ball3.AttachTo(Input.ThirdEquationSlot);
            Input.ThirdEquationSlot.Balloon = ball3;
            ball3.BalloonBody.CollidesWith = Category.None;

            ball4.AttachTo(Input.FourthEquationSlot);
            Input.FourthEquationSlot.Balloon = ball4;
            ball4.BalloonBody.CollidesWith = Category.None;
        }

        private void AddBalloonsAddition(EquationInput input) {
            var rand = new Random();
            int Number1 = rand.Next(10, 90);
            int Number2 = rand.Next(10, 100 - Number1);
            var ball1 = new Balloon((int)input.X, (int)input.Y, GetIntArray(Number1)[0]);
            var ball2 = new Balloon((int)input.X, (int)input.Y, GetIntArray(Number2)[0]);
            var ball3 = new Balloon((int)input.X, (int)input.Y, GetIntArray(Number1)[1]);
            var ball4 = new Balloon((int)input.X, (int)input.Y, GetIntArray(Number2)[1]);

            AddEntity(ball1);
            AddEntity(ball2);
            AddEntity(ball3);
            AddEntity(ball4);

            ball1.AttachTo(Input.FirstEquationSlot);
            Input.FirstEquationSlot.Balloon = ball1;
            ball1.BalloonBody.CollidesWith = Category.None;

            ball2.AttachTo(Input.SecondEquationSlot);
            Input.SecondEquationSlot.Balloon = ball2;
            ball2.BalloonBody.CollidesWith = Category.None;

            ball3.AttachTo(Input.ThirdEquationSlot);
            Input.ThirdEquationSlot.Balloon = ball3;
            ball3.BalloonBody.CollidesWith = Category.None;

            ball4.AttachTo(Input.FourthEquationSlot);
            Input.FourthEquationSlot.Balloon = ball4;
            ball4.BalloonBody.CollidesWith = Category.None;
        }

        private void AddBalloonsMultipliction(EquationInput input) {
            var rand = new Random();
            var ball1 = new Balloon((int)input.X, (int)input.Y, rand.Next(0, 11));
            var ball2 = new Balloon((int)input.X, (int)input.Y, rand.Next(0, 11));

            AddEntity(ball1);
            AddEntity(ball2);

            ball1.AttachTo(Input.FirstEquationSlot);
            Input.FirstEquationSlot.Balloon = ball1;
            ball1.BalloonBody.CollidesWith = Category.None;

            ball2.AttachTo(Input.SecondEquationSlot);
            Input.SecondEquationSlot.Balloon = ball2;
            ball2.BalloonBody.CollidesWith = Category.None;
        }

        int[] GetIntArray(int num) {
            List<int> listOfInts = new List<int>();
            while (num > 0) {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts.ToArray();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (!PlayerOne.IsReady || !PlayerTwo.IsReady) {
                AddPauseScreen();
            }

            if (!Ended) {
                if (MainClock.Value <= 0f) {
                    EndGame();
                }
            }

            if (paused) {
                var ps = new PauseScreen(Context);
                ps.Pause();

                Manager.AddScreen(ps);

                paused = false;
            }
        }

        public void EndGame() {
            //RemoveEntity(Input);

            Ended = true;

            var saveTo = MainGame.CoopHighscoreDirectory;
            if (this.GetType() == typeof(SinglePlayerScreen))
                saveTo = MainGame.SingleHighscoreDirectory;

            var camera = new Camera(saveTo);
            AddEntity(camera);

            Manager.FadeInSong(Assets.WinSong, false, 0.8f);

            Actions.AddAction(new EndEquationInput(Input), true);

            var moveBalloonsActions = new ActionList();

            foreach (DragableNumber num in Entities.Where(ent => ent.CollisionType == "number")) {
                moveBalloonsActions.AddAction(new TweenPositionTo(num, new Vector2(1300, -200), 1f, Linear.EaseIn), false);
            }

            Actions.AddAction(moveBalloonsActions, true);

            //AttachWinnerBalloon(PlayerOne);
            //AttachWinnerBalloon(PlayerTwo);

            Actions.AddAction(new WaitForCondition(delegate() { return moveBalloonsActions.IsComplete(); }), true);
            Actions.AddAction(new CallFunction(delegate() { camera.TakePicture(Score.Value); }), true);
            Actions.AddAction(new WaitForCondition(delegate() { return camera.Actions.IsComplete(); }), true);

            Actions.AddAction(new CallFunction(delegate() { addButtons(); }), true);
        }

        private void addButtons() {
            var posMenu = new Vector2(300, (MainGame.Height / 2) - 250);
            var menu = new MenuItem(Assets.SignMenu, -100, -300, delegate() { Manager.SwitchScreen(new MenuScreen(Context)); Manager.FadeInSong(Assets.MenuSong, true, 0.5f); });
            menu.Actions.AddAction(new TweenPositionTo(menu, posMenu, 2f, Back.EaseOut), true);
            AddEntity(menu);

            var posRestart = new Vector2(MainGame.Width - 600, (MainGame.Height / 2) - 250);


            var restart = new MenuItem(Assets.MenuSignRestart, MainGame.Width + 100, -300, delegate() { Manager.SwitchScreen(new CoopPlayerScreen(Context)); Manager.FadeInSong(Assets.GameSong, true, 0.2f); });
            restart.Actions.AddAction(new TweenPositionTo(restart, posRestart, 2f, Back.EaseOut), true);
            AddEntity(restart);

            var posHighscore = new Vector2(MainGame.Width - 250, (MainGame.Height / 2) - 200);

            var loadFrom = MainGame.CoopHighscoreDirectory;
            if (this.GetType() == typeof(SinglePlayerScreen))
                loadFrom = MainGame.SingleHighscoreDirectory;

            var highscore = new MenuItem(Assets.SignHighscore, MainGame.Width + 100, -300, delegate() { Manager.SwitchScreen(new HighscoreScreen(Context, loadFrom)); Manager.FadeInSong(Assets.MenuSong, true, 0.2f); });
            highscore.Actions.AddAction(new TweenPositionTo(highscore, posHighscore, 2f, Back.EaseOut), true);
            AddEntity(highscore);
        }

        //private void AttachWinnerBalloon(Player player) {
        //    for (int i = 0; i < 2; i++) {
        //        var hand = i < 1 ? player.LeftHand : player.RightHand;

        //        var balloon = new Balloon(100, 0, 11);
        //        AddEntity(balloon);

        //        balloon.AttachTo(hand);
        //    }
        //}

        public void AddPauseScreen() {
            var pauseScreen = new PauseScreen(Context);
            var sil1 = new SimpleGraphic(Assets.PlayerSilhouette, 250, 75, 480, 588);
            var sil2 = new SimpleGraphic(Assets.PlayerSilhouette, 650, 75, 480, 588);
            pauseScreen.AddEntity(sil1);
            pauseScreen.AddEntity(sil2);
            pauseScreen.WaitForPlayerCount(NeededPlayerCount);

            Manager.AddScreen(pauseScreen);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }
    }
}