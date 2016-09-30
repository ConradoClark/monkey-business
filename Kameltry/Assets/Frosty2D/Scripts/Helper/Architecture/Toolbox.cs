using UnityEngine;
using System.Collections;

[AddComponentMenu("Frosty2D/Architecture/Toolbox Behaviour")]
[DisallowMultipleComponent]
public class Toolbox : Singleton<Toolbox>
{
    public FrostyTime time;
    public FrostyPoolManager pool;
    public FrostyRandom random;
    public LevelManager levelManager;
    public OverUI overUI;
    public GalaxyCamera galaxyCamera;

    protected Toolbox() { } // guarantee this will be always a singleton only - can't use the constructor!
    
    void Awake()
    {
        time = RegisterComponent<FrostyTime>();
        pool = RegisterComponent<FrostyPoolManager>();
        random = RegisterComponent<FrostyRandom>();
    }

    // (optional) allow runtime registration of global objects
    static public T RegisterComponent<T>() where T : Component
    {
        return Instance.GetOrAddComponent<T>();
    }
}