using Klank.Generic;
using System.Management;

namespace Klank
{
    public class Hardware
    {
        public static string SSlotSerial
        {
            get
            {
                var managementClass = new ManagementClass("Win32_SystemSlot");
                var instances = managementClass.GetInstances();

                foreach (ManagementObject managObj in instances)
                {
                    double hash = 0;

                    foreach (var property in managObj.Properties)
                    {
                        if (property.Value == null || property.IsArray)
                            continue;

                        if (property.Value.GetType() == typeof(string))
                            hash += ((string)property.Value).HashSE();
                        else
                            hash += property.Value.HashSE();
                    }

                    return hash.HashToAlpha();
                }

                return string.Empty;
            }
        }

        public static string MemorySerial
        {
            get
            {
                var managementClass = new ManagementClass("Win32_PhysicalMemory");
                var instances = managementClass.GetInstances();

                foreach (ManagementObject managObj in instances)
                {
                    double hash = ((string)managObj.Properties["SerialNumber"].Value).HashSE() + ((string)managObj.Properties["PartNumber"].Value).HashSE();

                    return hash.HashToAlpha();
                }

                return string.Empty;
            }
        }

        public static string DiskSerial
        {
            get
            {
                var managementClass = new ManagementClass("Win32_DiskDrive");
                var instances = managementClass.GetInstances();

                foreach (ManagementObject managObj in instances)
                {
                    return managObj.Properties["SerialNumber"].Value.ToString();
                }

                return string.Empty;
            }
        }
    }
}
