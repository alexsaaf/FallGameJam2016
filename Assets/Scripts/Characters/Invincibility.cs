using UnityEngine;
using System.Collections;

/// <summary>
/// Each character/enemy/whatever that needs to be invincible should be given an instance of this class.
/// It will make their sprite flash for the time it is invincible.
/// </summary>
public class Invincibility {

    private bool isActivated = false;
    private bool displaySprite = false;
    private int defaultInvincibilityDuration = 30; // The number of frames that it will be invincible for
    private float invincibilityTimer = 0;
    private int flashDuration = 5; // The number of frames that it will take for the sprite to flash
    private SpriteRenderer spriteRenderer;

    public Invincibility(SpriteRenderer sr) {
        this.spriteRenderer = sr;
    }

    public void ActivateInvincibility() {
        ActivateInvincibility(defaultInvincibilityDuration);
    }

    public void ActivateInvincibility(float duration) {
        isActivated = true;
        invincibilityTimer = duration;
    }
    
	public void Update () {
	    if (isActivated) {
            if (invincibilityTimer % flashDuration == 0) {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            invincibilityTimer--;
            if (invincibilityTimer <= 0) {
                isActivated = false;
            }
        }
	}

    public bool isInvincible() {
        return isActivated;
    }
}
