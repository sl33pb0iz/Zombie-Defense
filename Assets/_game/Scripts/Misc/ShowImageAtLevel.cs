using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unicorn;

public class ShowImageAtLevel : MonoBehaviour
{
    [SerializeField] private int targetSceneIndex = 3; // Số index của scene bạn muốn kiểm tra

    void Start()
    {
        int currentSceneIndex = SceneManager.GetSceneAt(1).buildIndex;

        if (currentSceneIndex > targetSceneIndex)
        {
            // Hiển thị gameobject nếu scene hiện tại có chỉ số lớn hơn targetSceneIndex
            this.gameObject.SetActive(true);
        }
        else
        {
            // Không hiển thị gameobject nếu scene hiện tại có chỉ số nhỏ hơn hoặc bằng targetSceneIndex
            this.gameObject.SetActive(false);
        }
    }
}
