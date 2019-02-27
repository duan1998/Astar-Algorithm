using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Test18
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        { get { return instance; } }

        private static GameManager instance;

        public GameObject cube0;
        public GameObject cube1;
        public GameObject cube2;
        public GameObject cube3;

        public Transform pathRoot;

        public Vector2Int playerPos = Vector2Int.zero;
        public Vector2Int enemyPos = Vector2Int.zero;

        private Transform player;
        private Transform enemy;

        [HideInInspector]
        public Vector3ArrEvent PlayerHasMoved;

        public int[,] map=new int[10, 10] {
        { 0,0,0,0,0,0,0,0,0,0},
        { 0,1,1,1,1,1,1,1,1,0},
        { 0,1,1,0,1,1,1,1,1,0},
        { 0,1,2,0,0,0,0,0,1,0},
        { 0,0,0,0,1,1,1,1,1,0},
        { 0,1,1,1,1,0,0,0,1,0},
        { 0,1,0,1,1,0,1,1,1,0},
        { 0,1,1,0,1,0,3,1,1,0},
        { 0,1,1,1,0,1,1,1,1,0},
        { 0,0,0,0,0,0,0,0,0,0}
        };

        private void Awake()
        {
            instance = this;
            PlayerHasMoved = new Vector3ArrEvent();
        }
        // Start is called before the first frame update
        void Start()
        {
            for(int i=0;i<10;i++)
            {
                for(int j=0;j<10;j++)
                {
                    switch(map[i,j])
                    {
                        case 0:
                            Instantiate(cube0, new Vector3(i+0.5f, 0, j+0.5f), Quaternion.identity);
                            break;
                        case 1:

                            break;

                        case 2:
                            playerPos = new Vector2Int(i, j);
                            player=Instantiate(cube1, new Vector3(i+0.5f, 0, j+0.5f), Quaternion.identity).GetComponent<Transform>();
                            break;
                        case 3:
                            enemyPos = new Vector2Int(i, j);
                            enemy=Instantiate(cube3, new Vector3(i+0.5f, 0, j+0.5f), Quaternion.identity).GetComponent<Transform>();
                            break;
                    }
                }
            }
            //Invoke("UpdatePath", 0f);
            InvokeRepeating("UpdatePath", 0f, 3f);
        }


        private void Update()
        {
            
            Vector2Int newPlayerPos = new Vector2Int(GetNearestInteger(player.position.x), GetNearestInteger(player.position.z));
            Vector2Int newEnemyPos = new Vector2Int(GetNearestInteger(enemy.position.x), GetNearestInteger(enemy.position.z));
            if (newPlayerPos != playerPos)
            {
                map[playerPos.x, playerPos.y] = 1;
                map[newPlayerPos.x, newPlayerPos.y] = 2;
                playerPos = newPlayerPos;
            }
            if (newEnemyPos != enemyPos)
            {
                map[enemyPos.x, enemyPos.y] = 1;
                map[newEnemyPos.x, newEnemyPos.y] = 3;
                enemyPos = newEnemyPos;
            }
            
        }

        void UpdatePath()
        {
            for(int i=0;i<pathRoot.childCount;i++)
                Destroy(pathRoot.GetChild(i).gameObject);
            List<Vector2Int> pathList=AStar.AutomaticPathFinding(enemyPos,playerPos,map,10,10);
            if(pathList!=null)
            {
                for (int i = 0; i < pathList.Count; i++)
                {
                    Instantiate(cube2, new Vector3(pathList[i].x + 0.5f, 0, pathList[i].y + 0.5f), Quaternion.identity, pathRoot);
                }
                if (PlayerHasMoved != null)
                {
                    Vector3[] targets = new Vector3[pathList.Count];
                    for(int i=0;i< pathList.Count; i++)
                    {
                        targets[i].x = pathList[pathList.Count - i - 1].x+0.5f;
                        targets[i].y = 0;
                        targets[i].z = pathList[pathList.Count - i - 1].y + 0.5f;
                    }
                    PlayerHasMoved.Invoke(targets);
                }
                   
            }
            else
            {
                print("无法获取寻路信息");
            }

        }
        int GetNearestInteger(float num)
        {
            return (float)(num - Mathf.Floor(num)) > 0.0001f ?(int)num :(int)(num-1);
        }
        
    }
    public class Vector3ArrEvent : UnityEvent<Vector3[]>
    {
    }
}


