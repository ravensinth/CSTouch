using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ClownSchool.Tweening;


namespace ClownSchool.Bang.Actions {
    class TweenPositionToEntity : IAction {

        private bool isBlocking { get; set; }
        private bool isComplete { get; set; }

        private Tweener tweenerX;
        private Tweener tweenerY;

        private bool tweenerXFinished;
        private bool tweenerYFinished;

        private BaseEntity to { get; set; }
        private Point offset { get; set; }
        private float duration { get; set; }
        private TweeningFunction tween { get; set; }

        private BaseEntity entity;

        public TweenPositionToEntity(BaseEntity entity, BaseEntity to, Point offset, float duration, TweeningFunction tween) {
            this.entity = entity;
            this.to = to;
            this.offset = offset;
            this.duration = duration;
            this.tween = tween;

            isComplete = false;
        }

        bool IAction.IsBlocking() {
            return isBlocking;
        }

        bool IAction.IsComplete() {
            return isComplete;
        }

        void IAction.Block() {
            isBlocking = true;
        }

        void IAction.Unblock() {
            isBlocking = false;
        }

        void initTweeners() {
            tweenerX = new Tweener(entity.X, to.X + offset.X, duration, tween);
            tweenerY = new Tweener(entity.Y, to.Y + offset.Y, duration, tween);            

            tweenerX.Ended += delegate() { tweenerXFinished = true; };
            tweenerY.Ended += delegate() { tweenerYFinished = true; };
        }

        void IAction.Update(GameTime gameTime) {
            if (tweenerX == null || tweenerY == null) {
                initTweeners();
            }

            tweenerX.Update(gameTime);
            tweenerY.Update(gameTime);

            entity.X = (int)tweenerX.Position;
            entity.Y = (int)tweenerY.Position;

            if (tweenerXFinished && tweenerYFinished) {
                isComplete = true;
            }
        }

        void IAction.Complete() {
            isComplete = true;
        }
    }
}
