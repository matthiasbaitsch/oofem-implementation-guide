using BoDraw;
using static System.Math;

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
        this.UpdateParameters();

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
        this.UpdateParameters();
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
        this.UpdateParameters();
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

    public void UpdateParameters()
    {
        double s = this.SystemSize();
        double maxF = this.Structure.Nodes.Max(n => n.Force.Components.Norm(2));
        double maxU = this.Structure.Nodes.Max(n => n.Displacement.Norm(2));
        double minN = this.Structure.Elements.Min(e => e.NormalForce());
        double maxN = this.Structure.Elements.Max(e => e.NormalForce());

        this.ConstraintSize = 0.03 * s;
        this.ForceScale = 0.2 / maxF * s;
        this.DisplacementScale = 0.1 * s / maxU;
        this.ElementForceScale = 0.3 * s / (maxN - minN);
    }

    public double SystemSize()
    {
        double[,] ranges = new double[,] {
             { double.MaxValue, double.MinValue },
             { double.MaxValue, double.MinValue }
        };

        foreach (Node node in this.Structure.Nodes)
        {
            for (int i = 0; i < 2; i++)
            {
                double p = node.Position[i];
                ranges[i, 0] = Min(ranges[i, 0], p);
                ranges[i, 1] = Max(ranges[i, 1], p);
            }
        }

        double d1 = ranges[0, 1] - ranges[0, 0];
        double d2 = ranges[1, 1] - ranges[1, 0];

        return Sqrt(d1 * d2);
    }
}