import sys
from pathlib import Path


# Add the neighboring "oofem" and "oofem-visualizer" folders
# to the Python search path
parent_directory = Path(__file__).resolve().parent.parent
sys.path.append(str(parent_directory / "oofem"))
sys.path.append(str(parent_directory / "oofem-visualizer"))

from excel_reader import read_excel
from visualizer import Visualizer


structure = read_excel("oofem-examples/structures/girder.xlsx")
# structure = read_excel("oofem-examples/structures/guide-example.xlsx")

structure.solve()
structure.print()
structure.print_results()

visualizer = Visualizer(structure)
visualizer.draw_element_forces()
