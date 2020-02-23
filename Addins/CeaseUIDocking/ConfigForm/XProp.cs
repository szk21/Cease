using System; 
using System.ComponentModel; 
using System.Collections.Generic; 
using System.Text;

namespace Props
{
    public class XProp
    {
        public XProp() { }
        public XProp(string _strName, string _strVal, string _strDes)
        {
            Name = _strName;
            Description = _strDes;

            if (_strVal.ToString().ToUpper() == "TRUE")
            {
                Value = true;
            }
            else if (_strVal.ToString().ToUpper() == "FALSE")
            {
                Value = false;
            }
            else
            {
                Value = _strVal;
            }
        }
        private string theCategory = "";

        private string theName = "";

        private string theDescription = "";

        private object theValue = null;
        private bool theReadOnly = true;

        private object theEditorType=null;
        private object theConverType = null;
        public String EditText = "";
        public String[] sOptions = null;
        public object ConverType
        {
            get { return theConverType; }
            set { theConverType = Value; }
        }
        public object EditorType
        {
            get { return theEditorType; }
            set { theEditorType = Value; }
        }
        public string Category
        {

            get { return theCategory; }

            set { theCategory = value; }

        }
        public string Name
        {
            get
            {
                return theName;
            }

            set
            {
                theName = value;
            }

        }
        public object Value
        {
            get { return theValue; }
            set { theValue = value; }
        }
        public string Description
        {

            get { return theDescription; }

            set { theDescription = value; }

        }
        public string IsReadOnly
        {
            get 
            {
                if (theReadOnly == true)
                {
                    return "TRUE";
                }
                else
                {
                    return "FALSE";
                }
                
            }
            set
            {
                if (value == "TRUE")
                {
                    theReadOnly = true;
                }
                else
                {
                    theReadOnly = false;
                }
            }
        }
    }
};