using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation.Scripts
{
    public class EnemyFinisherHandler : MonoBehaviour
    {
        public GameObject enemy;
        private Animator _enemyAnimator;

        private void Awake()
        {
            _enemyAnimator = enemy.GetComponent<Animator>();
        }

        public IEnumerator RepositionEnemyCoroutine()
        {
            _enemyAnimator.enabled = false;
            yield return new WaitForSeconds(1.15f);

            enemy.SetActive(false);
            yield return new WaitForSeconds(4.0f);

            while (true)
            {
                var randomVector = Random.insideUnitCircle * 10;
                var newVector = new Vector3(randomVector.x + transform.position.x, 0, randomVector.y + transform.position.z);
                var distance = Vector3.Distance(transform.position, newVector);
                if (distance > 6)
                {
                    transform.position = newVector;
                    break;
                }

                yield return new WaitForSeconds(0.1f);
            }

            enemy.SetActive(true);
            _enemyAnimator.enabled = true;
        }
    }
}