using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject item;
    private Collider _collider;
    private bool preventSpawn;

    private void Awake()
    {
        preventSpawn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponentInChildren<SphereCollider>();
        item.SetActive(false);
        _collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InteractAction()
    {
        GameManager.Instance.AddAmmo();
        DeactivateItem();
    }

    public void ActivateItem()
    {
        item.SetActive(true);
        _collider.enabled = true;
    }

    public void DeactivateItem()
    {
        item.SetActive(false);
        _collider.enabled = false;
        ItemManager.Instance.RemoveActive(this.gameObject);
    }

    public bool IsSpawnPrevented()
    {
        return preventSpawn;
    }

    public void UpdateSpawnPrevented(bool state)
    {
        preventSpawn = state;
    }
}
