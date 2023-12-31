using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spicyy.AI
{
    public class DropItemModule : MonoBehaviour
    {
        [Title("GOLD", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private GameObject goldDrop;
        [SerializeField] private int amountGold;

        [Title("COIN", titleAlignment: TitleAlignments.Centered)]
        [Range(0, 100)] public int dropRate;
        [SerializeField] private GameObject coinDrop;
        [SerializeField] private int amountCoin;

        [Title("RESOURCE", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private GameObject weaponResource;
        [SerializeField] private GameObject backpackResource;
        [SerializeField] private GameObject helmetResource;
        [SerializeField] private GameObject armorResource;
        [SerializeField] private GameObject bootResource;


        public void DropItems()
        {
            

            if (amountGold > 0)
            {
                for (int i = 0; i <= amountGold; i++)
                {
                    GameObject gold = PoolManager.Instance.ReuseObject(goldDrop, transform.position + new Vector3(0, 8, 0), Quaternion.identity);
                    gold.SetActive(true);
                }
            }

            if (amountCoin > 0)
            {
                int dropMoneyRate = Random.Range(1, 100);
                if (dropRate > dropMoneyRate)
                {
                    for (int i = 0; i <= amountCoin; i++)
                    {
                        GameObject money = PoolManager.Instance.ReuseObject(coinDrop, transform.position, Quaternion.identity);
                        money.SetActive(true);
                    }
                }
            }

            int randomIndex = Random.Range(0, 6); // Lấy một số ngẫu nhiên từ 0 đến 4

            GameObject resourceToDrop = null;

            // Dựa vào chỉ số ngẫu nhiên, chọn loại resource tương ứng
            switch (randomIndex)
            {
                case 0:
                    resourceToDrop = weaponResource;
                    break;
                case 1:
                    resourceToDrop = backpackResource;
                    break;
                case 2:
                    resourceToDrop = helmetResource;
                    break;
                case 3:
                    resourceToDrop = armorResource;
                    break;
                case 4:
                    resourceToDrop = bootResource;
                    break;
                default:
                    // Nếu chỉ số ngẫu nhiên nằm ngoài phạm vi từ 0 đến 4, không làm gì cả
                    break;
            }

            if (resourceToDrop != null)
            {
                GameObject droppedResource = PoolManager.Instance.ReuseObject(resourceToDrop, transform.position + new Vector3(0, 8, 0), Quaternion.identity);
                droppedResource.SetActive(true);
            }
        }
    }
}

