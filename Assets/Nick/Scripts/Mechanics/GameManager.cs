using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Mode { Race, Survival, TimeTrial }
    public enum Map { Day, Night }

    public static Mode mode;
    public static Map map;

    public void ChangeMode(int index) => mode = (Mode)index;
    public void ChangeMap(int index) => map = (Map)index;
}
