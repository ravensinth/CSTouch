using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ClownSchool.Bang {
    public class ActionList : IAction {

        private List<IAction> Actions = new List<IAction>();

        private bool isBlocking { get; set; }

        public int Count {
            get {
                return Actions.Count;
            }
        }

        bool IAction.IsBlocking() {
            return isBlocking;
        }

        public bool IsComplete() {
            return Actions.Count == 0;
        }

        void IAction.Block() {
            isBlocking = true;
        }

        void IAction.Unblock() {
            isBlocking = false;
        }

        public void Insert(int position, IAction action, bool blocking) {
            if (blocking) {
                action.Block();
            } else {
                action.Unblock();
            }

            Actions.Insert(position, action);
        } 
        
        public void AddAction(IAction action, bool blocking) {
            Insert(Actions.Count, action, blocking);            
        }

        public void InsertAfter(IAction after, IAction action, bool blocking) {
            Insert(Actions.IndexOf(after) + 1, action, blocking);
        }

        public void Update(GameTime gameTime) {
            foreach (IAction action in Actions.ToArray<IAction>()) {
                action.Update(gameTime);

                if (action.IsComplete()) Actions.Remove(action);

                if (action.IsBlocking()) break;
            }            
        }

        void IAction.Complete() {            
        }
    }
}
