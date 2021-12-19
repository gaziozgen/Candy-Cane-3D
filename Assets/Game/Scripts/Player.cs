using UnityEngine;
using FateGames;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 speed = Vector3.one;
    [SerializeField] private Transform candyStackPosition = null;
    public Animator animator = null;
    private List<Transform> candyStack;
    private Swerve1D swerve = null;
    private CharacterController controller = null;
    private float swerveAnchorX = 0;
    private bool end = false;
    private float rate = 0;

    private void Awake()
    {
        candyStack = new List<Transform>();
        swerve = InputManager.CreateSwerve1D(Vector2.right, Screen.width * 0.5f);
        swerve.OnStart = () => { swerveAnchorX = transform.position.x; };
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.IN_GAME & !end)
        {
            CheckInput();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.IN_GAME & !end)
        {
            MoveForward();
            Move(rate);
            AdjustCandyBitPositions();
        }
    }

    private void CheckInput()
    {
        if (swerve.Active)
            rate = swerve.Rate;
    }

    private void Move(float rate)
    {
        //Road road = MainLevelManager.Instance.Road;
        Vector3 targetPosition = transform.position;
        targetPosition.x = swerveAnchorX + rate * 8; // edited from road.Width to 8
        Vector3 motion = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed.x) - transform.position;
        if ((transform.position + motion).x - controller.radius > 8 / -2 && (transform.position + motion).x + controller.radius < 8 / 2)
            controller.Move(motion);
    }

    private void MoveForward()
    {
        controller.Move(Vector3.forward * Time.deltaTime * speed.z);
    }

    public void AddCandyBit(SouceMachine.Souce souce, Material souceMaterial, bool sprinkleHeart, bool sprinkleStar)
    {
        Transform candyBit = ObjectPooler.Instance.SpawnFromPool("Candy Bit", (candyStack.Count > 0 ? candyStack[candyStack.Count - 1].position + Vector3.forward : candyStackPosition.position), Quaternion.identity).transform;
        candyStack.Add(candyBit);
        candyBit.GetComponent<CandyBit>().SetUpgrades(souce, souceMaterial, sprinkleHeart, sprinkleStar);
        WaveEffect();
    }

    private void AdjustCandyBitPositions()
    {
        if (candyStack.Count > 0)
        {
            candyStack[0].position = candyStackPosition.position;
            for (int i = 1; i < candyStack.Count; i++)
            {
                Transform candyBit = candyStack[i];
                Transform previousCandyBit = candyStack[i - 1];

                Vector3 targetPosition = previousCandyBit.position;
                targetPosition.x = candyBit.position.x;
                targetPosition.z += 1;
                candyBit.position = targetPosition;
                targetPosition.x = previousCandyBit.position.x;

                candyBit.position = Vector3.Lerp(candyBit.position, targetPosition, Time.fixedDeltaTime * speed.x);
            }
        }
    }

    public void ThrowCandiesLaterFrom(GameObject startCandy)
    {
        int index = candyStack.IndexOf(startCandy.transform);
        candyStack.Remove(startCandy.transform);

        if (index < candyStack.Count)
        {
            Vector3 position = new Vector3(0, 1, candyStack[index].transform.position.z + 12);

            while (index < candyStack.Count)
            {
                CandyBit candyBit = candyStack[index].GetComponent<CandyBit>();
                candyStack.Remove(candyBit.transform);
                candyBit.gameObject.SetActive(false);

                CollectibleCandyBit collectibleCandyBit = ObjectPooler.Instance.SpawnFromPool("Collectible Candy Bit", candyBit.transform.position, Quaternion.identity).GetComponent<CollectibleCandyBit>();
                collectibleCandyBit.SetUpgrades(candyBit.souce, candyBit.souceMaterial, candyBit.sprinkleHeart, candyBit.sprinkleStar);
                Vector3 targetPosition = position + new Vector3((8f / 2 - 0.5f) * (Random.value * 2f - 1), 0, Random.Range(-4f, 4f)); // MainLevelManager.Instance.Road.Width to 8
                collectibleCandyBit.transform.LeanMoveX(targetPosition.x, 0.35f);
                collectibleCandyBit.transform.LeanMoveZ(targetPosition.z, 0.35f);
                collectibleCandyBit.transform.LeanMoveY(4, 0.17f).setEaseInSine().setOnComplete(() =>
                {
                    collectibleCandyBit.transform.LeanMoveY(2, 0.18f).setEaseOutBounce().setOnComplete(() => collectibleCandyBit.ActivateCollider());
                });
            }
        }
    }

    private void WaveEffect()
    {
        int size = candyStack.Count;
        for (int i = 0; i < candyStack.Count; i++)
        {
            candyStack[i].GetComponent<CandyBit>().Hop((size-i) * 0.03f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            ICollectible collectible = other.GetComponent<ICollectible>();
            collectible.GetCollected();
        }
        else if (other.CompareTag("Obsticle"))
        {
            int layerMask = 1 << 3;
            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out hit, 2f, layerMask))
            {
                Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                print(hit.transform.gameObject.name);
                if (hit.transform.CompareTag("Obsticle"))
                {
                    controller.Move(-Vector3.forward * 10);
                }
            }
        }
        else if (other.CompareTag("FinalMachine"))
        {
            Camera.main.GetComponent<CameraFollow>().enabled = false;
            other.GetComponent<FinalMachine>().FinalShow();
            animator.SetTrigger("idle");
            end = true;
        }
        else if (other.CompareTag("SouceMachine"))
        {
            //other.gameObject.GetComponent<SouceMachine>().FadeOut();
        }
        else if (other.CompareTag("SprinkleMachine"))
        {
            //other.gameObject.GetComponent<SprinkleMachine>().FadeOut();
        }
        else if (other.CompareTag("SaveMachine"))
        {
            //other.gameObject.GetComponent<SaveMachine>().FadeOut();
        }
    }
}
