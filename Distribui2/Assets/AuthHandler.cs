using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class AuthHandler : MonoBehaviour
{

    public string apiUrl = "https://sid-restapi.onrender.com/api";
    
   [SerializeField] private TMP_InputField usernameInputField;
   [SerializeField] private TMP_InputField passwordInputField;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
    }

    IEnumerator SendRegister(string json)
    {
        UnityWebRequest www = UnityWebRequest.Put(apiUrl+"/usuarios", json);
        www.SetRequestHeader("Content-Type", "application/jason");
        
        yield return www.Send();

        if(www.result==UnityWebRequest.Result.ConnectionError) Debug.Log("Ese maestro no existe");
        else
        {/*
            if (www.responseCode == 200)
            {
                Trainer trainer = JsonUtility.FromJson<Trainer>(www.downloadHandler.text);
                
                StartCoroutine(DownloadImage(trainer.img, trainerPhoto));
                trainerNameTMP.text = trainer.name;
                
                for (int i = 0; i < trainer.deck.Length; i++)
                {
                    pokemonTeam[i].SearchPokemon(trainer.deck[i]);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                string error_message = "Status: " + www.responseCode;
                error_message += "\nContent-Type: " + www.GetResponseHeader("content-type") + "\nError:" + www.error;
                Debug.Log(error_message);
            }*/
        }
    }

}

