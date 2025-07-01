using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseInitializer : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("✅ Firebase is ready!");
                // You can now safely use Firebase Auth, Firestore, etc.
            }
            else
            {
                Debug.LogError("❌ Could not resolve Firebase dependencies: " + task.Result);
            }
        });
    }
}
