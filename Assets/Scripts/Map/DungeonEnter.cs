using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마을에서 던전으로 이동하게 해줍니다.

public class DungeonEnter : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject lobbyMap;
    public GameObject robbyDoor;
    public GameObject playerGO = null;

    public FadeEffect fadeEffect;

    private void Start()
    {
        robbyDoor.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 플레이어의 X좌표와 Door의 Y좌표 이용 (collision.transform.position으로 했을 경우 문이 파묻혀서 생성된다.)
            Vector3 tempPos = new Vector3(collision.transform.position.x, robbyDoor.transform.position.y);
            robbyDoor.transform.position = tempPos;
            robbyDoor.SetActive(true);
            playerGO = collision.gameObject;
            StartCoroutine(WaitAndEnterDungeon(1f));
        }
    }

    private IEnumerator WaitAndEnterDungeon(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EnterDungeon();
    }

    public void EnterDungeon()
    {
        if (playerGO != null)
        {
            fadeEffect.FadeIn();
            Destroy(lobbyMap, 1.0f);
            // 던전 생성로직
            fadeEffect.DelayedFadeOut(1.5f);
        }
    }
}
