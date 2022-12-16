using Klank;

// Retrieve table structures from DSP Pdf
//DSPInterpreter.Interpret();
SMBIOS smbios = new SMBIOS();
Console.WriteLine((TableIDs)smbios[27].Type);