using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class CandyBit : MonoBehaviour, IPooledObject
{
    private BoxCollider boxCollider = null;
    private Rigidbody rb = null;
    public SouceMachine.Souce souce;
    public Material souceMaterial = null;
    public bool sprinkleHeart = false;
    public bool sprinkleStar = false;

    [SerializeField] private GameObject generalMesh = null;
    [SerializeField] private float timePerCandy = 0.2f;
    [SerializeField] private Renderer[] souceParts = null;
    [SerializeField] private GameObject[] sprinkleHeartParts = null;
    [SerializeField] private GameObject[] sprinkleStarParts = null;

    [SerializeField] private ParticleSystem candyExplode = null;
    [SerializeField] private ParticleSystem money = null;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            ICollectible collectible = other.GetComponent<ICollectible>();
            collectible.GetCollected();
        }
        else if (other.CompareTag("SaveMachine"))
        {
            MainLevelManager.Instance.Player.ThrowCandiesLaterFrom(gameObject);

            SaveMachine machine = other.GetComponent<SaveMachine>();
            machine.CutCandy(souce, sprinkleHeart, sprinkleStar);

            Save(1.2f);
        }
        else if (other.CompareTag("Obsticle"))
        {
            MainLevelManager.Instance.Player.ThrowCandiesLaterFrom(gameObject);
            Smash();
        }
        else if (other.CompareTag("FinalMachine"))
        {
            MainLevelManager.Instance.Player.ThrowCandiesLaterFrom(gameObject);
            FinalMachine.Instance.CandyToMoney(souce, sprinkleHeart, sprinkleStar);
            FinalCut(1.5f);
        }
        else if (other.CompareTag("SouceMachine"))
        {
            SouceMachine machine = other.GetComponent<SouceMachine>();
            souce = machine.souce;
            souceMaterial = machine.souceMaterial;
            PaintSouce(machine.souceMaterial);
        }
        else if (other.CompareTag("SprinkleMachine"))
        {
            SprinkleMachine machine = other.GetComponent<SprinkleMachine>();
            if (machine.sprinkle == SprinkleMachine.Sprinkle.Heart)
            {
                sprinkleHeart = true;
                ActivateSprinkles(sprinkleHeartParts, sprinkleHeart);
            }
            else if (machine.sprinkle == SprinkleMachine.Sprinkle.Star)
            {
                sprinkleStar = true;
                ActivateSprinkles(sprinkleStarParts, sprinkleStar);
            }
        }
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        souce = SouceMachine.Souce.Empty;
    }

    public void OnObjectSpawn()
    {
        boxCollider.enabled = true;
    }

    private void Smash()
    {
        boxCollider.enabled = false;
        candyExplode.Play();
        generalMesh.SetActive(false);
        LeanTween.delayedCall(0.5f, () =>
        {
            generalMesh.SetActive(true);
            gameObject.SetActive(false);
        });    }

    private void Save(float time)
    {
        boxCollider.enabled = false;
        rb.isKinematic = false;
        money.gameObject.SetActive(true);

        float x = Random.Range(300f, 400f);
        float y = Random.Range(600f, 800f);
        float z = Random.Range(1200f, 1600f);

        rb.AddForce(x, y, z);
        transform.LeanRotate(new Vector3(180, 40, 40), time);

        LeanTween.delayedCall(time, () => {
            money.gameObject.SetActive(false);
            rb.isKinematic = true;
            transform.rotation = Quaternion.identity;

            FinalMachine.Instance.SaveCandyBit(transform);
        });
        
    }

    private void FinalCut(float time)
    {
        boxCollider.enabled = false;
        rb.isKinematic = false;
        money.gameObject.SetActive(true);

        float x = Random.Range(-100f, 100f);
        float y = Random.Range(600f, 800f);
        float z = Random.Range(400f, 600f);

        rb.AddForce(x, y, z);
        transform.LeanRotate(new Vector3(180, 40, 40), time);

        LeanTween.delayedCall(time, () =>
        {
            rb.isKinematic = true;
            FinalMachine.Instance.PlaceCandyBit(transform);
        });
    }

    public void Hop(float delay)
    {
        StartCoroutine(HopCoroutine(delay));
    }

    private IEnumerator HopCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        LeanTween.cancel(generalMesh);
        generalMesh.LeanScale(new Vector3(1.5f, 1.5f, 1), 0.1f).setOnComplete(() =>
        {
            generalMesh.LeanScale(Vector3.one, 0.1f);
        });
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

    public void PaintSouce(Material mat)
    {
        float timePerParticle = timePerCandy / souceParts.Length;
        for (int i = 0; i < souceParts.Length; i++)
        {
            PaintSingleSouce((i+1) * timePerParticle, i, mat);
        }
    }

    private void PaintSingleSouce(float time, int index, Material mat)
    {
        LeanTween.delayedCall(time, () => {
            souceParts[index].material = mat;
        });
    }

    public void ActivateSprinkles(GameObject[] parts, bool sprinkle)
    {
        float timePerParticle = timePerCandy / parts.Length;
        for (int i = 0; i < parts.Length; i++)
        {
            StartCoroutine(ActivateSingleSprinkle(i * timePerParticle, parts[i]));
        }
    }

    private IEnumerator ActivateSingleSprinkle(float delay, GameObject part)
    {
        yield return new WaitForSeconds(delay);
        part.SetActive(true);

    }

}
