using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ClownSchool.Bang.Actions;
using ClownSchool.Entity;
using ClownSchool.Entity.Menu;

namespace ClownSchool.Screens {
    public class PauseScreen : GameScreen {

        public PauseState State;

        public Player Player;

        public PauseScreen(KinectContext context): base(context) {
            State = PauseState.Default;
        }

        public void Pause() {
            State = PauseState.Default;

            Player = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            Player.DrawHands = true;

            AddEntity(Player);

            var posMenu = new Point(300, (MainGame.Height / 2) - 250);
            var posResume = new Point(MainGame.Width - 600, (MainGame.Height / 2) - 250);

            var mi = new MenuItem(Assets.SignMenu, posMenu.X, posMenu.Y, delegate() { Manager.SwitchScreen(new MenuScreen(Context)); });
            AddEntity(mi);

            mi = new MenuItem(Assets.SignResume, posResume.X, posMenu.Y, delegate() { Manager.RemoveScreen(this); });
            AddEntity(mi);
            
        }

        public void WaitForPlayerCount(int count) {
            State = PauseState.WaitingForPlayers;

            Player = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            Player.DrawHands = true;

            AddEntity(Player);


            var mi = new MenuItem(Assets.SignMenu, MainGame.Width - 300, (MainGame.Height / 2) - 100, delegate() { Manager.SwitchScreen(new MenuScreen(Context)); });
            AddEntity(mi);

            Actions.AddAction(new WaitForPlayerCount(count, Context), true);
            Actions.AddAction(new CallFunction(delegate() { State = PauseState.Countdown; }), true);
        }
    
        private float countDownTimer = 3;
        private float secondTimer = 0;
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            switch (State) {
                case PauseState.Countdown:
                    countDownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    secondTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (secondTimer <= 0) {
                        Assets.TimeShort.Play();
                        secondTimer = 1f;
                    }

                    if(countDownTimer <= 0) {
                        Manager.RemoveScreen(this);
                    }

                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {           
            spriteBatch.Draw(Assets.PauseBackground, new Rectangle(0, 0, MainGame.Width, MainGame.Height), new Color(255, 255, 255, 200));

            switch (State) {
                case PauseState.WaitingForPlayers:
                    spriteBatch.Draw(Assets.TextPlayerLeft, new Rectangle((MainGame.Width / 2), 50, 717, 83), null, Color.White, 0, new Vector2(717 / 2, 83 / 2), SpriteEffects.None, 0);
                    spriteBatch.Draw(Assets.TextPleaseComeBack, new Rectangle((MainGame.Width / 2), MainGame.Height - 100, 814, 58), null, Color.White, 0, new Vector2(814 / 2, 58 / 2), SpriteEffects.None, 0);

                    break;
                case PauseState.Countdown:
                    var countdown = new FontNumber((int)countDownTimer, (MainGame.Width / 2) - 50, (MainGame.Height / 2) - 50, new Point(100, 100), FontNumber.FontNumberColor.Red);
                    countdown.Draw(spriteBatch);

                    break;
                default:
                    break;
            }

            base.Draw(spriteBatch);
        }

        public enum PauseReason {
            PlayerLeft,
            PauseOnPurpose
        }

        public enum PauseState {
            Default,
            WaitingForPlayers,
            Countdown
        }
    }
}
