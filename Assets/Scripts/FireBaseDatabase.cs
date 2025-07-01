using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;

public class EmailAuthManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;


    private FirebaseAuth auth;
    private FirebaseUser user;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    // 📝 Called on Register Button Click
    public void Register()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("❗ Email or password is empty.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("❌ Registration failed: " + task.Exception);
                return;
            }

            user = task.Result.User; // ✅ FIXED LINE
            Debug.Log("✅ User registered: " + user.Email);

            // Send verification email
            user.SendEmailVerificationAsync().ContinueWithOnMainThread(emailTask =>
            {
                if (emailTask.IsCompletedSuccessfully)
                    Debug.Log("📨 Verification email sent to " + user.Email);
                else
                    Debug.LogError("❌ Failed to send verification email: " + emailTask.Exception);
            });
        });
    }

    // ✅ Called on Login Button Click
    public void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("❗ Email or password is empty.");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("❌ Login failed: " + task.Exception);
                return;
            }

            user = task.Result.User; // ✅ FIXED LINE

            if (user.IsEmailVerified)
            {
                Debug.Log("✅ Login successful! Email is verified.");
                // Proceed to game or main menu here
                SceneManager.LoadScene("Game");
            }
            else
            {
                Debug.LogWarning("⚠️ Email not verified. Please check your inbox.");
                // Optional: Show resend button
            }
        });
    }

    // 🔁 Optional: Resend email verification
    // public void ResendVerificationEmail()
    // {
    //     if (auth.CurrentUser != null)
    //     {
    //         auth.CurrentUser.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
    //         {
    //             if (task.IsCompletedSuccessfully)
    //                 Debug.Log("📨 Verification email re-sent.");
    //             else
    //                 Debug.LogError("❌ Failed to resend: " + task.Exception);
    //         });
    //     }
    // }
}
