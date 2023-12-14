
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownShooting : MonoBehaviour
{
    private ProjectileManager _projectileManager;
    private TopDownCharacterController _contoller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    public AudioClip shootingClip;

    private void Awake()
    {
        _contoller = GetComponent<TopDownCharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _projectileManager = ProjectileManager.Instance;
        _contoller.OnAttackEvent += OnShoot;
        _contoller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnShoot(AttackSO attackSO)
    {
        RangedAttackData rangedAttackData = attackSO as RangedAttackData;
        float projectilesAnglespace = rangedAttackData.multipleProjectilesAngle;
        int numberofProjectilesPerShot = rangedAttackData.numberofProjectilesPerShot;

        float minAngle = -(numberofProjectilesPerShot / 2f) * projectilesAnglespace * 0.5f * rangedAttackData.multipleProjectilesAngle;


        for (int i=0; i<numberofProjectilesPerShot; i++)
        {
            float angle = minAngle + projectilesAnglespace * i;
            float randomSpread = Random.Range(-rangedAttackData.spread, rangedAttackData.spread);
            angle += randomSpread;
            CreateProjectile(rangedAttackData, angle);
        }

    }

    private void CreateProjectile(RangedAttackData rangedAttackData, float angle)
    {
        _projectileManager.ShootBullet(
                projectileSpawnPosition.position,
                RotateVector2(_aimDirection, angle),
                rangedAttackData
                );

        if (shootingClip)
            SoundManager.PlayClip(shootingClip);
    }

    private static Vector2 RotateVector2( Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }



}
