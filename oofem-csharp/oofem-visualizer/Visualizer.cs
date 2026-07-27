using BoDraw;

public class Visualizer
{
    public Structure Structure;

    public double ForceScale = 1;
    public double ConstraintSize = 1;
    public double ElementForceScale = 1;
    public double DisplacementScale = 1;

    public Visualizer(Structure structure)
    {
        this.Structure = structure;
    }

    public void DrawSystem(IBoDraw bd)
    {
        // Elements
        foreach (Element element in this.Structure.Elements)
        {
            Vector p1 = element.Node1.Position;
            Vector p2 = element.Node2.Position;

            bd.Add(new Line(p1[0], p1[1], p2[0], p2[1]).WithThickness(3));
        }

        // Constraints
        double a = this.ConstraintSize / 2;
        double b = 0.886 * this.ConstraintSize;

        foreach (Node node in this.Structure.Nodes)
        {
            double x1 = node.Position[0];
            double x2 = node.Position[1];

            if (node.Constraint.Fixed[0])
            {
                bd.Add(new Polygon(x1, x2, x1 - b, x2 - a, x1 - b, x2 + a));
            }
            if (node.Constraint.Fixed[1])
            {
                bd.Add(new Polygon(x1, x2, x1 - a, x2 - b, x1 + a, x2 - b));
            }
        }

        // Forces
        foreach (Node node in this.Structure.Nodes)
        {
            var f = node.Force.Components;

            if (f.Norm(2) != 0)
            {
                var p1 = node.Position;
                var p2 = p1 + this.ForceScale * f;

                bd.Add(new Arrow(p1[0], p1[1], p2[0], p2[1]).WithColor(Colors.Red).WithThickness(1.5));
            }
        }
    }

    public void DrawDeformation(BoDrawApp bd)
    {
        foreach (Element element in this.Structure.Elements)
        {
            Vector p1 = element.Node1.Position;
            Vector p2 = element.Node2.Position;
            Vector pu1 = p1 + this.DisplacementScale * element.Node1.Displacement;
            Vector pu2 = p2 + this.DisplacementScale * element.Node2.Displacement;

            bd.Add(new Line(p1[0], p1[1], p2[0], p2[1]).WithThickness(1));
            bd.Add(new Line(pu1[0], pu1[1], pu2[0], pu2[1]).WithThickness(3).WithColor(Colors.DarkBlue));
        }
    }

    public void DrawElementForces(BoDrawApp bd)
    {
        foreach (Element element in this.Structure.Elements)
        {
            double N = element.NormalForce();
            Vector d = element.D();
            Vector n = Vector.Build.DenseOfArray([d[1], -d[0]]);
            Vector p1 = element.Node1.Position;
            Vector p2 = element.Node2.Position;
            Vector p3 = p2 + this.ElementForceScale * N * n;
            Vector p4 = p1 + this.ElementForceScale * N * n;

            Polygon p = new Polygon(p1[0], p1[1], p2[0], p2[1], p3[0], p3[1], p4[0], p4[1]);
            if (N >= 0) { p.FillColor = Colors.Blue; }
            else { p.FillColor = Colors.Red; }
            p.FillOpacity = 0.3;
            bd.Add(p);
            bd.Add(new Line(p1[0], p1[1], p2[0], p2[1]).WithThickness(3));
        }
    }
}