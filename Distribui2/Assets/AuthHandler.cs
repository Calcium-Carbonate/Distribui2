using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class AuthHandler : MonoBehaviour
{ 
    string apiUrl = "https://sid-restapi.onrender.com/api/";
    
    
   [SerializeField] private TMP_InputField usernameInputField;
   [SerializeField] private TMP_InputField passwordInputField;
   
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
    
    //REGISTER
    IEnumerator SendRegister(string json)
    {
        UnityWebRequest www = UnityWebRequest.Put(apiUrl+"usuarios", json);
        www.SetRequestHeader("Content-Type", "application/json");
        www.method = "POST";
        yield return www.Send();

        if(www.result==UnityWebRequest.Result.ConnectionError) Debug.Log("Ese pana no existe");
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.responseCode == 200)
            {
                AuthData data = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
                Debug.Log("Usuario registrando con el ID "+data.usuario._id);
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
        UnityWebRequest www = UnityWebRequest.Put(apiUrl+"auth/login", json);
        www.SetRequestHeader("Content-Type", "application/json");
        www.method = "POST";
        yield return www.Send();

        if(www.result==UnityWebRequest.Result.ConnectionError) Debug.Log("Ese pana no existe");
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.responseCode == 200)
            {
                AuthData data = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
                Debug.Log("El usuario inicio sesión con éxito con el ID "+data.usuario._id);
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }

}
[System.Serializable]
public class AuthData
{
    public string username;
    public string password;
    public UserData usuario;
    public string token;
}
[System.Serializable]
public class UserData
{
    public string _id;
    public string username;
    public bool estado;
}
