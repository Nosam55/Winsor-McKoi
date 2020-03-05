﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    /* UNITY INSPECTOR VARIABLES */
    [SerializeField]
    private FishMachine fishMachine;
    [SerializeField]
    private OSC oscServer;

    /* MEMBER VARIABLES */
    private readonly Dictionary<KeyCode, FishMachine.Interaction> controls;

    public InputManager()
    {
        //Define the mapping between the controls available to the user and the valid interactions
        controls = new Dictionary<KeyCode, FishMachine.Interaction>()
        {
            { KeyCode.S, FishMachine.Interaction.SPRINKLING },
            { KeyCode.P, FishMachine.Interaction.POINTING },
            { KeyCode.T, FishMachine.Interaction.TRACKING }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        oscServer.SetAddressHandler("/feedfish", OnFeedFish);
        oscServer.SetAddressHandler("/petfish", OnPetFish);

        //If our fish machine was not specified in the Unity Editor, throw an informative exception now :)
        if(fishMachine == null)
        {
            throw new UnityException("FishMachine for InputManager not defined!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if any of our inputs are being detected this frame
        foreach(KeyValuePair<KeyCode, FishMachine.Interaction> pair in controls)
        {
            //If one of the keys we are watching for is pressed
            if (Input.GetKeyDown(pair.Key)) //We use GetKeyDown() to ensure each key press simulates a discrete interacton
            {
                //Then tell the fishmachine
                fishMachine.Interact(pair.Value);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
                
        }
    }

    void OnFeedFish(OscMessage message)
    {
        Debug.Log($"FeedFish: {message.ToString()}");
        fishMachine.Interact(FishMachine.Interaction.TWEETFEED);
    }

    void OnPetFish(OscMessage message)
    {
        Debug.Log($"PetFish: {message.ToString()}");
        fishMachine.Interact(FishMachine.Interaction.TWEETPET);
    }
}
