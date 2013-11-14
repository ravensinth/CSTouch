using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Kinect;
using ClownSchool.Entity;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Linq;
using System.Diagnostics;

namespace ClownSchool {

    public class PlayerHand : BaseEntity {

        public Vector2 Position { get; set; }
        public Player Player { get; private set; }
        public JointType Hand { get; private set; }
        public Balloon DraggingBalloon { get; set; }

        public bool IsGrabbing { get; set; }

        public MouseState MouseState;
        public MouseState OldMouseState { get; set; }
        public TouchLocationState OldTouchState { get; set; }
        public TouchLocationState TouchState { get; set; }

        public KinectContext Context {
            get {
                return Player.Context;
            }
        }

        public PlayerHand(Player player) {
            this.Offset = new Point(-30, -30);
            this.Size = new Point(50, 50);

            Player = player;

            CollisionType = "hand";

            ZDepth = 1000;
        }

        public override void Init() {
        }

        public override void Update(GameTime gameTime) {
            Matrix Scale = Matrix.CreateScale(1, 1, 1);
            OldMouseState = MouseState;
            MouseState = Mouse.GetState();          

            var touch = TouchPanel.GetState().FirstOrDefault();
            if (touch != null) {
                OldTouchState = TouchState;
                TouchState = touch.State;
            }

            Vector2 pos = new Vector2();
            if (InputObserver.IsUsingTouchScreen) {
                if (touch.State != TouchLocationState.Invalid) {
                    //Position = Vector2.Transform(touch.Position - new Vector2(Game.GraphicsDevice.Viewport.X, Game.GraphicsDevice.Viewport.Y), Matrix.Invert(Resolution.getTransformationMatrix()));
                    pos = touch.Position;
                }
            }
            else {
                pos = new Vector2(MouseState.X, MouseState.Y);
                //Position = Vector2.Transform(new Vector2(MouseState.X, MouseState.Y) - new Vector2(Game.GraphicsDevice.Viewport.X, Game.GraphicsDevice.Viewport.Y), Matrix.Invert(Resolution.getTransformationMatrix()));
            }

            this.Position = Vector2.Transform(pos, Scale);
            X = Position.X;
            Y = Position.Y;

          //  this.HandClosed = MouseState.LeftButton == ButtonState.Pressed;
        }

        public bool Clicked {
            get {
                if (InputObserver.IsUsingTouchScreen) {
                    Debug.WriteLine("Alt" + (OldTouchState == TouchLocationState.Pressed) + " Neu: " + (TouchState == TouchLocationState.Pressed));
                    return OldTouchState != TouchLocationState.Released && TouchState == TouchLocationState.Released;
                }
                else {
                    return OldMouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released;
                }
            }
        }


        public bool Pressing {
            get {
                if (InputObserver.IsUsingTouchScreen) {
                    return TouchState == TouchLocationState.Pressed || TouchState == TouchLocationState.Moved;
                }
                else {
                    return MouseState.LeftButton == ButtonState.Pressed;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Player.DrawHands) {
                var glove = IsGrabbing ? Assets.GloveFist : Assets.Glove;
                spriteBatch.Draw(glove, new Rectangle((int)Position.X, (int)Position.Y, 56, 64), null, Color.White, 0, new Vector2(glove.Width / 2, glove.Height / 2), Hand == JointType.HandLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
        }

        public void Grab(Balloon balloon) {
            balloon.AttachTo(this);
            this.DraggingBalloon = balloon;
            Assets.BalloonGrab.Play();
        }

        public override void Delete() {
        }
    }
}