using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;

[Serializable]
public class OpenAiAPI : MonoBehaviour
{
    public ObjectsOfGame[] gameObjects;
    //void Start()
    //{
    //    Debug.Log("Testas ar veikia OpenAi");
    //    GetData();
    //}
    public void GetData() => StartCoroutine(GetData_Coroutine());
    IEnumerator GetData_Coroutine()
    {
        string uri = "https://localhost:7142/api/games/1/gameObjects/2";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string result = fixJson(request.downloadHandler.text);
                gameObjects = JsonHelper.FromJson<ObjectsOfGame>(result);
                Debug.Log(gameObjects[0].text);
            }
        }
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }
        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }
        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }
        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}








