using Conversa.Demo;
using Conversa.Runtime;
using Conversa.Runtime.Events;
using Conversa.Runtime.Interfaces;
using System;

public class DialogueController
{
    public Action<string> OnEventTrigger;
    public Action OnDialogueEnd;
    ConversationRunner runner;

    public DialogueController(Conversation conversation)
    {
        NewConversation(conversation);
    }

    public DialogueController() { }

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
        DialogueUI.Instance.ShowMessage(charName, evt.Message, evt.Advance, avatar);
    }

    private void HandleActorChoiceEvent(ActorChoiceEvent evt)
    {
        var charName = evt.Actor == null ? "" : evt.Actor.DisplayName;
        var avatar = evt.Actor is Character character ? character.Avatar : evt.Actor is DemoActor demoActor ? demoActor.Avatar : null;
        DialogueUI.Instance.ShowChoice(charName, evt.Message, evt.Options, avatar);
    }

    private void HandleMessage(MessageEvent e) => DialogueUI.Instance.ShowMessage(e.Actor, e.Message, () => e.Advance());

    private void HandleChoice(ChoiceEvent e) => DialogueUI.Instance.ShowChoice(e.Actor, e.Message, e.Options);

    private void HandleUserEvent(UserEvent userEvent)
    {
        OnEventTrigger?.Invoke(userEvent.Name);
    }

    private void HandleEnd()
    {
        DialogueUI.Instance.Hide();
        OnDialogueEnd?.Invoke();
    }
}