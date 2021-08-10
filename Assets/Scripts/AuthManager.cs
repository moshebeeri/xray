using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using System;
using Debug  = UnityEngine.Debug;

public class AuthManager : MonoBehaviour
{
    public FirebaseUser User;
    private DependencyStatus dependencyStatus;
    private FirebaseAuth auth;

    public bool UIEnabled { get; private set; }

    public virtual void Start() {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available) {
        InitializeFirebase();
        } else {
            Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Handle initialization of the necessary firebase modules:
    protected void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        // auth.StateChanged += AuthStateChanged;
        // auth.IdTokenChanged += IdTokenChanged;
        DebugLog("Setting up Firebase Auth");
    }
    public Task SigninAnonymouslyAsync() {
        DebugLog("Attempting to sign anonymously...");
        return auth.SignInAnonymouslyAsync().ContinueWith(HandleSignInWithUser);
    }
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task) {
        EnableUI();
        DebugLog(String.Format("{0} signed in", task.Result.DisplayName));
    }

    public void DebugLog(string s)
    {
        Debug.Log(s);
    }
    public void TestMe()
    {
        Debug.Log("TestMe");
    }
    void DisableUI() {
        UIEnabled = false;
    }

    void EnableUI() {
        UIEnabled = true;
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            Debug.LogError(message);
            // warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            // confirmLoginText.text = "Logged In";
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            Debug.Log("Missing Username");
        }
        // else if(passwordRegisterField.text != passwordRegisterVerifyField.text)
        // {
        //     //If the password does not match show a warning
        //     warningRegisterText.text = "Password Does Not Match!";
        // }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                Debug.LogError(message);
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        Debug.LogError("Username Set Failed!");
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        // UIManager.instance.LoginScreen();
                        // warningRegisterText.text = "";
                    }
                }
            }
        }
    }
}
