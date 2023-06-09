using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    private void Start() => StartCoroutine(WaitForInitCoroutine());

    private IEnumerator WaitForInitCoroutine()
    {
        GameState.Instance.RequestState(GameState.Type.Menu);
        
        // TODO: Wait until everything is initialized
        yield return null;
        SceneManager.LoadScene(1);
    }
}
