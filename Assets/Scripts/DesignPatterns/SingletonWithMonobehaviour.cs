using UnityEngine;

public class SingletonWithMonobehaviour<T> : MonoBehaviour where T: SingletonWithMonobehaviour<T>
{
    public static T instance { get; protected set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
    }
}
