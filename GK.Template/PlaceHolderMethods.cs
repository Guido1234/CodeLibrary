//using GK.Library.Utilities;
using GK.Template.Methods;
using System.Collections.Generic;

namespace GK.Template
{
    public sealed class PlaceHolderMethods : PlaceHolder
    {
        public List<MethodBase> Methods = new List<MethodBase>();

        public PlaceHolderMethods()
        {
        }

        public int ValuesIndex { get; set; }

        public string Execute(string[] values, string line, string data, int LineNumber, bool isLastCommand, out bool skippedData)
        {
            // TODO: ValueIndex -2 en -3
            string value = Utils.ArrayValue(values, ValuesIndex);
            if (ValuesIndex == -2)
            {
                value = line;
            }
            if (ValuesIndex == -3)
            {
                value = data;
            }

            foreach (MethodBase method in Methods)
            {
                method.LineNumber = LineNumber;
                method.IsLastLine = isLastCommand;
                method.Templates = this.Templates;
                if (method.GetType() == typeof(MethodIF))
                {
                    string newval = method.Apply(value);
                    if (newval == null)
                        break;
                }
                else
                {
                    value = method.Apply(value);
                }
                if (method.SkipData)
                {
                    // overule all other applies
                    skippedData = true;
                    method.SkipData = false;
                    return string.Empty;
                }
            }
            skippedData = false;
            return value;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", this.GetType().Name, Text);
        }
    }
}