using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EyeAnimation : MonoBehaviour
{
    [SerializeField] private Transform _eyePupil;
    [SerializeField] private Animator _animator;
    public float speed = 10;
    public float intensity = 0.3f;

    private void Start()
    {
        StartCoroutine(StartAnim(Random.Range(0.0f, 2.0f)));
    }

    IEnumerator StartAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.Play("Blink");
    }

    void Update()
    {
        /* Get the mouse position in world space rather than screen space. */
        var mouseWorldCoord = Camera.main.ScreenPointToRay(Input.mousePosition).origin;

        /* Get a vector pointing from initialPosition to the target. Vector shouldn't be longer than maxDistance. */
        var originToMouse = mouseWorldCoord - transform.position;
        originToMouse = Vector3.ClampMagnitude(originToMouse, intensity);

        /* Linearly interpolate from current position to mouse's position. */
        _eyePupil.transform.position = Vector3.Lerp(_eyePupil.transform.position, transform.position + originToMouse, speed * Time.deltaTime);
    }
}
