using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagingScript : MonoBehaviour
{
    public AudioClip destroySound;
    private AudioSource audioSource;
    public List<GameObject> objects;
    private InputSystem_Actions inputActions;
    private float spawnRate = 1.0f;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI gameoverText;
    public GameObject Sensor;
    private GameOverScript gs;
    public Button restart;
    private int score = 10;
    private bool isGameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        StartCoroutine(SpawnTarget());
        gs = Sensor.GetComponent<GameOverScript>();

        gameoverText.enabled = false;
        restart.gameObject.SetActive(false);
        lifeText.text = "Life:" + gs.life;
    }
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        inputActions.BallInteraction.Enable();
        inputActions.BallInteraction.Smash.performed += OnSmash;
    }
    void OnDisable()
    {
        inputActions.BallInteraction.Disable();
        inputActions.BallInteraction.Smash.performed -= OnSmash;

    }
    void OnSmash(InputAction.CallbackContext context)
    {

        // Vector2 position;

        // // Handle mouse click
        // if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        // {
        //     position = Mouse.current.position.ReadValue();
        // }
        // // Handle touch tap
        // else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        // {
        //     position = Touchscreen.current.primaryTouch.position.ReadValue();
        // }
        // else
        // {
        //     return; // No valid input
        // }

        // Ray ray = Camera.main.ScreenPointToRay(position);
        // RaycastHit hit;

        // if (Physics.Raycast(ray, out hit))
        // {
        //     GameObject hitObject = hit.collider.gameObject;

        //     if (hitObject.CompareTag("Objects")) // Make sure tag is set properly
        //     {
        //         Debug.Log("Smashed: " + hitObject.name);
        //         Destroy(hitObject);
        //     }

        // }

        Debug.Log("Smash triggered");

        if (Touchscreen.current == null)
        {
            Debug.LogWarning("Touchscreen.current is null");
            return;
        }

        // Get touch position directly
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Debug.Log("Touch position: " + touchPosition);

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Hit object: " + hitObject.name);

            if (hitObject.CompareTag("Objects"))
            {
                Debug.Log("Touched and destroying: " + hitObject.name);

                // Get the ObjectScript and its assigned particle system
                PlayEffect(hitObject);
                PlaySound();

                // Destroy the hit object

                DestroyGameObject(hitObject);
            }
            else
            {
                Debug.Log("Hit object doesn't have tag 'Objects'");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything");
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        lifeText.text = "Life: " + gs.life;

        if (gs.life == 0)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        isGameOver = true;
        gameoverText.enabled = true;
        restart.gameObject.SetActive(true);
        inputActions.BallInteraction.Disable(); // disable input instead of freezing time
                                                // NO Time.timeScale = 0f
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }





    IEnumerator SpawnTarget()
    {
        yield return new WaitForSecondsRealtime(0.1f); // Short delay to ensure physics system is ready
        while (!isGameOver)
        {
            yield return new WaitForSecondsRealtime(spawnRate);
            int index = UnityEngine.Random.Range(0, objects.Count);
            GameObject obj = Instantiate(objects[index]);
            Debug.Log("Spawning and applying physics to: " + obj.name);
        }
    }


    void DestroyGameObject(GameObject hitObject)
    {
        Destroy(hitObject);
        ScoreIncrement();
    }
    void ScoreIncrement()
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    void PlayEffect(GameObject hitObject)
    {
        ObjectScript objectScript = hitObject.GetComponent<ObjectScript>();

        if (objectScript != null && objectScript.explosionEffect != null)
        {
            // Instantiate particle system at the object's position
            ParticleSystem ps = Instantiate(
                objectScript.explosionEffect,
                hitObject.transform.position,
                Quaternion.identity
            );

            // Destroy the particle system after it finishes
            Destroy(ps.gameObject, ps.main.duration + 0.5f);
        }
    }

    void PlaySound()
    {
        if (destroySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(destroySound);
        }
    }

}
