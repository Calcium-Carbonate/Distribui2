using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthHandler : MonoBehaviour
{
    string apiUrl = "https://sid-restapi.onrender.com/api/";


    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    private string token;
    private string username;

    private void Start()
    {
        token = PlayerPrefs.GetString("token");

        if (string.IsNullOrEmpty(token))
        {
            Debug.Log("No hay Token almacenado");
        }
        else
        {
            username = PlayerPrefs.GetString("username");
            StartCoroutine(GetProfile(username));
        }
    }

    public void Register()
    {
        AuthData _authData = new AuthData();
        _authData.username = usernameInputField.text;
        _authData.password = passwordInputField.text;

        string json = JsonUtility.ToJson(_authData);

        StartCoroutine(SendRegister(json));
    }

    public void Login()
    {
        AuthData _authData = new AuthData();
        _authData.username = usernameInputField.text;
        _authData.password = passwordInputField.text;

        string json = JsonUtility.ToJson(_authData);

        StartCoroutine(SendLogin(json));
    }

    IEnumerator GetProfile(string username)
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl + "usuarios/" + username);
        www.SetRequestHeader("x-token", token);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR: " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.responseCode == 200)
            {
                AuthData data = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
                Debug.Log("Sesion Activada de usuario " + data.usuario.username);
                // SceneManager.LoadScene("Game");
            }
        }

    }

    //REGISTER
    IEnumerator SendRegister(string json)
    {
        UnityWebRequest www = UnityWebRequest.Put(apiUrl + "usuarios", json);
        www.SetRequestHeader("Content-Type", "application/json");
        www.method = "POST";
        yield return www.Send();

        if (www.result == UnityWebRequest.Result.ConnectionError) Debug.Log("Ese pana no existe");
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.responseCode == 200)
            {
                AuthData data = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
                Debug.Log("Usuario registrando con el ID " + data.usuario._id);
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }

    //LOGIN
    IEnumerator SendLogin(string json)
    {
        UnityWebRequest www = UnityWebRequest.Put(apiUrl + "auth/login", json);
        www.SetRequestHeader("Content-Type", "application/json");
        www.method = "POST";
        yield return www.Send();

        if (www.result == UnityWebRequest.Result.ConnectionError) Debug.Log("Ese pana no existe");
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.responseCode == 200)
            {
                AuthData data = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);

                Debug.Log("El usuario inicio sesión con éxito con el ID " + data.usuario._id);
                PlayerPrefs.SetString("token", data.token);
                PlayerPrefs.SetString("username", data.username);
                Debug.Log("Tu puntaje es  " + data.usuario.data.score);
            }
            else
            {
                Debug.Log(www.error);
            }
        }

        IEnumerator EditScoreData(string json)
        {
            UnityWebRequest www = UnityWebRequest.Put(apiUrl + "usuarios/" + username, json);
            www.SetRequestHeader("x-token", token);
            www.SetRequestHeader("Content-Type", "application/json");
            www.method = "PATCH";
            yield return www.Send();

        }

        IEnumerator ScoreChart(string json)
        {
            UnityWebRequest www = UnityWebRequest.Get(apiUrl + "usuarios/");
            www.SetRequestHeader("x-token", token);
            www.method = "PATCH";
            yield return www.Send();
            if (www.result == UnityWebRequest.Result.ConnectionError) Debug.Log("Ese pana no existe");
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.responseCode == 200)
                {
                    UserList jsonData = JsonUtility.FromJson<UserList>(www.downloadHandler.text);

                    List<User> userList = jsonData.usuarios;
                    List<User> usuariosOrdenados = userList.OrderByDescending(x => x.data.score).ToList<User>();

                    int index = 0;

                    foreach (User user in usuariosOrdenados)
                    {
                        if (index <= 4)
                        {
                            string userScore = index + 1 + " - " + user.username + " Score: " + user.data.score;
                        }
                    }

                }
                else
                {
                    Debug.Log(www.error);
                }
            }
        }

    }
}

[System.Serializable]
public class AuthData
{
    public string username;
    public string password;
    
    public User usuario;
    public string token;

    //public User[] usuarios;
}
[System.Serializable]
public class User
{
    public string _id;
    public string username;
    public bool estado;
    public Data data;
}

[System.Serializable]
public class Data
{
    public int score;
    //public User[] friends;
}

[System.Serializable]
public class UserList
{
    public List<User> usuarios;
}  

