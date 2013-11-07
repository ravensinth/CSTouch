using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClownSchool.Bang;
using ClownSchool.Bang.Actions;
using ClownSchool.Entity;
using ClownSchool.Tweening;
using FarseerPhysics.Dynamics;
using System;
using ClownSchool.Bang.Coroutine;
using System.Diagnostics;

namespace ClownSchool.Screens {

    public abstract class GameScreen {

        public KinectContext Context { get; private set; }
        public ScreenManager Manager { get; set; }

        public bool Inited { get; private set; }

        public List<BaseEntity> Entities = new List<BaseEntity>();
        public ActionList Actions = new ActionList();
        public Coroutines Coroutines = new Coroutines();

        private SimpleGraphic CurtainLeft;
        private SimpleGraphic CurtainRight;
        private SimpleGraphic BackgroundLeft;
        private SimpleGraphic BackgroundRight;

        //private Stopwatch sw = new Stopwatch();

        public bool SomePlayerIsDragging {
            get {
                return Entities.Where(h => (h.CollisionType == "hand" && (h as PlayerHand).DraggingBalloon != null)).Count() > 0;
            } 
        }

        //Physics
        public World World;

        public GameScreen(KinectContext context) {
            Context = context;
            Inited = false;
        }

        public void AddEntity(BaseEntity entity) {
            Entities.Add(entity);
            entity.Screen = this;

            entity.Init();
        }

        public void RemoveEntity(BaseEntity entity) {
            if (!Entities.Contains(entity))
                return;

            entity.Delete();
            Entities.Remove(entity);
        }

        public virtual void Init() {            
            World = new World(new Vector2(0f, 10f));

            Inited = true;
        }

        public void AddCurtain() {
            CurtainLeft = new SimpleGraphic(Assets.CurtainTopLeft, 0, 0, 260, MainGame.Height - 50);
            CurtainRight = new SimpleGraphic(Assets.CurtainTopRight, MainGame.Width - 260, 0, 260, MainGame.Height - 50);

            BackgroundLeft = new SimpleGraphic(Assets.CurtainBottomLeft, 0, 0, (MainGame.Width / 2) + 10, MainGame.Height + 20);
            BackgroundRight = new SimpleGraphic(Assets.CurtainBottomRight, (MainGame.Width / 2) - 10, 0, (MainGame.Width / 2) + 10, MainGame.Height + 20);

            BackgroundLeft.ZDepth = BackgroundRight.ZDepth = -1;

            AddEntity(BackgroundLeft);
            AddEntity(BackgroundRight);

            AddEntity(CurtainLeft);
            AddEntity(CurtainRight);
        }

        public void OpenCurtain() {
            Actions.AddAction(new TweenPositionTo(BackgroundLeft, new Vector2(-(MainGame.Width / 2) + 160, 0), 2f, Sinusoidal.EaseOut), false);
            Actions.AddAction(new TweenPositionTo(BackgroundRight, new Vector2(MainGame.Width - 160, 0), 2f, Sinusoidal.EaseOut), false);
        }

        public virtual void Update(GameTime gameTime) {
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            //Dirty? Calling ToArray to make a copy of the entity collection preventing crashing when entities create other entities through an update call
            //sw.Start();
            foreach (var ent in Entities.ToArray()) {
                ent.Update(gameTime);                
                //Debug.WriteLine(ent.GetType().ToString());
            }
            //Debug.WriteLine("Update Entities: "+ sw.ElapsedMilliseconds.ToString());
            //sw.Reset();

            Actions.Update(gameTime);
            //Debug.WriteLine("Update Actions: " + sw.ElapsedMilliseconds.ToString());
            //sw.Reset();

            Coroutines.Update();
            //Debug.WriteLine("Update Coroutines: " + sw.ElapsedMilliseconds.ToString());
            //sw.Stop();

        }
        public virtual void Draw(SpriteBatch spriteBatch) {
            //sw.Start();
            foreach (var ent in Entities.ToArray().OrderBy(e => e.ZDepth)) {
                ent.Draw(spriteBatch);
            }
            //Debug.WriteLine("Draw Entities: " + sw.ElapsedMilliseconds.ToString());
            //sw.Stop();
        }
    }
}
