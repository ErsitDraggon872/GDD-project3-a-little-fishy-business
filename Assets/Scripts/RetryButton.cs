using UnityEngine;

public class RetryButton : MonoBehaviour
{
    public void retryGame()
    {
        MainSceneManager.Instance.SceneTransition(0);
    }
}
