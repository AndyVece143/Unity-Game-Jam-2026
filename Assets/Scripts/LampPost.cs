using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LampPost : MonoBehaviour
{
    public Light2D coolLight;
    public float timer;
    private float timerMax;
    public TownManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerMax = timer;
        manager = TownManager.FindAnyObjectByType<TownManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.gameProgress >= 2)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                LightCheck();
            }
        }

    }

    private void LightCheck()
    {
        int i = Random.Range(0, 7);
        if (i == 6)
        {
            StartCoroutine(LightFlicker());
        }

        timer = timerMax;
    }

    private IEnumerator LightFlicker()
    {
        coolLight.enabled = false;
        yield return new WaitForSeconds(0.1f);
        coolLight.enabled = true;
    }
}
