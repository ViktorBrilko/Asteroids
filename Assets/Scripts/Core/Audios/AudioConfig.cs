using UnityEngine;

namespace Core.Audios
{
    [CreateAssetMenu(menuName = "AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        public AudioClip LaserShoot;
        public AudioClip BulletShoot;
        public AudioClip Explosion;
        public AudioClip UiClick;
        public AudioClip Collision;
        public AudioClip PlayerDeath;
        public AudioClip Music;
    }
}