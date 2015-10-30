using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AISandbox
{
    [RequireComponent(typeof(Image))]
    public class TerrainChangeButton : MonoBehaviour
    {
        [SerializeField]
        private Text Text;
        [SerializeField]
        private Color[] Colors;

        private bool m_active;
        private int m_index = 0;
        private Image m_image;

        [SerializeField]
        private EntityType EntityType;

        [SerializeField]
        private ButtonManager ButtonManager;
        public void Awake()
        {
            m_image = GetComponent<Image>();
        }

        public void Click()
        {
            ButtonManager.SetCurrentText(Text);       
            m_index = (m_index + 1) % Colors.Length;
            m_image.color = Colors[m_index];

            TerrainType terrainType = TerrainType.Plain + m_index;
            ButtonManager.TerrainType = terrainType;
            Text.text = Enum.GetName(typeof(TerrainType), terrainType);
            ButtonManager.EntityType = EntityType;           
        }
    }
}
