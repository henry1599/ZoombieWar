using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance {get; private set;}
    public float GameTime {get; private set;} = 0;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerPrefs.DeleteKey("GameTime");
        this.GameTime = 0;
    }
    void Update()
    {
        this.GameTime += Time.deltaTime;
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetFloat("GameTime", this.GameTime);
        }
        else
        {
            this.GameTime = PlayerPrefs.GetFloat("GameTime", 0);
        }
    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            PlayerPrefs.SetFloat("GameTime", this.GameTime);
        }
        else
        {
            this.GameTime = PlayerPrefs.GetFloat("GameTime", 0);
        }
    }
}