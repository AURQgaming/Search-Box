using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SearchManager : MonoBehaviour
{
    

    public TextAsset jsonFile;

    public List<string> namelist;
    public List<string> codelist;


    public TMP_InputField SearchWord;

    public string m_KeyWord;

    public GameObject Element;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        datalist dataInJson = JsonUtility.FromJson<datalist>(jsonFile.text);

        foreach (DataList m_data in dataInJson.data)
        {
            namelist.Add(m_data.name);
            codelist.Add(m_data.code);
            Debug.Log("Found data: " + m_data.name + " " + m_data.code);
        }
    }

    public void LoadScene()
    {
        m_KeyWord = SearchWord.textComponent.text;
        if (!string.IsNullOrWhiteSpace(m_KeyWord))
        {
            StartCoroutine(LoadYourAsyncScene());
        }
        

    }

    IEnumerator LoadYourAsyncScene()
    {
       
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Output_Scene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        GameObject Container = GameObject.Find("Scroll View/Viewport/Content");
        datalist dataInJson = JsonUtility.FromJson<datalist>(jsonFile.text);

        int i = 0;

        foreach (DataList m_data in dataInJson.data)
        {
            GameObject go = Instantiate(Element, Container.transform);
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i++.ToString();
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_data.name;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = m_data.code;
            Debug.Log("Found data: " + m_data.name + " " + m_data.code);
        }

        foreach (Transform m_go in Container.transform)
        {
            if(m_go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text == m_KeyWord)
            {
                m_go.GetComponent<Image>().color = Color.green;
            }
            
        }
    }
}

[System.Serializable]
public class datalist
{
    
    public DataList[] data;
}

[System.Serializable]
public class DataList
{
    public string name;
    public string code;
}
