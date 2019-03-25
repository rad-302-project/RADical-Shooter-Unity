﻿using Assets.Scenes.Default.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerListener : MonoBehaviour // This script is solely responsible for obtaining information from the server.
{
    public static ServerListener instance;
    public bool LoggedIn;
    

    SignalRController signalRController;
    UiController uiController;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {              
        signalRController = GameObject.Find("SignalRController").GetComponent<SignalRController>();
        uiController = GameObject.Find("Controller_Menu").GetComponent<UiController>();
    }

    void Update()
    {
        //if (matchInProgress && player1Respawn.RemainingStocks == 0 || matchInProgress && player2Respawn.RemainingStocks == 0)
        //{
        //    EndMatch();
        //}
    }   

    void EndMatch()
    {
        // End the match.
        //matchInProgress = false;

        // We should load a results screen scene here.       

        // Upload results to the database.
        UploadMatchResults();
    }

    // This method sends the match results to the signalR Controller which in turn will send them to the server.
    public void UploadMatchResults()
    {
        // Apply values to signalR variables. I'll likely use a more graceful method later on.
        //signalRController.p1Username = player1.Username;
        //signalRController.p2Username = player2.Username;
        //signalRController.p1StockCount = player1Respawn.RemainingStocks;
        //signalRController.p2StockCount = player2Respawn.RemainingStocks;

        signalRController.UploadMatchResults();
    }

    public void OnReceiveResults(ApplicationUser winner, ApplicationUser loser)
    {
        print(winner.UserName + " has won the match!");

        print("Winner: " + winner.UserName + " Total Wins: " + winner.Wins + " Total Losses: " + winner.Losses);
        print("Runner-up: " + loser.UserName + " Total Wins: " + loser.Wins + " Total Losses: " + loser.Losses);
    }

    public void OnReceiveRegistrationMessage(string status, string input)
    {
        if (status.ToUpper() == "EMAIL TAKEN")
        {
            uiController.UpdateServerFeedback(input + " already has a NULL VOID account attached to it!");                      
        }

        else if (status.ToUpper() == "USERNAME TAKEN")
        {
            uiController.UpdateServerFeedback("The username " + "'" + input + "'" + " has already been taken.");           
        }

        else if (status.ToUpper() == "SUCCESS")
        {
            uiController.UpdateServerFeedback("Welcome to NULL VOID, " + input + "! Please log in so you can play the game!");           
        }
    }

    public void OnReceiveLoginMessage(string status, string username, int wins, int losses)
    {
        if (status.ToUpper() == "NOT FOUND")
        {
            uiController.UpdateServerFeedback("No player named " + "'" + username + "'" + " found!");
            print("No player named " + "'" + username + "'" + " found!");
        }

        else if (status.ToUpper() == "PASSWORD INVALID")
        {
            uiController.UpdateServerFeedback("Invalid password.");
            //print("Invalid password.");
        }

        else if (status.ToUpper() == "PASSWORD VALID")
        {
            LoggedIn = true;
            uiController.UpdateServerFeedback("Welcome back, " + username + "!");
            uiController.EnableLoginMode(username, wins, losses);
            // Give user info to the UI controller. 
        }       
    }
}