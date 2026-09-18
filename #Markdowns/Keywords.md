**Properties** are specialized members typically written as:
(Accessibility Modifier) (Type) (MemberName)
    { 
            (You can write this also using Lamda Expressions:)
                    get {} 
                    set {}
    }

**Example:**
      public int SaileBitGoal
        {
            get => The.Game.GetIntGameState("SaileBitGoal");
            set => The.Game.SetIntGameState("SaileBitGoal", value);
        } 

**Parameters** are members added to Functions/Methods as you create them:
(Accessibility Modifier) (Type) (Method/Function)((ParameterType) (ParameterName))
            {
                [Function Actions]
            }
**Example:**    
      public override bool ExampleMethod(GameObject ObjectParameter) 
