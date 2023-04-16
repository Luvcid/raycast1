using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycast : MonoBehaviour
{
    public LineRenderer LineOfSight;
    public int reflection;
    public float MaxRayDistance;
    public LayerMask LayerDetection;
    public float rotationSpeed;

    private void Start() 
    {
        Physics2D.queriesStartInColliders = false;
    }
    
    private void Update() 
    {
        transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);

        LineOfSight.positionCount =1;
        LineOfSight.SetPosition(0, transform.position);

        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, MaxRayDistance, LayerDetection);

        bool IsMirror = false;
        Vector2 mirrorHitpoint = Vector2.zero;
        Vector2 mirrorHitnormal = Vector2.zero;

        for (int i = 0; i < reflection; i++)
        {
            LineOfSight.positionCount += 1;
            if (hitinfo.collider != null)
            {
                LineOfSight.SetPosition(LineOfSight.positionCount - 1, hitinfo.point);

                IsMirror =  false;
                if (hitinfo.collider.CompareTag("Mirror"))
                {
                    mirrorHitpoint = (Vector2)hitinfo.point;
                    mirrorHitnormal = (Vector2)hitinfo.normal;
                    hitinfo = Physics2D.Raycast((Vector2)hitinfo.point, Vector2.Reflect(hitinfo.point, hitinfo.normal), MaxRayDistance, LayerDetection);
                    IsMirror = true;
                    
                }
                else 
                    break;

            }
            else 
            {
                if (IsMirror) 
                {
                    LineOfSight.SetPosition(LineOfSight.positionCount - 1, mirrorHitpoint + Vector2.Reflect(mirrorHitpoint, mirrorHitnormal) * MaxRayDistance);
                    break;
                }
                else 
                {
                    LineOfSight.SetPosition(LineOfSight.positionCount - 1, transform.position + transform.right * MaxRayDistance);
                    break;
                }
            }
        }


    }

}
