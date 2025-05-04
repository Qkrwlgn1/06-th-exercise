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
        //��ư�� �ߺ��ؼ� ������ ���� �����ϱ� ���� �ʿ�
        if ( isOnTowerButton == true )
        {
            return;
        }
        
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� X
        if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            //��尡 �����ؼ� Ÿ�� �ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        //Ÿ�� �Ǽ� ��ư�� �����ٰ� ����
        isOnTowerButton = true;
        //���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
        followTowerClone = Instantiate(towerTemplate.followTowerPrefab, transform);
        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnTowerCancelSystem");
    }
    
    
    public void SpawnTower(Transform tileTransform)
    {
        //Ÿ�� �Ǽ� ��ư�� ������ ���� Ÿ�� �Ǽ� ����
        if ( isOnTowerButton == false )
        {
            return;
        }
        
        Tile tile = tileTransform.GetComponent<Tile>();

        //2. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ Ÿ�� �Ǽ� X
        if ( tile.IsBuildTower == true )
        {
            //���� ��ġ�� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        //Ÿ���� �Ǽ��Ǿ� �������� ����
        tile.IsBuildTower = true;
        //Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
        //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ� ( Ÿ�Ϻ��� z�� -1 ��ġ�� ��ġ )
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity );
        //Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold, tile);

        //Ÿ���� ��ġ�߱� ������ ���콺�� ����ٴϴ� �ӽ�Ÿ�� ����
        Destroy( followTowerClone );
        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while ( true )
        {
            //ESCŰ �Ǵ� ���콺 ������ ��ư�� ������ �� �Ǽ� ���
            if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                //���콺�� ����ٴϴ� �ӽ�Ÿ�� ����
                Destroy( followTowerClone );
                break;
            }
            yield return null;
        }
    }
}
