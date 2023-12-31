using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class ScrollingBackground : MonoBehaviour
    {

        public float scrollSpeed = 0.5f;
        public Renderer m_renderer;

        private void Update()
        {
            float offset = Time.time * scrollSpeed;
            m_renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }

    }
}