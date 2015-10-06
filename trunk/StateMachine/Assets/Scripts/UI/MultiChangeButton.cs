using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AISandbox
{
    public class MultiChangeButton : MonoBehaviour
    {
        public Text Text;
        public Color[] Colors;
        private bool m_active;
        private int m_index = 0;
        private Image m_image;

        public EntityType EntityType;

        public bool Terrain;

        public ButtonManager ButtonManager;
        public void Awake()
        {
            m_image = GetComponent<Image>();
        }

        public void Click()
        {
            ButtonManager.ResetFont(this);
            m_index = (m_index + 1)%Colors.Length;
            m_image.color = Colors[m_index];

            if (Terrain)
            {
                TerrainType terrainType = TerrainType.Plain + m_index;
                ButtonManager.terrainType = terrainType;
                Text.text = Enum.GetName(typeof(TerrainType), terrainType);
                ButtonManager.entity.EntityType = EntityType;
            }
            else
            {
                Text.text = Enum.GetName(typeof(EntityType), EntityType);
                ButtonManager.entity.EntityType = EntityType;
                ButtonManager.entity.Color = Colors[m_index];
            }
        }
    }
}
