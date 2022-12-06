using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    public UnityEvent EnterEvent;
    public UnityEvent ExitEvent;
    public UnityEvent StayEvent;
    [SerializeField] string tagCheck;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagCheck)
        {
            EnterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == tagCheck)
        {
            ExitEvent.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == tagCheck)
        {
            StayEvent.Invoke();
        }
    }

}
