using BoDraw;

Structure s = new Structure();

Node n1 = s.AddNode(0, 0);
n1.Constraint.Fixed[0] = true;
n1.Constraint.Fixed[1] = true;
Node n2 = s.AddNode(4, 0);
n2.Constraint.Fixed[1] = true;
Node n3 = s.AddNode(0, 3);
n3.Force.Components[0] = 1200;

s.AddElement(10e9, 1e-4, 0, 1);
s.AddElement(10e9, 1e-4, 0, 2);
s.AddElement(10e9, 1e-4, 2, 1);

BoDrawApp app = new BoDrawApp();
Visualizer v = new Visualizer(s);
v.ForceScale = 0.001;
v.ConstraintSize = 0.1;
v.DrawSystem(app);
app.Show();

s.Solve();

v.DisplacementScale = 75;
v.DrawDeformation(app);
app.Show();

v.ElementForceScale = 1e-3;
v.DrawElementForces(app);
app.Show();
