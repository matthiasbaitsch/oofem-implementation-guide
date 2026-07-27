public class Structure
{
    public List<Node> Nodes = [];
    public List<Element> Elements = [];

    public Node AddNode(double x1, double x2)
    {
        Node n = new Node(x1, x2);
        this.Nodes.Add(n);
        return n;
    }

    public Element AddElement(double e, double a, int n1, int n2)
    {
        Node node1 = this.Nodes[n1];
        Node node2 = this.Nodes[n2];
        Element element = new Element(e, a, node1, node2);
        this.Elements.Add(element);
        return element;
    }

    public int EnumerateDOFs()
    {
        int n = 0;
        foreach (Node node in this.Nodes)
        {
            n = node.EnumerateDOFs(n);
        }
        return n;
    }

    public void Solve()
    {
        int N = this.EnumerateDOFs();
        Matrix K = Matrix.Build.Dense(N, N);
        Vector r = Vector.Build.Dense(N);

        // Elements
        foreach (Element element in this.Elements)
        {
            int[] dofs = element.DOFs();
            Matrix Ke = element.StiffnessMatrix();

            for (int i = 0; i < dofs.Length; i++)
            {
                int I = dofs[i];

                for (int j = 0; j < dofs.Length; j++)
                {
                    int J = dofs[j];

                    if (I != -1 && J != -1)
                    {
                        K[I, J] += Ke[i, j];
                    }
                }
            }
        }

        // Forces
        foreach (Node node in this.Nodes)
        {
            int[] dofs = node.DOFs;

            for (int i = 0; i < dofs.Length; i++)
            {
                int I = dofs[i];

                if (I != -1)
                {
                    r[I] += node.Force.Components[i];
                }
            }
        }

        // Solve
        Vector u = K.Solve(r);

        // Nodal displacements
        foreach (Node node in this.Nodes)
        {
            int[] dofs = node.DOFs;

            for (int i = 0; i < dofs.Length; i++)
            {
                int I = dofs[i];

                if (I != -1)
                {
                    node.Displacement[i] = u[I];
                }
            }
        }

        // Print
        Console.WriteLine("K = " + K);
        Console.WriteLine("r = " + r);
        Console.WriteLine("u = " + u);
    }

    public void Print()
    {
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        Console.WriteLine("                          N O D E S");
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        Console.WriteLine($"         Position               Constraint       Force");
        Console.WriteLine($"         X           Y          Cx     Cy        Fx          Fy");
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        for (int i = 0; i < this.Nodes.Count; i++)
        {
            Console.WriteLine($"{i,3}{this.Nodes[i]}");
        }
        Console.WriteLine("──────────────────────────────────────────────────────────────────");

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        Console.WriteLine("                         E L E M E N T S");
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        Console.WriteLine($"                 E         A              L");
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
        for (int i = 0; i < this.Elements.Count; i++)
        {
            Console.WriteLine($"{i,3}{this.Elements[i]}");
        }
        Console.WriteLine("──────────────────────────────────────────────────────────────────");
    }
}