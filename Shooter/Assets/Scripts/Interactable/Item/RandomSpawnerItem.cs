using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shooter.Interactable.Item
{
    public class RandomSpawnerItem : MonoBehaviour
    {
        [SerializeField] private PhysicalItem spawnItem;
        [SerializeField] [Min(1)] private int amountOfItems;
        [SerializeField] private Transform[] spawnPoints;
        private int selectedSpawnerIndex;
        private int currentAmountOfItems;
        private bool[] isBusySlot;

        private void Awake()
        {
            isBusySlot = new bool[spawnPoints.Length];
        }

        private IEnumerator Start()
        {
            if (amountOfItems > spawnPoints.Length)
            {
                Debug.LogError("amountOfItems > spawnPoints.Length");
                yield break;
            }
                
            while (currentAmountOfItems < amountOfItems)
            {
                if (Random.value > 0.5 && !isBusySlot[selectedSpawnerIndex])
                {
                    var item = Instantiate(spawnItem, spawnPoints[selectedSpawnerIndex].position,
                        Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                    item.transform.SetParent(spawnPoints[selectedSpawnerIndex]);
                    currentAmountOfItems++;
                    isBusySlot[selectedSpawnerIndex] = true;
                    yield return new WaitForSeconds(0.05f);
                }
                
                ChangeSelectedSpawnPoint();
            }
        }
        
        private void ChangeSelectedSpawnPoint()
        {
            if (selectedSpawnerIndex >= spawnPoints.Length - 1)
                selectedSpawnerIndex = 0;
            else
                selectedSpawnerIndex++;
        }
    }
}
