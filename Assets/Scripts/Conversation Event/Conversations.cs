using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversations : MonoBehaviour
{
    public int ConversationIndex = 0;

    List<Dictionary<List<(string, Color)>, string>> Dialogues = new List<Dictionary<List<(string, Color)>, string>>() {
        new Dictionary<List<(string, Color)>, string>()
        {
            {new List<(string, Color)>(){ ("Guard", Color.white) }, "Sir, research director Dr. Liam Bennett has arrived." },
            {new List<(string, Color)>(){ ("William Strauss", Color.red) }, "Let him in." },
            {new List<(string, Color)>(){ ("Guard", Color.white) }, "*Opens the door*" },
            {new List<(string, Color)>(){ ("Liam Bennett", Color.green) }, "*Walks in* Good morning Mr. Strauss." },
            {new List<(string, Color)>(){ ("William Strauss", Color.red) }, "Good morning Mr. Bennett. You're probably aware of why it is I called you here." },
            {new List<(string, Color)>(){ ("Liam Bennett", Color.green) }, "It's about that new project, isn't it?" },
            {new List<(string, Color)>(){ ("William Strauss", Color.red) }, "Good, then I can skip the introduction. We want your team to begin today. The government has requested additional...'aids'. The products we have provided thus far is not nearly enough." },
            {new List<(string, Color)>(){ ("Liam Bennett", Color.green) }, "Not nearly enough? What do you mean? We-" },
            {new List<(string, Color)>(){ ("William Strauss", Color.red) }, "We need something stronger. Something that could ensure our victory, with no room for doubts. Our enemies have become accustomed to our old products. We need something new." },
            {new List<(string, Color)>(){ ("Liam Bennett", Color.green) }, "But sir, what would our people say if they knew about this? They were furious when they learned about our past product! About- about him-" },
            {new List<(string, Color)>(){ ("William Strauss", Color.red) }, "There is no room for debate. You know what they will do, don't question them, Mr. Bennett." },
            {new List<(string, Color)>(){ ("Liam Bennett", Color.green) }, "I'm sorry, Mr. Strauss. This is completely unreasonable. You assured me, assured everyone, that 'he' was the last one we'd make. That we wouldn't go back to those days ever again." },
            {new List<(string, Color)>(){ ("William Strauss", Color.red) }, "Times are changing, Mr. Bennett. If we remained where we are today, where would our country be? Where would WE be? These products are what is bringing in revenue, Mr. Bennett." },
            {new List<(string, Color)>(){ ("Liam Bennett", Color.green) }, "I'm sorry, I can't. I spent 20 years with this deep seated guilt. Our products cries of pain haunts me every night...and the damn protests do not help. " },
            {new List<(string, Color)>(){ ("Liam Bennett", Color.green) }, "I'm sorry, but I'm going to turn in my resignat-" },
            {new List<(string, Color)>(){ ("Guard", Color.white) }, "*Gun shot*" },
            {new List<(string, Color)>(){ ("William Strauss", Color.red) }, "Resigning was never an option, Mr. Bennett." }
        }

};

    public Dictionary<List<(string, Color)>, string> GetConversations()
    {
        return Dialogues[ConversationIndex];
    }
}
