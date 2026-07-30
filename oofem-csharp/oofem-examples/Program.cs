using BoDraw;

Structure s = ExcelReader.Read("structures/girder.xlsx");
// Structure s = ExcelReader.Read("structures/guide-example.xlsx");

s.Solve();
s.Print();
s.PrintResults();

BoDrawApp app = new BoDrawApp();
Visualizer v = new Visualizer(s);
// v.DrawSystem(app);
// v.DrawDeformation(app);
v.DrawElementForces(app);
app.Show();


