using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text ammoText;

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
        UIManager.Instance.UpdateAmmoCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmoCount()
    {
        ammoText.text = "Ammo: " + GameManager.Instance.GetAmmo().ToString();
    }
}
