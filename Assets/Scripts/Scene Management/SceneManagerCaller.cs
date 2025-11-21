using UnityEngine;

public class SceneManagerCaller : MonoBehaviour
{

    public void TransitionToScene(int index)
    {
        MainSceneManager.Instance.SceneTransition(index);
    }
}
