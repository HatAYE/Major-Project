﻿using Conversa.Demo;
using Conversa.Runtime;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using System;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] Conversation conversation;
    public Action<string> OnEventTrigger;
    public Action OnDialogueEnd;
    DialogueUI dialogueUI;
    ConversationRunner runner;

    private void Awake()
    {
        dialogueUI = FindObjectOfType<DialogueUI>();
        runner = new ConversationRunner(conversation);
        runner.OnConversationEvent.AddListener(HandleConversationEvent);
    }

    public void BeginDialogue()
    {
        runner.ResetProperties();
        runner.Begin();
    }
    public void NewConversation(Conversation conversation)
    {
        runner = new ConversationRunner(conversation);
        runner.OnConversationEvent.AddListener(HandleConversationEvent);
    }
    void HandleConversationEvent(IConversationEvent e)
    {
        switch (e)
        {
            case MessageEvent messageEvent:
                HandleMessage(messageEvent);
                break;
            case ChoiceEvent choiceEvent:
                HandleChoice(choiceEvent);
                break;
            case ActorMessageEvent actorMessageEvent:
                HandleActorMessageEvent(actorMessageEvent);
                break;
            case ActorChoiceEvent actorChoiceEvent:
                HandleActorChoiceEvent(actorChoiceEvent);
                break;
            case UserEvent userEvent:
                HandleUserEvent(userEvent);
                break;
            case EndEvent _:
                HandleEnd();
                break;
        }
    }

    private void HandleActorMessageEvent(ActorMessageEvent evt)
    {
        var charName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        var avatar = evt.Actor is Character character ? character.Avatar : evt.Actor is DemoActor demoActor ? demoActor.Avatar : null;
        dialogueUI.ShowMessage(charName, evt.Message, evt.Advance, avatar);
    }

    private void HandleActorChoiceEvent(ActorChoiceEvent evt)
    {
        var charName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        var avatar = evt.Actor is Character character ? character.Avatar : evt.Actor is DemoActor demoActor ? demoActor.Avatar : null;
        dialogueUI.ShowChoice(charName, evt.Message, evt.Options, avatar);
    }

    private void HandleMessage(MessageEvent e) => dialogueUI.ShowMessage(e.Actor, e.Message, () => e.Advance());

    private void HandleChoice(ChoiceEvent e) => dialogueUI.ShowChoice(e.Actor, e.Message, e.Options);

    private void HandleUserEvent(UserEvent userEvent)
    {
        OnEventTrigger?.Invoke(userEvent.Name);
    }

    private void HandleEnd()
    {
        dialogueUI.Hide();
        OnDialogueEnd?.Invoke();
    }
}