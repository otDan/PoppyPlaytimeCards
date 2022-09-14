using System;
using System.Collections.Generic;
using ModdingUtils.GameModes;
using PoppyPlaytimeCards.Asset;
using UnboundLib;
using UnityEngine;
using UnityEngine.Video;

namespace PoppyPlaytimeCards.Component.Mono
{
    internal class JumpScareMono : MonoBehaviour, IPointEndHookHandler, IPickStartHookHandler
    {
        public static JumpScareMono Instance;
        private readonly GameObject _jumpScarePlayer;
        private readonly VideoPlayer _videoPlayer;
        private readonly List<JumpScare> _jumpScares = new();
        private readonly Dictionary<Player, bool> _playerScared = new();

        public JumpScareMono()
        {
            Instance = this;
            _jumpScarePlayer = Instantiate(AssetManager.JumpScarePlayer);
            _videoPlayer = _jumpScarePlayer.GetComponent<VideoPlayer>();
            foreach (var player in PlayerManager.instance.players)
            {
                _playerScared[player] = false;
            }

            _videoPlayer.loopPointReached += _ =>
            {
                _videoPlayer.targetTexture.Release();
                _videoPlayer.clip = GetJumpScare(_jumpScares.GetRandom<JumpScare>());
                _videoPlayer.Prepare();

                _jumpScarePlayer.SetActive(false);
            };
            _jumpScarePlayer.SetActive(false);
        }

        public void AddJumpScare(JumpScare jumpScare)
        {
            if (!_jumpScares.Contains(jumpScare)) _jumpScares.Add(jumpScare);

            _jumpScarePlayer.SetActive(true);
            _videoPlayer.targetTexture.Release();
            _videoPlayer.clip = GetJumpScare(_jumpScares.GetRandom<JumpScare>());
            _videoPlayer.Prepare();
            _jumpScarePlayer.SetActive(false);
        }

        public void Destroy()
        {
            Destroy(_jumpScarePlayer);
        }

        private static VideoClip GetJumpScare(JumpScare jumpScare)
        {
            switch (jumpScare)
            {
                case JumpScare.BunzoBunny:
                    return AssetManager.BunzoBunny;
                case JumpScare.HuggyWuggy:
                    return AssetManager.HuggyWuggy;
                case JumpScare.KissyMissy:
                    return AssetManager.KissyMissy;
                case JumpScare.MiniHuggies:
                    int number = UnityEngine.Random.Range(0, 3);
                    return number switch
                    {
                        0 => AssetManager.MiniHuggiesGreen,
                        1 => AssetManager.MiniHuggiesRed,
                        2 => AssetManager.MiniHuggiesYellow,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                case JumpScare.MommyLongLegs:
                    return AssetManager.MommyLongLegs;
                default:
                    throw new ArgumentOutOfRangeException(nameof(jumpScare), jumpScare, null);
            }
        }

        public enum JumpScare
        {
            BunzoBunny,
            HuggyWuggy,
            KissyMissy,
            MiniHuggies,
            MommyLongLegs
        }

        public void CallDamage(Player player, Player damagedPlayer)
        {
            if (damagedPlayer == null) return;
            if (player == damagedPlayer) return;
            if (!damagedPlayer.data.view.IsMine) return;
            if (_playerScared[damagedPlayer]) return;

            _jumpScarePlayer.SetActive(true);
            _videoPlayer.Play();
            _playerScared[damagedPlayer] = true;
            PoppyPlaytimeCards.Instance.ExecuteAfterSeconds(5, () =>
            {
                _playerScared[damagedPlayer] = false;
            });
        }

        public void OnPointEnd()
        { 
            _videoPlayer.targetTexture.Release();
            _jumpScarePlayer.SetActive(false);
        }

        public void OnPickStart()
        {
            _videoPlayer.targetTexture.Release();
            _jumpScarePlayer.SetActive(false);
        }
    }
}
