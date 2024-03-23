using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3 (0, 180, 0);
    AudioSource pickupScore;
    // Start is called before the first frame update
    private void Awake()
    {
        pickupScore = GetComponent<AudioSource>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        damageable.Heal(healthRestore);
            if (pickupScore)
            {
                AudioSource.PlayClipAtPoint(pickupScore.clip, gameObject.transform.position, pickupScore.volume);
            }
        if(collision.gameObject.tag == "Player")
        {   
            Destroy(gameObject);
        }

        //if (damageable)
        //{
        //    bool wasHealed = damageable.Heal(healthRestore);

        //    if(wasHealed)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
}
