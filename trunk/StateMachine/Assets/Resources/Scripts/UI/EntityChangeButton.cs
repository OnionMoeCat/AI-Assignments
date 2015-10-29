using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AISandbox
{
    [RequireComponent(typeof(Image))]
    public class EntityChangeButton : MonoBehaviour
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
            m_index = (m_index + 1)%Colors.Length;
            m_image.color = Colors[m_index];

            Text.text = Enum.GetName(typeof(EntityType), EntityType);
            ButtonManager.EntityType = EntityType;
            ButtonManager.Color = Colors[m_index];
        }

        public bool SetFont(FontStyle i_fontStyle)
        {
            if (Text != null)
            {
                Text.fontStyle = i_fontStyle;
            }
            return false;
        }
    }
}
