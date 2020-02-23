using System; 
using System.ComponentModel; 
using System.Collections.Generic;
using System.Text;
// using PropsBox;
//using System.Windows.Forms.Design;
// using System.Drawing.Design;
namespace Props
{
    public class XProps : List<XProp>, ICustomTypeDescriptor
    {
        public bool m_bChecked = true;
        public string m_strCategory = "CHILD";

        #region   overloads

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] filter)
        {

            PropertyDescriptor[] newProps = new PropertyDescriptor[Count];


            for (int i = 0; i < Count; i++)
            {

                newProps[i] = new XPropDescriptor(this[i], filter);

            }


            return new PropertyDescriptorCollection(newProps);

        }


        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {

            return TypeDescriptor.GetAttributes(this, true);

        }


        string ICustomTypeDescriptor.GetClassName()

        { return TypeDescriptor.GetClassName(this, true); }


        string ICustomTypeDescriptor.GetComponentName()

        { return TypeDescriptor.GetComponentName(this, true); }


        TypeConverter ICustomTypeDescriptor.GetConverter()

        { return TypeDescriptor.GetConverter(this, true); }


        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()

        { return TypeDescriptor.GetDefaultEvent(this, true); }


        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(System.Attribute[] attributes)

        { return TypeDescriptor.GetEvents(this, attributes, true); }


        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()

        { return TypeDescriptor.GetEvents(this, true); }


        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()

        { return TypeDescriptor.GetDefaultProperty(this, true); }


        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()

        { return TypeDescriptor.GetProperties(this, true); }


        object ICustomTypeDescriptor.GetEditor(System.Type editorBaseType)

        { return TypeDescriptor.GetEditor(this, editorBaseType, true); }


        object ICustomTypeDescriptor.GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd)

        { return this; }


        #endregion



        public override string ToString()
        {

            StringBuilder sbld = new StringBuilder();

            return sbld.ToString();

        }

    }
}


