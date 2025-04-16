// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Drawing;
using CSC365_Project2.BplusOps;
using CSC365_Project2;
using CSC365_Project2.Models;
using CSC365_Project2.Project1Ops;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.IO.Enumeration;

FileOperations fileOps = new();
CollectionsOperations collOps = new();
StringBuilder narrative = new();

Console.WriteLine("Reading VAERS data files...");

// --------------------------------------------------------------------------------------------
//  Read all the VAERSVAX files and collect only COVID19 records.
//  Also fills the HashSet CovidVaersIds with each (COVID19) VAERS_ID from the files
//  The Hashset is used in subsequent operations to only read in records from the other files
//  that have VAERS_IDs from COVID19 vaccines
// --------------------------------------------------------------------------------------------
HashSet<int> CovidVaersIds = [];
List<VaxInfo> vaersVaxRecords = fileOps.ReadAllVaxInfoFiles("Data", CovidVaersIds, ["COVID19", "COVID19-2"]);

narrative.AppendLine($"February Vaccine Information Read: {vaersVaxRecords.Count}");
narrative.AppendLine($"There are {CovidVaersIds.Count} unique VAERS_IDs collected from the various VAERSVAX files");

// --------------------------------------------------------------------------------------------
//  Read all the Patient Data files and collect only records where the VAERS_ID on the incoming
//  record is in the Hashset CovidVaersIds
// --------------------------------------------------------------------------------------------

List<PatientRec> patientDataRecs = fileOps.ReadAllPatientRecFiles("Data", CovidVaersIds);
narrative.AppendLine($"February Patient Records Read: {patientDataRecs.Count}");

// --------------------------------------------------------------------------------------------
//  Read all the Symptom Data files and collect only records where the VAERS_ID on the incoming
//  record is in the Hashset CovidVaersIds
// --------------------------------------------------------------------------------------------

List<Symptoms> symptomRecs = fileOps.ReadAllSymptomsFiles("Data", CovidVaersIds);
narrative.AppendLine($"February Symptoms Records Read: {symptomRecs.Count}");

// --------------------------------------------------------------------------------------------
//  Create VAERS_COVID_DataFebruary2025 csv output file containing the combination of info
//  from all 3 sources (this is the deliverable for Task 1)
// --------------------------------------------------------------------------------------------
IEnumerable<VaersReport> februaryData = collOps.GetCombinedDataSet(vaersVaxRecords, patientDataRecs, symptomRecs);
fileOps.WriteCSVFile(februaryData, "VAERS_COVID_DataFebruary2025.csv");
narrative.AppendLine($"VAERS_COVID_DataFebruary2025.csv was created and contains {februaryData.Count()} records\n");

// -------------------------------------------------------------------------------------------------------------------------------------------------------
// -------------------------------------------------------------------------------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------------
//  Read in VAERS_COVID_DataDecember2024.csv file and put data into VaersReport items
// --------------------------------------------------------------------------------------------
IEnumerable<VaersReport> decemberData = fileOps.ReadVaersReportData("VAERS_COVID_DataDecember2024.csv");
narrative.AppendLine($"VAERS_COVID_DataDecember2024.csv was created and contains {decemberData.Count()} records");


// --------------------------------------------------------------------------------------------
//  Get the maximum degree of a node from user and create tree with Deccember data
// --------------------------------------------------------------------------------------------
int k;
Console.WriteLine("Input an integer for the maximum degree of a node, k: ");
while (!int.TryParse(Console.ReadLine(), out k))
{
    Console.WriteLine("Invalid input. Please enter a valid integer.");
    Console.Write("Input an integer for the maximum degree of a node, k: ");
}

var bPlusTree = new Tree<int, List<VaersReport>>(k);

var groupedData = decemberData.GroupBy(x => x.VAERS_ID);
foreach (var item in groupedData)
{
    bPlusTree.Insert(item.Key, item.Select(x => x).ToList());
}

narrative.AppendLine($"The B+ tree contains {groupedData.Count()} unique records");


// --------------------------------------------------------------------------------------------
//  Insert February data into the tree
// --------------------------------------------------------------------------------------------
var groupedData2 = februaryData.GroupBy(x => x.VAERS_ID);
foreach (var item in groupedData2)
{
    bPlusTree.Insert(item.Key, item.Select(x => x).ToList());
}

narrative.AppendLine($"{groupedData2.Count()} unique records were added to the B+ tree from the February 2025 data");


// --------------------------------------------------------------------------------------------
//  Prompt user (optional tree statistics)
// --------------------------------------------------------------------------------------------
Console.Write("Do you want to display the contents of narrative? (y/n): ");
string input = Console.ReadLine()?.Trim().ToLower();

while (input != "y" && input != "n")
{
    Console.WriteLine("Invalid input. Please enter 'y' for Yes or 'n' for No.");
    Console.Write("Do you want to display the contents of narrative? (y/n): ");
    input = Console.ReadLine()?.Trim().ToLower();
}

if (input == "y")
{
    Console.WriteLine("\nNarrative contents:\n");
    Console.WriteLine(narrative.ToString());
}
else
{
    Console.WriteLine("\nNarrative display skipped.");
}

// --------------------------------------------------------------------------------------------
//  Implement a simple (text-based) B+ tree visualization and save it into a txt file
// --------------------------------------------------------------------------------------------
string fileName = "treeVisualization.txt";
bPlusTree.WriteTreeToFile(fileName);
Console.WriteLine($"Tree was output to {fileName}");
