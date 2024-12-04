using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private bool useSameMultiplier = true;
    [SerializeField] private float multiplier = .5f;

    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteHorizontal = false, infiniteVertical = false;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private Vector2 textureUnitSize;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSize.Set(texture.width / sprite.pixelsPerUnit, texture.height / sprite.pixelsPerUnit);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        Vector2 appliedMultiplier = useSameMultiplier? new Vector2(multiplier, multiplier) : parallaxEffectMultiplier;

        transform.position += new Vector3(deltaMovement.x * appliedMultiplier.x, deltaMovement.y * appliedMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (infiniteHorizontal)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSize.x)
            {
                float offsetPositionX = (transform.position.x - cameraTransform.position.x) % textureUnitSize.x;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
            /*if((cameraTransform.position.x - transform.position.x) <= -textureUnitSize.x)
            transform.Translate(Vector3.right * textureUnitSize.x);*/
        }
        if (infiniteVertical)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSize.y)
            {
                float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSize.y;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
            }
        }

    }
}
