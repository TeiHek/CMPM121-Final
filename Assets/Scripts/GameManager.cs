using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Stats")]
    [SerializeField] private int ammo;
    [SerializeField] private int maxAmmoCount;
    [SerializeField] private int ammoOnPickup;

    [Header("Game State")]
    [SerializeField] private bool alive;

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
    }

    public void ConsumeAmmo()
    {
        ammo--;
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
}
