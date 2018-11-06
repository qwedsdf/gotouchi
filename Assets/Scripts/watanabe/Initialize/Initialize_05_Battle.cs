using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize_05_Battle : MonoBehaviour
{
    void Awake()
    {
        GameObject player = AvatarManager.Instance.player;

        // 必要なコンポーネントを追加して設定
        GameObject armCol = Instantiate(Resources.Load("Prefabs/ArmCol", typeof(GameObject))) as GameObject;
        armCol.transform.parent = player.transform;
        armCol.name = "ArmCol";
        player.AddComponent<CharacterStatus>();
        player.AddComponent<Player>();
        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        BoxCollider boxCol = player.AddComponent<BoxCollider>();
        boxCol.isTrigger = true;
        boxCol.center = new Vector3(0.0f, 0.75f, 0.0f);
        boxCol.size = new Vector3(1.0f, 1.5f, 0.5f);
        player.tag = "Player";
    }

    void Start()
    {

    }
}
