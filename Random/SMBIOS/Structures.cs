using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Klank
{
    public enum TableIDs : byte
    {
        BIOSInformation = 0,
        SystemInformation = 1,
        BaseboardInformation = 2,
        SystemEnclosure = 3,
        ProcessorInformation = 4,
        PortConnectorInformation = 8,
        SystemSlots = 9,
        OEMStrings = 11,
        BIOSLanguageInformation = 13,
        PhysicalMemoryArray = 16,
        MemoryDevice = 17,
        MemoryArrayMappedAddress = 19,
        SystemBootInformation = 32,
        IPMIDeviceInformation = 38,
        OnboardDevicesExtendedInformation = 41,
        MemoryControllerInformation = 5,
        MemoryModuleInformation = 6,
        CacheInformation = 7,
        OnboardDevicesInformation = 10,
        SystemConfigurationOptions = 12,
        GroupAssociations = 14,
        SystemEventLog = 15,
        TTMemoryErrorInformation = 18,
        MemoryDeviceMappedAddress = 20,
        BuiltinPointingDevice = 21,
        PortableBattery = 22,
        SystemReset = 23,
        HardwareSecurity = 24,
        SystemPowerControl = 25,
        VoltageProbe = 26,
        CoolingDevice = 27,
        TemperatureProbe = 28,
        ElectricalCurrentProbe = 29,
        OOBRemoteAccess = 30,
        SFMemoryErrorInformation = 33,
        ManagementDevice = 34,
        ManagementDeviceComponent = 35,
        ManagementDeviceThresholdData = 36,
        MemoryChannel = 37,
        SystemPowerSupply = 39,
        AdditionalInformation = 40,
        ManagementControllerHostInterface = 42,
        TPMDevice = 43,
        Inactive = 126,
        Terminator = 127
    }

    public enum Providers : uint
    {
        ACPI = 0x41435049,
        FIRM = 0x4649524D,
        RSMB = 0x52534D42
    }

    /*
     * TABLES STRUCTURES
     */

    [StructLayout(LayoutKind.Sequential)]
    public class BaseTable
    {
        public byte Type { get; protected set; }
        public byte Length { get; protected set; }
        public ushort Handle { get; protected set; }
    }

    public class BaseAbstractedTable : BaseTable
    {
        public Type? RTYPE_;
    }

    public class Irregular : BaseAbstractedTable
    {
        public List<string> StringTable { get; protected set; }
        public byte[] Payload { get; protected set; }

        public Irregular(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(Irregular);

            StringTable = strings;
            Payload = data;
        }
    }


    // automated by stink-o-tron 3000 /// Page: 1 Doc: 11 TableID: BYTE
    public class oBIOSInformation : BaseAbstractedTable
    {
        public string? Vendor { get; protected set; } // Page: 2 Doc: 11
        public string? BIOSVersion { get; protected set; } // Page: 2 Doc: 11
        public ushort BIOSStartingAddressSegment { get; protected set; } // Page: 2 Doc: 11
        public string? BIOSReleaseDate { get; protected set; } // Page: 2 Doc: 11
        public byte BIOSROMSize { get; protected set; } // Page: 2 Doc: 11
        public ulong BIOSCharacteristics { get; protected set; } // Page: 2 Doc: 11
        public byte BIOSCharacteristicsExtensionBytes { get; protected set; } // Page: 2 Doc: 11
        public byte SystemBIOSMajorRelease { get; protected set; } // Page: 0 Doc: 12
        public byte SystemBIOSMinorRelease { get; protected set; } // Page: 0 Doc: 12
        public byte EmbeddedControllerFirmwareMajorRelease { get; protected set; } // Page: 0 Doc: 12
        public byte EmbeddedControllerFirmwareMinorRelease { get; protected set; } // Page: 0 Doc: 12
        public ushort ExtendedBIOSROMSize { get; protected set; } // Page: 0 Doc: 12

        public oBIOSInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oBIOSInformation);

            try
            {
                Vendor = strings[0];
                BIOSVersion = strings[1];
                BIOSStartingAddressSegment = BitConverter.ToUInt16(data, 6);
                BIOSReleaseDate = strings[2];
                BIOSROMSize = data[9];
                BIOSCharacteristics = BitConverter.ToUInt64(data, 10);
                BIOSCharacteristicsExtensionBytes = data[18];
                SystemBIOSMajorRelease = data[20];
                SystemBIOSMinorRelease = data[21];
                EmbeddedControllerFirmwareMajorRelease = data[22];
                EmbeddedControllerFirmwareMinorRelease = data[23];
                ExtendedBIOSROMSize = BitConverter.ToUInt16(data, 24);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 13 TableID: BYTE
    public class oSystemInformation : BaseAbstractedTable
    {
        public string? Manufacturer { get; protected set; } // Page: 0 Doc: 13
        public string? ProductName { get; protected set; } // Page: 0 Doc: 13
        public string? Version { get; protected set; } // Page: 0 Doc: 13
        public string? SerialNumber { get; protected set; } // Page: 0 Doc: 13
        public byte UUID { get; protected set; } // Page: 0 Doc: 13
        public WakeupType WakeupType { get; protected set; } // Page: 0 Doc: 13
        public string? SKUNumber { get; protected set; } // Page: 0 Doc: 13
        public string? Family { get; protected set; } // Page: 0 Doc: 13

        public oSystemInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oSystemInformation);

            try
            {
                Manufacturer = strings[0];
                ProductName = strings[1];
                Version = strings[2];
                SerialNumber = strings[3];
                UUID = data[8];
                WakeupType = (WakeupType)data[24];
                SKUNumber = strings[4];
                Family = strings[5];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 13 TableID: 2
    public class oBaseboardInformation : BaseAbstractedTable
    {
        public string? Manufacturer { get; protected set; } // Page: 2 Doc: 13
        public string? Product { get; protected set; } // Page: 2 Doc: 13
        public string? Version { get; protected set; } // Page: 2 Doc: 13
        public string? SerialNumber { get; protected set; } // Page: 2 Doc: 13
        public string? AssetTag { get; protected set; } // Page: 2 Doc: 13
        public byte FeatureFlags { get; protected set; } // Page: 2 Doc: 13
        public string? LocationinChassis { get; protected set; } // Page: 2 Doc: 13
        public ushort ChassisHandle { get; protected set; } // Page: 2 Doc: 13
        public BoardType BoardType { get; protected set; } // Page: 2 Doc: 13
        public byte NumberofContainedObjectHandles { get; protected set; } // Page: 2 Doc: 13
        public byte ContainedObjectHandles { get; protected set; } // Page: 2 Doc: 13

        public oBaseboardInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oBaseboardInformation);

            try
            {
                Manufacturer = strings[0];
                Product = strings[1];
                Version = strings[2];
                SerialNumber = strings[3];
                AssetTag = strings[4];
                FeatureFlags = data[9];
                LocationinChassis = strings[5];
                ChassisHandle = BitConverter.ToUInt16(data, 11);
                BoardType = (BoardType)data[13];
                NumberofContainedObjectHandles = data[14];
                ContainedObjectHandles = data[15];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 14 TableID: BYTE
    public class oSystemEnclosure : BaseAbstractedTable
    {
        public string? Manufacturer { get; protected set; } // Page: 1 Doc: 14
        public byte Type2 { get; protected set; } // Page: 1 Doc: 14
        public string? Version { get; protected set; } // Page: 1 Doc: 14
        public string? SerialNumber { get; protected set; } // Page: 1 Doc: 14
        public string? AssetTagNumber { get; protected set; } // Page: 1 Doc: 14
        public BootupState BootupState { get; protected set; } // Page: 1 Doc: 14
        public PowerSupplyState PowerSupplyState { get; protected set; } // Page: 1 Doc: 14
        public ThermalState ThermalState { get; protected set; } // Page: 1 Doc: 14
        public SecurityStatus SecurityStatus { get; protected set; } // Page: 1 Doc: 14
        public uint OEMdefined { get; protected set; } // Page: 1 Doc: 14
        public byte Height { get; protected set; } // Page: 1 Doc: 14
        public byte NumberofPowerCords { get; protected set; } // Page: 1 Doc: 14
        public byte ContainedElementCount { get; protected set; } // Page: 2 Doc: 14
        public byte ContainedElementRecordLength { get; protected set; } // Page: 2 Doc: 14
        public byte ContainedElements { get; protected set; } // Page: 2 Doc: 14

        public oSystemEnclosure(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oSystemEnclosure);

            try
            {
                Manufacturer = strings[0];
                Type2 = data[5];
                Version = strings[1];
                SerialNumber = strings[2];
                AssetTagNumber = strings[3];
                BootupState = (BootupState)data[9];
                PowerSupplyState = (PowerSupplyState)data[10];
                ThermalState = (ThermalState)data[11];
                SecurityStatus = (SecurityStatus)data[12];
                OEMdefined = BitConverter.ToUInt32(data, 13);
                Height = data[17];
                NumberofPowerCords = data[18];
                ContainedElementCount = data[19];
                ContainedElementRecordLength = data[20];
                ContainedElements = data[21];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 15 TableID: BYTE
    public class oProcessorInformation : BaseAbstractedTable
    {
        public string? SocketDesignation { get; protected set; } // Page: 0 Doc: 16
        public ProcessorType ProcessorType { get; protected set; } // Page: 0 Doc: 16
        public ProcessorFamily ProcessorFamily { get; protected set; } // Page: 0 Doc: 16
        public string? ProcessorManufacturer { get; protected set; } // Page: 0 Doc: 16
        public ulong ProcessorID { get; protected set; } // Page: 0 Doc: 16
        public string? ProcessorVersion { get; protected set; } // Page: 0 Doc: 16
        public byte Voltage { get; protected set; } // Page: 0 Doc: 16
        public ushort ExternalClock { get; protected set; } // Page: 0 Doc: 16
        public ushort MaxSpeed { get; protected set; } // Page: 0 Doc: 16
        public ushort CurrentSpeed { get; protected set; } // Page: 0 Doc: 16
        public byte Status { get; protected set; } // Page: 0 Doc: 16
        public ProcessorUpgrade ProcessorUpgrade { get; protected set; } // Page: 0 Doc: 16
        public ushort L1CacheHandle { get; protected set; } // Page: 1 Doc: 16
        public ushort L2CacheHandle { get; protected set; } // Page: 1 Doc: 16
        public ushort L3CacheHandle { get; protected set; } // Page: 1 Doc: 16
        public string? SerialNumber { get; protected set; } // Page: 1 Doc: 16
        public string? AssetTag { get; protected set; } // Page: 1 Doc: 16
        public string? PartNumber { get; protected set; } // Page: 1 Doc: 16
        public byte CoreCount { get; protected set; } // Page: 1 Doc: 16
        public byte CoreEnabled { get; protected set; } // Page: 1 Doc: 16
        public byte ThreadCount { get; protected set; } // Page: 1 Doc: 16
        public ushort ProcessorCharacteristics { get; protected set; } // Page: 1 Doc: 16
        public ProcessorFamily2 ProcessorFamily2 { get; protected set; } // Page: 1 Doc: 16
        public ushort CoreCount2 { get; protected set; } // Page: 2 Doc: 16
        public ushort CoreEnabled2 { get; protected set; } // Page: 2 Doc: 16
        public ushort ThreadCount2 { get; protected set; } // Page: 2 Doc: 16
        public ushort ThreadEnabled { get; protected set; } // Page: 2 Doc: 16

        public oProcessorInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oProcessorInformation);

            try
            {
                SocketDesignation = strings[0];
                ProcessorType = (ProcessorType)data[5];
                ProcessorFamily = (ProcessorFamily)data[6];
                ProcessorManufacturer = strings[1];
                ProcessorID = BitConverter.ToUInt64(data, 8);
                ProcessorVersion = strings[2];
                Voltage = data[17];
                ExternalClock = BitConverter.ToUInt16(data, 18);
                MaxSpeed = BitConverter.ToUInt16(data, 20);
                CurrentSpeed = BitConverter.ToUInt16(data, 22);
                Status = data[24];
                ProcessorUpgrade = (ProcessorUpgrade)data[25];
                L1CacheHandle = BitConverter.ToUInt16(data, 26);
                L2CacheHandle = BitConverter.ToUInt16(data, 28);
                L3CacheHandle = BitConverter.ToUInt16(data, 30);
                SerialNumber = strings[3];
                AssetTag = strings[4];
                PartNumber = strings[5];
                CoreCount = data[35];
                CoreEnabled = data[36];
                ThreadCount = data[37];
                ProcessorCharacteristics = BitConverter.ToUInt16(data, 38);
                ProcessorFamily2 = (ProcessorFamily2)data[40];
                CoreCount2 = BitConverter.ToUInt16(data, 42);
                CoreEnabled2 = BitConverter.ToUInt16(data, 44);
                ThreadCount2 = BitConverter.ToUInt16(data, 46);
                ThreadEnabled = BitConverter.ToUInt16(data, 48);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 21 TableID: BYTE
    public class oMemoryController : BaseAbstractedTable
    {
        public ErrorDetectingMethod ErrorDetectingMethod { get; protected set; } // Page: 1 Doc: 21
        public byte ErrorCorrectingCapability { get; protected set; } // Page: 1 Doc: 21
        public SupportedInterleave SupportedInterleave { get; protected set; } // Page: 1 Doc: 21
        public CurrentInterleave CurrentInterleave { get; protected set; } // Page: 1 Doc: 21
        public byte MaximumMemoryModuleSize { get; protected set; } // Page: 1 Doc: 21
        public ushort SupportedSpeeds { get; protected set; } // Page: 1 Doc: 21
        public ushort SupportedMemoryTypes { get; protected set; } // Page: 1 Doc: 21
        public byte MemoryModuleVoltage { get; protected set; } // Page: 1 Doc: 21
        public byte NumberofAssociatedMemorySlots { get; protected set; } // Page: 2 Doc: 21

        public oMemoryController(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oMemoryController);

            try
            {
                ErrorDetectingMethod = (ErrorDetectingMethod)data[4];
                ErrorCorrectingCapability = data[5];
                SupportedInterleave = (SupportedInterleave)data[6];
                CurrentInterleave = (CurrentInterleave)data[7];
                MaximumMemoryModuleSize = data[8];
                SupportedSpeeds = BitConverter.ToUInt16(data, 9);
                SupportedMemoryTypes = BitConverter.ToUInt16(data, 11);
                MemoryModuleVoltage = data[13];
                NumberofAssociatedMemorySlots = data[14];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 22 TableID: 6
    public class oMemoryModuleConfiguration : BaseAbstractedTable
    {
        public string? SocketDesignation { get; protected set; } // Page: 0 Doc: 22
        public byte BankConnections { get; protected set; } // Page: 0 Doc: 22
        public byte CurrentSpeed { get; protected set; } // Page: 1 Doc: 22
        public ushort CurrentMemoryType { get; protected set; } // Page: 1 Doc: 22
        public byte InstalledSize { get; protected set; } // Page: 1 Doc: 22
        public byte EnabledSize { get; protected set; } // Page: 1 Doc: 22
        public byte ErrorStatus { get; protected set; } // Page: 1 Doc: 22

        public oMemoryModuleConfiguration(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oMemoryModuleConfiguration);

            try
            {
                SocketDesignation = strings[0];
                BankConnections = data[5];
                CurrentSpeed = data[6];
                CurrentMemoryType = BitConverter.ToUInt16(data, 7);
                InstalledSize = data[9];
                EnabledSize = data[10];
                ErrorStatus = data[11];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 23 TableID: BYTE
    public class oCacheInformation : BaseAbstractedTable
    {
        public string? SocketDesignation { get; protected set; } // Page: 1 Doc: 23
        public ushort CacheConfiguration { get; protected set; } // Page: 2 Doc: 23
        public ushort MaximumCacheSize { get; protected set; } // Page: 2 Doc: 23
        public ushort InstalledSize { get; protected set; } // Page: 2 Doc: 23
        public ushort SupportedSRAMType { get; protected set; } // Page: 2 Doc: 23
        public ushort CurrentSRAMType { get; protected set; } // Page: 2 Doc: 23
        public byte CacheSpeed { get; protected set; } // Page: 2 Doc: 23
        public ErrorCorrectionType ErrorCorrectionType { get; protected set; } // Page: 2 Doc: 23
        public SystemCacheType SystemCacheType { get; protected set; } // Page: 2 Doc: 23
        public Associativity Associativity { get; protected set; } // Page: 2 Doc: 23
        public byte MaximumCacheSize2 { get; protected set; } // Page: 0 Doc: 24
        public byte InstalledCacheSize2 { get; protected set; } // Page: 0 Doc: 24

        public oCacheInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oCacheInformation);

            try
            {
                SocketDesignation = strings[0];
                CacheConfiguration = BitConverter.ToUInt16(data, 5);
                MaximumCacheSize = BitConverter.ToUInt16(data, 7);
                InstalledSize = BitConverter.ToUInt16(data, 9);
                SupportedSRAMType = BitConverter.ToUInt16(data, 11);
                CurrentSRAMType = BitConverter.ToUInt16(data, 13);
                CacheSpeed = data[15];
                ErrorCorrectionType = (ErrorCorrectionType)data[16];
                SystemCacheType = (SystemCacheType)data[17];
                Associativity = (Associativity)data[18];
                MaximumCacheSize2 = data[19];
                InstalledCacheSize2 = data[23];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 24 TableID: 8
    public class oConnectorInformation : BaseAbstractedTable
    {
        public string? InternalReferenceDesignator { get; protected set; } // Page: 2 Doc: 24
        public InternalConnectorType InternalConnectorType { get; protected set; } // Page: 2 Doc: 24
        public string? ExternalReferenceDesignator { get; protected set; } // Page: 2 Doc: 24
        public ExternalConnectorType ExternalConnectorType { get; protected set; } // Page: 2 Doc: 24
        public PortType PortType { get; protected set; } // Page: 2 Doc: 24

        public oConnectorInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oConnectorInformation);

            try
            {
                InternalReferenceDesignator = strings[0];
                InternalConnectorType = (InternalConnectorType)data[5];
                ExternalReferenceDesignator = strings[1];
                ExternalConnectorType = (ExternalConnectorType)data[7];
                PortType = (PortType)data[8];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 25 TableID: BYTE
    public class oSystemSlotStructure : BaseAbstractedTable
    {
        public string? SlotDesignation { get; protected set; } // Page: 0 Doc: 26
        public SlotType SlotType { get; protected set; } // Page: 0 Doc: 26
        public SlotDataBusWidth SlotDataBusWidth { get; protected set; } // Page: 0 Doc: 26
        public CurrentUsage CurrentUsage { get; protected set; } // Page: 0 Doc: 26
        public SlotLength SlotLength { get; protected set; } // Page: 0 Doc: 26
        public ushort SlotID { get; protected set; } // Page: 0 Doc: 26
        public byte SlotCharacteristics1 { get; protected set; } // Page: 0 Doc: 26
        public byte SlotCharacteristics2 { get; protected set; } // Page: 0 Doc: 26
        public ushort SegmentGroupNumber { get; protected set; } // Page: 0 Doc: 26
        public byte BusNumber { get; protected set; } // Page: 0 Doc: 26
        public byte Device { get; protected set; } // Page: 0 Doc: 26
        public byte DataBusWidth { get; protected set; } // Page: 0 Doc: 26
        public byte Peer { get; protected set; } // Page: 0 Doc: 26
        public byte Peer2 { get; protected set; } // Page: 0 Doc: 26

        public oSystemSlotStructure(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oSystemSlotStructure);

            try
            {
                SlotDesignation = strings[0];
                SlotType = (SlotType)data[5];
                SlotDataBusWidth = (SlotDataBusWidth)data[6];
                CurrentUsage = (CurrentUsage)data[7];
                SlotLength = (SlotLength)data[8];
                SlotID = BitConverter.ToUInt16(data, 9);
                SlotCharacteristics1 = data[11];
                SlotCharacteristics2 = data[12];
                SegmentGroupNumber = BitConverter.ToUInt16(data, 13);
                BusNumber = data[15];
                Device = data[16];
                DataBusWidth = data[17];
                Peer = data[18];
                Peer2 = data[19];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 28 TableID: 10
    public class oOnBoardDevicesInformation : BaseAbstractedTable
    {

        public oOnBoardDevicesInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oOnBoardDevicesInformation);

            try
            {
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 29 TableID: 11
    public class oOEMStrings : BaseAbstractedTable
    {
        public byte Count { get; protected set; } // Page: 0 Doc: 29

        public oOEMStrings(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oOEMStrings);

            try
            {
                Count = data[4];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 29 TableID: 12
    public class oConfigurationInformation : BaseAbstractedTable
    {
        public byte Count { get; protected set; } // Page: 0 Doc: 29

        public oConfigurationInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oConfigurationInformation);

            try
            {
                Count = data[4];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 29 TableID: BYTE
    public class oLanguageInformation : BaseAbstractedTable
    {
        public byte InstallableLanguages { get; protected set; } // Page: 0 Doc: 29
        public byte Flags { get; protected set; } // Page: 1 Doc: 29
        public byte Reserved { get; protected set; } // Page: 1 Doc: 29
        public string? CurrentLanguage { get; protected set; } // Page: 1 Doc: 29

        public oLanguageInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oLanguageInformation);

            try
            {
                InstallableLanguages = data[4];
                Flags = data[5];
                Reserved = data[6];
                CurrentLanguage = strings[0];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 29 TableID: 14
    public class oGroupAssociations : BaseAbstractedTable
    {
        public string? GroupName { get; protected set; } // Page: 2 Doc: 29
        public byte ItemType { get; protected set; } // Page: 2 Doc: 29
        public ushort ItemHandle { get; protected set; } // Page: 2 Doc: 29

        public oGroupAssociations(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oGroupAssociations);

            try
            {
                GroupName = strings[0];
                ItemType = data[5];
                ItemHandle = BitConverter.ToUInt16(data, 6);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 30 TableID: BYTE
    public class oEventLogType : BaseAbstractedTable
    {
        public ushort LogAreaLength { get; protected set; } // Page: 0 Doc: 30
        public ushort LogHeaderStartOffset { get; protected set; } // Page: 0 Doc: 30
        public ushort LogDataStartOffset { get; protected set; } // Page: 1 Doc: 30
        public byte AccessMethod { get; protected set; } // Page: 1 Doc: 30
        public byte LogStatus1 { get; protected set; } // Page: 1 Doc: 30
        public uint LogChangeToken { get; protected set; } // Page: 1 Doc: 30
        public uint AccessMethodAddress { get; protected set; } // Page: 1 Doc: 30
        public LogHeaderFormat LogHeaderFormat { get; protected set; } // Page: 2 Doc: 30
        public byte NumberofSupportedLogTypeDescriptors { get; protected set; } // Page: 2 Doc: 30
        public byte LengthofeachLogTypeDescriptor { get; protected set; } // Page: 2 Doc: 30

        public oEventLogType(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oEventLogType);

            try
            {
                LogAreaLength = BitConverter.ToUInt16(data, 4);
                LogHeaderStartOffset = BitConverter.ToUInt16(data, 6);
                LogDataStartOffset = BitConverter.ToUInt16(data, 8);
                AccessMethod = data[10];
                LogStatus1 = data[11];
                LogChangeToken = BitConverter.ToUInt32(data, 12);
                AccessMethodAddress = BitConverter.ToUInt32(data, 16);
                LogHeaderFormat = (LogHeaderFormat)data[20];
                NumberofSupportedLogTypeDescriptors = data[21];
                LengthofeachLogTypeDescriptor = data[22];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 33 TableID: BYTE
    public class oPhysicalMemoryArraytype : BaseAbstractedTable
    {
        public Location Location { get; protected set; } // Page: 1 Doc: 33
        public Use Use { get; protected set; } // Page: 1 Doc: 33
        public MemoryErrorCorrection MemoryErrorCorrection { get; protected set; } // Page: 2 Doc: 33
        public uint MaximumCapacity { get; protected set; } // Page: 2 Doc: 33
        public ushort MemoryErrorInformationHandle { get; protected set; } // Page: 2 Doc: 33
        public ushort NumberofMemoryDevices { get; protected set; } // Page: 2 Doc: 33
        public ulong ExtendedMaximumCapacity { get; protected set; } // Page: 2 Doc: 33

        public oPhysicalMemoryArraytype(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oPhysicalMemoryArraytype);

            try
            {
                Location = (Location)data[4];
                Use = (Use)data[5];
                MemoryErrorCorrection = (MemoryErrorCorrection)data[6];
                MaximumCapacity = BitConverter.ToUInt32(data, 7);
                MemoryErrorInformationHandle = BitConverter.ToUInt16(data, 11);
                NumberofMemoryDevices = BitConverter.ToUInt16(data, 13);
                ExtendedMaximumCapacity = BitConverter.ToUInt64(data, 15);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 34 TableID: BYTE
    public class oMemoryDevicetype : BaseAbstractedTable
    {
        public ushort PhysicalMemoryArrayHandle { get; protected set; } // Page: 2 Doc: 34
        public ushort MemoryErrorInformationHandle { get; protected set; } // Page: 2 Doc: 34
        public ushort TotalWidth { get; protected set; } // Page: 2 Doc: 34
        public ushort DataWidth { get; protected set; } // Page: 2 Doc: 34
        public ushort Size { get; protected set; } // Page: 2 Doc: 34
        public FormFactor FormFactor { get; protected set; } // Page: 2 Doc: 34
        public byte DeviceSet { get; protected set; } // Page: 0 Doc: 35
        public string? DeviceLocator { get; protected set; } // Page: 0 Doc: 35
        public string? BankLocator { get; protected set; } // Page: 0 Doc: 35
        public MemoryType MemoryType { get; protected set; } // Page: 0 Doc: 35
        public ushort TypeDetail { get; protected set; } // Page: 0 Doc: 35
        public ushort Speed { get; protected set; } // Page: 0 Doc: 35
        public string? Manufacturer { get; protected set; } // Page: 0 Doc: 35
        public string? SerialNumber { get; protected set; } // Page: 0 Doc: 35
        public string? AssetTag { get; protected set; } // Page: 0 Doc: 35
        public string? PartNumber { get; protected set; } // Page: 0 Doc: 35
        public byte Attributes { get; protected set; } // Page: 0 Doc: 35
        public uint ExtendedSize { get; protected set; } // Page: 1 Doc: 35
        public ushort ConfiguredMemorySpeed { get; protected set; } // Page: 1 Doc: 35
        public ushort Minimumvoltage { get; protected set; } // Page: 1 Doc: 35
        public ushort Maximumvoltage { get; protected set; } // Page: 1 Doc: 35
        public ushort Configuredvoltage { get; protected set; } // Page: 1 Doc: 35
        public byte MemoryTechnology { get; protected set; } // Page: 1 Doc: 35
        public ushort MemoryOperatingModeCapability { get; protected set; } // Page: 1 Doc: 35
        public string? FirmwareVersion { get; protected set; } // Page: 1 Doc: 35
        public ushort ModuleManufacturerID { get; protected set; } // Page: 1 Doc: 35
        public ushort ModuleProductID { get; protected set; } // Page: 1 Doc: 35
        public ushort MemorySubsystemControllerManufacturerID { get; protected set; } // Page: 1 Doc: 35
        public ushort MemorySubsystemControllerProductID { get; protected set; } // Page: 1 Doc: 35
        public ulong NonvolatileSize { get; protected set; } // Page: 1 Doc: 35
        public ulong VolatileSize { get; protected set; } // Page: 2 Doc: 35
        public ulong CacheSize { get; protected set; } // Page: 2 Doc: 35
        public ulong LogicalSize { get; protected set; } // Page: 2 Doc: 35
        public uint ExtendedSpeed { get; protected set; } // Page: 2 Doc: 35
        public uint ExtendedConfiguredMemorySpeed { get; protected set; } // Page: 2 Doc: 35

        public oMemoryDevicetype(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oMemoryDevicetype);

            try
            {
                PhysicalMemoryArrayHandle = BitConverter.ToUInt16(data, 4);
                MemoryErrorInformationHandle = BitConverter.ToUInt16(data, 6);
                TotalWidth = BitConverter.ToUInt16(data, 8);
                DataWidth = BitConverter.ToUInt16(data, 10);
                Size = BitConverter.ToUInt16(data, 12);
                FormFactor = (FormFactor)data[14];
                DeviceSet = data[15];
                DeviceLocator = strings[0];
                BankLocator = strings[1];
                MemoryType = (MemoryType)data[18];
                TypeDetail = BitConverter.ToUInt16(data, 19);
                Speed = BitConverter.ToUInt16(data, 21);
                Manufacturer = strings[2];
                SerialNumber = strings[3];
                AssetTag = strings[4];
                PartNumber = strings[5];
                Attributes = data[27];
                ExtendedSize = BitConverter.ToUInt32(data, 28);
                ConfiguredMemorySpeed = BitConverter.ToUInt16(data, 32);
                Minimumvoltage = BitConverter.ToUInt16(data, 34);
                Maximumvoltage = BitConverter.ToUInt16(data, 36);
                Configuredvoltage = BitConverter.ToUInt16(data, 38);
                MemoryTechnology = data[40];
                MemoryOperatingModeCapability = BitConverter.ToUInt16(data, 41);
                FirmwareVersion = strings[6];
                ModuleManufacturerID = BitConverter.ToUInt16(data, 44);
                ModuleProductID = BitConverter.ToUInt16(data, 46);
                MemorySubsystemControllerManufacturerID = BitConverter.ToUInt16(data, 48);
                MemorySubsystemControllerProductID = BitConverter.ToUInt16(data, 50);
                NonvolatileSize = BitConverter.ToUInt64(data, 52);
                VolatileSize = BitConverter.ToUInt64(data, 60);
                CacheSize = BitConverter.ToUInt64(data, 68);
                LogicalSize = BitConverter.ToUInt64(data, 76);
                ExtendedSpeed = BitConverter.ToUInt32(data, 84);
                ExtendedConfiguredMemorySpeed = BitConverter.ToUInt32(data, 88);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 37 TableID: BYTE
    public class o32bitMemoryErrorInformationtype : BaseAbstractedTable
    {
        public ErrorType ErrorType { get; protected set; } // Page: 2 Doc: 37
        public ErrorGranularity ErrorGranularity { get; protected set; } // Page: 2 Doc: 37
        public ErrorOperation ErrorOperation { get; protected set; } // Page: 2 Doc: 37
        public uint VendorSyndrome { get; protected set; } // Page: 2 Doc: 37
        public uint MemoryArrayErrorAddress { get; protected set; } // Page: 2 Doc: 37
        public uint DeviceErrorAddress { get; protected set; } // Page: 2 Doc: 37
        public uint ErrorResolution { get; protected set; } // Page: 0 Doc: 38

        public o32bitMemoryErrorInformationtype(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(o32bitMemoryErrorInformationtype);

            try
            {
                ErrorType = (ErrorType)data[4];
                ErrorGranularity = (ErrorGranularity)data[5];
                ErrorOperation = (ErrorOperation)data[6];
                VendorSyndrome = BitConverter.ToUInt32(data, 7);
                MemoryArrayErrorAddress = BitConverter.ToUInt32(data, 11);
                DeviceErrorAddress = BitConverter.ToUInt32(data, 15);
                ErrorResolution = BitConverter.ToUInt32(data, 19);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 38 TableID: BYTE
    public class oMemoryArrayMappedAddress : BaseAbstractedTable
    {
        public uint StartingAddress { get; protected set; } // Page: 1 Doc: 38
        public uint EndingAddress { get; protected set; } // Page: 1 Doc: 38
        public ushort MemoryArrayHandle { get; protected set; } // Page: 1 Doc: 38
        public byte PartitionWidth { get; protected set; } // Page: 1 Doc: 38
        public ulong ExtendedStartingAddress { get; protected set; } // Page: 2 Doc: 38
        public ulong ExtendedEndingAddress { get; protected set; } // Page: 2 Doc: 38

        public oMemoryArrayMappedAddress(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oMemoryArrayMappedAddress);

            try
            {
                StartingAddress = BitConverter.ToUInt32(data, 4);
                EndingAddress = BitConverter.ToUInt32(data, 8);
                MemoryArrayHandle = BitConverter.ToUInt16(data, 12);
                PartitionWidth = data[14];
                ExtendedStartingAddress = BitConverter.ToUInt64(data, 15);
                ExtendedEndingAddress = BitConverter.ToUInt64(data, 23);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 38 TableID: BYTE
    public class oMemoryDeviceMappedAddress : BaseAbstractedTable
    {
        public uint StartingAddress { get; protected set; } // Page: 2 Doc: 38
        public uint EndingAddress { get; protected set; } // Page: 2 Doc: 38
        public ushort MemoryDeviceHandle { get; protected set; } // Page: 0 Doc: 39
        public ushort MemoryArrayMappedAddressHandle { get; protected set; } // Page: 0 Doc: 39
        public byte PartitionRowPosition { get; protected set; } // Page: 0 Doc: 39
        public byte InterleavePosition { get; protected set; } // Page: 0 Doc: 39
        public byte InterleavedDataDepth { get; protected set; } // Page: 0 Doc: 39
        public ulong ExtendedStartingAddress { get; protected set; } // Page: 0 Doc: 39
        public ulong ExtendedEndingAddress { get; protected set; } // Page: 0 Doc: 39

        public oMemoryDeviceMappedAddress(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oMemoryDeviceMappedAddress);

            try
            {
                StartingAddress = BitConverter.ToUInt32(data, 4);
                EndingAddress = BitConverter.ToUInt32(data, 8);
                MemoryDeviceHandle = BitConverter.ToUInt16(data, 12);
                MemoryArrayMappedAddressHandle = BitConverter.ToUInt16(data, 14);
                PartitionRowPosition = data[16];
                InterleavePosition = data[17];
                InterleavedDataDepth = data[18];
                ExtendedStartingAddress = BitConverter.ToUInt64(data, 19);
                ExtendedEndingAddress = BitConverter.ToUInt64(data, 27);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 39 TableID: BYTE
    public class oBuiltinPointingDevice : BaseAbstractedTable
    {
        public Type2 Type2 { get; protected set; } // Page: 1 Doc: 39
        public Interface Interface { get; protected set; } // Page: 1 Doc: 39
        public byte NumberofButtons { get; protected set; } // Page: 1 Doc: 39

        public oBuiltinPointingDevice(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oBuiltinPointingDevice);

            try
            {
                Type2 = (Type2)data[4];
                Interface = (Interface)data[5];
                NumberofButtons = data[6];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 39 TableID: BYTE
    public class oPortableBattery : BaseAbstractedTable
    {
        public string? Location { get; protected set; } // Page: 2 Doc: 39
        public string? Manufacturer { get; protected set; } // Page: 2 Doc: 39
        public string? ManufactureDate { get; protected set; } // Page: 2 Doc: 39
        public string? SerialNumber { get; protected set; } // Page: 0 Doc: 40
        public string? DeviceName { get; protected set; } // Page: 0 Doc: 40
        public DeviceChemistry DeviceChemistry { get; protected set; } // Page: 0 Doc: 40
        public ushort DesignCapacity { get; protected set; } // Page: 0 Doc: 40
        public ushort DesignVoltage { get; protected set; } // Page: 0 Doc: 40
        public string? SBDSVersionNumber { get; protected set; } // Page: 0 Doc: 40
        public byte MaximumErrorinBatteryData { get; protected set; } // Page: 0 Doc: 40
        public ushort SBDSSerialNumber { get; protected set; } // Page: 0 Doc: 40
        public ushort SBDSManufactureDate { get; protected set; } // Page: 0 Doc: 40
        public string? SBDSDeviceChemistry { get; protected set; } // Page: 1 Doc: 40
        public byte DesignCapacityMultiplier { get; protected set; } // Page: 1 Doc: 40
        public uint OEMspecific { get; protected set; } // Page: 1 Doc: 40

        public oPortableBattery(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oPortableBattery);

            try
            {
                Location = strings[0];
                Manufacturer = strings[1];
                ManufactureDate = strings[2];
                SerialNumber = strings[3];
                DeviceName = strings[4];
                DeviceChemistry = (DeviceChemistry)data[9];
                DesignCapacity = BitConverter.ToUInt16(data, 10);
                DesignVoltage = BitConverter.ToUInt16(data, 12);
                SBDSVersionNumber = strings[5];
                MaximumErrorinBatteryData = data[15];
                SBDSSerialNumber = BitConverter.ToUInt16(data, 16);
                SBDSManufactureDate = BitConverter.ToUInt16(data, 18);
                SBDSDeviceChemistry = strings[6];
                DesignCapacityMultiplier = data[21];
                OEMspecific = BitConverter.ToUInt32(data, 22);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 40 TableID: 23
    public class oSystemReset : BaseAbstractedTable
    {
        public byte Capabilities { get; protected set; } // Page: 2 Doc: 40
        public ushort ResetCount { get; protected set; } // Page: 2 Doc: 40
        public ushort ResetLimit { get; protected set; } // Page: 2 Doc: 40
        public ushort TimerInterval { get; protected set; } // Page: 2 Doc: 40
        public ushort Timeout { get; protected set; } // Page: 2 Doc: 40

        public oSystemReset(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oSystemReset);

            try
            {
                Capabilities = data[4];
                ResetCount = BitConverter.ToUInt16(data, 5);
                ResetLimit = BitConverter.ToUInt16(data, 7);
                TimerInterval = BitConverter.ToUInt16(data, 9);
                Timeout = BitConverter.ToUInt16(data, 11);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 40 TableID: 24
    public class oHardwareSecurity : BaseAbstractedTable
    {
        public byte HardwareSecuritySettings { get; protected set; } // Page: 0 Doc: 41

        public oHardwareSecurity(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oHardwareSecurity);

            try
            {
                HardwareSecuritySettings = data[4];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 41 TableID: 25
    public class oSystemPowerControls : BaseAbstractedTable
    {
        public byte NextScheduledPoweronMonth { get; protected set; } // Page: 0 Doc: 41
        public byte NextScheduledPoweronDayofmonth { get; protected set; } // Page: 1 Doc: 41
        public byte NextScheduledPoweronHour { get; protected set; } // Page: 1 Doc: 41
        public byte NextScheduledPoweronMinute { get; protected set; } // Page: 1 Doc: 41
        public byte NextScheduledPoweronSecond { get; protected set; } // Page: 1 Doc: 41

        public oSystemPowerControls(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oSystemPowerControls);

            try
            {
                NextScheduledPoweronMonth = data[4];
                NextScheduledPoweronDayofmonth = data[5];
                NextScheduledPoweronHour = data[6];
                NextScheduledPoweronMinute = data[7];
                NextScheduledPoweronSecond = data[8];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 41 TableID: 26
    public class oVoltageProbe : BaseAbstractedTable
    {
        public string? Description { get; protected set; } // Page: 1 Doc: 41
        public byte LocationandStatus { get; protected set; } // Page: 1 Doc: 41
        public ushort MaximumValue { get; protected set; } // Page: 1 Doc: 41
        public ushort MinimumValue { get; protected set; } // Page: 1 Doc: 41
        public ushort Resolution { get; protected set; } // Page: 1 Doc: 41
        public ushort Tolerance { get; protected set; } // Page: 1 Doc: 41
        public ushort Accuracy { get; protected set; } // Page: 1 Doc: 41
        public uint OEMdefined { get; protected set; } // Page: 2 Doc: 41
        public ushort NominalValue { get; protected set; } // Page: 2 Doc: 41

        public oVoltageProbe(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oVoltageProbe);

            try
            {
                Description = strings[0];
                LocationandStatus = data[5];
                MaximumValue = BitConverter.ToUInt16(data, 6);
                MinimumValue = BitConverter.ToUInt16(data, 8);
                Resolution = BitConverter.ToUInt16(data, 10);
                Tolerance = BitConverter.ToUInt16(data, 12);
                Accuracy = BitConverter.ToUInt16(data, 14);
                OEMdefined = BitConverter.ToUInt32(data, 16);
                NominalValue = BitConverter.ToUInt16(data, 20);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 41 TableID: BYTE
    public class oCoolingDevice : BaseAbstractedTable
    {
        public ushort TemperatureProbeHandle { get; protected set; } // Page: 0 Doc: 42
        public byte DeviceTypeandStatus { get; protected set; } // Page: 0 Doc: 42
        public byte CoolingUnitGroup { get; protected set; } // Page: 0 Doc: 42
        public uint OEMdefined { get; protected set; } // Page: 0 Doc: 42
        public ushort NominalSpeed { get; protected set; } // Page: 0 Doc: 42
        public string? Description { get; protected set; } // Page: 0 Doc: 42

        public oCoolingDevice(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oCoolingDevice);

            try
            {
                TemperatureProbeHandle = BitConverter.ToUInt16(data, 4);
                DeviceTypeandStatus = data[6];
                CoolingUnitGroup = data[7];
                OEMdefined = BitConverter.ToUInt32(data, 8);
                NominalSpeed = BitConverter.ToUInt16(data, 12);
                Description = strings[0];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 42 TableID: 28
    public class oTemperatureProbe : BaseAbstractedTable
    {
        public string? Description { get; protected set; } // Page: 1 Doc: 42
        public byte LocationandStatus { get; protected set; } // Page: 1 Doc: 42
        public ushort MaximumValue { get; protected set; } // Page: 1 Doc: 42
        public ushort MinimumValue { get; protected set; } // Page: 1 Doc: 42
        public ushort Resolution { get; protected set; } // Page: 1 Doc: 42
        public ushort Tolerance { get; protected set; } // Page: 1 Doc: 42
        public ushort Accuracy { get; protected set; } // Page: 1 Doc: 42
        public uint OEMdefined { get; protected set; } // Page: 1 Doc: 42
        public ushort NominalValue { get; protected set; } // Page: 1 Doc: 42

        public oTemperatureProbe(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oTemperatureProbe);

            try
            {
                Description = strings[0];
                LocationandStatus = data[5];
                MaximumValue = BitConverter.ToUInt16(data, 6);
                MinimumValue = BitConverter.ToUInt16(data, 8);
                Resolution = BitConverter.ToUInt16(data, 10);
                Tolerance = BitConverter.ToUInt16(data, 12);
                Accuracy = BitConverter.ToUInt16(data, 14);
                OEMdefined = BitConverter.ToUInt32(data, 16);
                NominalValue = BitConverter.ToUInt16(data, 20);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 42 TableID: 29
    public class oElectricalCurrentProbe : BaseAbstractedTable
    {
        public string? Description { get; protected set; } // Page: 2 Doc: 42
        public LocationandStatus LocationandStatus { get; protected set; } // Page: 2 Doc: 42
        public ushort MaximumValue { get; protected set; } // Page: 2 Doc: 42
        public ushort MinimumValue { get; protected set; } // Page: 0 Doc: 43
        public ushort Resolution { get; protected set; } // Page: 0 Doc: 43
        public ushort Tolerance { get; protected set; } // Page: 0 Doc: 43
        public ushort Accuracy { get; protected set; } // Page: 0 Doc: 43
        public uint OEMdefined { get; protected set; } // Page: 0 Doc: 43
        public ushort NominalValue { get; protected set; } // Page: 0 Doc: 43

        public oElectricalCurrentProbe(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oElectricalCurrentProbe);

            try
            {
                Description = strings[0];
                LocationandStatus = (LocationandStatus)data[5];
                MaximumValue = BitConverter.ToUInt16(data, 6);
                MinimumValue = BitConverter.ToUInt16(data, 8);
                Resolution = BitConverter.ToUInt16(data, 10);
                Tolerance = BitConverter.ToUInt16(data, 12);
                Accuracy = BitConverter.ToUInt16(data, 14);
                OEMdefined = BitConverter.ToUInt32(data, 16);
                NominalValue = BitConverter.ToUInt16(data, 20);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 43 TableID: 30
    public class oOutofBandRemoteAccess : BaseAbstractedTable
    {
        public string? ManufacturerName { get; protected set; } // Page: 1 Doc: 43
        public byte Connections { get; protected set; } // Page: 1 Doc: 43

        public oOutofBandRemoteAccess(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oOutofBandRemoteAccess);

            try
            {
                ManufacturerName = strings[0];
                Connections = data[5];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 43 TableID: 32
    public class oSystemBootInformation : BaseAbstractedTable
    {
        public byte Reserved { get; protected set; } // Page: 2 Doc: 43
        public byte BootStatus { get; protected set; } // Page: 2 Doc: 43

        public oSystemBootInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oSystemBootInformation);

            try
            {
                Reserved = data[4];
                BootStatus = data[10];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 44 TableID: 33
    public class o64bitMemoryErrorInformationtype : BaseAbstractedTable
    {
        public ErrorType ErrorType { get; protected set; } // Page: 0 Doc: 44
        public ErrorGranularity ErrorGranularity { get; protected set; } // Page: 0 Doc: 44
        public ErrorOperation ErrorOperation { get; protected set; } // Page: 0 Doc: 44
        public uint VendorSyndrome { get; protected set; } // Page: 0 Doc: 44
        public ulong MemoryArrayErrorAddress { get; protected set; } // Page: 0 Doc: 44
        public ulong DeviceErrorAddress { get; protected set; } // Page: 0 Doc: 44
        public uint ErrorResolution { get; protected set; } // Page: 0 Doc: 44

        public o64bitMemoryErrorInformationtype(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(o64bitMemoryErrorInformationtype);

            try
            {
                ErrorType = (ErrorType)data[4];
                ErrorGranularity = (ErrorGranularity)data[5];
                ErrorOperation = (ErrorOperation)data[6];
                VendorSyndrome = BitConverter.ToUInt32(data, 7);
                MemoryArrayErrorAddress = BitConverter.ToUInt64(data, 11);
                DeviceErrorAddress = BitConverter.ToUInt64(data, 19);
                ErrorResolution = BitConverter.ToUInt32(data, 27);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 44 TableID: 34
    public class oManagementDevice : BaseAbstractedTable
    {
        public string? Description { get; protected set; } // Page: 0 Doc: 44
        public byte Type2 { get; protected set; } // Page: 0 Doc: 44
        public uint Address { get; protected set; } // Page: 0 Doc: 44
        public byte AddressType { get; protected set; } // Page: 1 Doc: 44

        public oManagementDevice(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oManagementDevice);

            try
            {
                Description = strings[0];
                Type2 = data[5];
                Address = BitConverter.ToUInt32(data, 6);
                AddressType = data[10];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 44 TableID: 35
    public class oManagementDeviceComponent : BaseAbstractedTable
    {
        public string? Description { get; protected set; } // Page: 2 Doc: 44
        public ushort ManagementDeviceHandle { get; protected set; } // Page: 2 Doc: 44
        public ushort ComponentHandle { get; protected set; } // Page: 2 Doc: 44
        public ushort ThresholdHandle { get; protected set; } // Page: 2 Doc: 44

        public oManagementDeviceComponent(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oManagementDeviceComponent);

            try
            {
                Description = strings[0];
                ManagementDeviceHandle = BitConverter.ToUInt16(data, 5);
                ComponentHandle = BitConverter.ToUInt16(data, 7);
                ThresholdHandle = BitConverter.ToUInt16(data, 9);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 44 TableID: 36
    public class oManagementDeviceThresholdData : BaseAbstractedTable
    {
        public ushort LowerThresholdNoncritical { get; protected set; } // Page: 2 Doc: 44
        public ushort UpperThresholdNoncritical { get; protected set; } // Page: 2 Doc: 44
        public ushort LowerThresholdCritical { get; protected set; } // Page: 2 Doc: 44
        public ushort UpperThresholdCritical { get; protected set; } // Page: 2 Doc: 44
        public ushort LowerThresholdNonrecoverable { get; protected set; } // Page: 2 Doc: 44
        public ushort UpperThresholdNonrecoverable { get; protected set; } // Page: 2 Doc: 44

        public oManagementDeviceThresholdData(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oManagementDeviceThresholdData);

            try
            {
                LowerThresholdNoncritical = BitConverter.ToUInt16(data, 4);
                UpperThresholdNoncritical = BitConverter.ToUInt16(data, 6);
                LowerThresholdCritical = BitConverter.ToUInt16(data, 8);
                UpperThresholdCritical = BitConverter.ToUInt16(data, 10);
                LowerThresholdNonrecoverable = BitConverter.ToUInt16(data, 12);
                UpperThresholdNonrecoverable = BitConverter.ToUInt16(data, 14);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 45 TableID: 37
    public class oManagementDeviceThresholdData2 : BaseAbstractedTable
    {
        public byte ChannelType { get; protected set; } // Page: 0 Doc: 45
        public byte MaximumChannelLoad { get; protected set; } // Page: 0 Doc: 45
        public byte MemoryDeviceCount { get; protected set; } // Page: 0 Doc: 45
        public byte Memory1DeviceLoad { get; protected set; } // Page: 0 Doc: 45
        public ushort MemoryDevice1Handle { get; protected set; } // Page: 0 Doc: 45

        public oManagementDeviceThresholdData2(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oManagementDeviceThresholdData2);

            try
            {
                ChannelType = data[4];
                MaximumChannelLoad = data[5];
                MemoryDeviceCount = data[6];
                Memory1DeviceLoad = data[7];
                MemoryDevice1Handle = BitConverter.ToUInt16(data, 8);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 45 TableID: 38
    public class oIPMIDeviceInformation : BaseAbstractedTable
    {
        public InterfaceType InterfaceType { get; protected set; } // Page: 1 Doc: 45
        public byte IPMISpecificationRevision { get; protected set; } // Page: 1 Doc: 45
        public byte I2CTargetAddress { get; protected set; } // Page: 1 Doc: 45
        public byte NVStorageDeviceAddress { get; protected set; } // Page: 1 Doc: 45
        public ulong BaseAddress { get; protected set; } // Page: 1 Doc: 45
        public byte BaseAddressModifier { get; protected set; } // Page: 2 Doc: 45
        public byte InterruptNumber { get; protected set; } // Page: 2 Doc: 45

        public oIPMIDeviceInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oIPMIDeviceInformation);

            try
            {
                InterfaceType = (InterfaceType)data[4];
                IPMISpecificationRevision = data[5];
                I2CTargetAddress = data[6];
                NVStorageDeviceAddress = data[7];
                BaseAddress = BitConverter.ToUInt64(data, 8);
                BaseAddressModifier = data[16];
                InterruptNumber = data[17];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 46 TableID: 39
    public class oPowerSupplyStructure : BaseAbstractedTable
    {
        public byte PowerUnitGroup { get; protected set; } // Page: 0 Doc: 46
        public string? Location { get; protected set; } // Page: 0 Doc: 46
        public string? DeviceName { get; protected set; } // Page: 0 Doc: 46
        public string? Manufacturer { get; protected set; } // Page: 0 Doc: 46
        public string? SerialNumber { get; protected set; } // Page: 0 Doc: 46
        public string? AssetTagNumber { get; protected set; } // Page: 0 Doc: 46
        public string? ModelPartNumber { get; protected set; } // Page: 0 Doc: 46
        public string? RevisionLevel { get; protected set; } // Page: 0 Doc: 46
        public ushort MaxPowerCapacity { get; protected set; } // Page: 0 Doc: 46
        public ushort PowerSupplyCharacteristics { get; protected set; } // Page: 0 Doc: 46
        public ushort InputVoltageProbeHandle { get; protected set; } // Page: 0 Doc: 46
        public ushort CoolingDeviceHandle { get; protected set; } // Page: 0 Doc: 46
        public ushort InputCurrentProbeHandle { get; protected set; } // Page: 0 Doc: 46

        public oPowerSupplyStructure(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oPowerSupplyStructure);

            try
            {
                PowerUnitGroup = data[4];
                Location = strings[0];
                DeviceName = strings[1];
                Manufacturer = strings[2];
                SerialNumber = strings[3];
                AssetTagNumber = strings[4];
                ModelPartNumber = strings[5];
                RevisionLevel = strings[6];
                MaxPowerCapacity = BitConverter.ToUInt16(data, 12);
                PowerSupplyCharacteristics = BitConverter.ToUInt16(data, 14);
                InputVoltageProbeHandle = BitConverter.ToUInt16(data, 16);
                CoolingDeviceHandle = BitConverter.ToUInt16(data, 18);
                InputCurrentProbeHandle = BitConverter.ToUInt16(data, 20);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 46 TableID: 40
    public class oAdditionalInformationtype : BaseAbstractedTable
    {
        public byte EntryLength { get; protected set; } // Page: 2 Doc: 46
        public ushort ReferencedHandle { get; protected set; } // Page: 2 Doc: 46
        public byte ReferencedOffset { get; protected set; } // Page: 2 Doc: 46
        public string? String { get; protected set; } // Page: 2 Doc: 46
        public byte Value { get; protected set; } // Page: 2 Doc: 46

        public oAdditionalInformationtype(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oAdditionalInformationtype);

            try
            {
                EntryLength = data[0];
                ReferencedHandle = BitConverter.ToUInt16(data, 1);
                ReferencedOffset = data[3];
                String = strings[0];
                Value = data[5];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 47 TableID: 41
    public class oOnboardDevicesExtendedInformation : BaseAbstractedTable
    {
        public byte ReferenceDesignation { get; protected set; } // Page: 0 Doc: 47
        public DeviceType DeviceType { get; protected set; } // Page: 0 Doc: 47
        public byte DeviceTypeInstance { get; protected set; } // Page: 0 Doc: 47
        public ushort SegmentGroupNumber { get; protected set; } // Page: 0 Doc: 47
        public byte BusNumber { get; protected set; } // Page: 0 Doc: 47
        public byte Device { get; protected set; } // Page: 0 Doc: 47

        public oOnboardDevicesExtendedInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oOnboardDevicesExtendedInformation);

            try
            {
                ReferenceDesignation = data[4];
                DeviceType = (DeviceType)data[5];
                DeviceTypeInstance = data[6];
                SegmentGroupNumber = BitConverter.ToUInt16(data, 7);
                BusNumber = data[9];
                Device = data[10];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 47 TableID: 42
    public class oManagementControllerHostInterface : BaseAbstractedTable
    {
        public InterfaceType InterfaceType { get; protected set; } // Page: 2 Doc: 47
        public byte InterfaceTypeSpecificDataLength { get; protected set; } // Page: 2 Doc: 47
        public byte InterfaceTypeSpecificData { get; protected set; } // Page: 2 Doc: 47

        public oManagementControllerHostInterface(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oManagementControllerHostInterface);

            try
            {
                InterfaceType = (InterfaceType)data[4];
                InterfaceTypeSpecificDataLength = data[5];
                InterfaceTypeSpecificData = data[6];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 48 TableID: 43
    public class oTPMDevice : BaseAbstractedTable
    {
        public byte VendorID { get; protected set; } // Page: 1 Doc: 48
        public byte MajorSpecVersion { get; protected set; } // Page: 1 Doc: 48
        public byte MinorSpecVersion { get; protected set; } // Page: 1 Doc: 48
        public uint FirmwareVersion1 { get; protected set; } // Page: 1 Doc: 48
        public uint FirmwareVersion2 { get; protected set; } // Page: 1 Doc: 48
        public string? Description { get; protected set; } // Page: 1 Doc: 48
        public ulong Characteristics { get; protected set; } // Page: 1 Doc: 48
        public uint OEMdefined { get; protected set; } // Page: 1 Doc: 48

        public oTPMDevice(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oTPMDevice);

            try
            {
                VendorID = data[4];
                MajorSpecVersion = data[8];
                MinorSpecVersion = data[9];
                FirmwareVersion1 = BitConverter.ToUInt32(data, 10);
                FirmwareVersion2 = BitConverter.ToUInt32(data, 14);
                Description = strings[0];
                Characteristics = BitConverter.ToUInt64(data, 19);
                OEMdefined = BitConverter.ToUInt32(data, 27);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 2 Doc: 48 TableID: 44
    public class oProcessorAdditionalInformation : BaseAbstractedTable
    {
        public ushort ReferencedHandle { get; protected set; } // Page: 2 Doc: 48
        public byte ProcessorSpecificBlock { get; protected set; } // Page: 2 Doc: 48

        public oProcessorAdditionalInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oProcessorAdditionalInformation);

            try
            {
                ReferencedHandle = BitConverter.ToUInt16(data, 4);
                ProcessorSpecificBlock = data[6];
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 0 Doc: 49 TableID: 45
    public class oFirmwareInventoryInformation : BaseAbstractedTable
    {

        public oFirmwareInventoryInformation(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oFirmwareInventoryInformation);

            try
            {
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 50 TableID: 46
    public class oStringProperty : BaseAbstractedTable
    {
        public ushort StringPropertyID { get; protected set; } // Page: 1 Doc: 50
        public string? StringPropertyValue { get; protected set; } // Page: 1 Doc: 50
        public ushort Parenthandle { get; protected set; } // Page: 1 Doc: 50

        public oStringProperty(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oStringProperty);

            try
            {
                StringPropertyID = BitConverter.ToUInt16(data, 4);
                StringPropertyValue = strings[0];
                Parenthandle = BitConverter.ToUInt16(data, 7);
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000 /// Page: 1 Doc: 50 TableID: 126
    public class oInactive : BaseAbstractedTable
    {

        public oInactive(byte type, byte length, ushort handle, byte[] data, List<string> strings)
        {
            Type = type;
            Length = length;
            Handle = handle;
            RTYPE_ = typeof(oInactive);

            try
            {
            }
            catch
            {
                Console.WriteLine("[!] Exhausted table structure");
            }
        }
    }
    // automated by stink-o-tron 3000
    public enum WakeupType : byte
    {
        Reserved = 0,
        Other = 1,
        Unknown = 2,
        APMTimer = 3,
        ModemRing = 4,
        LANRemote = 5,
        PowerSwitch = 6,
        PCIPME = 7,
        ACPowerRestored = 8,
    }
    // automated by stink-o-tron 3000
    public enum BoardType : byte
    {
        Unknown = 1,
        Other = 2,
        ServerBlade = 3,
        ConnectivitySwitch = 4,
        SystemManagementModule = 5,
        ProcessorModule = 6,
        I = 7,
        MemoryModule = 8,
        Daughterboard = 9,
        Motherboard = 10,
        Processor = 11,
        Processor3 = 12,
        Interconnectboard = 13,
    }
    // automated by stink-o-tron 3000
    public enum BootupState : byte
    {
        Other = 1,
        Unknown = 2,
        Safe = 3,
        Warning = 4,
        Critical = 5,
        Nonrecoverable = 6,
    }
    // automated by stink-o-tron 3000
    public enum PowerSupplyState : byte
    {
        Other = 1,
        Unknown = 2,
        None = 3,
        Externalinterfacelockedout = 4,
        Externalinterfaceenabled = 5,
    }
    // automated by stink-o-tron 3000
    public enum ThermalState : byte
    {
        Other = 1,
        Unknown = 2,
        CentralProcessor = 3,
        MathProcessor = 4,
        DSPProcessor = 5,
        VideoProcessor = 6,
    }
    // automated by stink-o-tron 3000
    public enum SecurityStatus : byte
    {
        Other = 1,
        Unknown = 2,
        DaughterBoard = 3,
        ZIFSocket = 4,
        ReplaceablePiggyBack = 5,
        None = 6,
        LIFSocket = 7,
    }
    // automated by stink-o-tron 3000
    public enum ProcessorType : byte
    {
        Slot1 = 8,
        Slot2 = 9,
        D370pinsocket = 10,
        SlotA = 11,
        SlotM = 12,
        Socket423 = 13,
        SocketA = 14,
        Socket478 = 15,
        Socket754 = 16,
        Socket940 = 17,
        Socket939 = 18,
        SocketmPGA604 = 19,
        SocketLGA771 = 20,
        SocketLGA775 = 21,
        SocketS1 = 22,
        SocketAM2 = 23,
        SocketF = 24,
        SocketLGA1366 = 25,
        SocketG34 = 26,
        SocketAM3 = 27,
        SocketC32 = 28,
        SocketLGA1156 = 29,
        SocketLGA1567 = 30,
        SocketPGA988A = 31,
        SocketBGA1288 = 32,
        SocketrPGA988B = 33,
        SocketBGA1023 = 34,
        SocketBGA1224 = 35,
        SocketLGA1155 = 36,
        SocketLGA1356 = 37,
        SocketLGA2011 = 38,
        SocketFS1 = 39,
        SocketFS2 = 40,
        SocketFM1 = 41,
        SocketFM2 = 42,
        SocketLGA20113 = 43,
    }
    // automated by stink-o-tron 3000
    public enum ProcessorFamily : byte
    {
        SocketLGA13563 = 44,
        SocketLGA1150 = 45,
        SocketBGA1168 = 46,
        SocketBGA1234 = 47,
        SocketBGA1364 = 48,
        SocketAM4 = 49,
        SocketLGA1151 = 50,
        SocketBGA1356 = 51,
        SocketBGA1440 = 52,
        SocketBGA1515 = 53,
        SocketLGA36471 = 54,
        SocketSP3 = 55,
        SocketSP3r2 = 56,
        SocketLGA2066 = 57,
        SocketBGA1392 = 58,
        SocketBGA1510 = 59,
        SocketBGA1528 = 60,
        SocketLGA4189 = 61,
        SocketLGA1200 = 62,
        SocketLGA4677 = 63,
        SocketLGA1700 = 64,
        SocketBGA1744 = 65,
        SocketBGA1781 = 66,
        SocketBGA1211 = 67,
        SocketBGA2422 = 68,
        SocketLGA1211 = 69,
        SocketLGA2422 = 70,
        SocketLGA5773 = 71,
        SocketBGA5773 = 72,
    }
    // automated by stink-o-tron 3000
    public enum ProcessorUpgrade : byte
    {
        Other = 1,
        Unknown = 2,
        None = 3,
        D8bitParity = 4,
        D32bitECC = 5,
        D64bitECC = 6,
        D128bitECC = 7,
        CRC = 8,
    }
    // automated by stink-o-tron 3000
    public enum ProcessorFamily2 : byte
    {
        Other = 1,
        Unknown = 2,
        OneWayInterleave = 3,
        TwoWayInterleave = 4,
        FourWayInterleave = 5,
        EightWayInterleave = 6,
        SixteenWayInterleave = 7,
    }
    // automated by stink-o-tron 3000
    public enum ErrorDetectingMethod : byte
    {
        Other = 1,
        Unknown = 2,
        None = 3,
        Parity = 4,
        SinglebitECC = 5,
        MultibitECC = 6,
    }
    // automated by stink-o-tron 3000
    public enum SupportedInterleave : byte
    {
        Other = 1,
        Unknown = 2,
        Instruction = 3,
        Data = 4,
        Unified = 5,
    }
    // automated by stink-o-tron 3000
    public enum CurrentInterleave : byte
    {
        Other = 1,
        Unknown = 2,
        DirectMapped = 3,
        D2waySetAssociative = 4,
        D4waySetAssociative = 5,
        FullyAssociative = 6,
        D8waySetAssociative = 7,
        D16waySetAssociative = 8,
        D12waySetAssociative = 9,
        D24waySetAssociative = 10,
        D32waySetAssociative = 11,
        D48waySetAssociative = 12,
        D64waySetAssociative = 13,
        D20waySetAssociative = 14,
    }
    // automated by stink-o-tron 3000
    public enum ErrorCorrectionType : byte
    {
        None = 0,
        Centronics = 1,
        MiniCentronics = 2,
        Proprietary = 3,
        DB25pinmale = 4,
        DB25pinfemale = 5,
        DB15pinmale = 6,
        DB15pinfemale = 7,
        DB9pinmale = 8,
        DB9pinfemale = 9,
        RJ11 = 10,
        RJ45 = 11,
        D50pinMiniSCSI = 12,
        MiniDIN = 13,
        MicroDIN = 14,
        PS = 15,
        Infrared = 16,
        HPHIL = 17,
        AccessBus = 18,
        SSASCSI = 19,
    }
    // automated by stink-o-tron 3000
    public enum SystemCacheType : byte
    {
        CircularDIN8male = 20,
        CircularDIN8female = 21,
        OnBoardIDE = 22,
        OnBoardFloppy = 23,
        D9pinDualInline = 24,
        D25pinDualInline = 25,
        D50pinDualInline = 26,
        D68pinDualInline = 27,
        OnBoardSoundInputfromCDROM = 28,
        MiniCentronicsType14 = 29,
        MiniCentronicsType26 = 30,
        Minijack = 31,
        BNC = 32,
        D1394 = 33,
        SAS = 34,
        USBTypeCReceptacle = 35,
        PC98 = 160,
        PC98Hireso = 161,
        PCH98 = 162,
        PC98Note = 163,
        PC98Full = 164,
        OtherUseReferenceDesignatorStringstosupplyinformation = 255,
    }
    // automated by stink-o-tron 3000
    public enum Associativity : byte
    {
        None = 0,
        ParallelPortXT = 1,
        ParallelPortPS = 2,
        ParallelPortECP = 3,
        ParallelPortEPP = 4,
        ParallelPortECP2 = 5,
        SerialPortXT = 6,
        SerialPort16450Compatible = 7,
        SerialPort16550Compatible = 8,
        SerialPort16550ACompatible = 9,
        SCSIPort = 10,
        MIDIPort = 11,
        JoyStickPort = 12,
        KeyboardPort = 13,
    }
    // automated by stink-o-tron 3000
    public enum InternalConnectorType : byte
    {
        MousePort = 14,
        SSASCSI = 15,
        USB = 16,
        FireWire = 17,
        PCMCIATypeI2 = 18,
        PCMCIATypeII = 19,
        PCMCIATypeIII = 20,
        Cardbus = 21,
        AccessBusPort = 22,
        SCSIII = 23,
        SCSIWide = 24,
        PC98 = 25,
        PC98Hireso = 26,
        PCH98 = 27,
        VideoPort = 28,
        AudioPort = 29,
        ModemPort = 30,
        NetworkPort = 31,
        SATA = 32,
        SAS = 33,
        MFDP = 34,
        Thunderbolt = 35,
        D8251Compatible = 160,
        D8251FIFOCompatible = 161,
        Other = 15,
    }
    // automated by stink-o-tron 3000
    public enum ExternalConnectorType : byte
    {
        Other = 1,
        Unknown = 2,
        D8bit = 3,
        D16bit = 4,
        D32bit = 5,
        D64bit = 6,
        D128bit = 7,
        D1xorx1 = 8,
        D2xorx2 = 9,
        D4xorx4 = 10,
    }
    // automated by stink-o-tron 3000
    public enum PortType : byte
    {
        D8xorx8 = 11,
        D12xorx12 = 12,
        D16xorx16 = 13,
        D32xorx32 = 14,
    }
    // automated by stink-o-tron 3000
    public enum SlotType : byte
    {
        Other = 1,
        Unknown = 2,
        Available = 3,
        Inuse = 4,
        UnavailableForexampleconnectedtoaprocessorthatisnotinstalled = 5,
    }
    // automated by stink-o-tron 3000
    public enum SlotDataBusWidth : byte
    {
        Other = 1,
        Unknown = 2,
        ShortLength = 3,
        LongLength = 4,
        D25driveformfactor = 5,
        D35driveformfactor = 6,
    }
    // automated by stink-o-tron 3000
    public enum CurrentUsage : byte
    {
        Notapplicable = 0,
        Other = 1,
        Unknown = 2,
        Fullheight = 3,
        Lowprofile = 4,
    }
    // automated by stink-o-tron 3000
    public enum SlotLength : byte
    {
        Other = 1,
        Unknown = 2,
        Video = 3,
        SCSIController = 4,
        Ethernet = 5,
        TokenRing = 6,
        Sound = 7,
        PATAController = 8,
        SATAController = 9,
        SASController = 10,
    }
    // automated by stink-o-tron 3000
    public enum LogHeaderFormat : byte
    {
        Noheader = 0,
        Type1logheadersee71651 = 1,
        Availableforfutureassignmentbythisspecification = 2,
        BIOSvendororOEMspecificformat = 128,
    }
    // automated by stink-o-tron 3000
    public enum LogType : byte
    {
        Other = 1,
        Unknown = 2,
        Systemboardormotherboard = 3,
        ISAaddoncard = 4,
        EISAaddoncard = 5,
        PCIaddoncard = 6,
        MCAaddoncard = 7,
        PCMCIAaddoncard = 8,
        Proprietaryaddoncard = 9,
        NuBus = 10,
    }
    // automated by stink-o-tron 3000
    public enum VariableDataFormatType : byte
    {
        PC98 = 160,
        PC982 = 161,
        PC983 = 162,
        PC984 = 163,
        CXLaddoncard = 164,
    }
    // automated by stink-o-tron 3000
    public enum Location : byte
    {
        Other = 1,
        Unknown = 2,
        Systemmemory = 3,
        Videomemory = 4,
        Flashmemory = 5,
        NonvolatileRAM = 6,
        Cachememory = 7,
    }
    // automated by stink-o-tron 3000
    public enum Use : byte
    {
        Other = 1,
        Unknown = 2,
        None = 3,
        Parity = 4,
        SinglebitECC = 5,
        MultibitECC = 6,
        CRC = 7,
    }
    // automated by stink-o-tron 3000
    public enum MemoryErrorCorrection : byte
    {
        Other = 1,
        Unknown = 2,
        SIMM = 3,
        SIP = 4,
        Chip = 5,
        DIP = 6,
        ZIP = 7,
        ProprietaryCard = 8,
        DIMM = 9,
        TSOP = 10,
    }
    // automated by stink-o-tron 3000
    public enum FormFactor : byte
    {
        Rowofchips = 11,
        RIMM = 12,
        SODIMM = 13,
        SRIMM = 14,
        FBDIMM = 15,
        Die = 16,
    }
    // automated by stink-o-tron 3000
    public enum MemoryType : byte
    {
        Other = 1,
        Unknown = 2,
        DRAM = 3,
        EDRAM = 4,
        VRAM = 5,
        SRAM = 6,
        RAM = 7,
        ROM = 8,
        FLASH = 9,
        EEPROM = 10,
        FEPROM = 11,
        EPROM = 12,
        CDRAM = 13,
        D3DRAM = 14,
        SDRAM = 15,
        SGRAM = 16,
        RDRAM = 17,
        DDR = 18,
        DDR2 = 19,
        DDR2FBDIMM = 20,
        Reserved = 21,
        DDR3 = 24,
        FBD2 = 25,
        DDR4 = 26,
        LPDDR = 27,
        LPDDR2 = 28,
        LPDDR3 = 29,
    }
    // automated by stink-o-tron 3000
    public enum ErrorType : byte
    {
        LPDDR4 = 30,
        Logicalnonvolatiledevice = 31,
        HBM = 32,
        HBM2 = 33,
        DDR5 = 34,
        LPDDR5 = 35,
        HBM3 = 36,
    }
    // automated by stink-o-tron 3000
    public enum ErrorGranularity : byte
    {
        Other = 1,
        Unknown = 2,
        DRAM = 3,
        NVDIMMN = 4,
        NVDIMMF = 5,
        NVDIMMP = 6,
        IntelOptanepersistentmemory = 7,
    }
    // automated by stink-o-tron 3000
    public enum ErrorOperation : byte
    {
        Other = 1,
        Unknown = 2,
        OK = 3,
        Badread = 4,
        Parityerror = 5,
        Singlebiterror = 6,
        Doublebiterror = 7,
        Multibiterror = 8,
        Nibbleerror = 9,
        Checksumerror = 10,
        CRCerror = 11,
        Correctedsinglebiterror = 12,
        Correctederror = 13,
        Uncorrectableerror = 14,
    }
    // automated by stink-o-tron 3000
    public enum Type2 : byte
    {
        Other = 1,
        Unknown = 2,
        Devicelevel = 3,
        Memorypartitionlevel = 4,
    }
    // automated by stink-o-tron 3000
    public enum Interface : byte
    {
        Other = 1,
        Unknown = 2,
        Read = 3,
        Write = 4,
        Partialwrite = 5,
    }
    // automated by stink-o-tron 3000
    public enum DeviceChemistry : byte
    {
        Other = 1,
        Unknown = 2,
        Mouse = 3,
        TrackBall = 4,
        TrackPoint = 5,
        GlidePoint = 6,
        TouchPad = 7,
        TouchScreen = 8,
        OpticalSensor = 9,
    }
    // automated by stink-o-tron 3000
    public enum LocationandStatus : byte
    {
        Other = 1,
        Unknown = 2,
        Serial = 3,
        PS = 4,
        Infrared = 5,
        HPHIL = 6,
        Busmouse = 7,
        ADB = 8,
        BusmouseDB9 = 160,
        BusmousemicroDIN = 161,
        USB = 162,
        I2C = 163,
        SPI = 164,
    }
    // automated by stink-o-tron 3000
    public enum ErrorType2 : byte
    {
        Other = 1,
        Unknown = 2,
        LeadAcid = 3,
        NickelCadmium = 4,
        Nickelmetalhydride = 5,
        Lithiumion = 6,
        Zincair = 7,
        LithiumPolymer = 8,
    }
    // automated by stink-o-tron 3000
    public enum ErrorGranularity2 : byte
    {
        Other = 1,
        Unknown = 2,
        NationalSemiconductorLM75 = 3,
        NationalSemiconductorLM78 = 4,
        NationalSemiconductorLM79 = 5,
        NationalSemiconductorLM80 = 6,
        NationalSemiconductorLM81 = 7,
        AnalogDevicesADM9240 = 8,
        DallasSemiconductorDS1780 = 9,
        Maxim1617 = 10,
        GenesysGL518SM = 11,
        WinbondW83781D = 12,
        HoltekHT82H791 = 13,
    }
    // automated by stink-o-tron 3000
    public enum ErrorOperation2 : byte
    {
        Other = 1,
        Unknown = 2,
        I = 3,
        Memory = 4,
        SMBus = 5,
    }
    // automated by stink-o-tron 3000
    public enum InterfaceType : byte
    {
        Other = 1,
        Unknown = 2,
    }
    // automated by stink-o-tron 3000
    public enum DeviceType : byte
    {
        Rambus = 3,
        SyncLink = 4,
    }
    // automated by stink-o-tron 3000
    public enum InterfaceType2 : byte
    {
        Unknown = 0,
        KCSKeyboardControllerStyle = 1,
        SMICServerManagementInterfaceChip = 2,
        BTBlockTransfer = 3,
        SSIFSMBusSystemInterface = 4,
        Reservedforfutureassignmentbythisspecification = 5,
    }
}