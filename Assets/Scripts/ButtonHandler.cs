using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PerformAction();
        }
    }

    private void PerformAction()
    {
        switch (gameObject.name)
        {
            case "StartButton":
                SceneController.Instance.LoadScene("123"); 
                break;
            case "QuitButton":
                SceneController.Instance.QuitGame();
                break;
            case "MenuButton":
                SceneController.Instance.LoadScene("MainMenu");
                break;
        }
    }
}
