Structure s = new Structure();

Node n1 = s.AddNode(0, 0);
n1.Constraint.Fixed[0] = true;
n1.Constraint.Fixed[1] = true;
Node n2 = s.AddNode(4, 0);
n2.Constraint.Fixed[1] = true;
Node n3 = s.AddNode(0, 3);
n3.Force.Components[0] = 1200;

Element e1 = s.AddElement(10e9, 1e-4, 0, 1);
Element e2 = s.AddElement(10e9, 1e-4, 0, 2);
Element e3 = s.AddElement(10e9, 1e-4, 2, 1);

s.Print();
Console.WriteLine("\nK3");
Console.WriteLine(e3.StiffnessMatrix());

// Degrees of freedom
int N = s.EnumerateDOFs();

Console.WriteLine($"Number of DOFs: {N}");
for (int i = 0; i < s.Nodes.Count; i++)
{
    Console.WriteLine($"Node {i} has DOFs [{string.Join(", ", s.Nodes[i].DOFs)}]");
}
for (int i = 0; i < s.Elements.Count; i++)
{
    Console.WriteLine($"Element {i} has DOFs [{string.Join(", ", s.Elements[i].DOFs())}]");
}

s.Solve();
Console.WriteLine($"Node 2 has displacement [{string.Join(", ", s.Nodes[2].Displacement)}]");
Console.WriteLine($"\nN3 = {e3.NormalForce()}");
s.PrintResults();