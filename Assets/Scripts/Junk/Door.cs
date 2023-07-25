using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float openPositionY;
    [SerializeField]
    private float doorSpeed = 1f;

    public void OpenDoor()
    {
        StartCoroutine(OpenDoors());
    }

    private IEnumerator OpenDoors()
    {
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = new Vector3(initialPosition.x, openPositionY, initialPosition.z);

            float distance = Vector3.Distance(initialPosition, targetPosition);
            float duration = distance / doorSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

                transform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

                yield return null;
            }

            // Disable the collider component of the door to allow walking through
            Collider doorCollider = GetComponent<Collider>();
            if (doorCollider != null)
                doorCollider.enabled = false;

            // Door transition complete
    }

}
