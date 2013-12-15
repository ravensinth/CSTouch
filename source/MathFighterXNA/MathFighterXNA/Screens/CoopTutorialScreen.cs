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

namespace ClownSchool.Screens {

    public class CoopTutorialScreen : GameScreen {

        public Player PlayerOne { get; set; }
        private TutorialHand hand { get; set; }    

        public Clock MainClock { get; set; }
        public ScoreSign Score { get; set; }        
        
        public EquationInput Input { get; set; }

        private SimpleGraphic message { get; set; }

        private Dictionary<DragableNumber, Vector2> Numbers { get; set; }

        public CoopTutorialScreen(KinectContext context)
            : base(context) {
        }

        public override void Init() {
            base.Init();
            Settings.USE_ADDITION = false;
            Settings.USE_SUBTRACTION = false;
            Settings.USE_MULTIPLICATION = true;

            Manager.FadeInSong(Assets.GameSong, true, 0.2f);

            AddCurtain();
            OpenCurtain();

            PlayerOne = new Player(Context, SkeletonPlayerAssignment.FirstSkeleton);
            
            hand = new TutorialHand(PlayerOne, Microsoft.Kinect.JointType.HandRight);

            PlayerOne.RightHand = hand;

            AddEntity(hand);

            hand.Position = new Vector2(500, 300);

            MainClock = new Clock(20, 20, 90);
            MainClock.Paused = true;
            
            AddEntity(MainClock);

            Score = new ScoreSign(MainGame.Width - 190, 25);
            Score.Value = 0;
            AddEntity(Score);

            Coroutines.Start(LoadNumbersFromFile());

            AddInput();

            if (!Configuration.GRABBING_ENABLED) {
                AddEntity(new Scissors(120, MainGame.Height - 350, Scissors.ScissorPosition.Left));
                AddEntity(new Scissors(MainGame.Width - 120, MainGame.Height - 350, Scissors.ScissorPosition.Right));
            }            
        }

        DragableNumber getNumber(int number) {
            foreach (var num in Numbers.Keys) {
                if (num.Number == number) {
                    return num;
                }
            }

            return null;
        }

        SimpleGraphic getMessage(Texture2D texture) {
            var msg = new SimpleGraphic(texture, (MainGame.Width / 2) - (texture.Width / 6), MainGame.Height, texture.Width / 3, texture.Height / 3);

            return msg;
        }

        void grabBalloonTo(TutorialHand hand, DragableNumber num, NumberSlot to) {
            moveHandAction(num);
            Actions.AddAction(new CallFunction(delegate() { hand.Grab(); }), true);
            moveHandAction(to);
        }

        void tutorialAction() {           
            {
                showMsgAction(Assets.TutorialStep1);

                var three = getNumber(3);
                moveHandAction(three);
                showMsgAction(Assets.TutorialStep2);
                Actions.AddAction(new CallFunction(delegate() { hand.Grab(); }), true);
                showMsgAction(Assets.TutorialStep3);
                moveHandAction(Input.FirstProductSlot.BoundingBox.Center);
                Actions.AddAction(new CallFunction(delegate() { hand.IsGrabbing = false; }), true);

                var zero = getNumber(0);
                moveHandAction(zero);
                Actions.AddAction(new CallFunction(delegate() { hand.Grab(); }), true);
                moveHandAction(Input.SecondProductSlot.BoundingBox.Center);
                Actions.AddAction(new CallFunction(delegate() { hand.IsGrabbing = false; }), true);
            }


            Actions.AddAction(new WaitForCondition(delegate() { return Input.FirstProductSlot.Balloon == null; }), true);

            {
                showMsgAction(Assets.TutorialStep4);
                var four = getNumber(4);
                moveHandAction(four);
                Actions.AddAction(new CallFunction(delegate() { hand.Grab(); }), true);
                moveHandAction(Input.FirstProductSlot.BoundingBox.Center);
                Actions.AddAction(new CallFunction(delegate() { hand.IsGrabbing = false; }), true);

                showMsgAction(Assets.TutorialStep5);
                var three = getNumber(3);
                moveHandAction(three);
                Actions.AddAction(new CallFunction(delegate() { hand.Grab(); }), true);
                moveHandAction(Input.FirstProductSlot.BoundingBox.Center);
                Actions.AddAction(new CallFunction(delegate() { hand.IsGrabbing = false; }), true);

                var zero = getNumber(0);
                moveHandAction(zero);
                Actions.AddAction(new CallFunction(delegate() { hand.Grab(); }), true);
                moveHandAction(Input.SecondProductSlot.BoundingBox.Center);
                Actions.AddAction(new CallFunction(delegate() { hand.IsGrabbing = false; }), true);
            }

                        
            showMsgAction(Assets.TutorialStep7);
            showMsgAction(Assets.TutorialStep8);
            showMsgAction(Assets.TutorialStep6);

            Actions.AddAction(new WaitForSeconds(2f), true);
            Actions.AddAction(new CallFunction(delegate() { Manager.SwitchScreen(new MenuScreen(Context)); }), true);
        }

        void showMsgAction(Texture2D message) {
            var msg = getMessage(message);

            var messagePresent = new Vector2(msg.X, msg.Y - Assets.MenuSignHighscore.Height / 2);

            AddEntity(msg);

            Actions.AddAction(new TweenPositionTo(msg, messagePresent, 1f, Linear.EaseIn), true);
            Actions.AddAction(new WaitForSeconds(4f), true);
            Actions.AddAction(new TweenPositionTo(msg, new Vector2(msg.X, msg.Y), 1f, Linear.EaseIn), true);
        }

        void moveHandAction(BaseEntity to) {
            Actions.AddAction(new TweenPositionToEntity(hand, to, new Point(15, 30), 1f, Linear.EaseIn), true);
        }

        void moveHandAction(Point to) {
            Actions.AddAction(new TweenPositionTo(hand, new Vector2(to.X + 15, to.Y + 30) , 1f, Linear.EaseIn), true);
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

                        var num = new DragableNumber(PlayerOne, posX, posY, value);
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

                            var num2 = new DragableNumber(PlayerOne, posX2, posY2, value2);
                            Numbers.Add(num2, new Vector2(posX2, posY2));

                            num2.ZDepth = -1;

                            yield return Pause(0.1f);
                            Assets.BalloonPlace.Play(0.5f, 0, 0);
                            AddEntity(num2);                            
                        }
                    }
                }
            }

            tutorialAction();
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

                num.Owner = PlayerOne;
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
                } else {
                    MainClock.AddTime();
                    Score.AddPoints();
                }
                RemoveEntity(Input);
                AddInput();
            }), true);

            AddEntity(Input);

            AddBalloons(Input);

            Input.FirstEquationSlot.Player = Input.SecondEquationSlot.Player = null;
            Input.FirstProductSlot.Player = PlayerOne;
            Input.SecondProductSlot.Player = PlayerOne;
        }

        private void AddBalloons(EquationInput input) {
            var rand = new Random();
            var ball1 = new Balloon((int)input.X, (int)input.Y, 5);
            var ball2 = new Balloon((int)input.X, (int)input.Y, 6);

            AddEntity(ball1);
            AddEntity(ball2);

            ball1.AttachTo(Input.FirstEquationSlot);
            Input.FirstEquationSlot.Balloon = ball1;

            ball2.AttachTo(Input.SecondEquationSlot);
            Input.SecondEquationSlot.Balloon = ball2;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);


        }

        public void EndGame() {

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

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }
    }
}