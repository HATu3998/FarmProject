using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FireBaseLoginManager : MonoBehaviour
{
    //dang ky
    [Header("Register")]
    public InputField ipRegisterEmail;
    public InputField ipRegisterPassword;

    public Button buttonRegister;
    //dang nhap
    [Header("Login")]
    public InputField ipLoginEmail;
    public InputField ipLoginPassword;

    public Button buttonLogin;
    private FirebaseAuth auth;

    //chuyen doi qua lai giua dang ky dang nhap

    [Header("Switch form")]
    public Button buttonMoveToSignIn;
    public Button buttonMoveToRegister;
    public GameObject loginForm;
    public GameObject registerForm;

    //upload data firebase to 
    private FireBaseDatabaseManager databaseManager;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase s?n s?ng -> l?y instance Auth
                auth = FirebaseAuth.DefaultInstance;

                // ??ng k? n?t sau khi ?? c? auth (ho?c b?n c? th? g?n tr??c, nh?ng an to?n h?n ? ??y)
                if (buttonRegister != null)
                    buttonRegister.onClick.AddListener(RegisterAccountWithFireBase);
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });

        buttonLogin.onClick.AddListener(signInAccountWithFirebase);
        buttonMoveToRegister.onClick.AddListener(SwitchForm);
        buttonMoveToSignIn.onClick.AddListener(SwitchForm);
        databaseManager = GetComponent<FireBaseDatabaseManager>();
    }
    public void RegisterAccountWithFireBase()
    {
        string email = ipRegisterEmail.text;
        string password = ipRegisterPassword.text;
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(  task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("dang ky bi huy");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogWarning("??ng k? th?t b?i: " + task.Exception);
                // in t?ng inner exception ?? bi?t m? l?i Firebase c? th?
                foreach (var inner in task.Exception.Flatten().InnerExceptions)
                {
                    Debug.LogError("Inner exception: " + inner.GetType() + " | " + inner.Message);
                    var firebaseEx = inner as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        // chuy?n code th?nh AuthError n?u l? FirebaseAuth l?i
                        var authErr = (Firebase.Auth.AuthError)firebaseEx.ErrorCode;
                        Debug.LogError("Firebase AuthError: " + authErr);
                    }
                }
                return;
            }
            if (task.IsCompleted)
            {
                Map mapInGame = new Map();
                User userInGame = new User("." ,100,50,mapInGame);
                Debug.Log("dang ky thanh cong");
                FirebaseUser firebaseUser = task.Result.User;
                databaseManager.writeDatabase("Users/" + firebaseUser.UserId, userInGame.ToString());
                SceneManager.LoadScene("LoginScene");
                 
            }
        });
    }
    public void signInAccountWithFirebase()
    {
        string email = ipLoginEmail.text;
        string password = ipLoginPassword.text;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("dang ky bi huy");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogWarning("??ng k? th?t b?i: " + task.Exception);
                // in t?ng inner exception ?? bi?t m? l?i Firebase c? th?
                foreach (var inner in task.Exception.Flatten().InnerExceptions)
                {
                    Debug.LogError("Inner exception: " + inner.GetType() + " | " + inner.Message);
                    var firebaseEx = inner as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        // chuy?n code th?nh AuthError n?u l? FirebaseAuth l?i
                        var authErr = (Firebase.Auth.AuthError)firebaseEx.ErrorCode;
                        Debug.LogError("Firebase AuthError: " + authErr);
                    }
                }
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("dang nhap thanh cong");
                FirebaseUser user = task.Result.User;
                SceneManager.LoadScene("SampleScene");
            }


        });
    }
    public void SwitchForm()
    {
        loginForm.SetActive(!loginForm.activeSelf);
        registerForm.SetActive(!registerForm.activeSelf);
    }
}
