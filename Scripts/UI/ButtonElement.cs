using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonElement : Button
{
    VisualElement buttonElement = UtilsHomeBar.visualTreeStatic.CloneTree();
    // public static UnityEvent resetcolorevent;
    public static Action<bool, bool, bool> buttonnotifier;
    public static Action<int> buttonPressedIndex;
    private Dictionary<Button, EventCallback<ClickEvent>> buttonCallbacks = new Dictionary<Button, EventCallback<ClickEvent>>();


    public ButtonElement(VisualElement parent, string title, string filter, Sprite image, Sprite icon, int index)
    {      
        VisualElement cardButton = new VisualElement();
        cardButton = buttonElement;
        cardButton.name = title.ToLower();
        
        var buttonClicked = cardButton.Q<Button>();
        buttonClicked.name = title;

        buttonClicked.AddToClassList("CardButton");

        cardButton.AddToClassList("ButtonCard");
        cardButton.style.display = DisplayStyle.Flex;
        parent.Add(cardButton);

        Label buttonTitle = cardButton.Q<Label>(className: "buttonTitle");

        buttonTitle.text = title;  
        
        VisualElement buttonImage = cardButton.Q<VisualElement>(className: "buttonImage");
        buttonImage.style.backgroundImage = new StyleBackground(image);
        
        VisualElement buttonIcon = cardButton.Q<VisualElement>(className: "buttonIcon");
        buttonIcon.style.backgroundImage = new StyleBackground(icon);
        buttonIcon.style.unityBackgroundImageTintColor = UIExtentions.grey05;
        
        Label buttonFilterText = cardButton.Q<Label>(className: "buttonFilterText");
        buttonFilterText.text = filter;       

        // buttonClicked.clicked += () => 
        // {
        //     OnButtonClick(index);
        // };
        EventCallback<ClickEvent> callback = (ClickEvent evt) => 
        {
            OnButtonClick(index);
        };
        buttonClicked.RegisterCallback(callback);
        buttonCallbacks[buttonClicked] = callback;
    }
    public void OnButtonClick(int index)
    {
        UtilsHomeBar.infoCheck32 = true;
        UtilsHomeBar.SelectedPOIButton(index);

        if(GameObject.Find("GameManager").GetComponent<StateTracker>().gameState == StateTracker.GameState.Routes)
        {
            if(InfoState.staticState.isStarted == true)
            {
                buttonnotifier?.Invoke(true, false, false);
            }
            else if(InfoState.staticState.isStarted == false)
            {
                buttonnotifier?.Invoke(true, true, false); 
            }
        }
        else if(GameObject.Find("GameManager").GetComponent<StateTracker>().gameState == StateTracker.GameState.Map)
        {
            if(MapState.staticStartState.isStarted == true)
            {
                buttonnotifier?.Invoke(true, false, false);
            }
            else if(MapState.staticStartState.isStarted == false)
            {
                buttonnotifier?.Invoke(true, true, false); 
            }
        }


        buttonPressedIndex?.Invoke(index);
        // Handle the button click event with the provided index
    }
    public void UnregisterButtonCallbacks()
    {
        foreach (var kvp in buttonCallbacks)
        {
            kvp.Key.UnregisterCallback(kvp.Value);
        }
        buttonCallbacks.Clear();
    }
}
