using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;


    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;

    private void Awake()
    {
        //"MainCamera" �±׸� ������ �ִ� ������Ʈ Ž�� �� Camera ������Ʈ ���� ����
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();�� ������
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //���콺�� UI�� �ӹ��� ���� ���� �Ʒ� �ڵ尡 ������� �ʵ��� ��
        if ( EventSystem.current.IsPointerOverGameObject() == true )
        {
            return;
        }
        
        //���콺 ������ ������ ��
        if (Input.GetMouseButtonDown(0))
        {
            //ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ���� ����
            //ray.origion : ������ ������ġ (= ī�޶� ��ġ)
            //ray.direction : ������ ���� ����
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //2D ����͸� ���� 3D ������ ������Ʈ�� ���콺�� �����ϴ� ���
            //������ �ε����� ������Ʈ�� �����ؼ� hit�� ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;

                //������ �ε��� ������Ʈ�� �±װ� "Tile"�̶��
                if (hit.transform.CompareTag("Tile"))
                {
                    //Ÿ���� �����ϴ� SpawnTower ȣ��
                    towerSpawner.SpawnTower(hit.transform);
                }
                //Ÿ���� �����ϸ� �ش� Ÿ�� ������ ����ϴ� Ÿ�� ����â ON
                else if (hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if ( Input.GetMouseButtonUp(0))
        {
            //���콺�� ������ �� ������ ������Ʈ�� ���ų� ������ ������Ʈ�� Ÿ���� �ƴ϶��
            if ( hitTransform == null || hitTransform.CompareTag("Tower") == false )
            {
                //Ÿ�� ���� �г��� ��Ȱ��ȭ
                towerDataViewer.OffPanel();
            }
            
            hitTransform = null;
        }
    }
}
