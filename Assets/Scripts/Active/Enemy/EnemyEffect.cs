using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem fireParticle = null;
    [SerializeField]
    private float delayTime = 0.6f;

    [SerializeField]
    private List<SkinnedMeshRenderer> smrList = new List<SkinnedMeshRenderer>();
    [SerializeField]
    private List<MeshRenderer> mrList = new List<MeshRenderer>();


    public void DeadMats()
    {
        foreach (SkinnedMeshRenderer smr in smrList)
        {
            Material mat = smr.material;
            GenerateFireParticle(smr);
            GetoffOutline(mat);
            DeadEffect(mat, delayTime);
        }
        foreach (MeshRenderer mr in mrList)
        {
            Material mat = mr.material;
            GenerateFireParticle(mr);
            GetoffOutline(mat);
            DeadEffect(mat, delayTime);
        }
    }

    private void GenerateFireParticle(SkinnedMeshRenderer _smr)
    {
        ParticleSystem ps = Instantiate(fireParticle);
        ParticleSystem.ShapeModule shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
        shape.skinnedMeshRenderer = _smr;
    }
    private void GenerateFireParticle(MeshRenderer _mr)
    {
        ParticleSystem ps = Instantiate(fireParticle);
        ParticleSystem.ShapeModule shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.MeshRenderer;
        shape.meshRenderer = _mr;
    }

    private void GetoffOutline(Material _mat)
    {
        _mat.SetFloat("_Outline", 0);
    }

    private void DeadEffect(Material _mat, float _delayTime)
    {
        StartCoroutine(DeadEffectCoroutine(_mat, _delayTime));
    }

    private IEnumerator DeadEffectCoroutine(Material _mat, float _delayTime)
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
