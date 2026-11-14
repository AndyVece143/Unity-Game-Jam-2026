using System.Collections;
using UnityEngine;

public class ThrownKey : MonoBehaviour
{
    public int rotateSpeed;
    public float distance;
    public float duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //float distance = transform.position.x * -1;
        Debug.Log(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * (rotateSpeed * Time.deltaTime));
    }

    public IEnumerator GoRight()
    {
        Vector3 ogPosition = transform.position;
        Vector3 newPosition = new Vector3(distance, ogPosition.y, ogPosition.z);
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector3.Lerp(ogPosition, newPosition, t);
            yield return null;
        }
    }
}
