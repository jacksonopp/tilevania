using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : SceneLoader
{
    [SerializeField] Animator transition;

    [Range(.1f, 2f)] [SerializeField] float sceneDelay = 1f;

    public IEnumerator LoadNextSceneAfterAnim()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(sceneDelay);
        LoadNextScene();
    }

}
