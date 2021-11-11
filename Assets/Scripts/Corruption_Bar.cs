using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Corruption_Bar : MonoBehaviour
{
    public ParticleSystem particleSys;
    private Slider slider;
    public float FillSpeed = 0.1f;
    private float targetProgress = 1.0f;
    private int statusUp = 0;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        //particleSys = GameObject.Find("Fill particle").GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            CorruptionCollection(.05f);
            if (slider.value < targetProgress)
            {


                slider.value += FillSpeed * Time.deltaTime;
                //if (particleSys.isPlaying)
                    //particleSys.Play();

            }
            else
            {
                //particleSys.Stop();

            }
            return;
        }
        if(slider.value == 1.0f)
        {
            slider.value = 0.0f;
            statusUp += 1;
        }
    }

    //Collecting corruption
    public void CorruptionCollection(float newCorruption)
    {
        targetProgress = slider.value += newCorruption;
    }
}