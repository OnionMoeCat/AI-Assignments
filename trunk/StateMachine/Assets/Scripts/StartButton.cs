using UnityEngine;
using System.Collections;
using AISandbox;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{

    private void Update()
    {
        string start = "start";
        string notReady = "not ready";
        bool ready = EntityManager.IsReady();
        GetComponent<Button>().enabled = ready;
        GetComponentInChildren<Text>().text = (ready) ? start : notReady;
    }

    public void Click()
    {
        GetComponent<Button>().enabled = false;
        GameObject.FindGameObjectWithTag("UI").SetActive(false);
        GameObject.FindGameObjectWithTag("Pathfollowing").GetComponent<Pathfollowing>();
    }
}
