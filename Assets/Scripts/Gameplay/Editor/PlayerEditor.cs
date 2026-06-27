using Gameplay.Players;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Editor
{
    [CustomEditor(typeof(PlayerDeathHandler))]
    public class PlayerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var player = (PlayerDeathHandler)target;

            if (GUILayout.Button("Kill player")) player.Die();
            
            if (GUILayout.Button("+100 HP")) player.HealthComponent.Heal(100);
        }
    }
}