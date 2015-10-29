using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace AISandbox
{
    public class ButtonManager : MonoBehaviour
    {
        public TerrainType TerrainType;
        public EntityType EntityType;
        public Color Color;

        [SerializeField]
        private Text Current;

        private void Start()
        {
            Current.fontStyle = FontStyle.Bold;
        }

        public void SetCurrentText(Text i_text)
        {
            if (Current != null)
            {
                Current.fontStyle = FontStyle.Normal;
            }
            Current = i_text;
            if (i_text != null)
            {
                Current.fontStyle = FontStyle.Bold;
            }
        }
    }

}
