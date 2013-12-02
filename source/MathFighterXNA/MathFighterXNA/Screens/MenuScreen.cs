using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownSchool.Entity.Menu;
using ClownSchool.Entity;
using ClownSchool.Tweening;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using ClownSchool.Bang.Actions;
using Microsoft.Xna.Framework.Input;

namespace ClownSchool.Screens {
    public class MenuScreen : GameScreen {

        public Menu MainMenu;

        public Player Player;

        private Tweener tweenerY;

        private Dictionary<BaseEntity, Vector2> tweenObjects = new Dictionary<BaseEntity, Vector2>();
        
        private static Vector2 topRightPosition = new Vector2(MainGame.Width - 400, 250);
        private static Vector2 topLeftPosition = new Vector2(200, 250);
        private static Vector2 bottomRightPosition = new Vector2(MainGame.Width - 400, 450);
        private static Vector2 bottomLeftPosition = new Vector2(200, 450);            

        private MenuItem TopRight;
        private MenuItem TopLeft;
        private MenuItem BottomRight;
        private MenuItem BottomLeft;

        public MenuScreen(KinectContext context): base(context) {
            MainMenu = new Menu();
        }

        public override void Init() {
            base.Init();

            MediaPlayer.Volume = 0f;

            Manager.FadeInSong(Assets.MenuSong, true, 0.5f);

            tweenerY = new Tweener(0, 20, 1.5f, Linear.EaseIn);
            tweenerY.Ended += delegate() { tweenerY.Reverse(); };

            Player = new Player(Context, SkeletonPlayerAssignment.LeftSkeleton);
            Player.DrawHands = true;

            AddEntity(Player);

            AddCurtain();            

            int logoWidth = 350;
            int logoHeight = 357;
            
            var logo = new SimpleGraphic(Assets.MenuLogo, (MainGame.Width / 2) - logoWidth / 2, ((MainGame.Height / 2) - logoHeight / 2) - 100, logoWidth, logoHeight);
            AddEntity(logo);

            MainMenu.AddItem(new MenuItem(Assets.MenuSignMultiPlayer, 0, 0, OnClick_Multiplayer));
            MainMenu.AddItem(new MenuItem(Assets.MenuSignSinglePlayer, 0, 0, OnClick_SettingsSinglePlayer));
            MainMenu.AddItem(new MenuItem(Assets.MenuSignHighscore, 0, 0, OnClick_Highscore));
            MainMenu.AddItem(new MenuItem(Assets.MenuSignHelp, 0, 0, OnClick_Help));

            LoadMenu(MainMenu);

            //var exit = new MenuButton(Assets.MenuButtonExit, (MainGame.Width / 2) - 65, MainGame.Height - 150, OnClick_Exit);
            //AddEntity(exit);
        }

        void OnClick_Exit() {
            Manager.Game.Exit();
        }

        void OnClick_Multiplayer() {
            LoadMultiPlayerMenu();
        }

        void OnClick_Versus() {
            Manager.SwitchScreen(new VersusPlayerScreen(Context));      
        }

        void OnClick_Coop() {
            Manager.SwitchScreen(new CoopPlayerScreen(Context));
        }

        void OnClick_SettingsSinglePlayer() {
            LoadSettingsMenu();
        }

        void OnClick_SinglePlayer() {
            Manager.SwitchScreen(new SinglePlayerScreen(Context));
        }

        void OnClick_Help() {
            LoadTutorialMenu();
        }

        void OnClick_Highscore() {
            LoadHighscoreMenu();
        }

        void OnClick_Highscore_Coop() {
            Manager.SwitchScreen(new HighscoreScreen(Context, MainGame.CoopHighscoreDirectory));
        }

        void OnClick_Highscore_Single() {
            Manager.SwitchScreen(new HighscoreScreen(Context, MainGame.SingleHighscoreDirectory));
        }

        void OnClick_Tutorial_CoopSingle() {
            Manager.SwitchScreen(new CoopTutorialScreen(Context));
        }

        void OnClick_Tutorial_Versus() {
            Manager.SwitchScreen(new VersusTutorialScreen(Context));
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            base.Update(gameTime);


            if (TopRight != null && TopRight.Actions.IsComplete()) {
                tweenerY.Update(gameTime);

                TopRight.Y = (int)topRightPosition.Y + (int)tweenerY.Position;
            }

            if (TopLeft != null && TopLeft.Actions.IsComplete()) {
                TopLeft.Y = (int)topLeftPosition.Y + (int)tweenerY.Position;
            }

            if (BottomRight != null && BottomRight.Actions.IsComplete()) {
                BottomRight.Y = (int)bottomRightPosition.Y + (int)tweenerY.Position;
            }

            if (BottomLeft != null && BottomLeft.Actions.IsComplete()) {
                BottomLeft.Y = (int)bottomLeftPosition.Y + (int)tweenerY.Position;
            }
        }

        public void LoadMenu(Menu menu) {
            for (int i = 0; i < menu.Items.Count; i++) {
                var mi = menu.Items[i];

                switch (i) {
                    case 0:
                        mi.Position = new Vector2((int)topLeftPosition.X - 600, (int)topLeftPosition.Y - 600);
                        mi.RenderBalloons = true;

                        if (i + 1 <= menu.Items.Count - 1)
                            mi.RenderRopes = true;

                        TopLeft = mi;
                        TopLeft.Actions.AddAction(new TweenPositionTo(mi, topLeftPosition, 3f, Back.EaseOut), true);

                        break;
                    case 1:
                        mi.Position = new Vector2((int)bottomLeftPosition.X - 600, (int)bottomLeftPosition.Y + 600);
                        BottomLeft = mi;
                        BottomLeft.Actions.AddAction(new TweenPositionTo(mi, bottomLeftPosition, 3f, Back.EaseOut), true);

                        break;
                    case 2:
                        mi.Position = new Vector2((int)topRightPosition.X + 600, (int)topRightPosition.Y - 600);
                        mi.RenderBalloons = true;

                        if (i + 1 <= menu.Items.Count - 1)
                            mi.RenderRopes = true;

                        TopRight = mi;
                        TopRight.Actions.AddAction(new TweenPositionTo(mi, topRightPosition, 3f, Back.EaseOut), true);

                        break;
                    case 3:
                        mi.Position = new Vector2((int)bottomRightPosition.X + 600, (int)bottomRightPosition.Y + 600);
                        BottomRight = mi;
                        BottomRight.Actions.AddAction(new TweenPositionTo(mi, bottomRightPosition, 3f, Back.EaseOut), true);

                        break;
                    default:
                        throw new ArgumentException("No more than 4 menu items allowed!");
                }
            }

            AddEntity(menu);
        }

        public void LoadMultiPlayerMenu() {
            RemoveEntity(MainMenu);

            var multiPlayer = new Menu();

            multiPlayer.AddItem(new MenuItem(Assets.MenuSignVersus, 0, 0, OnClick_Versus));           
            multiPlayer.AddItem(new MenuItem(Assets.MenuSignMenu, 0, 0, delegate() { RemoveEntity(multiPlayer); LoadMenu(MainMenu); }));
            multiPlayer.AddItem(new MenuItem(Assets.MenuSignCoop, 0, 0, OnClick_Coop));

            LoadMenu(multiPlayer);           
        }

        public void LoadHighscoreMenu() {
            RemoveEntity(MainMenu);

            var highscore = new Menu();

            highscore.AddItem(new MenuItem(Assets.MenuSignCoop, 0, 0, OnClick_Highscore_Coop));
            highscore.AddItem(new MenuItem(Assets.MenuSignMenu, 0, 0, delegate() { RemoveEntity(highscore); LoadMenu(MainMenu); }));
            highscore.AddItem(new MenuItem(Assets.MenuSignSinglePlayer, 0, 0, OnClick_Highscore_Single));

            LoadMenu(highscore);
        }

        public void LoadTutorialMenu() {
            RemoveEntity(MainMenu);

            var tutorial = new Menu();

            tutorial.AddItem(new MenuItem(Assets.MenuSignCoop, 0, 0, OnClick_Tutorial_CoopSingle));
            tutorial.AddItem(new MenuItem(Assets.MenuSignMenu, 0, 0, delegate() { RemoveEntity(tutorial); LoadMenu(MainMenu); }));
            tutorial.AddItem(new MenuItem(Assets.MenuSignSinglePlayer, 0, 0, OnClick_Tutorial_CoopSingle));
            tutorial.AddItem(new MenuItem(Assets.MenuSignVersus, 0, 0, OnClick_Tutorial_Versus));

            LoadMenu(tutorial);
        }

        public void LoadSettingsMenu() {
            RemoveEntity(MainMenu);

            var settings = new Menu();

            settings.AddItem(new MenuItem(Assets.MenuSignMenu, 0, 0, delegate() { RemoveEntity(settings); LoadMenu(MainMenu); }));
            settings.AddItem(new MenuItem(Assets.MenuSignSinglePlayer, 0, 0, OnClick_SinglePlayer));

            var addition = new CheckBox(Settings.type.Addition, Assets.MenuCheckboxAdditionOn, Assets.MenuCheckboxAdditionOff, 900, 100);
            AddEntity(addition);
            var subtraction = new CheckBox(Settings.type.Subtraction, Assets.MenuCheckboxSubtractionOn, Assets.MenuCheckboxSubtractionOff, 900, 300);
            AddEntity(subtraction);
            var muliplication = new CheckBox(Settings.type.Multiplication, Assets.MenuCheckboxMultiplicationOn, Assets.MenuCheckboxMultiplicationOff, 900, 500);
            AddEntity(muliplication);

            LoadMenu(settings);
        }
    }
}
