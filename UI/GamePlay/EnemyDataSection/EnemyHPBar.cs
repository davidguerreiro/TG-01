﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour {

    [Header("Components")]

    public FadeElement fill;                                // Fill fade class component reference.

    [Header("Settings")]
    // public float offset;                                    // Extra value to ensure the bar is not too slow for enemies with low HP.
    // public float toIgnoreOffset;                            // Ignore offset for enemy with HP superior to this amount.

    private Slider _slider;                                 // Slider component reference.
    private RectTransform _rect;                            // Rect transform component reference.


    // Start is called before the first frame update
    void Awake() {
        Init();   
    }

    /// <summary>
    /// Update enemy HP value.
    /// </summary>
    /// <param name="currentHP">float - new enemy HP value.</param>
    public void UpdateHP( float currentHP ) {
        if ( _slider != null ) {
            _slider.value = currentHP;

            if ( currentHP <= 0 ) {
                fill.gameObject.SetActive( false );
            }
        }

    }

    /// <summary>
    /// Set up enemy HP bar for new enemy.
    /// </summary>
    /// <param name="currentHP">float - enemy current HP</param>
    /// <param name="maxHP">float - enemy max HP</param>
    public void SetUp( float currentHP, float maxHP ) {

        if ( _slider != null /* && _rect != null */ ) {

            // set up HP values.
            _slider.maxValue = maxHP;
            _slider.value = currentHP;

            // resize enemy HP bar to adjust to enemy max HP.
            // float toAdd = ( maxHP < toIgnoreOffset ) ? maxHP + offset : maxHP;
            // _rect.SetInsetAndSizeFromParentEdge( RectTransform.Edge.Right, 0, toAdd );

            if ( currentHP > 0f ) {
                fill.gameObject.SetActive( true );
            }
        }        
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init() {

        // get slider component refernece.
        _slider = GetComponent<Slider>();

        // get rect transform component reference.
        // _rect = GetComponent<RectTransform>();
    }
}
