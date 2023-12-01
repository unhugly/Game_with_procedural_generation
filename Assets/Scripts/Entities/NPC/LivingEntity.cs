using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class LivingEntity : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField] float maxHealth = 1.0f;
    [SerializeField] float maxArmor = 1.0f;
    [SerializeField] float missChance = 0.0f;
    [SerializeField] float chaseRange = 1f;
    [SerializeField] float runSpeed = 0.4f;
    [SerializeField] float chasingSpeed = 0.3f;
    [SerializeField] float attackRange = 0.2f;
    [SerializeField] float attackSpeed = 0.2f;
    [SerializeField] float damage = 0;
    [SerializeField] Fight_Mode mode = Fight_Mode.Neutral;
    [SerializeField] Material skinMaterial;
    [SerializeField] Material damagedMaterial;
    [SerializeField] public string spawnTag;

    [Header("Prefabs")]
    [SerializeField] GameObject diePref;

    [Header("Sounds")]
    [SerializeField] List<AudioClip> lessHPsounds;
    [SerializeField] List<AudioClip> missSounds;
    [SerializeField] List<AudioClip> dieSounds;
    [SerializeField] List<AudioClip> armorHitSounds;
    [SerializeField] List<AudioClip> armorCrashedSounds;


    private bool isChasing = false;
    private float lastAttackTime = 0f;
    private float currentHealth;
    private float currentArmor;
    private Renderer[] renderers;
    private HPScript hPScript;
    private AudioSource audioSource;
    [HideInInspector] public AliveStatus entityStatus;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentArmor = maxArmor;
        entityStatus = AliveStatus.Alive;
    }

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        audioSource = GetComponent<AudioSource>();
        hPScript = GetComponentInChildren<HPScript>();
    }

    private void Update()
    {
        switch (mode)
        {
            case Fight_Mode.Agressive:
                float distanceToPlayer = Vector3.Distance(transform.position, PlayerReference.player.transform.position);
                if (distanceToPlayer <= attackRange)
                {
                    if (!isChasing)
                    {
                        GetComponent<RandomWalker>().enabled = false;
                        isChasing = true;
                    }
                    Attack();
                }
                else if (distanceToPlayer <= chaseRange && !isChasing)
                {
                    ChasePlayer();
                }
                else if (isChasing)
                {
                    StopChasingPlayer();
                }
                break;
            case Fight_Mode.Peaceful:
                if (currentHealth < maxHealth || currentArmor < maxArmor)
                {
                    RunFromPlayer();
                }
                break;
            case Fight_Mode.Neutral:
                break;
            default:
                break;
        }
    }

    private void ChasePlayer()
    {
        isChasing = true;
        GetComponent<RandomWalker>().enabled = false;

        Vector3 direction = (PlayerReference.player.transform.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, PlayerReference.player.transform.position, chasingSpeed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void StopChasingPlayer()
    {
        isChasing = false;
        GetComponent<RandomWalker>().enabled = true;
    }

    private void Attack()
    {
        GetComponent<RandomWalker>().enabled = false;
        if (isChasing)
        {
            isChasing = false;
            if (Time.time - lastAttackTime >= attackSpeed)
            {
                if (transform.Find("RightHand") == null) return;
                StartCoroutine(RotateHandCoroutine());
                lastAttackTime = Time.time;
                transform.LookAt(PlayerReference.player.transform);
            }
        }
    }

    private IEnumerator RotateHandCoroutine()
    {
        Transform rightHand = transform.Find("RightHand");
        Quaternion originalRotation = rightHand.localRotation;
        Quaternion targetRotation = Quaternion.Euler(rightHand.localEulerAngles.x, rightHand.localEulerAngles.y - 120f, rightHand.localEulerAngles.z - 60);

        float time = 0f;
        while (time < 0.15f)
        {
            rightHand.localRotation = Quaternion.Lerp(originalRotation, targetRotation, time / 0.15f);
            time += Time.deltaTime;
            yield return null;
        }
        rightHand.localScale = rightHand.localScale * 1.2f;
        rightHand.localRotation = targetRotation;

        if (Vector3.Distance(transform.position, PlayerReference.player.transform.position) <= attackRange) 
        {
            PlayerReference.player.GetComponent<PlayerBattleController>().healthPoints = PlayerReference.player.GetComponent<PlayerBattleController>().healthPoints - damage;
            Debug.Log($"{PlayerReference.player.GetComponent<PlayerBattleController>().GetCurrentHealth()}/{PlayerReference.player.GetComponent<PlayerBattleController>().GetMaxHealth()}");
        }

        time = 0f;
        while (time < 0.15f)
        {
            rightHand.localRotation = Quaternion.Lerp(targetRotation, originalRotation, time / 0.15f);
            time += Time.deltaTime;
            yield return null;
        }
        rightHand.localRotation = originalRotation;
        rightHand.localScale = rightHand.localScale * 0.8333f;
    }

    public void TakeDamage(float damage)
    {
        if (entityStatus == AliveStatus.Dead) { return; }
        if (Random.Range(0, 100) > missChance)
        {
            if (currentArmor > 0)
            {
                currentArmor -= damage;
                Debug.Log($"{gameObject.name} took {damage} damage");

                if (currentArmor <= 0)
                {
                    float leftoverDamage = -currentArmor;
                    currentArmor = 0;
                    currentHealth -= leftoverDamage;
                    PlaySounds(armorCrashedSounds);
                    PlayParticles(Color.black, "Crashed!");
                }
                else 
                {
                    PlaySounds(armorHitSounds);
                    PlayParticles(Color.grey, $"{damage}");
                }
            }
            else
            {
                currentHealth -= damage;
                Debug.Log($"{gameObject.name} took {damage} damage");
            }

            if (currentHealth <= 0)
            {
                entityStatus = AliveStatus.Dead;
                PlaySounds(dieSounds);
                PlayParticles(Color.red, "DEAD!");
                Die();
            }
            else if (currentHealth < maxHealth)
            {
                PlaySounds(lessHPsounds);
                PlayParticles(Color.red, $"{damage}");
                entityStatus = AliveStatus.Wounded;
            }
            GetComponent<Rigidbody>().AddForce(Vector3.up * 0.035f, ForceMode.Impulse);
            StartCoroutine(Blink());
        }
        else 
        {
            PlaySounds(missSounds);
            hPScript.ChangeHP(0, transform.position, Color.cyan, "miss =)");
        }
    }

    private void RunFromPlayer()
    {
        GetComponent<RandomWalker>().enabled = false;
        if (PlayerReference.player.transform != null)
        {
            Vector3 directionToRun = transform.position - PlayerReference.player.transform.position;
            directionToRun.Normalize();
            Vector3 runVector = runSpeed * Time.deltaTime * directionToRun;
            transform.position += runVector;
            transform.rotation = Quaternion.LookRotation(runVector);
        }
    }

    private void Die()
    {
        if (diePref) Instantiate(diePref, transform.position, transform.rotation);

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        if (meshRenderers.Length > 0)
        { 
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = false;
            }
        }
        Destroy(gameObject, 1f);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == maxHealth && currentArmor == maxArmor)
        {
            entityStatus = AliveStatus.Alive;
        }
    }

    public void RepairArmor(float amount)
    {
        currentArmor += amount;
        currentArmor = Mathf.Clamp(currentArmor, 0, maxArmor);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetCurrentArmor()
    {
        return currentArmor;
    }

    private IEnumerator Blink()
    {
        Material[] originalMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].sharedMaterial == skinMaterial)
            {
                originalMaterials[i] = renderers[i].sharedMaterial;
                renderers[i].sharedMaterial = damagedMaterial;
            }
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].sharedMaterial == damagedMaterial)
            {
                renderers[i].sharedMaterial = originalMaterials[i];
            }
        }
    }

    private void PlayParticles(Color color, string text) 
    {
        if (hPScript != null) hPScript.ChangeHP(0, transform.position + new Vector3(0,0.2f,0), color, text);
    }

    private void PlaySounds(List<AudioClip> audios) 
    {
        if (audios.Count > 0) audioSource.PlayOneShot(audios[Random.Range(0, audios.Count)]);
    }
}
