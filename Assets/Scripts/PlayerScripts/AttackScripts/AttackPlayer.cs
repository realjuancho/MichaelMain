using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    public int AttackComboOrder;

    public float attackRange;
    public float coolDownTime = 1;
    public float coolDownTimeRate = 1f;
    public bool DebugAttack = false;
    public LayerMask enemyLayers;
    public Common.MainAttackType attackType;
    public Common.AttackReaction attackReaction;
    public float attackReactionPower = 1f;
    public float Damage;


    float coolDown;
    Bezier bezier;
    bool AttackFired;
    bool cancel;
    float currentStep;

    TrailRenderer trailRenderer;
    LineRenderer lineRenderer;
    CameraShake cameraShake;

    void Awake()
    {
        //Obtener los puntos donde va a pasar el ataque
        bezier = GetComponent<Bezier>();
        bezier.origin = GetComponentInParent<Transform>();
        trailRenderer = GetComponent<TrailRenderer>();
        lineRenderer = GetComponent<LineRenderer>();

        cameraShake = GameObject.FindObjectOfType<CameraShake>();

        InitializeDamage();
    }

    void Update()
    {
        HandleCoolDown();
        HandleCancel();
    }


    /// <summary>
    /// Initializes the damage.
    /// </summary>
    void InitializeDamage()
    { 
        switch (attackType)
        {
            case Common.MainAttackType.Light:
                Damage = Common.LightDamage;
                break;
            case Common.MainAttackType.Medium:
                Damage = Common.MediumDamage;
                break;
            case Common.MainAttackType.Fierce:
                Damage = Common.HeavyDamage;
                break;

        }
    }

    /// <summary>
    /// Handles the cool down.
    /// </summary>
    void HandleCoolDown()
    {
        if (AttackFired && coolDown == 0){
            coolDown = coolDownTime;
            AddTrailComponent();
        }

        if (coolDown > 0)
        {
            //Handle CoolDown Time
            coolDown -= coolDownTimeRate * Time.deltaTime;
            float step = coolDownTime / bezier.points.Length;
            float cooldowntimeleft = coolDownTime - coolDown;

            //Calculate which point the ray should be cast to, as per it's Cooldown time
            int index = (int)Mathf.Clamp(cooldowntimeleft / step, 0, bezier.points.Length - 1);

            Vector3 targetPosition = bezier.points[index];
            Vector3 direction = (targetPosition - transform.position);
            float distance = Vector3.Distance(targetPosition, transform.position) * attackRange;

            float pointDamage = Damage / bezier.points.Length;

            //Cast a ray to catch the objects within effect distance
            Ray attackRay = new Ray(transform.position, direction);
            RaycastHit attackHit;
            Physics.Raycast(attackRay, out attackHit, distance, enemyLayers);

            if (attackHit.collider)
            {
                GameObject enemigo = attackHit.collider.gameObject;
                Enemigo enemy = enemigo.GetComponent<Enemigo>();
                if (enemy)
                {
                    Salud health = enemigo.GetComponent<Salud>();
                    if (health)
                    {
                        health.TakeDamage(pointDamage);
                    }

                    switch (attackReaction)
                    {
                        case Common.AttackReaction.Juggle:
                            MalabarEnemigo malabarEnemigo = enemigo.GetComponent<MalabarEnemigo>();
                            if (malabarEnemigo)
                                malabarEnemigo.AddJuggle(pointDamage * attackReactionPower);
                            break;

                        case Common.AttackReaction.Pushback:
                            Pushback pushback = enemigo.GetComponent<Pushback>();
                            if (pushback)
                            {
                                pushback.AddPushback(pointDamage * attackReactionPower, transform.position);
                            }
                            break;
                    }

                    Debug.Log(attackReaction.ToString());
                    

                    if (cameraShake)
                    {
                        CameraShake.ShakeType ShakeType;
                        switch (attackType)
                        {
                            case Common.MainAttackType.Light:
                                ShakeType = CameraShake.ShakeType.Light;
                                    break;
                            case Common.MainAttackType.Medium:
                                ShakeType = CameraShake.ShakeType.Medium;
                                break;
                            case Common.MainAttackType.Fierce:
                                ShakeType = CameraShake.ShakeType.Strong;
                                break;
                            default:
                                ShakeType = CameraShake.ShakeType.Medium;
                                break;
                        }
                        cameraShake.RequestShake(pointDamage, ShakeType);
                    }
                }
            }


            if (DebugAttack) Debug.DrawRay(transform.position, direction * attackRange, Color.red);



            ///Create trail effect
            if (trailObject)
            {
                Vector3 trailObjectPosition = attackRay.GetPoint(distance);
                trailObject.transform.position = trailObjectPosition;

                LineRenderer line = trailObject.GetComponent<LineRenderer>();
                line.SetPositions(new Vector3[] {
                    trailObject.transform.position
                               ,transform.position
                });
            }                

        }
                                            
        else{
            coolDown = 0;
            AttackFired = false;
            Destroy(trailObject);
        }
    }

    /// <summary>
    /// The trail object.
    /// </summary>
    GameObject trailObject;


    /// <summary>
    /// Adds the trail component.
    /// </summary>
    void AddTrailComponent()
    {

        if (!trailObject)
        {
            trailObject = new GameObject(gameObject.name + "trail");
            //trailObject.transform.parent = gameObject.transform;

            TrailRenderer trail = trailObject.AddComponent<TrailRenderer>();
            trail.colorGradient = trailRenderer.colorGradient;
            trail.minVertexDistance = trailRenderer.minVertexDistance;
            trail.widthCurve = trailRenderer.widthCurve;
            trail.materials = trailRenderer.materials;
            trail.time = trailRenderer.time;
            trail.numCornerVertices = trailRenderer.numCornerVertices;
            trail.numCapVertices = trailRenderer.numCapVertices;
            trail.shadowCastingMode = trailRenderer.shadowCastingMode;
            trail.receiveShadows = trailRenderer.receiveShadows;

            LineRenderer line = trailObject.AddComponent<LineRenderer>();
            line.widthCurve = lineRenderer.widthCurve;
            line.materials = lineRenderer.materials;
            line.colorGradient = lineRenderer.colorGradient;


        }

    }

    void HandleCancel()
    {
        if (cancel)
        {
            coolDown = 0;
            AttackFired = false;
            cancel = false;
            Destroy(trailObject);
        }
    }

    public void Fire(){
        AttackFired = true;
    }

    public bool IsFired()
    {
        return AttackFired;
    }
        

    public void Cancel()
    {
        cancel = true;
    }
	
    public float GetCoolDown(){
        return coolDown;
    }

}
