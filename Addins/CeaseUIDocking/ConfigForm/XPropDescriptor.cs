using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms.Design;
//using System.Drawing.Design;
//using CustomerPropertyGrid;
namespace Props
{
    /// <summary>
    /// XPropDescriptor 的摘要说明。
    /// </summary>
    public class XPropDescriptor : PropertyDescriptor
    {

        private XProp theProp;
        public bool Enable
        {
            get
            {
                if (theProp.Name.Contains("TestCase"))
                {
                    char cCaseJu = theProp.Name.Substring("TestCase".Length, 1).ToCharArray()[0];
                    if (cCaseJu <= '9' && cCaseJu >= '0')
                    {
                        if (theProp.IsReadOnly == "TRUE")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            //             set
            //             {
            //                 theProp.IsReadOnly = value ? "TRUE" : "FALSE";
            //             }

        }
        public XPropDescriptor(XProp prop, Attribute[] attrs)
            : base(prop.Name, attrs)
        {

            theProp = prop;
        }


        public override bool CanResetValue(object component)
        {
            return false;

        }


        public override System.Type ComponentType
        {
            get { return this.GetType(); }
        }


        public override object GetValue(object component)
        {
            if (theProp.EditorType != null || theProp.ConverType != null)
            {
                return theProp.EditText;
            }
            else
            {
                return theProp.Value;
            }
        }


        public override string Category
        {
            get
            {
                return theProp.Category;
            }
        }
        /// <EditorAttr>
        public override object GetEditor(Type editorBaseType)
        {

            return theProp.EditorType;

        }
        public override TypeConverter Converter
        {
            get
            {
                return (TypeConverter)theProp.ConverType;
            }

        }
        /// </summary>

        public override string Description
        {
            get
            {

                return theProp.Description;

            }
        }


        public override bool IsReadOnly
        {
            get
            {
//                 if (theProp.Name.Contains("TestCase"))
//                 {
//                     char cCaseJu = theProp.Name.Substring("TestCase".Length, 1).ToCharArray()[0];
//                     if (cCaseJu <= '9' && cCaseJu >= '0')
//                     {
//                         if (theProp.IsReadOnly == "TRUE")
//                         {
//                             return true;
//                         }
//                         else
//                         {
//                             return false;
//                         }
//                     }
//                     else
//                         return false;
//                 } 
//                 else
                {
                    return false;
                }
            }
            //             set
            //             {
            //                 theProp.IsReadOnly = value ? "TRUE" : "FALSE";
            //             }

        }


        public override System.Type PropertyType
        {

            get { return theProp.Value.GetType(); }

        }


        public override void ResetValue(object component)
        {

        }


        public override void SetValue(object component, object value)
        {

            if (theProp.EditorType != null || theProp.ConverType != null)
            {
                theProp.EditText = value.ToString();
            }
            else
            {
                theProp.Value = value;
            }

        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

    }

};