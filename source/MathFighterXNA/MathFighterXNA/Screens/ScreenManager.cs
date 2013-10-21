using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ClownSchool.Bang;
using Microsoft.Xna.Framework.Media;
using ClownSchool.Bang.Actions;
using Microsoft.Xna.Framework.Input;

namespace ClownSchool.Screens {
    public class ScreenManager {
        private List<GameScreen> Screens = new List<GameScreen>();

        public ActionList Actions = new ActionList();

        public GameScreen TopScreen {
            get {
                return Screens.Last();
            }
        }

        public MainGame Game { get; private set; }

        public ScreenManager(MainGame game) {
            Game = game;
        }

        public void AddScreen(GameScreen screen) {
            Screens.Add(screen);
            screen.Manager = this;
            
            if (!screen.Inited)
                screen.Init();            
        }

        public void FadeInSong(Song song, bool repeat, float maxVolume) {
            Actions.AddAction(new FadeInSong(song, repeat, maxVolume), true);
        }

        public void RemoveScreen(GameScreen screen) {
            Screens.Remove(screen);
        }

        public void SwitchScreen(GameScreen screen) {
            Screens.Clear();
            AddScreen(screen);
        }

        public void Update(GameTime gameTime) {
            Actions.Update(gameTime);
            Screens.Last().Update(gameTime);

            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.M)) {
                SwitchScreen(new MenuScreen(Game.kinectContext));
            }

            if (keyState.IsKeyDown(Keys.C)) {
                SwitchScreen(new CoopPlayerScreen(Game.kinectContext));
            }

            if (keyState.IsKeyDown(Keys.S)) {
                SwitchScreen(new SinglePlayerScreen(Game.kinectContext));
            }

            if (keyState.IsKeyDown(Keys.V)) {
                SwitchScreen(new VersusPlayerScreen(Game.kinectContext));
            }

            if (keyState.IsKeyDown(Keys.T)) {
                SwitchScreen(new CoopTutorialScreen(Game.kinectContext));
            }

            if (keyState.IsKeyDown(Keys.P)) {
                if (MediaPlayer.State == MediaState.Playing) {
                    MediaPlayer.Pause();
                } else {
                    MediaPlayer.Resume();
                }
            }                                 
        }

        public void Draw(ExtendedSpriteBatch spriteBatch) {
            foreach (var screen in Screens) {
                screen.Draw(spriteBatch);
            }
        }        
    }
}
