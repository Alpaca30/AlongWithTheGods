using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem fireParticle = null;

    [SerializeField]
    private SkinnedMeshRenderer playerMr = null;
    [SerializeField]
    private MeshRenderer swordMr = null;

    private Material toonMat = null;


    private void Awake()
    {
        if (playerMr == null)
        {
            playerMr = GetComponent<SkinnedMeshRenderer>();
        }
        toonMat = playerMr.material;

        if (fireParticle != null)
        {
            ParticleSystem.ShapeModule shape = fireParticle.shape;
            shape.skinnedMeshRenderer = playerMr;
        }
    }


    public void SetDieEffect()
    {
        swordMr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        playerMr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        playerMr.receiveShadows = false;

        toonMat.SetFloat("_Edges", 0.15f);
        toonMat.SetFloat("_Outline", 0f);
        DieEffect(toonMat, 0.7f);
    }

    private void DieEffect(Material _mat, float _delayTime)
    {
        ParticleSystem ps = Instantiate(fireParticle);
        ParticleSystem.ShapeModule shape = ps.shape;
        shape.skinnedMeshRenderer = playerMr;
        StartCoroutine(DieEffectCoroutine(_mat, _delayTime));
    }
    private IEnumerator DieEffectCoroutine(Material _mat, float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        _mat.SetFloat("_AlphaCut", 0f);
        float t = 0f;
        while (t < 1f)
        {
            _mat.SetFloat("_AlphaCut", t);
            t += Time.deltaTime;
            yield return null;
        }
        t = 1f;
        _mat.SetFloat("_AlphaCut", t);
    }
}
