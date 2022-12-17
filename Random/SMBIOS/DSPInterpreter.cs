using Spire.Pdf;
using Spire.Pdf.Utilities;
using System.Text.RegularExpressions;

namespace Klank
{
    // this code is atrocious, i mean it
    public class DSPInterpreter
    {
        public enum Sizes : uint
        {
            BYTE = 1,
            WORD = 2,
            DWORD = 4,
            QWORD = 8,
            Zero = 0
        }

        public static string ParseType(string tval, uint size)
        {
            switch (tval)
            {
                case "Bit Field":
                    return "byte";
                case "STRING":
                    return "string?";
                default:
                    switch (size)
                    {
                        case 1:
                            return "byte";
                        case 2:
                            return "ushort";
                        case 4:
                            return "uint";
                        case 8:
                            return "ulong";
                        case 16:
                            return "Guid";
                        default:
                            return "byte";
                    }
            }
        }

        public static void Interpret()
        {
            List<object[]> fields = new List<object[]>();
            List<object[]> enums = new List<object[]>();
            List<string> fieldEnums = new List<string>();

            for (int pi = 1; pi < 57; pi++)
            {
                //Load a PDF document using PdfDocument class
                PdfDocument pdf = new PdfDocument($"D:\\DSP\\DSPLatest-{pi}.pdf");
                //Loop through the pages in the PDF document
                for (int pageIndex = 0; pageIndex < pdf.Pages.Count + 1; pageIndex++)
                {
                    //Create a PdfTableExtractor instance
                    PdfTableExtractor extractor = new PdfTableExtractor(pdf);
                    //Extract table(s) from each page into a PdfTable array
                    PdfTable[] tableLists = extractor.ExtractTable(pageIndex);
                    if (tableLists != null && tableLists.Length > 0)
                    {
                        //Loop through tables in the PdfTable array
                        foreach (PdfTable table in tableLists)
                        {
                            List<string> globalNames = new List<string>();

                            if (table.GetColumnCount() == 2 && table.GetText(0, 0) == "ByteValue")
                            {
                                enums.Add(new object[1] { Guid.NewGuid() });

                                for (int i = 1; i < table.GetRowCount(); i++)
                                {
                                    uint byteValue = Convert.ToUInt32(table.GetText(i, 0).Substring(0, 2), 16);
                                    string name = table.GetText(i, 1);
                                    name = name.Contains('/') ? name.Substring(0, name.IndexOf('/')) : name;
                                    name = name.Contains('(') ? name.Substring(0, name.IndexOf('(')) : name;

                                    enums.Add(new object[2]
                                    {
                                        byteValue,
                                        Regex.Replace(name, "[^a-zA-Z0-9]", String.Empty)
                                    });
                                }
                            }
                            else if (table.GetColumnCount() == 5 && table.GetText(0, 0) == "Offset" && table.GetText(1, 0).Length < 4)
                            {
                                for (int i = 1; i < table.GetRowCount(); i++)
                                {
                                    if (table.GetText(i, 0).Length > 4)
                                        continue;

                                    uint offset = Convert.ToUInt32("0" + table.GetText(i, 0).Replace("h", string.Empty).Remove(2), 16);
                                    string name = table.GetText(i, 1);
                                    name = name.Contains('/') ? name.Substring(0, name.IndexOf('/')) : name;
                                    name = name.Contains('(') ? name.Substring(0, name.IndexOf('(')) : name;
                                    name = Regex.Replace(name, "[^a-zA-Z0-9]", String.Empty);

                                    uint size = Enum.TryParse(typeof(Sizes), table.GetText(i, 2), out _) != false ? (uint)Enum.Parse(typeof(Sizes), table.GetText(i, 2))
                                                                                                                  : 1;

                                    if (name == "Type" && offset != 0)
                                        name = "Type2";

                                    string type = name == "Type" ? "o" + Regex.Replace(table.GetText(i, 4), "[^a-zA-Z0-9]", String.Empty)
                                                                 : ParseType(table.GetText(i, 3), size);

                                    if (globalNames.Contains(name))
                                        name += (globalNames.Where(x => x.Contains(name)).ToArray().Length + 1).ToString();

                                    globalNames.Add(name);

                                    if (name == "Type")
                                        type = type.Contains("structure") ? type.Substring(0, type.IndexOf("structure")) : type.Contains("indicator") ? type.Substring(0, type.IndexOf("indicator"))
                                            : type;
                                    else if (table.GetText(i, 3).ToLower().Contains("enum"))
                                    {
                                        type = name;
                                        fieldEnums.Add(name);
                                    }

                                    object[] field = new object[5]
                                    {
                                    offset,
                                    size,
                                    name,
                                    type,
                                    name == "Type" ? $"Page: {pageIndex} Doc: {pi} TableID: {table.GetText(i, 3)}"
                                                   : $"Page: {pageIndex} Doc: {pi}"
                                    };

                                    fields.Add(field);
                                }
                            }
                            else if (table.GetColumnCount() == 6 && table.GetText(0, 0) == "Offset" && table.GetText(1, 0).Length < 5)
                            {
                                for (int i = 1; i < table.GetRowCount(); i++)
                                {
                                    if (table.GetText(i, 0).Length > 4)
                                        continue;

                                    uint offset = Convert.ToUInt32("0" + table.GetText(i, 0).Replace("h", string.Empty), 16);
                                    string name = table.GetText(i, 2);
                                    name = name.Contains('/') ? name.Substring(0, name.IndexOf('/')) : name;
                                    name = name.Contains('(') ? name.Substring(0, name.IndexOf('(')) : name;
                                    name = Regex.Replace(name, "[^a-zA-Z0-9]", String.Empty);

                                    uint size = Enum.TryParse(typeof(Sizes), table.GetText(i, 3), out _) != false ? (uint)Enum.Parse(typeof(Sizes), table.GetText(i, 3))
                                                                                                                  : 1;

                                    if (name == "Type" && offset != 0)
                                        name = "Type2";

                                    string type = name == "Type" ? "o" + Regex.Replace(table.GetText(i, 5), "[^a-zA-Z0-9]", String.Empty)
                                                                 : ParseType(table.GetText(i, 4), size);

                                    if (globalNames.Contains(name))
                                        name += (globalNames.Where(x => x.Contains(name)).ToArray().Length + 1).ToString();

                                    globalNames.Add(name);

                                    if (name == "Type")
                                        type = type.Contains("structure") ? type.Substring(0, type.IndexOf("structure")) : type.Contains("indicator") ? type.Substring(0, type.IndexOf("indicator"))
                                            : type;
                                    else if (table.GetText(i, 4).ToLower().Contains("enum"))
                                    {
                                        type = name;
                                        fieldEnums.Add(name);
                                    }

                                    object[] field = new object[5]
                                    {
                                    offset,
                                    size,
                                    name,
                                    type,
                                    name == "Type" ? $"Page: {pageIndex} Doc: {pi} TableID: {table.GetText(i, 3)}"
                                                   : $"Page: {pageIndex} Doc: {pi}"
                                    };

                                    fields.Add(field);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine(pi);
            }

            string fileBuffer = string.Empty;
            string switchBuffer = string.Empty;
            int seenStringFields = 0;
            List<string> localEnumAndTypes = new List<string>();

            for (int tables = 0; tables < fields.Where(x => x[2].ToString() == "Type").Count() - 1; tables++)
            {
                int seenTypes = 0;
                uint lastValidOffset = 0;
                int seenStringFieldsHackyWorkaround = 0;

                // write fields & class name
                foreach (object[] field in fields)
                {
                    if (field[2] is not "Type" and not "Length" and not "Handle")
                    {
                        if (seenTypes != tables + 1 || (uint)field[0] < lastValidOffset)
                            continue;

                        if (char.IsLetter(((string)field[2])[0]) == false)
                            field[2] = "D" + field[2];

                        fileBuffer += $"    public {field[3]} {Regex.Replace(field[2].ToString(), "[^a-zA-Z0-9]", String.Empty)} {{ get; protected set; }} // {field[4]}\n";
                        lastValidOffset = (uint)field[0];
                    }
                    else if (field[2] is "Type" && (uint)field[0] is 0)
                    {
                        seenTypes++;
                        if (seenTypes != tables + 1)
                            continue;

                        if (localEnumAndTypes.Contains(field[3].ToString()))
                            field[3] += (localEnumAndTypes.Where(x => x.Contains(field[3].ToString())).ToArray().Length + 1).ToString();

                        localEnumAndTypes.Add(field[3].ToString());
                        fileBuffer += $"\n// automated by stink-o-tron 3000 /// {field[4]}\npublic class {field[3]} : BaseAbstractedTable\n{{\n";
                    }
                }

                seenTypes = 0;
                lastValidOffset = 0;

                // write constructor
                foreach (object[] field in fields)
                {
                    if ((uint)field[1] is not 0 && field[2] is not "Type" and not "Length" and not "Handle")
                    {
                        if (seenTypes != tables + 1 || (uint)field[0] < lastValidOffset)
                            continue;

                        if (char.IsLetter(((string)field[2])[0]) == false)
                            field[2] = "D" + field[2];

                        switch (field[3])
                        {
                            case "string?":
                                fileBuffer += $"            {Regex.Replace(field[2].ToString(), "[^a-zA-Z0-9]", String.Empty)} = strings[{fields.Where(x => x[3].ToString() == "string?").ToList().IndexOf(field) - seenStringFields}];\n";
                                seenStringFieldsHackyWorkaround++;
                                break;
                            case "byte":
                                fileBuffer += $"            {Regex.Replace(field[2].ToString(), "[^a-zA-Z0-9]", String.Empty)} = data[{field[0]}];\n";
                                break;
                            case "ushort":
                                fileBuffer += $"            {Regex.Replace(field[2].ToString(), "[^a-zA-Z0-9]", String.Empty)} = BitConverter.ToUInt16(data, {field[0]});\n";
                                break;
                            case "uint":
                                fileBuffer += $"            {Regex.Replace(field[2].ToString(), "[^a-zA-Z0-9]", String.Empty)} = BitConverter.ToUInt32(data, {field[0]});\n";
                                break;
                            case "ulong":
                                fileBuffer += $"            {Regex.Replace(field[2].ToString(), "[^a-zA-Z0-9]", String.Empty)} = BitConverter.ToUInt64(data, {field[0]});\n";
                                break;
                            default:
                                fileBuffer += $"            {Regex.Replace(field[2].ToString(), "[^a-zA-Z0-9]", String.Empty)} = ({Regex.Replace(field[2].ToString(), "[^a-zA-Z0-9]", String.Empty)})data[{field[0]}];\n";
                                break;
                        }

                        lastValidOffset = (uint)field[0];
                    }
                    else if (field[2] is "Type" && (uint)field[0] is 0)
                    {
                        seenTypes++;
                        if (seenTypes != tables + 1)
                            continue;

                        switchBuffer += @$"                    case {field[3].ToString().Split(' ').Last()}:
                        Tables.Add(new {field[3]}(header.Type, header.Length, header.Handle, data, strings));
                        break;" + "\n";
                        fileBuffer += $"\n    public {field[3]}(byte type, byte length, ushort handle, byte[] data, List<string> strings)\n    {{\n        Type = type;\n        Length = length;\n        Handle = handle;\n        RTYPE_ = typeof({field[3]});\n\n        try\n        {{\n";
                    }
                }

                seenStringFields += seenStringFieldsHackyWorkaround;
                fileBuffer += "        }\n        catch\n        {\n            Console.WriteLine(\"[!] Exhausted table structure\");\n        }\n    }\n}";
            }

            for (int enumct = 0; enumct < enums.Where(x => x.Length == 1).Count() - 1; enumct++)
            {
                int seenEnums = 0;
                List<string> localFields = new List<string>();

                // write fields & class name
                foreach (object[] field in enums)
                {
                    if (field.Length != 1)
                    {
                        if (seenEnums != enumct + 1)
                            continue;

                        if (localFields.Contains(field[1].ToString()))
                            field[1] += (localFields.Where(x => x.Contains(field[1].ToString())).ToArray().Length + 1).ToString();

                        localFields.Add(field[1].ToString());

                        if (char.IsLetter(((string)field[1])[0]) == false)
                            field[1] = "D" + field[1];

                        fileBuffer += $"    {field[1]} = {field[0]},\n";
                    }
                    else
                    {
                        seenEnums++;
                        if (seenEnums != enumct + 1)
                            continue;

                        string name = fieldEnums[enums.Where(x => x.Length == 1).ToList().IndexOf(field)];
                        if (localEnumAndTypes.Contains(name))
                            name += (localEnumAndTypes.Where(x => x.Contains(name)).ToArray().Length + 1).ToString();

                        localEnumAndTypes.Add(name);
                        fileBuffer += $"\n// automated by stink-o-tron 3000\npublic enum {name} : byte\n{{\n";
                    }
                }

                fileBuffer = fileBuffer.Substring(0, fileBuffer.Length - 1) + "\n}";
            }

            File.WriteAllText("D:\\ParsedTables.cs", fileBuffer);
            Console.WriteLine("Saved tables!");
            File.WriteAllText("D:\\ParsedSwitch.cs", switchBuffer);
            Console.WriteLine("Saved switch!");
        }
    }
}
