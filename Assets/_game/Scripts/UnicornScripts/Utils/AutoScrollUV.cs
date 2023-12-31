using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn.Utilities
{
	/// <summary>
	/// Thay đổi UV của material theo thời gian
	/// </summary>
	public class AutoScrollUV : MonoBehaviour
	{
		public int targetMaterialSlot;
		public string propertyName = "_MainTex";
		public Vector2 scrollSpeed;

		private Renderer rend;
		private Vector2 offset = Vector2.zero;

		void Start()
		{
			rend = GetComponent<Renderer>();
			offset = rend.materials[targetMaterialSlot].GetTextureOffset(propertyName);
		}

		void Update()
		{
			offset.x += Time.deltaTime * scrollSpeed.x;
			offset.y += Time.deltaTime * scrollSpeed.y;

			rend.materials[targetMaterialSlot].SetTextureOffset(propertyName, offset);
		}
	}
}