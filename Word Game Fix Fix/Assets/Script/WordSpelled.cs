using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace EventCallbacks
{
    public class WordSpelled : MonoBehaviour
    {
        char[] word = new char[0];
        int wordLenght;
        int amountOfChars;
        // Start is called before the first frame update
        void Awake()
        {
            EventSystem.Current.RegisterListener(EVENT_TYPE.UNIT_PLACED, OnUnitPlace);
            EventSystem.Current.RegisterListener(EVENT_TYPE.WORD_RESET, ResetWord);

        }
        private void ResetWord(Event eventInfo)
        {
            print("here2");
            RestartEvent re = (RestartEvent)eventInfo;
            wordLenght = re.groupLenght;
            word = new char[wordLenght];
            amountOfChars = 0;

        }

        private void OnUnitPlace(Event eventInfo)
        {
            UnitPlacedEvent upe = (UnitPlacedEvent)eventInfo;

            char c = upe.letterPlaced;
            int i = upe.boxPlacedIn;
            print(c);
            print(word.ToString());

            word[i] = c;
            amountOfChars++;

            print(amountOfChars + " in box of " + wordLenght);

            if(amountOfChars >= wordLenght)
            {
                CheckWord();
            }


        }

        private void CheckWord()
        {

            NetSpell.SpellChecker.Dictionary.WordDictionary oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();


            oDict.DictionaryFile = "en-US.dic";
            oDict.Initialize();
            //string wordToCheck = new string(word);
            string wordToCheck = "door";
            NetSpell.SpellChecker.Spelling oSpell = new NetSpell.SpellChecker.Spelling();

            oSpell.Dictionary = oDict;
            if (oSpell.TestWord(wordToCheck))
            {
                print(wordToCheck + " EXISTS");
                //Word does not exist in dictionary
                
            }


        }
    }
}
