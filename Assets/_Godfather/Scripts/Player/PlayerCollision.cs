using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _blockingLayer;
    private int _blockDirection; // -1 = gauche bloquée, 1 = droite bloquée, 0 = rien
    public int BlockDirection => _blockDirection;
    
    
    private PlayerBattery _playerBattery;

    private void Start()
    {
        _playerBattery = GetComponent<PlayerBattery>();
        if (_playerBattery == null)
        {
            Debug.LogError("PlayerBattery not found on the player.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsInLayerMask(collision.gameObject.layer, _blockingLayer)) return;

        // On prend la première normale du contact
        Vector2 normal = collision.GetContact(0).normal;

        // Produit vectoriel avec transform.up pour savoir si c’est gauche/droite
        float side = Vector3.Dot(normal, -transform.right);

        if (side > 0.5f)
            _blockDirection = -1; // gauche bloquée
        else if (side < -0.5f)
            _blockDirection = 1;  // droite bloquée

        if (collision.gameObject.CompareTag("Meteor"))
        {
            _playerBattery.ConsumeBatterySlot();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsInLayerMask(collision.gameObject.layer, _blockingLayer)) return;
        _blockDirection = 0; // plus bloqué
    }

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }
    
    private void OnDrawGizmos()
    {
        if (_blockDirection == 0) return;

        Gizmos.color = Color.red;

        Vector3 dir = (_blockDirection == -1) ? -transform.right : transform.right;
        Gizmos.DrawLine(transform.position, transform.position + dir * 1f);

        // petit repère
        Gizmos.DrawSphere(transform.position + dir * 1f, 0.05f);
    }
}