using UnityEngine;
using System.Collections;
using AISandbox;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    [SerializeField]
    private Text Text;
    private void Update()
    {
        string start = "start";
        string notReady = "not ready";
        bool ready = EntityManager.IsReady();
        GetComponent<Button>().enabled = ready;
        Text.text = (ready) ? start : notReady;
    }
}
