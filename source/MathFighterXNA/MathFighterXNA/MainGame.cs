using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ClownSchool.Screens;
using ClownSchool.Physics;
using ClownSchool.Bang.Actions;
using System.IO;

namespace ClownSchool {

    public class MainGame : Microsoft.Xna.Framework.Game {

        public static GraphicsDeviceManager graphics;
        ExtendedSpriteBatch spriteBatch;
        public KinectContext kinectContext;

        DebugComponent debugComponent;

        private readonly Rectangle viewPortRectangle;

        public static int Width = 1324;
        public static int Height = 768;

        public static int KinectWidth = Width - 300;
        public static int KinectHeight = Height;

        public static float KinectScaleX = 640f / KinectWidth;
        public static float KinectScaleY = 480f / KinectHeight;

        public static int KinectOffsetX = 150;
        public static int KinectOffsetY = 0;

        public static string CoopHighscoreDirectory = @"highscores\coop";
        public static string SingleHighscoreDirectory = @"highscores\single";

        public ScreenManager ScreenManager;

        public bool DebugView = false;

        Matrix spriteScale;

        public MainGame() {
            graphics = new GraphicsDeviceManager(this); 
            Content.RootDirectory = "Content";
                       
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
            this.viewPortRectangle = new Rectangle(0, 0, Width, Height);

            graphics.IsFullScreen = true;
        }

        protected override void Initialize() {
            Assets.LoadContent(Content);
            kinectContext = new KinectContext(graphics.GraphicsDevice);
            kinectContext.Initialize();

            ScreenManager = new ScreenManager(this);
            var splash = new SplashScreen(kinectContext, Assets.SplashLogo, 2f);
            ScreenManager.AddScreen(splash);
            ScreenManager.Actions.AddAction(new WaitForCondition(delegate() { return splash.TweenerFinished; }), true);
            ScreenManager.Actions.AddAction(new CallFunction(delegate() { ScreenManager.SwitchScreen(new MenuScreen(kinectContext)); }), true);            

            debugComponent = new DebugComponent(this);            

            createHighscoreDirectories();

            base.Initialize();
        }

        void createHighscoreDirectories() {
            if (!Directory.Exists(@"highscores")) {
                Directory.CreateDirectory(CoopHighscoreDirectory);
                Directory.CreateDirectory(SingleHighscoreDirectory);
            }
        }

        public void SaveScreenshot(string filename) {
            Color[] screenData = new Color[GraphicsDevice.PresentationParameters.BackBufferWidth * GraphicsDevice.PresentationParameters.BackBufferHeight];

            RenderTarget2D screenShot = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

            GraphicsDevice.SetRenderTarget(screenShot);

            Draw(new GameTime());

            GraphicsDevice.SetRenderTarget(null);

            int index = 0;
            string name = string.Concat(filename, "_", index, ".jpg");
            while (File.Exists(name)) {
                index++;
                name = string.Concat(filename, "_", index, ".jpg");
            }

            using (FileStream stream = new FileStream(name, FileMode.CreateNew)) {
                screenShot.SaveAsJpeg(stream, screenShot.Width, screenShot.Height);
                screenShot.Dispose();
            }
        } 

        protected override void LoadContent() {           
            spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);

            float scaleX = graphics.GraphicsDevice.Viewport.Width / 1324f;
            float scaleY = graphics.GraphicsDevice.Viewport.Height / 768f;

            spriteScale = Matrix.CreateScale(scaleX, scaleY, 1);
        }

        protected override void UnloadContent() {
            kinectContext.StopSensor();
        }

        protected override void Update(GameTime gameTime) {           
            debugComponent.Update(gameTime);
            
            kinectContext.Update();

            ScreenManager.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.F12)) {
                DebugView = !DebugView;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spriteScale);

            //TODO move kinectContext into the corresponding screens
            if (kinectContext.CurrentBitmap != null) {
                spriteBatch.Draw(kinectContext.CurrentBitmap, new Rectangle(KinectOffsetX, KinectOffsetY, KinectWidth, KinectHeight), Color.White);
            }

            ScreenManager.Draw(spriteBatch);

            if (DebugView) {
                debugComponent.Draw(spriteBatch, gameTime);
            }

            if (kinectContext.Sensor == null) {
                spriteBatch.DrawString(Assets.DebugFont, "NO KINECT SENSOR FOUND! PLEASE CONNECT A WINDOWS KINECT AND RESTART THE GAME!", new Vector2(50, 50), Color.LimeGreen);
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
