using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace ClownSchool.Bang.Actions {
    public class FadeInSong : IAction {

        private bool isBlocking { get; set; }
        private bool isComplete { get; set; }

        public Song Song { get; set; }
        public bool Repeat { get; set; }
        public float MaxVolume { get; set; }

        public FadeInSong(Song song, bool repeat, float maxVolume) {
            Song = song;
            Repeat = repeat;
            MaxVolume = maxVolume;
        }

        public bool IsBlocking() {
            return isBlocking;
        }

        public bool IsComplete() {
            return isComplete;
        }

        public void Block() {
            isBlocking = true;
        }

        public void Unblock() {
            isBlocking = false;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime) {
            MediaPlayer.IsRepeating = Repeat;

            if(MediaPlayer.Queue.ActiveSong != Song) {
                if(MediaPlayer.Volume > 0) {
                    MediaPlayer.Volume -= 0.01f;
                } else {
                    MediaPlayer.Play(Song);
                }
            } else {
                if(MediaPlayer.Volume < MaxVolume) {
                    MediaPlayer.Volume += 0.01f;
                } else {
                    Complete();
                }
            }
        }

        public void Complete() {
            isComplete = true;  
        }
    }
}
