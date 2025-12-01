using UnityEngine;

public class NextButton : MonoBehaviour
{
    public void nextDay()
    {
        MainSceneManager.Instance.SceneTransition(1);   
    }
}
