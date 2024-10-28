using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    public Transform player;
    public Vector3 cameraPosition;
    public float cameraFollowSpeed;
    
    void LateUpdate()
    {
        
        Vector3 positionObjetivo = player.position + 
                                   (player.transform.right * cameraPosition.x) + 
                                   (player.transform.up * cameraPosition.y) + 
                                   (player.transform.forward * cameraPosition.z);
        
        Debug.DrawLine(player.position, positionObjetivo, Color.blue);

        RaycastHit hit;
        if (Physics.Linecast(player.position, positionObjetivo, out hit))
        {
            Vector3 puntoDeImpactoModificado = new Vector3(
                hit.point.x + (hit.normal.x * 0.3f), 
                hit.point.y, 
                hit.point.z + (hit.normal.z * 0.3f) );
            
            positionObjetivo = new Vector3(
                puntoDeImpactoModificado.x,
                puntoDeImpactoModificado.y,
                puntoDeImpactoModificado.z);
        }

        transform.position = Vector3.Lerp(transform.position, positionObjetivo, cameraFollowSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, cameraFollowSpeed * Time.deltaTime);

    }
}