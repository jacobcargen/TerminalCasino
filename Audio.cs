using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.ComponentModel;

namespace TerminalCasino
{
    static class Audio
    {
        // Save file folder
        private static string savePath = AppDomain.CurrentDomain.BaseDirectory + @"\audio";

        private static SoundPlayer cmdAccept = new SoundPlayer(savePath + @"\accepted.wav");
        private static SoundPlayer cmdFailed = new SoundPlayer(savePath + @"\error.wav");
        private static SoundPlayer welcomeSFX = new SoundPlayer(savePath + @"\welcomemsg.wav");
        private static SoundPlayer dealCard = new SoundPlayer(savePath + @"\dealcard.wav");
        private static SoundPlayer shuffle = new SoundPlayer(savePath + @"\shuffle.wav");
        //private static SoundPlayer menuMusic = new SoundPlayer(savePath + @"\menumusic.wav");


        public static void CheckDir() { Tools.CreateDirIfNone(savePath); }

        #region SFX

        public static void WelcomeSound()
        {
            //welcomeSFX.Play();
        }

        /// <summary>
        /// Call this to play the command accepted sound.
        /// </summary>
        public static void PlayCmdAcceptedSound()
        {
            //cmdAccept.Play();
        }

        /// <summary>
        /// Call this to play the command failed sound.
        /// </summary>
        public static void ErrorSound()
        {
            //cmdFailed.Play();
        }

        /// <summary>
        /// Play shuffling sound.
        /// </summary>
        public static void SuffleSound()
        {
            //shuffle.Play();
        }

        public static void DealCardSound()
        {
            //dealCard.Play();
        }

        #endregion

        #region Music

        public static void PlayMenuMusic(bool isPlay)
        {
            if (isPlay)
            {

            }
            else
            {

            }
        }
        public static void GameMusic()
        {

        }
        public static void CancelCurrentMusic()
        {

        }

        #endregion
    }
}
