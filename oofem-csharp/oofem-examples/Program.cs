using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using ExcelDataReader;

bool toBool(object cell) => Convert.ToBoolean(cell);
int toInt(object cell) => cell == DBNull.Value ? 0 : Convert.ToInt32(cell);
double toDouble(object cell) => cell == DBNull.Value ? 0 : Convert.ToDouble(cell);

string SourceDirectory([CallerFilePath] string file = "") => Path.GetDirectoryName(file)!;
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
string path = Path.Combine(SourceDirectory(), "structures/guide-example.xlsx");
using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
using var reader = ExcelReaderFactory.CreateReader(stream);

DataSet data = reader.AsDataSet(new ExcelDataSetConfiguration
{
    ConfigureDataTable = _ => new() { UseHeaderRow = true }
});


Structure s = new Structure();

Console.WriteLine(data.Tables["nodes"]);

foreach (DataRow row in data.Tables["nodes"]!.Rows)
{
    Node node = s.AddNode(toDouble(row["x"]), toDouble(row["y"]));
    node.Constraint.Fixed[0] = toBool(row["cx"]);
    node.Constraint.Fixed[1] = toBool(row["cy"]);
    node.Force.Components[0] = toDouble(row["fx"]);
    node.Force.Components[1] = toDouble(row["fy"]);
}

foreach (DataRow row in data.Tables["elements"]!.Rows)
{
    s.AddElement(toDouble(row["e"]), toDouble(row["a"]), toInt(row["n1"]), toInt(row["n2"]));
}

s.Print();
s.Solve();
s.PrintResults();
