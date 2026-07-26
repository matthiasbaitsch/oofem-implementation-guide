using MathNet.Numerics.LinearAlgebra;

public class Node
{
    public Force Force = new Force();
    public Constraint Constraint = new Constraint();
    public Vector<double> Position = Vector<double>.Build.Dense(2);

    public Node(double x1, double x2)
    {
        this.Position[0] = x1;
        this.Position[1] = x2;
    }

    public override string ToString()
    {
        int d = 2;
        string s = "";

        for (int i = 0; i < d; i++)
        {
            s += $"{this.Position[i],12:0.0000}";
        }

        for (int i = 0; i < d; i++)
        {
            if (this.Constraint.Fixed[i]) { s += "      X"; }
            else { s += "       "; }
        }

        for (int i = 0; i < d; i++)
        {
            double f = this.Force.Components[i];
            if (f != 0) { s += $"{f,12:0.00}"; }
            else { s += "            "; }
        }


        return s;
    }

}