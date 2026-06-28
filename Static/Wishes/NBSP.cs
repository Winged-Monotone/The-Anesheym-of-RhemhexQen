
    using XRL.World.Text.Attributes;
    using XRL.World.Text.Delegates;
    
    namespace Wingtone.VariableChanges
    {
        [HasVariableReplacer]
        public static class ReplaceNbsp
        {        
            [VariableReplacer]
            public static string ud_nbsp(DelegateContext Context)
            {
                string nbsp = "\xFF";
                    if (!Context.Parameters.IsNullOrEmpty()
                    && int.TryParse(Context.Parameters[0], out int count))
                {
                    string output = null;
                    for (int i = 0; i < count; i++)
                    {
                        output += nbsp;
                    }
                    return output;
                }
                return nbsp;
            }
        }
    }