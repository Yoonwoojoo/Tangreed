using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGate : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject playerGO = null;

    public FadeEffect fadeEffect;

    private bool isPlayerInTrigger = false;
    private GameObject stage1;

    private void Start()
    {
        // 현재 활성화된 Stage1을 찾습니다.
        stage1 = GameObject.Find("Stage1(Clone)");
        fadeEffect = FindObjectOfType<FadeEffect>();
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(SwitchToStage2());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerGO = collision.gameObject;
            isPlayerInTrigger = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerGO = null;
            isPlayerInTrigger = false;
            
        }
    }

    private IEnumerator SwitchToStage2()
    {
        // 페이드 인 및 페이드 아웃 시작 (1.5초 대기)
        fadeEffect.FadeInAndOut(1.5f);

        yield return new WaitForSeconds(1f);

        // Stage1 비활성화
        if (stage1 != null)
        {
            stage1.SetActive(false);
        }

        // Stage2 활성화
        GameObject stage2Instance = Instantiate(MapManager.Instance.mapList[1]);

        // Stage1 파괴
        if (stage1 != null)
        {
            Destroy(stage1);
        }
    }
}
