using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Xml;
using System.Net;

public class WebDataFetcher : MonoBehaviour
{
    public string url;
    public Text listItemPrefab;
    public Transform contentTransform;

    private async void Start()
    {
        string xmlData = await FetchXmlDataFromUrl(url);
        List<string> parsedData = ParseXmlData(xmlData);
        PopulateList(parsedData);
        ScaleListToScreen();
    }
    
    private async Task<string> FetchXmlDataFromUrl(string url)
    {
        using (WebClient client = new WebClient())
        {
            return await client.DownloadStringTaskAsync(url);
        }
    }

    private List<string> ParseXmlData(string xmlData)
    {
        List<string> parsedData = new List<string>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData);

        XmlNodeList nodeList = xmlDoc.GetElementsByTagName("item");

        foreach (XmlNode node in nodeList)
        {
            string data = node.InnerText;
            parsedData.Add(data);
        }

        return parsedData;
    }
    
    private void PopulateList(List<string> data)
    {
        foreach (string itemData in data)
        {
            Text listItem = Instantiate(listItemPrefab, contentTransform);
            listItem.text = itemData;
        }
    }
    
    private void ScaleListToScreen()
    {
        RectTransform rectTransform = contentTransform.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
    }
}