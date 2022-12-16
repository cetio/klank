using CommandLine.Text;
using Iced.Intel;
using Klank;
using Klank.Generic;
using Klank.RelectiveSaveStates;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32;
using System.Management;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Vurdalakov;

/*Example ex_ = new Example();
ReflectiveSave<Example> reflectiveSave = new ReflectiveSave<Example>();
reflectiveSave.Save(@"C:\Users\stake\Desktop\save.txt", ex_);
//reflectiveSave.Load(@"C:\Users\stake\Desktop\save.txt");

public class Example
{
    public int t1_ = 0;
    public Example2 e1_ = new Example2();
}

public class Example2
{
    public Example3 acab = new Example3();
}

public class Example3
{
    public string[] acabo = new string[] { "abolish", "dont abolish" };
    public string acabo2 = "never!";
}*/

try
{
    Console.WriteLine("Slot #:          " + Hardware.SSlotSerial);
    Console.WriteLine("Memory #:        " + Hardware.MemorySerial);
    Console.WriteLine("Display #:       " + Hardware.DisplaySerial);

    foreach (var tableId in FirmwareTables.EnumFirmwareTables(0x41435049))
    {
        var acpiTable = FirmwareTables.GetAcpiTable(tableId);
        string tableName = String.Concat(FirmwareTables.UInt32ToString(tableId).Reverse());

        if (tableName != "SSDT")
            continue;
        else
        {
            int stregLength = 0;
            string stregTableName = "N/A";
            byte[] stregPayload = new byte[0];
            string stregName = string.Empty;
            string stregPayloadHash = "N/A";

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("HARDWARE\\ACPI\\SSDT\\");
                stregName += $"{key.GetSubKeyNames()[0]}\\";
                key = key.OpenSubKey($"{key.GetSubKeyNames()[0]}\\");
                stregName += $"{key.GetSubKeyNames()[0]}\\";
                key = key.OpenSubKey($"{key.GetSubKeyNames()[0]}\\");
                stregName += $"{key.GetSubKeyNames()[0]}\\";
                key = key.OpenSubKey($"{key.GetSubKeyNames()[0]}\\");
                stregPayload = (byte[])key.GetValue("00000000");
                stregLength = stregPayload.Length;
                stregTableName = System.Text.Encoding.ASCII.GetString(stregPayload).Substring(0, 4);
                stregPayloadHash = BitConverter.ToString(stregPayload.SubArray(stregPayload.SearchBytes(acpiTable.Payload), acpiTable.Payload.Length)).HashSE().HashToAlpha();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            string tablePayloadHash = BitConverter.ToString(acpiTable.Payload).HashSE().HashToAlpha();

            Console.WriteLine("SataTabl #:      {0}", (acpiTable.Signature + acpiTable.Revision + acpiTable.OemRevision + acpiTable.CreatorId + acpiTable.CreatorRevision + tableName
                                                       + tablePayloadHash).Hash().HashToAlpha());
            Console.WriteLine("Payload #:       {0}", tablePayloadHash);
            Console.WriteLine("Name:            {0}", tableName);
            Console.WriteLine("Checksum:        {0}\n", acpiTable.ChecksumIsValid);

            Console.WriteLine("STREG Present:   {0} ({1})", stregLength != 0, stregName);
            Console.WriteLine("STREG Payload #: {0}", stregPayloadHash);
            Console.WriteLine("STREG Name:      {0}", stregTableName);
            Console.WriteLine("STREG Checksum:  {0}", acpiTable.Length == stregLength && tablePayloadHash == stregPayloadHash && tableName == stregTableName);
        }

        break;
    }

    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.ReadLine();
}