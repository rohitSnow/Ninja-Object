using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int life = 5;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter()
    {
        life -= 1;
    }
}
