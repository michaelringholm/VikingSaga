using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VikingSaga.Code.Resources;

namespace VikingSaga.Code
{
    class SoundUtil
    {
        private static MediaPlayer _mediaPlayer = new MediaPlayer();

        public enum SoundEnum { SwordSlash, MaleHurtShort, RipCard, BattleLost, BattleWon, Danger, WalkForest, LevelGained, ImportantMessage };

        internal static void PlaySound(SoundEnum soundEnum)
        {
            PlaySound(GetSoundLocation(soundEnum));
        }

        internal static void StartMP3Loop(string soundLocation)
        {
            MediaTimeline timeline = new MediaTimeline(ResourceManager.getPackedUri(soundLocation));
            timeline.RepeatBehavior = RepeatBehavior.Forever;
            MediaClock clock = timeline.CreateClock();
            //MediaPlayer player = new MediaPlayer();
            _mediaPlayer.Clock = clock;
        }

        internal static void PauseMP3Loop()
        {
            if (_mediaPlayer != null && _mediaPlayer.Clock != null)
                _mediaPlayer.Clock.Controller.Pause();
        }

        internal static void ResumeMP3Loop()
        {
            if (_mediaPlayer != null && _mediaPlayer.Clock != null)
                _mediaPlayer.Clock.Controller.Resume();
        }

        internal static void StopCurrentMP3Loop()
        {
            _mediaPlayer.Clock = null;
            _mediaPlayer.Stop();
        }

        internal static void PlayMP3(string soundLocation)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(ResourceManager.getPackedUri(soundLocation));
            mediaPlayer.Play();
        }

        internal static void PlaySound(string soundLocation)
        {
            var uri = ResourceManager.getPackedUri(soundLocation);
            var stream = System.Windows.Application.GetResourceStream(uri).Stream;
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(stream);
            player.Play();
        }

        internal static string GetSoundLocation(SoundEnum soundEnum)
        {
            String soundLocation = null;
            if(soundEnum == SoundEnum.SwordSlash)
                soundLocation = @"sword-slash.wav";
            else if(soundEnum == SoundEnum.MaleHurtShort)
                soundLocation = @"short-hurt-male.wav";
            else if (soundEnum == SoundEnum.RipCard)
                soundLocation = "sounds/paper-rip.wav";
            else if (soundEnum == SoundEnum.BattleWon)
                soundLocation = "sounds/fanfare-victory.wav";
            else if (soundEnum == SoundEnum.BattleLost)
                soundLocation = "sounds/loss.wav";
            else if (soundEnum == SoundEnum.Danger)
                soundLocation = "sounds/danger.wav";
            else if (soundEnum == SoundEnum.WalkForest)
                soundLocation = "sounds/walk-forest.wav";
            else if (soundEnum == SoundEnum.LevelGained)
                soundLocation = "sounds/fanfare-victory.wav";
            else if (soundEnum == SoundEnum.ImportantMessage)
                soundLocation = "sounds/danger.wav";

            return soundLocation;
        }
    }
}
