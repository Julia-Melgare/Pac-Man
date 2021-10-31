using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class AirConsoleManager : MonoBehaviour
{
    public Dictionary<int, PlayerInput> players = new Dictionary<int, PlayerInput>();
    private GameManager gameManager;

    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onReady += OnReady;
        AirConsole.instance.onConnect += OnConnect;
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnReady(string code)
    { 
        List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();
        foreach (int deviceID in connectedDevices)
        {
            AddNewPlayer(deviceID);
        }
    }

    void OnConnect(int device)
    {
        AddNewPlayer(device);
    }

    private void AddNewPlayer(int deviceID)
    {

        if (players.ContainsKey(deviceID))
        {
            return;
        }
        if (players.Count <= 0)
        {
            players.Add(deviceID, gameManager.pacman.GetComponent<PacmanPlayerInput>());
        }
        else
        {
            Ghost playerGhost = gameManager.ghosts[players.Count - 1];
            playerGhost.EnablePlayerInput();
            players.Add(deviceID, playerGhost.GetComponent<PlayerInput>());
        }
    }

    void OnMessage(int from, JToken data)
    {
        if (players.ContainsKey(from) && data["element"] != null)
        {
            if(data["element"].ToString() == "dpad")
            {
                if (data["data"]["key"] != null)
                {
                    players[from].ButtonInput(data["data"]["key"].ToString());
                }   
            }
        }
    }

    void OnDestroy()
    {
        if (AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
            AirConsole.instance.onReady -= OnReady;
            AirConsole.instance.onConnect -= OnConnect;
        }
    }
}

