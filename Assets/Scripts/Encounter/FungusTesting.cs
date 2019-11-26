using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusTesting : Singleton<FungusTesting>
{
    public Flowchart flowchart;
    public GameObject flowchartGameObject;

    private List<Block> previousQuestionChoices = new List<Block>();
    private List<Block> currentQuestionChoices = new List<Block>();

    private int commandIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        flowchartGameObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildQuestions()
    {
        for (int i = 0; i < StorageManager.Instance.currentEncounter.GetComponent<EncounterMono>().Question.Count; i++)
        {
            StorageManager.Instance.SetCurrentQuestion(i);

            if (currentQuestionChoices.Count != 0)
            {
                previousQuestionChoices.AddRange(currentQuestionChoices);
                currentQuestionChoices.Clear();
            }
            
            StringVariable stringQuestion = flowchartGameObject.AddComponent<StringVariable>();
            stringQuestion.Key = "Question" + (i + 1);

            flowchart.Variables.Add(stringQuestion);
            flowchart.SetVariable("Question" + (i + 1), stringQuestion);
            flowchart.SetStringVariable("Question" + (i + 1), StorageManager.Instance.GetQuestion(StorageManager.Instance.currentQuestion).QuestionString);


            Block questionBlock = flowchart.CreateBlock(new Vector2(i*15, i*15));
            questionBlock.BlockName = "Question" + (i + 1);

            Say sayQuestion = flowchartGameObject.AddComponent<Say>();
            sayQuestion.SetStandardText("{$Question" + (i + 1) + "}");
            sayQuestion.fadeWhenDone = false;
            sayQuestion.waitForClick = false;
            sayQuestion.ParentBlock = questionBlock;
            sayQuestion.CommandIndex = commandIndex++;
            questionBlock.CommandList.Add(sayQuestion);

            SetQuestionValues(questionBlock, i, i == (StorageManager.Instance.currentEncounter.GetComponent<EncounterMono>().Question.Count - 1));

            if (i != 0)
            {
                for (int j = 0; j < previousQuestionChoices.Count; j++)
                {
                    Call call =(Call) previousQuestionChoices[j].CommandList.Find(x => x.GetType().Equals(typeof(Call)));
                    call.targetBlock = questionBlock;
                }
            }
            flowchart.CheckItemIds();

            commandIndex = 0;
        }
    }

    public void SetQuestionValues(Block questionBlock, int questionId, bool lastQuestion)
    {
        for (int i = 0; i < StorageManager.Instance.currentQuestion.GetComponent<QuestionMono>().Choices.Count; i++)
        {
            Block choiceBlock = flowchart.CreateBlock(new Vector2(i * 100, i * 100));
            choiceBlock.BlockName = questionId + "Choice" + (i + 1);
            
            StringVariable choiceName = flowchartGameObject.AddComponent<StringVariable>();

            choiceName.Key = questionId + "Choice" + (i + 1);

            flowchart.Variables.Add(choiceName);
            flowchart.SetVariable(questionId + "Choice" + (i + 1), choiceName);
            flowchart.SetStringVariable(questionId + "Choice" + (i + 1), StorageManager.Instance.GetChoice(StorageManager.Instance.currentChoices[i]).ChoiceString);

            Fungus.Menu menuChoice = flowchartGameObject.AddComponent<Fungus.Menu>();
            menuChoice.SetStandardText("{$" + questionId + "Choice" + (i + 1) + "}");
            menuChoice.ParentBlock = questionBlock;
            menuChoice.CommandIndex = commandIndex++;

            Block targetBlock = choiceBlock;
            ChoiceMono choiceMono = StorageManager.Instance.currentChoices[i].GetComponent<ChoiceMono>();
            if (choiceMono.Correct)
            {
               // targetBlock = flowchart.FindBlock("Correct");

                menuChoice.targetBlock = targetBlock;

                Say sayCorrect = flowchartGameObject.AddComponent<Say>();
                sayCorrect.SetStandardText("{$Correct}");
                //sayCorrect.fadeWhenDone = false;
                //sayCorrect.waitForClick = false;
                sayCorrect.ParentBlock = choiceBlock;
                sayCorrect.CommandIndex = 0;
                targetBlock.CommandList.Add(sayCorrect);

                if (lastQuestion)
                {
                    SendMessage sendMessageOnePoint = flowchartGameObject.AddComponent<SendMessage>();
                    StringData onePoint = new StringData("One Point");
                    sendMessageOnePoint._message = onePoint;
                    sendMessageOnePoint.ParentBlock = choiceBlock;
                    sendMessageOnePoint.CommandIndex = 1;
                    targetBlock.CommandList.Add(sendMessageOnePoint);

                    SendMessage sendMessageStop = flowchartGameObject.AddComponent<SendMessage>();
                    StringData message = new StringData("Stop");
                    sendMessageStop._message = message;
                    sendMessageStop.ParentBlock = choiceBlock;
                    sendMessageStop.CommandIndex = 2;
                    targetBlock.CommandList.Add(sendMessageStop);
                }
                else
                {
                    currentQuestionChoices.Add(targetBlock);

                    Call callNextQuestion = flowchartGameObject.AddComponent<Call>();
                    //callNextQuestion.targetBlock = questionBlock;
                    callNextQuestion.ParentBlock = choiceBlock;
                    callNextQuestion.CommandIndex = commandIndex++;
                    targetBlock.CommandList.Add(callNextQuestion);
                }

                questionBlock.CommandList.Add(menuChoice);

            }
            else
            {
                //targetBlock = flowchart.FindBlock("Wrong");

                menuChoice.targetBlock = targetBlock;

                Say sayIncorrect = flowchartGameObject.AddComponent<Say>();
                sayIncorrect.SetStandardText("{$Incorrect}");
                //sayIncorrect.fadeWhenDone = false;
                //sayCorrect.waitForClick = false;
                sayIncorrect.ParentBlock = choiceBlock;
                sayIncorrect.CommandIndex = 0;
                targetBlock.CommandList.Add(sayIncorrect);


                SendMessage sendMessageDead = flowchartGameObject.AddComponent<SendMessage>();
                StringData dead = new StringData("Dead");
                sendMessageDead._message = dead;
                sendMessageDead.ParentBlock = choiceBlock;
                sendMessageDead.CommandIndex = 1;
                targetBlock.CommandList.Add(sendMessageDead);

                if (lastQuestion)
                {
                    SendMessage sendMessageStop = flowchartGameObject.AddComponent<SendMessage>();
                    StringData message = new StringData("Stop");
                    sendMessageStop._message = message;
                    sendMessageStop.ParentBlock = choiceBlock;
                    sendMessageStop.CommandIndex = 2;
                    targetBlock.CommandList.Add(sendMessageStop);
                }
                else
                {
                    currentQuestionChoices.Add(targetBlock);

                    Call callNextQuestion = flowchartGameObject.AddComponent<Call>();
                    //callNextQuestion.targetBlock = questionBlock;
                    callNextQuestion.ParentBlock = choiceBlock;
                    callNextQuestion.CommandIndex = commandIndex++;
                    targetBlock.CommandList.Add(callNextQuestion);
                }

                questionBlock.CommandList.Add(menuChoice);
            }
            
        }
    }
}
