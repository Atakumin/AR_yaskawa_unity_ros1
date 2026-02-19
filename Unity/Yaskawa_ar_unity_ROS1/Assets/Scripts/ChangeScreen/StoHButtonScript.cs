using UnityEngine;
using UnityEngine.SceneManagement;
 
public class StoHButtonScript : MonoBehaviour {
 
    public void StoHButton()
    {
        SceneManager.LoadScene("HomeScene");
    }
}