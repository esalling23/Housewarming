using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles a cat hiding in an IWorldObject.
/// </summary>
/// Shake effect from: https://gist.github.com/GuilleUCM/d882e228d93c7f7d0820
public class CatHiding : MonoBehaviour
{
    // Shake animation support
    private Vector3 originPosition;
    private Quaternion originRotation;
    private float shake_decay = 0.002f;
    private float shake_intensity = 0.09f;

    private float temp_shake_intensity = 0;

    private float modifier = .02f;

    private Transform _transform;

    // For the cat-found animation
    private GameObject _cats;
    private float _worldObjHeight;

    void Start()
    {
        _transform = transform;
        _worldObjHeight = gameObject.GetComponent<SpriteWorldObject>().SpriteHeight;
    }

    // Update is called once per frame
    void Update()
    {
        // Animate?
        if (temp_shake_intensity > 0)
        {
            _transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
            _transform.rotation = new Quaternion(
                originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * modifier,
                originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * modifier,
                originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * modifier,
                originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * modifier);
            temp_shake_intensity -= shake_decay;

            if (temp_shake_intensity < 0) temp_shake_intensity = 0;
        }
    }

    private void Shake()
    {
        originPosition = _transform.position;
        originRotation = _transform.rotation;
        temp_shake_intensity = shake_intensity;
    }

    private void OnMouseDown()
    {
        Debug.Log("Found one!");
        LTSeq seq = LeanTween.sequence();

        seq.append(() =>
        {
            GameObject catsPrefab = Resources.Load("Prefabs/Cats") as GameObject;
            // Instantiate the cats
            // SHould be at y pos = height of WO - height of cats so its' hidden
            _cats = Instantiate(catsPrefab, _transform.position, Quaternion.identity, _transform).gameObject;
        });

        seq.append(() => LeanTween.moveY(_cats, _transform.position.y + _worldObjHeight / 2, 1f).setEaseSpring());
        seq.append(1f);

        // last step
        seq.append(() => {
            EventManager.TriggerEvent(EventName.CatFound, new Dictionary<string, object> {
                { "worldObject", GetComponent<IWorldObject>() }
            });
        });
    }

    public void Clear()
    {
        Destroy(_cats);
        Destroy(this);
    }

    private void OnMouseEnter()
    {
        if (temp_shake_intensity == 0)
        {
            Shake();
        }
    }
}
