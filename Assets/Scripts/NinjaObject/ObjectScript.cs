using System;
using TMPro;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    private Rigidbody rb;
    private float range = 2.5f;
    public ParticleSystem explosionEffect;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    //     rb.AddForce(Vector3.up * RandomForce(), ForceMode.Impulse);
    //     rb.AddTorque(RandomTorque(), ForceMode.Impulse);
    //     transform.position = RandomSpawnPos();
    // }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = RandomSpawnPos(); // ✅ Set position first
        rb.AddForce(Vector3.up * RandomForce(), ForceMode.Impulse);
        rb.AddTorque(RandomTorque(), ForceMode.Impulse);
    }


    // Update is called once per frame
    void Update()
    {

    }

    float RandomForce()
    {
        return UnityEngine.Random.Range(12, 16);
    }
    Vector3 RandomSpawnPos()
    {
        return new Vector3(UnityEngine.Random.Range(-range, range), -2);
    }
    Vector3 RandomTorque()
    {
        return new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Destroy(gameObject);
        }
    }
}
