using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Klank
{
    public class SMBIOS
    {
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern int GetSystemFirmwareTable([In] UInt32 firmwareTableProviderSignature, [In] UInt32 firmwareTableId, [Out] IntPtr pFirmwareTableBuffer, [In] int bufferSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct RawSMBIOSData
        {
            public byte UsedCallingMethod;
            public byte MajorVersion;
            public byte MinorVersion;
            public byte DMIRevision;
            public int Length;
        }

        private RawSMBIOSData RawSMBIOS { get; set; }
        private List<BaseAbstractedTable> Tables { get; } = new List<BaseAbstractedTable>();
        public byte MajorVersion { get { return RawSMBIOS.MajorVersion; } }
        public byte MinorVersion { get { return RawSMBIOS.MinorVersion; } }
        public byte DMIRevision { get { return RawSMBIOS.DMIRevision; } }
        public int Count { get { return Tables.Where(x => x is not Irregular).Count(); } }
        public int IrregularCount { get { return Tables.Where(x => x is Irregular).Count(); } }

        public dynamic this[int index]
        {
            get
            {
                return Convert.ChangeType((object)Tables[index], Tables[index].RTYPE_);
            }
        }

        public SMBIOS()
        {
            // Determine size needed for entire SMBIOS table
            int size = GetSystemFirmwareTable(0x52534d42, 0x00000000, IntPtr.Zero, 0);
            IntPtr bptrSMBIOS = Marshal.AllocHGlobal(size) + 8;

            if (GetSystemFirmwareTable(0x52534d42, 0x00000000, bptrSMBIOS - 8, size) == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            RawSMBIOS = (RawSMBIOSData)Marshal.PtrToStructure(bptrSMBIOS - 8, typeof(RawSMBIOSData));

            int offset = 0;

            while (offset < RawSMBIOS.Length)
            {
                BaseTable header = (BaseTable)Marshal.PtrToStructure(bptrSMBIOS + offset, typeof(BaseTable));

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n[<] {(TableIDs)header.Type} ({header.Length}, {offset})");
                Console.ForegroundColor = ConsoleColor.Gray;

                List<string> strings = new List<string>();
                byte[] data = new byte[header.Length];
                Marshal.Copy(bptrSMBIOS + offset, data, 0, header.Length);

                Console.WriteLine($"[>] Table handle {header.Handle}");
                offset += header.Length;

                // get string table
                while (true)
                {
                    Console.Write($"[+] Created new string of length ");
                    string s_ = Marshal.PtrToStringAnsi(bptrSMBIOS + offset);
                    Console.WriteLine($"{s_.Length}");

                    // go to next string start, passing null terminator
                    offset += s_.Length + 1;

                    if (s_.Length == 0)
                        break;
                    else
                        strings.Add(s_);
                }

                // tables without strings have an extra null terminator (not really, string tables just have the strings between the 2 nulls)
                if (strings.Count == 0)
                    offset += 1;

                // print out the string table
                Console.WriteLine($@"[""{string.Join(@""", """, strings)}""]");

                switch ((TableIDs)header.Type)
                {
                    case TableIDs.BIOSInformation:
                        Tables.Add(new oBIOSInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemInformation:
                        Tables.Add(new oSystemInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.BaseboardInformation:
                        Tables.Add(new oBaseboardInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemEnclosure:
                        Tables.Add(new oSystemEnclosure(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.ProcessorInformation:
                        Tables.Add(new oProcessorInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.MemoryControllerInformation:
                        Tables.Add(new oMemoryController(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.MemoryModuleInformation:
                        Tables.Add(new oMemoryModuleConfiguration(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.CacheInformation:
                        Tables.Add(new oCacheInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.PortConnectorInformation:
                        Tables.Add(new oConnectorInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemSlots:
                        Tables.Add(new oSystemSlotStructure(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.OnboardDevicesInformation:
                        Tables.Add(new oOnBoardDevicesInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.OEMStrings:
                        Tables.Add(new oOEMStrings(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemConfigurationOptions:
                        Tables.Add(new oConfigurationInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.BIOSLanguageInformation:
                        Tables.Add(new oLanguageInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.GroupAssociations:
                        Tables.Add(new oGroupAssociations(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemEventLog:
                        Tables.Add(new oEventLogType(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.PhysicalMemoryArray:
                        Tables.Add(new oPhysicalMemoryArraytype(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.MemoryDevice:
                        Tables.Add(new oMemoryDevicetype(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.TTMemoryErrorInformation:
                        Tables.Add(new o32bitMemoryErrorInformationtype(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.MemoryArrayMappedAddress:
                        Tables.Add(new oMemoryArrayMappedAddress(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.MemoryDeviceMappedAddress:
                        Tables.Add(new oMemoryDeviceMappedAddress(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.BuiltinPointingDevice:
                        Tables.Add(new oBuiltinPointingDevice(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.PortableBattery:
                        Tables.Add(new oPortableBattery(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemReset:
                        Tables.Add(new oSystemReset(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.HardwareSecurity:
                        Tables.Add(new oHardwareSecurity(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemPowerControl:
                        Tables.Add(new oSystemPowerControls(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.VoltageProbe:
                        Tables.Add(new oVoltageProbe(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.CoolingDevice:
                        Tables.Add(new oCoolingDevice(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.TemperatureProbe:
                        Tables.Add(new oTemperatureProbe(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.ElectricalCurrentProbe:
                        Tables.Add(new oElectricalCurrentProbe(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.OOBRemoteAccess:
                        Tables.Add(new oOutofBandRemoteAccess(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemBootInformation:
                        Tables.Add(new oSystemBootInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SFMemoryErrorInformation:
                        Tables.Add(new o64bitMemoryErrorInformationtype(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.ManagementDevice:
                        Tables.Add(new oManagementDevice(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.ManagementDeviceComponent:
                        Tables.Add(new oManagementDeviceComponent(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.ManagementDeviceThresholdData:
                        Tables.Add(new oManagementDeviceThresholdData(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.MemoryChannel:
                        Tables.Add(new oManagementDeviceThresholdData2(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.IPMIDeviceInformation:
                        Tables.Add(new oIPMIDeviceInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.SystemPowerSupply:
                        Tables.Add(new oPowerSupplyStructure(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.AdditionalInformation:
                        Tables.Add(new oAdditionalInformationtype(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.OnboardDevicesExtendedInformation:
                        Tables.Add(new oOnboardDevicesExtendedInformation(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.ManagementControllerHostInterface:
                        Tables.Add(new oManagementControllerHostInterface(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.TPMDevice:
                        Tables.Add(new oTPMDevice(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    case TableIDs.Inactive:
                        Tables.Add(new oInactive(header.Type, header.Length, header.Handle, data, strings));
                        break;
                    default:
                        Tables.Add(new Irregular(header.Type, header.Length, header.Handle, data, strings));
                        break;
                }
            }

            Marshal.FreeHGlobal(bptrSMBIOS);

            Console.WriteLine($"\nRegular Tables: {Count}");
            Console.WriteLine($"Irregular Tables: {IrregularCount}");
        }
    }
}
