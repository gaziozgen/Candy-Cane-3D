using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class CollectibleCandyBit : MonoBehaviour, ICollectible, IPooledObject
{
    private BoxCollider boxCollider = null;
    public SouceMachine.Souce souce;
    public Material souceMaterial = null;
    public bool sprinkleHeart = false;
    public bool sprinkleStar = false;

    [SerializeField] private Renderer[] souceParts = null;
    [SerializeField] private GameObject[] sprinkleHeartParts = null;
    [SerializeField] private GameObject[] sprinkleStarParts = null;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        souce = SouceMachine.Souce.Empty;
        //UpdateMaterials();
    }

    public void GetCollected()
    {
        MainLevelManager.Instance.Player.AddCandyBit(souce, souceMaterial, sprinkleHeart, sprinkleStar);
        boxCollider.enabled = false;
        transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void OnObjectSpawn()
    {
        boxCollider.enabled = false;
    }

    public void ActivateCollider()
    {
        boxCollider.enabled = true;
    }

    public void SetUpgrades(SouceMachine.Souce souce, Material souceMaterial, bool heart, bool star)
    {
        this.souce = souce;
        sprinkleHeart = heart;
        sprinkleStar = star;
        this.souceMaterial = souceMaterial;
        UpdateMaterials();
    }

    private void UpdateMaterials()
    {
        //Material souceMat = FindObjectOfType<SouceMachine>().GetMaterialOfEnum(souce);
        foreach (Renderer part in souceParts)
        {
            part.material = souceMaterial;
        }

        foreach (GameObject part in sprinkleHeartParts)
        {
            part.SetActive(sprinkleHeart);
        }

        foreach (GameObject part in sprinkleStarParts)
        {
            part.SetActive(sprinkleStar);
        }
    }
}
