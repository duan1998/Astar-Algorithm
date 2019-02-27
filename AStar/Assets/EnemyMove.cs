using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test18
{
    public class EnemyMove : MonoBehaviour
    {
        public float speed ;
        private Rigidbody m_rigidbody;
        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            GameManager.Instance.PlayerHasMoved.AddListener(Follow);
        }

        public void Follow(Vector3[] targets)
        {
            StopAllCoroutines();
            index =0;
            StartCoroutine("Move", targets);

        }
        int index;
        IEnumerator Move(Vector3[] targets)
        {
            while(true)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targets[index], step);
                if (Vector3.Distance(transform.position, targets[index]) < 0.001f)
                {
                    if (index == targets.Length-1)
                    {
                        print("寻路结束");
                        break;
                    }
                    else
                    {
                        index++;
                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
    
}

