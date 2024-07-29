using Conversa.Runtime.Events;
using System;
using System.Collections;
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
    public float timePerWord = 0.1f;
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
        Coroutine lineAnimation = StartCoroutine(AnimateLine(message, onContinue));
        nextLineButton.enabled = true;
        

        Action displayFullLine = null;
        displayFullLine += () =>
        {
            StopCoroutine(lineAnimation);
            SetTextTransparency(dialogueLine, 255);
            UpdateSkipAction(onContinue);
        };

        UpdateSkipAction(displayFullLine);
    }

    private void UpdateSkipAction(Action skipAction)
    {
        nextLineButton.onClick.RemoveAllListeners();
        nextLineButton.onClick.AddListener(() => skipAction());
        skipDialogueAction = skipAction;
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
    
    // Instead of adding to the text character by character, this makes the entire text transparent then fades it in one word at a time.
    IEnumerator AnimateLine(string line, Action onContinue)
    {
        dialogueLine.text = line;
        dialogueLine.ForceMeshUpdate();

        SetTextTransparency(dialogueLine);

        string[] words = line.Split(' ');
        int wordStartIndex = 0;
        int wordEndIndex = 0;
        for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
        {
            wordEndIndex += words[wordIndex].Length;
            float time = 0;

            while (time < timePerWord)
            {
                time += Time.deltaTime;
                byte alpha = (byte)(Mathf.Lerp(0, 1, time / timePerWord) * 255);
                SetTextTransparency(dialogueLine, alpha, wordStartIndex, wordEndIndex);
                yield return null;
            }

            wordStartIndex = wordEndIndex + 1;
            wordEndIndex += 1;
        }

        UpdateSkipAction(onContinue);
    }

    public void SetTextTransparency(TextMeshProUGUI dialogueLine, byte alpha = 0, int startIndex = 0, int endIndex = int.MaxValue)
    {
        TMP_TextInfo textInfo = dialogueLine.textInfo;
        endIndex = Mathf.Min(endIndex, textInfo.characterCount - 1);

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            Color32[] newVertexColors = textInfo.meshInfo[materialIndex].colors32;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            Color32 color = newVertexColors[vertexIndex];
            color.a = alpha;

            newVertexColors[vertexIndex + 0] = color;
            newVertexColors[vertexIndex + 1] = color;
            newVertexColors[vertexIndex + 2] = color;
            newVertexColors[vertexIndex + 3] = color;
        }

        dialogueLine.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}