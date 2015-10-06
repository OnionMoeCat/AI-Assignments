using UnityEngine;
using System.Collections;

namespace AISandbox
{
    public class ButtonManager : MonoBehaviour
    {
        public TerrainType terrainType;
        public Entity entity;

        public MultiChangeButton Current;
        private void Awake()
        {
            entity = new Entity();
        }

        private void Start()
        {
            Current.Text.fontStyle = FontStyle.Bold;
        }

        public void ResetFont(MultiChangeButton i_button)
        {
            Current.Text.fontStyle = FontStyle.Normal;
            Current = i_button;
            Current.Text.fontStyle = FontStyle.Bold;
        }
    }

}
