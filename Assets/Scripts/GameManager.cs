using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Stats")]
    [SerializeField] private int ammo;
    [SerializeField] private int maxAmmoCount;
    [SerializeField] private int ammoOnPickup;
    [SerializeField] private bool justShot;

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
        Cursor.lockState = CursorLockMode.Locked;
        justShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Ammo Management

    public void AddAmmo()
    {
        ammo += ammoOnPickup;
        ammo = Mathf.Clamp(ammo, 0, maxAmmoCount);
        UIManager.Instance.UpdateAmmoCount();
    }

    public void ConsumeAmmo()
    {
        ammo--;
        UIManager.Instance.UpdateAmmoCount();
    }

    public int GetAmmo()
    {
        return ammo;
    }

    public int GetMaxAmmoCount()
    {
        return maxAmmoCount;
    }
    #endregion

    public bool GetJustShot()
    {
        return justShot;
    }

    public void SetJustShot(bool state)
    {
        justShot = state;
    }
}
