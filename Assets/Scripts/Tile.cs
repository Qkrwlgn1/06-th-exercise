using UnityEngine;

public class Tile : MonoBehaviour
{
    //타일에 타워가 건설되어 있는지 검사하는 변수
    public bool IsBuildTower{set; get;}

    public void Awake()
    {
        IsBuildTower = false;
    }
}
