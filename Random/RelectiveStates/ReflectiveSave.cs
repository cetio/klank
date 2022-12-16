using Klank.Generic;
using System.Collections;
using System.Reflection;

namespace Klank.RelectiveSaveStates
{
    public class ReflectiveSave<T>
    {
        public ReflectiveSave()
        {

        }

        public void Save(string path, T class_)
        {
            string content = Serialize(class_);
            content += content.HashSE().HashToAlpha();

            Console.WriteLine(content);
            File.WriteAllText(path, content);
        }

        public string Serialize(object class_)
        {
            IEnumerable<FieldInfo> fields = class_.GetType().GetFields();
            string content = string.Empty;

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsPrimitive || field.FieldType.IsPointer || field.FieldType == typeof(string))
                    content += $"Name:{field.Name} Value:{field.GetValue(class_)}" + Environment.NewLine;
                else if (field.FieldType.IsGenericType || field.FieldType.IsArray)
                    content += $"Class:{field.Name} Value:\n{SerializeEnumb(field.GetValue(class_))}" + Environment.NewLine;
                else
                    content += $"Class:{field.Name} Value:\n{Serialize(field.GetValue(class_))}" + Environment.NewLine;
            }

            return content + "\\end";
        }

        public string SerializeEnumb(object enumb)
        {
            IEnumerable objects = (IEnumerable)enumb;
            string content = string.Empty;
            int index = 0;

            foreach (object obj in objects)
            {
                Type type = obj.GetType();

                if (type.IsPrimitive || type.IsPointer || type == typeof(string))
                    content += $"Name:{index++} Value:{obj}" + Environment.NewLine;
                else
                    content += $"Name:{index++} Value:{Serialize(obj)}" + Environment.NewLine;
            }

            return content + "\\end";
        }
    }
}
