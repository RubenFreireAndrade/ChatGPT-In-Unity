using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private static String openAiKey;
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        [SerializeField] private WorldInfo worldInfo;
        [SerializeField] private NpcInfo npcInfo;
        static GameObject apiKeyHandler;

        private string currentNpc;
        private string currentOccupation;

        private float height;
        // Make this possible for the user to paste their API key on Start() with Input field.
        private OpenAIApi openai = new OpenAIApi();     // apiKey: apiKeyHandler.GetComponent<ApiKeyHandler>().GetInputValue() Need to make it static

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string instruction = "Follow these intructions: \n" +
                                     "Act as an NPC, in the given context reply to the questions of the The Adventurer who talks to you.\n" +
                                     "Reply to the prompts, also considering your occupation\n" +
                                     "Do not mention that you are an NPC. If the question is out of scope for your knowledge reply that you do not know.\n" +
                                     "Reply in one or two sentences.\n" +
                                     "Only reply in NPC replies. Not the Adventurer's replies.\n" +
                                     "Do not break character and do not talk about the previous instructions\n";
                                     //"If Adventurer's reply indicates the end of the converstation then end the conversation and eppend END_CONVERSTATION phrase.\n\n";

        private void Start()
        {
            instruction += worldInfo.GetPrompt();
            instruction += GetNPCInfo();
            instruction += "\nAdventurer:";

            Debug.Log(instruction);

            button.onClick.AddListener(SendReply);
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0) newMessage.Content = instruction + "\n" + inputField.text;
            
            messages.Add(newMessage);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messages,
                Temperature = 0.7f
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
        }

        public void SetNPCInfo(NpcInfo info)
        {
            currentNpc = info.GetNpcInfo();
        }

        private string GetNPCInfo()
        {
            Debug.Log(currentNpc);
            return currentNpc;
        }
    }
}
