using UnityEngine;
using System.Collections;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate towerTemplate;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;

    public void ReadyToSpawnTower()
    {
        //버튼을 중복해서 누르는 것을 방지하기 위해 필요
        if ( isOnTowerButton == true )
        {
            return;
        }
        
        //타워 건설 가능 여부 확인
        //타워를 건설할 만큼 돈이 없으면 타워 건설 X
        if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            //골드가 부족해서 타워 건실이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        //타워 건설 버튼을 눌렀다고 설정
        isOnTowerButton = true;
        //마우스를 따라다니는 임시 타워 생성
        followTowerClone = Instantiate(towerTemplate.followTowerPrefab, transform);
        //타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }
    
    
    public void SpawnTower(Transform tileTransform)
    {
        //타워 건설 버튼을 눌렀을 때만 타워 건설 가능
        if ( isOnTowerButton == false )
        {
            return;
        }
        
        Tile tile = tileTransform.GetComponent<Tile>();

        //2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 X
        if ( tile.IsBuildTower == true )
        {
            //현재 위치에 타워 건설이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        //타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        //타워 건설에 필요한 골드만큼 감소
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        //선택한 타워의 위치에 타워 건설 ( 타일보다 z축 -1 위치에 배치 )
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity );
        //타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold, tile);

        //타워를 배치했기 때문에 마우스를 따라다니는 임시타워 삭제
        Destroy( followTowerClone );
        //타워 건설을 취소할 수 있는 코루틴 함수 중지
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while ( true )
        {
            //ESC키 또는 마우스 오른쪽 버튼을 눌렀을 때 건설 취소
            if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                //마우스를 따라다니는 임시타워 삭제
                Destroy( followTowerClone );
                break;
            }
            yield return null;
        }
    }
}
