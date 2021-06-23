using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Threading;
using System.IO;
using com.okitoki.wavhello.chunks;
using com.okitoki.wavhello.events;

namespace com.okitoki.wavhello
{
    public class WavePlayer
    {
        private SoundPlayer player;

        private long startTime;
        private WaveFile currentFile;
        private Thread playThread;

        public event EventHandler<PlayCompletedEventArgs> PlayCompleted;
        public event EventHandler<PlayInterruptedEventArgs> PlayInterrupted;
        public event EventHandler<PlayProgressChangedEventArgs> PlayProgressChanged;

        public int CurrentTime { get; set; } = 0;

        public WavePlayer() { }

        public void Play(WaveFile file, bool loop = false)
        {
            Stop();

            this.currentFile = file;

            playThread = new Thread(() => 
            {
                try
                {
                    if (file.IsRead)
                    {
                        MemoryStream memStream = currentFile.GetStream();

                        do
                        {
                            startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            memStream.Seek(0, SeekOrigin.Begin);
                            player = new SoundPlayer(memStream);
                            player.Play();

                            //Start a timer to monitor progress.
                            TrackProgress();

                            PlayCompletedEventArgs args = new PlayCompletedEventArgs();
                            args.WaveFile = currentFile;
                            OnPlayCompleted(args);
                        }
                        while (loop);
                    }
                } catch(ThreadInterruptedException e) { }
            });

            playThread.Start();
        }

        public float GetProgress()
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            float progress = ((float)(currentTime - startTime)/1000) / currentFile.LengthInSeconds();
            return progress;
        }

        private void TrackProgress()
        {
            float progress = 0.0f;

            if (currentFile.LengthInSeconds() > 0)
            {
                int progressInt = 0;

                while (progressInt < 100)
                {
                    Thread.Sleep(1000);
                    progress = GetProgress();

                    if ((int)(progress * 100.0f) != progressInt)
                    {
                        progressInt++;

                        PlayProgressChangedEventArgs args = new PlayProgressChangedEventArgs();
                        args.Progress = (int)(progress * 100);
                        args.WaveFile = currentFile;
                        OnPlayProgressChanged(args);
                    }
                }
            }
        }

        private void OnPlayInterrupted(PlayInterruptedEventArgs args)
        {
            if(PlayInterrupted != null)
            {
                PlayInterrupted(this, args);
            }
        }

        private void OnPlayProgressChanged(PlayProgressChangedEventArgs args)
        {
            if(PlayProgressChanged != null)
            {
                PlayProgressChanged(this, args);
            }
        }

        private void OnPlayCompleted(PlayCompletedEventArgs args)
        {
            if(PlayCompleted != null)
            {
                PlayCompleted(this, args);
            }
        }

        public WaveFile Play(string filePath)
        {
            currentFile = WaveReader.ReadFile(filePath);
            Play(currentFile);
            return currentFile;
        }

        public void Stop()
        {
            if (playThread != null)
            {
                playThread.Interrupt();
            }

            if(player != null)
            {
                player.Stop();
            }

            PlayInterruptedEventArgs args = new PlayInterruptedEventArgs();
            args.WaveFile = currentFile;
            OnPlayInterrupted(args);
        }
    }
}
