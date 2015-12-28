using NAudio.Wave;
using NVorbis.NAudioSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vik.Code.Controls.Utility
{
    public class Sound
    {
        private class KeepAlive
        {
            public Stream Stream { get; set; }
            public VorbisWaveReader Reader { get; set; }
            public WaveOut WaveOut { get; set; }
        }

        private List<KeepAlive> _playingSounds = new List<KeepAlive>();
        private object _lock = new object();

        public void Play(string soundResourcePath)
        {
            try
            {
                var ext = Path.GetExtension(soundResourcePath).ToLower();
                if (ext == ".ogg")
                {
                    PlayOgg(soundResourcePath);
                }
                else if (ext == ".wav")
                {
                    PlaySound(soundResourcePath);
                }
                else
                {
                    PlayMedia(soundResourcePath);
                }
            }
            catch (Exception e)
            {
                UiUtil.ShowFloatingInfo(string.Format("Error playing sound [{0}]: {1}", soundResourcePath, e.Message), 0.5, 0.8, true, Colors.Red, false, 0, 0, -100, 20, 100, 8000, 4000);
            }
        }

        private void PlayMedia(string soundResourcePath)
        {
            var uri = VikGame.ResourceLocator.GetSoundResourceUri(soundResourcePath);
            var player = new MediaPlayer();
            player.Open(uri);
            player.Play();
        }

        private void PlaySound(string soundResourcePath)
        {
            var stream = VikGame.ResourceLocator.GetSoundResourceStream(soundResourcePath);
            var player = new SoundPlayer(stream);
            player.Play();
        }

        private void PlayOgg(string soundResourcePath)
        {
            var stream = VikGame.ResourceLocator.GetSoundResourceStream(soundResourcePath);
            var reader = new VorbisWaveReader(stream);
            var waveOut = new WaveOut();
            waveOut.PlaybackStopped += waveOut_PlaybackStopped;
            waveOut.Init(reader);
            waveOut.Play();

            lock (_lock)
            {
                _playingSounds.Add(new KeepAlive { Stream = stream, Reader = reader, WaveOut = waveOut });
            }
        }

        void waveOut_PlaybackStopped(object sender, NAudio.Wave.StoppedEventArgs e)
        {
            lock (_lock)
            {
                var keepAlive = _playingSounds.FirstOrDefault(k => k.WaveOut == sender);
                if (keepAlive == null)
                    return;

                keepAlive.WaveOut.Dispose();
                keepAlive.Reader.Dispose();
                keepAlive.Stream.Dispose();
                _playingSounds.Remove(keepAlive);
            }
        }
    }
}
