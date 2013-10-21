using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ClownSchool {

    public static class Assets {

        public static Texture2D NumberBackgroundSprite { get; set; }
        public static Texture2D JointSprite { get; set; }        
        public static Texture2D NumberSlotSprite { get; set; }
        public static Texture2D BalloonSpritesheet { get; set; }
        public static Texture2D CactusSprite { get; set; }
        public static Texture2D EquationInputSprite { get; set; }
        public static Texture2D ClockFrameSprite { get; set; }
        public static Texture2D ClockFillSprite { get; set; }
        public static Texture2D FontNumberRed { get; set; }
        public static Texture2D FontNumberYellow { get; set; }
        public static Texture2D ClockAddFive { get; set; }
        public static Texture2D ClockSubtractFive { get; set; }
        public static Texture2D ScoreSign { get; set; }

        public static Texture2D BalloonBoom { get; set; }
        public static Texture2D RopeSection { get; set; }
        public static Texture2D RopeKnot { get; set; }

        public static Texture2D IndicatorGreen { get; set; }
        public static Texture2D IndicatorYellow { get; set; }

        public static Texture2D InputTop { get; set; }
        public static Texture2D InputBottom { get; set; }

        public static Texture2D ScissorBottomRight { get; set; }
        public static Texture2D ScissorTopRight{ get; set; }
        public static Texture2D ScissorBottomLeft { get; set; }
        public static Texture2D ScissorTopLeft { get; set; }
        
        public static Texture2D CurtainTopLeft { get; set; }
        public static Texture2D CurtainTopRight { get; set; }
        public static Texture2D CurtainBottomLeft { get; set; }
        public static Texture2D CurtainBottomRight { get; set; }

        public static Texture2D CirclePartEmpty { get; set; }
        public static Texture2D CirclePartFilled { get; set; }

        public static Texture2D PauseBackground { get; set; }
        public static Texture2D TextPlayerLeft { get; set; }
        public static Texture2D TextPleaseComeBack { get; set; }
        public static Texture2D PlayerSilhouette { get; set; }

        public static Texture2D Glove { get; set; }
        public static Texture2D GloveFist { get; set; }

        public static Texture2D MenuBalloons { get; set; }
        public static Texture2D MenuLogo { get; set; }
        public static Texture2D MenuSignGlow { get; set; }
        public static Texture2D MenuSignSinglePlayer { get; set; }
        public static Texture2D MenuSignMultiPlayer { get; set; }
        public static Texture2D MenuSignCoop { get; set; }
        public static Texture2D MenuSignVersus { get; set; }
        public static Texture2D MenuSignHelp { get; set; }
        public static Texture2D MenuSignHighscore { get; set; }
        public static Texture2D MenuSignRestart { get; set; }
        public static Texture2D MenuSignMenu { get; set; }
        public static Texture2D MenuRope { get; set; }

        public static Texture2D MenuButtonGlow { get; set; }
        public static Texture2D MenuButtonExit { get; set; }

        public static Texture2D ButtonPause { get; set; }

        public static Texture2D SplashLogo { get; set; }
        public static Texture2D CameraFlash { get; set; }

        public static Texture2D HighscoreBoard { get; set; }

        public static Texture2D SignMenu { get; set; }
        public static Texture2D SignHighscore { get; set; }
        public static Texture2D SignResume { get; set; }

        public static Texture2D TutorialStep1 { get; set; }
        public static Texture2D TutorialStep2 { get; set; }
        public static Texture2D TutorialStep3 { get; set; }
        public static Texture2D TutorialStep4 { get; set; }
        public static Texture2D TutorialStep5 { get; set; }
        public static Texture2D TutorialStep6 { get; set; }
        public static Texture2D TutorialStep7 { get; set; }
        public static Texture2D TutorialStep8 { get; set; }
        public static Texture2D TutorialStep9 { get; set; }
        public static Texture2D TutorialStep10 { get; set; }       
        
        public static SpriteFont DebugFont { get; set; }
        public static SpriteFont SmallDebugFont { get; set; }

        public static SoundEffect BalloonGrab { get; set; }
        public static SoundEffect BalloonDrop { get; set; }
        public static SoundEffect MenuChoose { get; set; }
        public static SoundEffect MenuOver { get; set; }
        public static SoundEffect BalloonPop { get; set; }
        public static SoundEffect AnswerCorrect { get; set; }
        public static SoundEffect TimeShort { get; set; }
        public static SoundEffect AnswerWrong { get; set; }
        public static SoundEffect ScissorsSnip { get; set; }
        public static SoundEffect BalloonPlace { get; set; }

        public static SoundEffect CameraClick { get; set; }

        public static Song GameSong { get; set; }
        public static Song MenuSong { get; set; }
        public static Song WinSong { get; private set; }
        
        public static void LoadContent(ContentManager content) {
            NumberSlotSprite = content.Load<Texture2D>("balloon_gray");
            BalloonSpritesheet = content.Load<Texture2D>("balloon_spritesheet_blue");
            CactusSprite = content.Load<Texture2D>("cactus");
            EquationInputSprite = content.Load<Texture2D>("equals");
            ClockFrameSprite = content.Load<Texture2D>("timer_out");
            ClockFillSprite = content.Load<Texture2D>("timer_in");
            FontNumberRed = content.Load<Texture2D>("font_numbers");
            FontNumberYellow = content.Load<Texture2D>("font_numbers_yellow");
            ClockAddFive = content.Load<Texture2D>("timer_add_5");
            ClockSubtractFive = content.Load<Texture2D>("timer_subtract_5");
            ScoreSign = content.Load<Texture2D>("score_sign");

            BalloonBoom = content.Load<Texture2D>("balloon_boom");
            RopeSection = content.Load<Texture2D>("rope_section");
            RopeKnot = content.Load<Texture2D>("rope_knot");

            IndicatorGreen = content.Load<Texture2D>("indicator_green");
            IndicatorYellow = content.Load<Texture2D>("indicator_yellow");

            InputTop = content.Load<Texture2D>("input_top");
            InputBottom = content.Load<Texture2D>("input_bottom");

            ScissorBottomRight = content.Load<Texture2D>("scissors_bot_right");
            ScissorTopRight = content.Load<Texture2D>("scissors_top_right");

            ScissorBottomLeft = content.Load<Texture2D>("scissors_bot_left");
            ScissorTopLeft = content.Load<Texture2D>("scissors_top_left");

            CurtainTopLeft = content.Load<Texture2D>("curtain_top_left");
            CurtainTopRight = content.Load<Texture2D>("curtain_top_right");
            CurtainBottomLeft = content.Load<Texture2D>("curtain_bot_left");
            CurtainBottomRight = content.Load<Texture2D>("curtain_bot_right");

            CirclePartEmpty = content.Load<Texture2D>("circle_part_empty");
            CirclePartFilled = content.Load<Texture2D>("circle_part_filled");

            PauseBackground = content.Load<Texture2D>("pause_background");
            TextPlayerLeft = content.Load<Texture2D>("text_left");
            TextPleaseComeBack = content.Load<Texture2D>("text_come_back");
            PlayerSilhouette = content.Load<Texture2D>("silhouette");

            Glove = content.Load<Texture2D>("glove");
            GloveFist = content.Load<Texture2D>("glove_fist");

            MenuBalloons = content.Load<Texture2D>("Menu/menu_balloons");
            MenuLogo = content.Load<Texture2D>("Menu/menu_logo");
            MenuSignGlow = content.Load<Texture2D>("Menu/menu_sign_glow");
            MenuSignSinglePlayer = content.Load<Texture2D>("Menu/menu_sign_singleplayer");
            MenuSignMultiPlayer = content.Load<Texture2D>("Menu/menu_sign_multiplayer");
            MenuSignCoop = content.Load<Texture2D>("Menu/menu_sign_coop");
            MenuSignVersus = content.Load<Texture2D>("Menu/menu_sign_versus");
            MenuSignHighscore = content.Load<Texture2D>("Menu/menu_sign_highscore");
            MenuSignHelp = content.Load<Texture2D>("Menu/menu_sign_help");
            MenuSignRestart = content.Load<Texture2D>("Menu/menu_sign_restart");
            MenuSignMenu = content.Load<Texture2D>("Menu/menu_sign_menu");

            MenuButtonExit = content.Load<Texture2D>("Menu/menu_button_exit");
            MenuButtonGlow = content.Load<Texture2D>("Menu/menu_button_glow");
            MenuRope = content.Load<Texture2D>("Menu/menu_rope");

            ButtonPause = content.Load <Texture2D>("button_pause");

            SplashLogo = content.Load<Texture2D>("splash_logo");

            CameraFlash = content.Load<Texture2D>("camera_flash");

            HighscoreBoard = content.Load<Texture2D>("Highscore/highscore_board");

            SignMenu = content.Load<Texture2D>("sign_menu");
            SignHighscore = content.Load<Texture2D>("sign_highscore");
            SignResume = content.Load<Texture2D>("sign_resume");

            TutorialStep1 = content.Load<Texture2D>("Tutorial/toGrabPlace");
            TutorialStep2 = content.Load<Texture2D>("Tutorial/grabUntilAttached");
            TutorialStep3 = content.Load<Texture2D>("Tutorial/balloonsToSolve");
            TutorialStep4 = content.Load<Texture2D>("Tutorial/changeAnswer");
            TutorialStep5 = content.Load<Texture2D>("Tutorial/placeAndDisappears");
            TutorialStep6 = content.Load<Texture2D>("Tutorial/timerHitsZero");
            TutorialStep7 = content.Load<Texture2D>("Tutorial/correctAdds");
            TutorialStep8 = content.Load<Texture2D>("Tutorial/wrongCosts");

            TutorialStep9 = content.Load<Texture2D>("Tutorial/balloonsToForm");
            TutorialStep10 = content.Load<Texture2D>("Tutorial/runsOutLooses");

            DebugFont = content.Load<SpriteFont>("DebugFont");
            SmallDebugFont = content.Load<SpriteFont>("SmallDebugFont");

            BalloonGrab = content.Load<SoundEffect>("sounds/BalloonGrab"); //http://opengameart.org/content/battle-sound-effects
            BalloonDrop = content.Load<SoundEffect>("sounds/BalloonDrop"); //http://opengameart.org/content/battle-sound-effects
            MenuChoose = content.Load<SoundEffect>("sounds/MenuChoose"); //http://www.flashkit.com/soundfx/Interfaces/deep_pon-xrikazen-7422/index.php
            MenuOver = content.Load<SoundEffect>("sounds/MenuOver"); // http://www.flashkit.com/soundfx/Interfaces/cordless-xrikazen-7428/index.php
            BalloonPop = content.Load<SoundEffect>("sounds/BalloonPop"); //http://labs.petegoodman.com/ghetto_blaster/_includes/sfx/worms/WORMPOP.WAV
            AnswerCorrect = content.Load<SoundEffect>("sounds/AnswerCorrect"); //http://opengameart.org/content/completion-sound
            TimeShort = content.Load<SoundEffect>("sounds/TimeShort"); //http://www.flashkit.com/soundfx/Cartoon/Timer-GamePro9-8160/index.php
            AnswerWrong = content.Load<SoundEffect>("sounds/AnswerWrong"); //http://www.flashkit.com/soundfx/Cartoon/Slide_fl-Texavery-8987/index.php
            ScissorsSnip = content.Load<SoundEffect>("sounds/scissors_snip");
            BalloonPlace = content.Load<SoundEffect>("sounds/BalloonPlace");

            CameraClick = content.Load<SoundEffect>("sounds/camera_click");

            GameSong = content.Load<Song>("songs/toon");
            MenuSong = content.Load<Song>("songs/menu");
            WinSong = content.Load <Song>("songs/win");
        }
    }
}