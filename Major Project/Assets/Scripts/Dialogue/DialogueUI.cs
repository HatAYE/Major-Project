using Conversa.Runtime.Events;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] GameObject dialogueWindow;
    [SerializeField] GameObject choiceWindow;
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI dialogueLine;
    [SerializeField] Button nextLineButton;
    [SerializeField] GameObject choiceOptionButtonPrefab;
    public bool inDialogue => dialogueWindow.activeSelf;
    public static DialogueUI Instance { get; private set; }
    Action skipDialogueAction;
    public bool canSkipDialogue;
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(this); }
    }

    private void Start()
    {
        dialogueWindow.SetActive(false);
        choiceWindow.SetActive(false);
        characterImage.gameObject.SetActive(false);
        canSkipDialogue = true;
    }
    void Update()
    {
        if (inDialogue && skipDialogueAction != null)
        {
            if(canSkipDialogue)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
                {
                    SkipDialogue();
                }
            }
        }
    }
    public void ShowMessage(string actor, string message, Action onContinue, Sprite avatar = null)
    {
        choiceWindow.SetActive(false);
        dialogueWindow.SetActive(true);
        UpdateGameState();
        UpdateImage(avatar);
        characterName.text = actor;
        dialogueLine.text = message;

        nextLineButton.enabled = true;
        nextLineButton.onClick.RemoveAllListeners();
        nextLineButton.onClick.AddListener(() => onContinue());
        skipDialogueAction = onContinue;
    }

    void UpdateGameState()
    {
        if (dialogueWindow.activeSelf || choiceWindow.activeSelf)
        {
            GameManager.instance.ChangeState(gameStates.inDialogue);
        }
        else GameManager.instance.ChangeState(gameStates.playing);
    }

    public void ShowChoice(string characterName, string line, List<Option> options, Sprite avatar = null)
    {
        dialogueWindow.SetActive(false);
        if (avatar != null) { UpdateImage(avatar); }
        this.characterName.text = characterName;
        dialogueLine.text = line;
        nextLineButton.enabled = false;

        choiceWindow.SetActive(true);

        foreach (Transform child in choiceWindow.transform)
            Destroy(child.gameObject);

        options.ForEach(option =>
        {
            var instance = Instantiate(choiceOptionButtonPrefab, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(choiceWindow.transform);
            instance.GetComponentInChildren<Text>().text = option.Message;
            instance.GetComponent<Button>().onClick.AddListener(() => option.Advance());
        });
    }

    private void UpdateImage(Sprite sprite)
    {
        characterImage.gameObject.SetActive(sprite != null);
        characterImage.sprite = sprite;
    }

    public void Hide()
    {
        dialogueWindow.SetActive(false);
        choiceWindow.SetActive(false);
        characterImage.sprite = null;
        characterImage.gameObject.SetActive(false);
        UpdateGameState();
    }

    public void SkipDialogue()
    {
        skipDialogueAction.Invoke();
    }
}