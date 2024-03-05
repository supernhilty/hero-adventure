using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    Collider2D collider;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    // Start is called before the first frame update
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision); 
    }
}
