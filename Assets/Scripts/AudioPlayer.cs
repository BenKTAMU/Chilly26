using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip start_clip;
    public AudioClip repeat_clip;

    public AudioSource player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.PlayOneShot(start_clip);

        StartCoroutine(PlayMain());
    }

    public void Reset()
    {
        float time = player.time;
        player.time = 0;
        player.clip = start_clip;
        player.Play();

        StartCoroutine(FinishReset(time));
    }

    IEnumerator FinishReset(float time)
    {
        yield return new WaitForSeconds(7f);
        player.Stop();
        player.clip = repeat_clip;
        player.time = time;
        player.Play();
    }

    IEnumerator PlayMain()
    {
        yield return new WaitForSeconds(7f);

        player.clip = repeat_clip;
        player.time = 0f;
        player.Play();
        yield return new WaitForSeconds(65.6f);
        player.Stop();

        while (true)
        {
            player.clip = repeat_clip;
            player.time = 5.5f;
            player.Play();
            yield return new WaitForSeconds(60.1f);
            player.Stop();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
