using UnityEngine;

public class MenuBagController : MonoBehaviour
  
{
    public GameObject menuBagCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuBagCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            menuBagCanvas.SetActive(!menuBagCanvas.activeSelf);
        }
    }
}
