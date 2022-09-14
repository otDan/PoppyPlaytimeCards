using Sonigon;
using UnityEngine;

namespace PoppyPlaytimeCards.Util
{
    internal class AudioController
    {
        private static readonly SoundParameterIntensity SoundParameterIntensity = new(0f, UpdateMode.Continuous);

        public static void Play(AudioClip audioClip, Transform transform)
        {
            SoundParameterIntensity.intensity = 1f;
            var soundContainer = ScriptableObject.CreateInstance<SoundContainer>();
            soundContainer.setting.volumeIntensityEnable = true;
            soundContainer.audioClip[0] = audioClip;
            var soundEvent = ScriptableObject.CreateInstance<SoundEvent>();
            soundEvent.soundContainerArray[0] = soundContainer;
            SoundParameterIntensity.intensity = Optionshandler.vol_Sfx / 1f * Optionshandler.vol_Master; //ConfigController.TimerVolumeConfig.Value * Optionshandler.vol_Master;
            SoundManager.Instance.Play(soundEvent, transform, SoundParameterIntensity);
        }
    }
}