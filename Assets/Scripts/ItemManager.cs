using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private int minActiveItems;
    [SerializeField] private int maxActiveItems;
    [SerializeField] private List<GameObject> possibleItemSpawns;
    [SerializeField] private List<GameObject> activeItems;
    private bool activeShuffle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        activeShuffle = false;
        SpawnItems();
    }

    // Update is called once per frame
    void Update()
    {
        if(activeItems.Count < minActiveItems && GameManager.Instance.GetAmmo() < GameManager.Instance.GetMaxAmmoCount() && !activeShuffle)
        {
            SpawnItems();
        }
        if(GameManager.Instance.GetAmmo() >= GameManager.Instance.GetMaxAmmoCount())
        {
            RemoveAllAactive();
        }
    }

    private void ShuffleSpawns()
    {
        activeShuffle = true;
        int n = possibleItemSpawns.Count - 1;
        // Knuth Shuffle
        while (n > 0)
        {
            GameObject temp = possibleItemSpawns[n];
            // Use n + 1 to account for exclusive max
            int target = Random.Range(0, n + 1);
            possibleItemSpawns[n] = possibleItemSpawns[target];
            possibleItemSpawns[target] = temp;
            n--;
        }
        activeShuffle = false;
    }

    private void SpawnItems()
    {
        int i = 0;
        ShuffleSpawns();
        while (i < possibleItemSpawns.Count && activeItems.Count < maxActiveItems)
        {
            if(possibleItemSpawns[i].GetComponentInChildren<Interactable>().IsSpawnPrevented())
            {
                i++;
            }
            else
            {
                AddToActive(i);
            }
        }
    }

    private void AddToActive(int i)
    {
        GameObject item = possibleItemSpawns[i];
        item.GetComponentInChildren<Interactable>().ActivateItem();
        possibleItemSpawns.RemoveAt(i);
        activeItems.Add(item);
        
    }

    public void RemoveActive(GameObject item)
    {
        activeItems.Remove(item);
        possibleItemSpawns.Add(item);
    }

    private void RemoveAllAactive()
    {
        while(activeItems.Count > 0)
        {
            activeItems[0].GetComponentInChildren<Interactable>().DeactivateItem();
        }
    }
}
