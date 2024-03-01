using Conversa.Demo;
using Conversa.Runtime;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] Conversation conversation;
    DialogueUI dialogueUI;
    public ConversationRunner runner;

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
        var avatar = evt.Actor is Character character ? character.Avatar :
             evt.Actor is DemoActor demoActor ? demoActor.Avatar : null;
        dialogueUI.ShowMessage(charName, evt.Message, evt.Advance, avatar);
    }

    private void HandleActorChoiceEvent(ActorChoiceEvent evt)
    {
        var charName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        var avatar = evt.Actor is Character character ? character.Avatar :
             evt.Actor is DemoActor demoActor ? demoActor.Avatar : null;
        dialogueUI.ShowChoice(charName, evt.Message, evt.Options, avatar);
    }

    private void HandleMessage(MessageEvent e) => dialogueUI.ShowMessage(e.Actor, e.Message, () => e.Advance());

    private void HandleChoice(ChoiceEvent e) => dialogueUI.ShowChoice(e.Actor, e.Message, e.Options);

    private static void HandleUserEvent(UserEvent userEvent)
    {
        // I don't even know man. This passes in a string, and I'm supposed to use that to help me invoke an event somehow?
        // Leaving this blank until I figure out a way to do this besides hardcoding.
    }

    private void HandleEnd()
    {
        dialogueUI.Hide();
    }
}