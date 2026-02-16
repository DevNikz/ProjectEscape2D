using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    Button btn;
    private GameObject Lock;
    void Awake()
    {
        btn = GetComponent<Button>();
        Lock = transform.Find("Lock").gameObject;
        Lock.SetActive(false);
    }

    public void DisableButton()
    {
        btn.interactable = false;
        Lock.SetActive(true);
    }
}
