using UnityEngine;
using System.Collections;

public class AvatarControll : MonoBehaviour
{

    public MySkinnedMeshCombiner smc = null;

    void Start()
    {
        Combine();
    }

    /// <summary>
    /// スキンメッシュを結合
    /// </summary>
    public void Combine()
    {
        this.smc.Combine();
    }
}
