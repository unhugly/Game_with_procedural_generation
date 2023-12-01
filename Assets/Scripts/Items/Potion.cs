using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PotionType { HP, MOVEMENT_SPEED, ATTACK_SPEED, GOLDEN }

public class Potion : MonoBehaviour
{
    [SerializeField] PotionType type;
    [SerializeField] float value;
    [SerializeField] float duration;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GetComponent<AudioSource>() != null) 
            {
                GetComponent<AudioSource>().Play();
            }
                
            switch (type)
            {
                case PotionType.HP:
                    AddHP(value);
                    break;
                case PotionType.MOVEMENT_SPEED:
                    AddSpeed(value, duration);
                    break;
                case PotionType.GOLDEN:
                    AddMoney();
                    break;
                case PotionType.ATTACK_SPEED:
                    break;
                default:
                    break;
            }
            RemovePotion();
        }
    }

    void RemovePotion()
    {
        
        Collider[] colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }
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

    void AddMoney() 
    {
        PlayerWallet.money += value;
    }

    void AddHP(float value_) 
    {
        PlayerReference.player.GetComponent<PlayerBattleController>().Heal(value_);
        Debug.Log($"{PlayerReference.player.GetComponent<PlayerBattleController>().GetCurrentHealth()}/{PlayerReference.player.GetComponent<PlayerBattleController>().GetMaxHealth()}");
    }

    public void AddSpeed(float value_, float duration_)
    {
        PlayerReference.player.GetComponent<PlayerController>().ChangeMoveSpeed(value_, duration_);
    }
}
