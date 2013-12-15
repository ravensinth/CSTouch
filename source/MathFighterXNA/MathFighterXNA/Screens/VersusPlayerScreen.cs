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

namespace ClownSchool.Screens {

    public class VersusPlayerScreen : GameScreen {

        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public Player CurrentPlayer { get; set; }

        public Clock PlayerOneClock { get; set; }
        public Clock PlayerTwoClock { get; set; }        

        public EquationInput Input { get; set; }

        private Dictionary<DragableNumber, Vector2> Numbers { get; set; }

        private bool ended = false;

        private bool paused = false;

        public VersusPlayerScreen(KinectContext context) : base(context) {
        }

        public override void Init() {
            Manager.FadeInSong(Assets.GameSong, true, 0.2f);
            Settings.USE_MULTIPLICATION = true;
            Settings.USE_ADDITION = false;
            Settings.USE_SUBTRACTION = false;

            AddCurtain();
            OpenCurtain();

            PlayerOne = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            PlayerTwo = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);

            CurrentPlayer = PlayerOne;

            AddEntity(PlayerOne);
            AddEntity(PlayerTwo);

            PlayerOneClock = new Clock(20, 20, 90);
            PlayerTwoClock = new Clock(MainGame.Width - 130, 20, 90);

            PlayerTwoClock.Paused = true;

            AddEntity(PlayerOneClock);
            AddEntity(PlayerTwoClock);

            Coroutines.Start(LoadNumbersFromFile());

            AddInput();

            var posPause = new Vector2(MainGame.Width - 230, (MainGame.Height) - 300);

            var pause = new MenuButton(Assets.ButtonPause, (int)posPause.X, (int)posPause.Y, delegate() { paused = true; });
            AddEntity(pause);

            if (!Configuration.GRABBING_ENABLED) {
                AddEntity(new Scissors(120, MainGame.Height - 350, Scissors.ScissorPosition.Left));
                AddEntity(new Scissors(MainGame.Width - 120, MainGame.Height - 350, Scissors.ScissorPosition.Right));
            }

            base.Init();
        }

        private IEnumerator LoadNumbersFromFile() {
            Numbers = new Dictionary<DragableNumber, Vector2>();

            var rand = new Random();

            var values = new List<int>();
            for (int i = 0; i < 11; i++) {
                values.Add(i);
            }

            using (StreamReader reader = new StreamReader(@"BalloonArrangements\VersusPlayerScreen.csv")) {

                while (!reader.EndOfStream) {
                    string[] data = reader.ReadLine().Split(';');
                    if (data.Length == 2) {
                        int posX = int.Parse(data[0]);
                        int posY = int.Parse(data[1]);

                        var value = values[rand.Next(0, values.Count)];
                        values.Remove(value);

                        var num = new DragableNumber(CurrentPlayer, posX, posY, value);
                        Numbers.Add(num, new Vector2(posX, posY));

                        num.ZDepth = -1;

                        yield return Pause(0.1f);
                        Assets.BalloonPlace.Play(0.5f, 0, 0);
                        AddEntity(num);
                    }
                }
            }   
        }

        private static IEnumerator Pause(float time) {
            var watch = Stopwatch.StartNew();
            while (watch.Elapsed.TotalSeconds < time)
                yield return 0;
        }

        public void PauseClocks() {
            PlayerOneClock.Paused = true;
            PlayerTwoClock.Paused = true;
        }

        public void ResumeCurrentClock() {
            if (CurrentPlayer == PlayerOne) {
                PlayerOneClock.Paused = false;
            } else {
                PlayerTwoClock.Paused = false;
            }            
        }

        public void SwitchCurrentPlayer() {
            if (CurrentPlayer == PlayerOne) {
                CurrentPlayer = PlayerTwo;
            } else {
                CurrentPlayer = PlayerOne;
            }

            shuffleNumberPositions();

            foreach (var num in Numbers.Keys) {
                var posX = Numbers[num].X;
                var posY = Numbers[num].Y;

                if (CurrentPlayer == PlayerTwo) {
                    posX = MainGame.Width - Numbers[num].X - 62;
                }

                var tweenTo = new Vector2(posX, posY);

                var that = num;

                num.State = num.IdleState;
                num.Actions.AddAction(new TweenPositionTo(num, tweenTo, 1.5f, Back.EaseInOut), true);
                num.Actions.AddAction(new CallFunction(delegate() { that.State = new ClownSchool.Entity.NumberState.DefaultState(that); }), true);

                num.Owner = CurrentPlayer;
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

            var left = new Vector2(Input.X - 50, 250);
            var right = new Vector2(Input.X + 80, 250);

            Input.Actions.AddAction(new CallFunction(delegate() { PauseClocks(); }), true);
            Input.Actions.AddAction(new TweenPositionTo(Input, CurrentPlayer == PlayerOne ? left : right, 2f, Tweening.Back.EaseOut), true);
            Input.Actions.AddAction(new CallFunction(delegate() { ResumeCurrentClock(); }), true);
            Input.Actions.AddAction(new WaitForEquationInput(Input, EquationInputType.Equation), true);
            Input.Actions.AddAction(new CallFunction(delegate() { PauseClocks(); }), true);
            Input.Actions.AddAction(new CallFunction(delegate() { SwitchCurrentPlayer(); }), true);

            Input.Actions.AddAction(new TweenPositionTo(Input, CurrentPlayer == PlayerOne ? right : left, 2f, Tweening.Back.EaseOut), true);
            Input.Actions.AddAction(new CallFunction(delegate() { ResumeCurrentClock(); }), true);
            Input.Actions.AddAction(new WaitForEquationInput(Input, EquationInputType.Product), true);
            Input.Actions.AddAction(new CallFunction(delegate() { PauseClocks(); }), true);

            Input.Actions.AddAction(new EndEquationInput(Input), true);

            Input.Actions.AddAction(new CallFunction(delegate() {
                if (!Input.IsAnswerCorrect) {
                    if (CurrentPlayer == PlayerOne) {
                        PlayerOneClock.SubtractTime();
                    } else {
                        PlayerTwoClock.SubtractTime();
                    }
                }
                RemoveEntity(Input);
                AddInput();
            }), true);

            AddEntity(Input);

            Input.FirstEquationSlot.Player = Input.SecondEquationSlot.Player = CurrentPlayer == PlayerOne ? PlayerOne : PlayerTwo;
            Input.FirstProductSlot.Player = Input.SecondProductSlot.Player = CurrentPlayer == PlayerOne ? PlayerTwo : PlayerOne;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (!PlayerOne.IsReady || !PlayerTwo.IsReady) {
                AddPauseScreen();
            }

            if (!ended) {
                if (PlayerOneClock.Value == 0) {
                    EndGame(PlayerTwo);
                } else if (PlayerTwoClock.Value == 0) {
                    EndGame(PlayerOne);
                }
            }

            if (paused) {
                var ps = new PauseScreen(Context);
                ps.Pause();

                Manager.AddScreen(ps);

                paused = false;
            }

        }

        public void EndGame(Player winner) {
            //RemoveEntity(Input);

            ended = true;

            Manager.FadeInSong(Assets.WinSong, false, 0.8f);

            Actions.AddAction(new EndEquationInput(Input), true);

            foreach (DragableNumber num in Entities.Where(ent => ent.CollisionType == "number")) {
                Actions.AddAction(new TweenPositionTo(num, new Vector2(1300, -200), 1f, Linear.EaseIn), false);
            }

            for (int i = 0; i < 4; i++) {               
                var hand = i < 2 ? winner.LeftHand : winner.RightHand;

                var balloon = new Balloon(100 * i, 0, 11);
                AddEntity(balloon);

                balloon.AttachTo(hand);
            }

            var posMenu = new Vector2(300, (MainGame.Height / 2) - 250);
            var menu = new MenuItem(Assets.SignMenu, -100, -300, delegate() { Manager.SwitchScreen(new MenuScreen(Context)); });
            menu.Actions.AddAction(new TweenPositionTo(menu, posMenu, 2f, Back.EaseOut), true);
            AddEntity(menu);

            var posRestart = new Vector2(MainGame.Width - 600, (MainGame.Height / 2) - 250);

            var restart = new MenuItem(Assets.MenuSignRestart, MainGame.Width + 100, -300, delegate() { Manager.SwitchScreen(new VersusPlayerScreen(Context)); });
            restart.Actions.AddAction(new TweenPositionTo(restart, posRestart, 2f, Back.EaseOut), true);
            AddEntity(restart);
        }

        public void AddPauseScreen() {
            var pauseScreen = new PauseScreen(Context);
            var sil1 = new SimpleGraphic(Assets.PlayerSilhouette, 250, 75, 480, 588);
            var sil2 = new SimpleGraphic(Assets.PlayerSilhouette, 650, 75, 480, 588);
            pauseScreen.AddEntity(sil1);
            pauseScreen.AddEntity(sil2);
            pauseScreen.WaitForPlayerCount(2);
            
            Manager.AddScreen(pauseScreen);

        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }
    }
}